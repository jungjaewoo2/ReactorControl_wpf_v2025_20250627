using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.IO.MemoryMappedFiles;
using System.Reflection;
using System.IO.Compression;
using System.Windows.Threading;
using PDBLib;
using LibCommunication;

namespace ProcessValue
{
    /// <summary>
    /// AlarmViewer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AlarmViewer : UserControl
    {
       
        public AlarmViewer()
        {
            FontFamily = new FontFamily("Arial Monospaced");
            InitializeComponent();
            AssignPointer();
            loadAlarm();
            _shmAlarm = new AlarmSharedMemory();
            _shmAlarm.OpenAlarmSharedMemory();

            InitTimer();
        }

        const int AlarmPageMax = 5;
        public const string NewAlarmMark = "N";
        public const string AckAlarmMark = "A";
        public const string ClearMark    = "C";
        public const string RingBackMark = "R";

        List<AlarmViewItem> _AlarmViewList = null;  // 화면상에 표시하는 경보페이지의 경보 목록
        List<alarmItem> _alarmBuf = null;  // IPS로 부터 입력된 전체 경보 목록

        int _numOfPage = 25;
        static int _MaxAlarmView = 25;
        int _currentPage = 0, _pageCount = 0;
        uint _testMode = 0;
        int _selectedRow = 0;
        bool _isListUpdated = false;
        bool _blinkFlag = false, _slowBlinkFlag = false;

        int _filterOpt = (int)FilterOpt.None;
        int _sortOpt = (int)SortOpt.None;

        public Border[] ALBG = new Border[_MaxAlarmView];
        public TextBlock[] DSC = new TextBlock[_MaxAlarmView];
        public TextBlock[] STS = new TextBlock[_MaxAlarmView];
        public TextBlock[] PRI = new TextBlock[_MaxAlarmView];
        public TextBlock[] AID = new TextBlock[_MaxAlarmView];
        public TextBlock[] TME = new TextBlock[_MaxAlarmView];
        public TextBlock[] VAL = new TextBlock[_MaxAlarmView];
        public TextBlock[] SET = new TextBlock[_MaxAlarmView];
        public TextBlock[] UNT = new TextBlock[_MaxAlarmView];


        /*enum CommandID : int
        {
            AckCmd = 1,
            ResetCmd,
            SlientCmd,
            ServerStatus,
            ClearAlarm,

            TestCmd = 10
        }*/

        enum ColumnID : int
        {
            TagName = 0,
            Pri,
            Desc,
            Status,
            Value,
            SetPoint,
            Unit,
            Time,
            AlarmData
        }

        enum FilterOpt : int
        {
            None,
            UnAck,
            Ack,
            Ringback,
            Pri1,
            Pri2,
            Clear,
            Count
        }

        enum SortOpt : int
        {
            None,
            Time,
            TagName,
            Priority,
            Count
        }


        AlarmSharedMemory _shmAlarm = null;
        //OWSCommandClient _cmdClient = null;

        //DataTable _alarms = new DataTable();
        Mutex _mutex = new Mutex();

        DispatcherTimer timerAlarm = new DispatcherTimer();
        MultimediaTimer timer = new MultimediaTimer();

        IPSCommandClient commandClient = new IPSCommandClient("127.0.0.1", "192.168.0.1", "192.168.0.1", "192.168.0.1", 2019, 0);
        
        public void InitTimer()
        {
            timerAlarm.Interval = TimeSpan.FromMilliseconds(200);    //시간간격 설정
            timerAlarm.Tick += new EventHandler(timerAlarm_Tick);          //이벤트 추가
            timerAlarm.Start();                                       //타이머 시작. 종료는 timer.Stop(); 으로 한다

            timer.Elapsed += new EventHandler(timerBlink_Tick);
            timer.Interval = 250;
            timer.Start();

            // timerBlink.Interval = TimeSpan.FromMilliseconds(250);    //시간간격 설정
            //timerBlink.Tick += new EventHandler(timerBlink_Tick);          //이벤트 추가
            ///timerBlink.Start();                                       //타이머 시작. 종료는 timer.Stop(); 으로 한다
        }

        private void timerAlarm_Tick(object sender, EventArgs e)
        {
            ReadAlarmSource();
        }

        bool even = false;
        private void timerBlink_Tick(object sender, EventArgs e)
        { 
            AlarmBlinking();
            if (even)
            {
                even = false;
                SlowBlinking();
            } else
            {
                even = true;
            }
        }

        void AlarmBlinking()
        {
            if (_AlarmViewList == null) return;

            if (_blinkFlag)
                _blinkFlag = false;
            else
                _blinkFlag = true;

            int begin = _currentPage * _numOfPage;
            int end = begin + _numOfPage;
            if (_AlarmViewList.Count <= end) end = _AlarmViewList.Count;
            int vi = 0;
            for (int i = begin; i < end; i++)
            {
                AlarmViewItem alarm = _AlarmViewList[i];
                if (alarm.IsBlink)
                {
                    if (_blinkFlag)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { ALBG[vi].Background = AlarmBlcikFillColor; }));
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { DSC[vi].Foreground = AlarmTextColor; }));
                    }
                    else
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { ALBG[vi].Background = AlarmFillColor; }));
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { DSC[vi].Foreground = AlarmTextFillColor; }));
                    }
                }
                vi++;
            }
        }

        void SlowBlinking()
        {
            if (_AlarmViewList == null) return;
            if (_slowBlinkFlag)
                _slowBlinkFlag = false;
            else
                _slowBlinkFlag = true;

            int begin = _currentPage * _numOfPage;
            int end = begin + _numOfPage;
            if (_AlarmViewList.Count <= end)
                end = _AlarmViewList.Count;
            int vi = 0;

            for (int i = begin; i < end; i++)
            {

                AlarmViewItem alarm = _AlarmViewList[i];
                if (alarm.IsSlowBlink)
                {
                    if (_slowBlinkFlag)
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { ALBG[vi].Background = AlarmBlcikFillColor; }));
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { DSC[vi].Foreground = AlarmTextColor; }));
                    }
                    else
                    {
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { ALBG[vi].Background = AlarmSlowFillColor; }));
                        Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { DSC[vi].Foreground = AlarmTextFillColor; }));
                    }
                }
                vi++;

            }


        }



        public void loadAlarm()
        {
            if (_alarmBuf == null) _alarmBuf = new List<alarmItem>();

          /*  alarmItem alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Neutron Power High", "3.0", 1, "XD680001", "3.14", "%", "N", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "R", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680002", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "A", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680003", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Temperature Hi", "3.0", 2, "XD680001", "3.14", "%", "N", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680002", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680003", "Temperature Hi", "3.0", 2, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680002", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680003", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Neutron Power High", "3.0", 1, "XD680001", "3.14", "%", "N", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "R", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680002", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "A", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680003", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Temperature Hi", "3.0", 2, "XD680001", "3.14", "%", "N", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680002", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680003", "Temperature Hi", "3.0", 2, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680002", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680003", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Neutron Power High", "3.0", 1, "XD680001", "3.14", "%", "N", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "R", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680002", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "A", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680003", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Temperature Hi", "3.0", 2, "XD680001", "3.14", "%", "N", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680002", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680003", "Temperature Hi", "3.0", 2, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680002", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680003", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Neutron Power High", "3.0", 1, "XD680001", "3.14", "%", "N", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "R", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680002", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "A", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680003", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Temperature Hi", "3.0", 2, "XD680001", "3.14", "%", "N", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680002", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680003", "Temperature Hi", "3.0", 2, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680002", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680003", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "R", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680002", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "A", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680003", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Temperature Hi", "3.0", 2, "XD680001", "3.14", "%", "N", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680002", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680003", "Temperature Hi", "3.0", 2, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680000", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680002", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);

            alarmItem = new alarmItem(DateTime.Now, "AXD680003", "Temperature Hi", "3.0", 1, "XD680001", "3.14", "%", "C", 100);
            AddItem(alarmItem);
            */
        }

        void ReadAlarmSource()
        {
            if (_mutex.WaitOne())
            {
                List<alarmItem> lt = _shmAlarm.GetAlarmList();
                if (lt != null)
                {
                    _alarmBuf =lt;

                    _alarmBuf.Reverse();

                    AlarmSorting();
                    AlarmFiltering();   // copy alarm of alarmbuf to _AlarmViewList

                    if (_AlarmViewList == null)
                    {
                        _currentPage = 0;
                        _pageCount = 1;
                        return;
                    }

                    if (_AlarmViewList.Count == 0)
                    {
                        _currentPage = 0;
                        _pageCount = 1;
                    }

                    CalcPageCount();
                    UpdateCurrentStatus();

                    LoadAlarmPage();

                }
            }
            _mutex.ReleaseMutex();
        }

        void AlarmSorting()
        {
            if (_alarmBuf == null)
                return;

            List<alarmItem> bufList = _alarmBuf;

            switch (_sortOpt)
            {
                //case (int)SortOpt.Time:
                //    {
                //        _alarmBuf = bufList.OrderBy(opt => opt.AlarmDateTime).ToList();
                //    }
                //    break;
                case (int)SortOpt.TagName:
                    {
                        _alarmBuf = bufList.OrderBy(opt => opt.AlarmTag).ToList();
                    }
                    break;
                case (int)SortOpt.Priority:
                    {
                        _alarmBuf = bufList.OrderBy(opt => opt.AlarmPriority).ToList();
                    }
                    break;
                default:
                    {
                        _alarmBuf = bufList;
                    }
                    break;
            }
        }

        List<AlarmViewItem> BuildAlarmViewList (List<alarmItem> alarmItemList)
        {
            List<AlarmViewItem> aviList = new List<AlarmViewItem>();
            for (int i=0;i<alarmItemList.Count;i++)
            {
                aviList.Add(new AlarmViewItem(alarmItemList[i]));
            }
            return aviList;
        }

        void AlarmFiltering()
        {
            if (_alarmBuf == null)
                return;

            if (_filterOpt == (int)FilterOpt.None)
            {
                _AlarmViewList = BuildAlarmViewList(_alarmBuf);
                return;
            }

            switch (_filterOpt)
            {
                case (int)FilterOpt.UnAck:
                    {
                        List<alarmItem> alarmItemList = new List<alarmItem>();
                        foreach (alarmItem item in _alarmBuf)
                        {
                            if ((item.AlarmTranStatus != AckAlarmMark) && (item.AlarmTranStatus != ClearMark))
                                alarmItemList.Add(item);
                        }
                        _AlarmViewList = BuildAlarmViewList(alarmItemList);
                    }
                    break;
                case (int)FilterOpt.Ack:
                    {
                        _AlarmViewList = BuildAlarmViewList(_alarmBuf.Where(alarm => alarm.AlarmTranStatus == AckAlarmMark).ToList());
                    }
                    break;
                case (int)FilterOpt.Ringback:
                    {
                        _AlarmViewList = BuildAlarmViewList(_alarmBuf.Where(alarm => alarm.AlarmTranStatus == RingBackMark).ToList());
                    }
                    break;
                case (int)FilterOpt.Pri1:
                    {
                        _AlarmViewList = BuildAlarmViewList(_alarmBuf.Where(alarm => alarm.AlarmPriority == 1).ToList());
                    }
                    break;
                case (int)FilterOpt.Pri2:
                    {
                        _AlarmViewList = BuildAlarmViewList(_alarmBuf.Where(alarm => alarm.AlarmPriority == 2).ToList());
                    }
                    break;
                case (int)FilterOpt.Clear:
                    {
                        _AlarmViewList = BuildAlarmViewList(_alarmBuf.Where(alarm => alarm.AlarmTranStatus == ClearMark).ToList());
                    }
                    break;
            }
        }


        void EnableNavigateCtrl()
        {
            TXT_ALARMTITLE.Text = string.Format("({0}/{1})", _currentPage + 1, _pageCount);
            /*
            txtPageState.Text = string.Format("({0} / {1})", _currentPage + 1, _pageCount);

            if (_currentPage == 0)
            {
                btnGoBegin.Enabled = false;
                btnGoFront.Enabled = false;

                if (_currentPage < (_pageCount - 1))
                {
                    btnGoNext.Enabled = true;
                    btnGoEnd.Enabled = true;
                }
                else
                {
                    btnGoNext.Enabled = false;
                    btnGoEnd.Enabled = false;
                }
            }
            else if ((_pageCount - 1) <= _currentPage)
            {
                btnGoNext.Enabled = false;
                btnGoEnd.Enabled = false;

                if (1 < _pageCount)
                {
                    btnGoBegin.Enabled = true;
                    btnGoFront.Enabled = true;
                }
                else
                {
                    btnGoBegin.Enabled = false;
                    btnGoFront.Enabled = false;
                }

            }
            else
            {
                btnGoBegin.Enabled = true;
                btnGoFront.Enabled = true;
                btnGoNext.Enabled = true;
                btnGoEnd.Enabled = true;
            }*/
        }
        void GoFirst()
        {
            _currentPage = 0;
            LoadAlarmPage();
        }

        void GoPrevious()
        {
            if (_currentPage == _pageCount)
                _currentPage = _pageCount - 1;

            _currentPage--;

            if (_currentPage < 1)
                _currentPage = 0;

            LoadAlarmPage();
        }

        void GoNext()
        {
            _currentPage++;
            if ((_pageCount - 1) <= _currentPage)
                _currentPage = _pageCount - 1;
            LoadAlarmPage();
        }

        void GoLast()
        {
            _currentPage = _pageCount - 1;
            LoadAlarmPage();
        }

        void LoadAlarmPage()
        {
            EnableNavigateCtrl();

            if (_mutex.WaitOne(1000))
            {
                _isListUpdated = false;

                int begin = _currentPage * _numOfPage;
                int end = begin + _numOfPage;

                if (_AlarmViewList.Count <= end)
                    end = _AlarmViewList.Count;


                // 경보를 테이블에 랜더링
                int vi = 0;
                alarmItem ai;
                for (int i = begin; i < end; i++)
                {
                    ai = _AlarmViewList[i]._alarmItem;
                    DSC[vi].Text = ai.AlarmDesc;
                    STS[vi].Text = ai.AlarmTranStatus;
                    PRI[vi].Text = ai.AlarmPriority.ToString();
                    AID[vi].Text = ai.AlarmTag;
                    VAL[vi].Text = ai.AlarmProcessValue.ToString();
                    SET[vi].Text = ai.SetpointStr;
                    UNT[vi].Text = ai.Unit;
                    TME[vi].Text = ai.AlarmDateTime.ToString("HH:mm:ss yyyy-MM-dd");
                    vi++;
                }

                for (int i=vi;i<25;i++)
                {
                    DSC[i].Text = "";
                    STS[i].Text = "";
                    PRI[i].Text = "";
                    AID[i].Text = "";
                    VAL[i].Text = "";
                    SET[i].Text = "";
                    UNT[i].Text = "";
                    TME[i].Text = "";
                    ALBG[i].Background = AlarmTextFillColor;
                    DSC[i].Foreground = AlarmTextColor;

                }
                _isListUpdated = true;
                AlarmListColorCoding();
            }

            _mutex.ReleaseMutex();
        }


        Brush AlarmBlcikFillColor = new SolidColorBrush(Color.FromRgb(255, 255, 0));
        Brush AlarmFillColor = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        Brush AlarmSlowFillColor = new SolidColorBrush(Color.FromRgb(200, 0, 0));
        Brush AlarmTextColor = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        Brush AlarmTextFillColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        void AlarmListColorCoding()
        {
            // 27
            int begin = _currentPage * _numOfPage;
            int end = begin + _numOfPage;
            if (_AlarmViewList.Count <= end) end = _AlarmViewList.Count;


            AlarmViewItem avi = null;
            int vi = 0;
            for (int i = begin; i < end; i++)
            {

                avi = _AlarmViewList[i];
                if (avi.IsBlink)
                {
                    if (_blinkFlag)
                    {
                        ALBG[vi].Background = AlarmBlcikFillColor;
                        DSC[vi].Foreground = AlarmTextColor;

                    }
                    else
                    {
                        ALBG[vi].Background = AlarmFillColor;
                        DSC[vi].Foreground = AlarmTextFillColor;
                    }
                }
                else if (avi.IsSlowBlink)
                {
                    if (_slowBlinkFlag)
                    {
                        ALBG[vi].Background = AlarmBlcikFillColor;
                        DSC[vi].Foreground = AlarmTextColor;


                    }
                    else
                    {
                        ALBG[vi].Background = AlarmSlowFillColor;
                        DSC[vi].Foreground = AlarmTextFillColor;
                    }
                }
                else
                {
                    ALBG[vi].Background = AlarmTextFillColor;
                    DSC[vi].Foreground = AlarmTextColor;

                }
                vi++;
            }

            for (int i = vi; i < 25; i++)
            {
                ALBG[vi].Background = AlarmTextFillColor;
                DSC[vi].Foreground = AlarmTextColor;
            }
        }


        void CalcPageCount()
        {
            if (_AlarmViewList != null)
            {
                _pageCount = _AlarmViewList.Count / _numOfPage;
                if (0 < (_AlarmViewList.Count % _numOfPage))
                {
                    _pageCount++;
                }

                if (AlarmPageMax < _pageCount)
                {
                    _pageCount = AlarmPageMax;
                }
            }
            else
            {
                _pageCount = 1;
            }
        }
        void UpdateCurrentStatus()
        {
            int newAlmCnt = 0, ringCnt = 0, ackCnt = 0, clearCnt = 0;

            foreach (AlarmViewItem Viewitem in _AlarmViewList)
            {
                alarmItem item = Viewitem._alarmItem;
                if (item == null)
                    continue;

                switch (item.AlarmTranStatus)
                {
                    case NewAlarmMark:
                        newAlmCnt++;
                        break;
                    case AckAlarmMark:
                        ackCnt++;
                        break;
                    case RingBackMark:
                        ringCnt++;
                        break;
                    case ClearMark:
                        clearCnt++;
                        break;
                }
            }

            TXT_NEWALARM.Text = string.Format("N : {0}", newAlmCnt);
            TXT_ACKALARM.Text = string.Format("A : {0}", ackCnt);
            TXT_RINGALARM.Text = string.Format("R : {0}", ringCnt);
            TXT_CLRALARM.Text = string.Format("C : {0}", clearCnt);
        }

        void ClearPageAlarm()
        {
            if (_mutex.WaitOne(1000))
            {
                List<commandItem> cmdList = new List<commandItem>();
                cmdList.Add(new commandItem((int)CommandID.ALARM_CLEAR, "", 0, false));
                commandClient.sendCommandList(cmdList);
            }
            _mutex.ReleaseMutex();

        }



        void AckPageAlarm()
        {
            if (_mutex.WaitOne(1000))
            {

                int begin = _currentPage * _numOfPage;
                int end = begin + _numOfPage;

                if (_AlarmViewList.Count <= end)
                    end = _AlarmViewList.Count;

                alarmItem ai;
                List<commandItem> cmdList = new List<commandItem>();

                for (int i = begin; i < end; i++)
                {
                     
                    ai = _AlarmViewList[i]._alarmItem;
                    cmdList.Add(new commandItem((int)CommandID.ALARM_ACK, ai.AlarmTag, 0, false));
                }
                commandClient.sendCommandList(cmdList);
            }

            _mutex.ReleaseMutex();
        }


        private void BTN_ACK_Click(object sender, RoutedEventArgs e)
        {
            AckPageAlarm();
        }

        private void BTN_RESET_Click(object sender, RoutedEventArgs e)
        {
            //AckResetAlarm();
        }

        private void BTN_SLIENCE_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BTN_TEST_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BTN_DELCLR_Click(object sender, RoutedEventArgs e)
        {
            ClearPageAlarm();
        }

        private void BTN_SUPPRESS_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BTN_HISTORY_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BTN_PREPAGE_Click(object sender, RoutedEventArgs e)
        {
            GoPrevious();
        }

        private void BTN_NEXTPAGE_Click(object sender, RoutedEventArgs e)
        {
            GoNext();
        }

        private void BTN_SRT_UNACK_Click(object sender, RoutedEventArgs e)
        {
            _filterOpt = (int)FilterOpt.UnAck;
            ReadAlarmSource();
        }

        private void BTN_SRT_ACK_Click(object sender, RoutedEventArgs e)
        {
            _filterOpt = (int)FilterOpt.Ack;
            ReadAlarmSource();
        }

        private void BTN_SRT_P1_Click(object sender, RoutedEventArgs e)
        {
            _filterOpt = (int)FilterOpt.Pri1;
            ReadAlarmSource();
        }

        private void BTN_SRT_P2_Click(object sender, RoutedEventArgs e)
        {
            _filterOpt = (int)FilterOpt.Pri2;
            ReadAlarmSource();
        }

        private void BTN_SRT_RING_Click(object sender, RoutedEventArgs e)
        {
            _filterOpt = (int)FilterOpt.Ringback;
            ReadAlarmSource();
        }

        private void BTN_SRT_CLR_Click(object sender, RoutedEventArgs e)
        {
            _filterOpt = (int)FilterOpt.Clear;
            ReadAlarmSource();
        }

        public void AddItem(alarmItem aitem)
        {
            _alarmBuf.Add(aitem);
        }


        public void AssignPointer()
        {
            ALBG[0] = ALBG01;
            ALBG[1] = ALBG02;
            ALBG[2] = ALBG03;
            ALBG[3] = ALBG04;
            ALBG[4] = ALBG05;
            ALBG[5] = ALBG06;
            ALBG[6] = ALBG07;
            ALBG[7] = ALBG08;
            ALBG[8] = ALBG09;
            ALBG[9] = ALBG10;
            ALBG[10] = ALBG11;
            ALBG[11] = ALBG12;
            ALBG[12] = ALBG13;
            ALBG[13] = ALBG14;
            ALBG[14] = ALBG15;
            ALBG[15] = ALBG16;
            ALBG[16] = ALBG17;
            ALBG[17] = ALBG18;
            ALBG[18] = ALBG19;
            ALBG[19] = ALBG20;
            ALBG[20] = ALBG21;
            ALBG[21] = ALBG22;
            ALBG[22] = ALBG23;
            ALBG[23] = ALBG24;
            ALBG[24] = ALBG25;

            DSC[0] = DSC01;
            DSC[1] = DSC02;
            DSC[2] = DSC03;
            DSC[3] = DSC04;
            DSC[4] = DSC05;
            DSC[5] = DSC06;
            DSC[6] = DSC07;
            DSC[7] = DSC08;
            DSC[8] = DSC09;
            DSC[9] = DSC10;
            DSC[10] = DSC11;
            DSC[11] = DSC12;
            DSC[12] = DSC13;
            DSC[13] = DSC14;
            DSC[14] = DSC15;
            DSC[15] = DSC16;
            DSC[16] = DSC17;
            DSC[17] = DSC18;
            DSC[18] = DSC19;
            DSC[19] = DSC20;
            DSC[20] = DSC21;
            DSC[21] = DSC22;
            DSC[22] = DSC23;
            DSC[23] = DSC24;
            DSC[24] = DSC25;

            STS[0] = ST01;
            STS[1] = ST02;
            STS[2] = ST03;
            STS[3] = ST04;
            STS[4] = ST05;
            STS[5] = ST06;
            STS[6] = ST07;
            STS[7] = ST08;
            STS[8] = ST09;
            STS[9] = ST10;
            STS[10] = ST11;
            STS[11] = ST12;
            STS[12] = ST13;
            STS[13] = ST14;
            STS[14] = ST15;
            STS[15] = ST16;
            STS[16] = ST17;
            STS[17] = ST18;
            STS[18] = ST19;
            STS[19] = ST20;
            STS[20] = ST21;
            STS[21] = ST22;
            STS[22] = ST23;
            STS[23] = ST24;
            STS[24] = ST25;

            PRI[0] = PRI01;
            PRI[1] = PRI02;
            PRI[2] = PRI03;
            PRI[3] = PRI04;
            PRI[4] = PRI05;
            PRI[5] = PRI06;
            PRI[6] = PRI07;
            PRI[7] = PRI08;
            PRI[8] = PRI09;
            PRI[9] = PRI10;
            PRI[10] = PRI11;
            PRI[11] = PRI12;
            PRI[12] = PRI13;
            PRI[13] = PRI14;
            PRI[14] = PRI15;
            PRI[15] = PRI16;
            PRI[16] = PRI17;
            PRI[17] = PRI18;
            PRI[18] = PRI19;
            PRI[19] = PRI20;
            PRI[20] = PRI21;
            PRI[21] = PRI22;
            PRI[22] = PRI23;
            PRI[23] = PRI24;
            PRI[24] = PRI25;


            AID[0] = ALID01;
            AID[1] = ALID02;
            AID[2] = ALID03;
            AID[3] = ALID04;
            AID[4] = ALID05;
            AID[5] = ALID06;
            AID[6] = ALID07;
            AID[7] = ALID08;
            AID[8] = ALID09;
            AID[9] = ALID10;
            AID[10] = ALID11;
            AID[11] = ALID12;
            AID[12] = ALID13;
            AID[13] = ALID14;
            AID[14] = ALID15;
            AID[15] = ALID16;
            AID[16] = ALID17;
            AID[17] = ALID18;
            AID[18] = ALID19;
            AID[19] = ALID20;
            AID[20] = ALID21;
            AID[21] = ALID22;
            AID[22] = ALID23;
            AID[23] = ALID24;
            AID[24] = ALID25;

            VAL[0] = VAL01;
            VAL[1] = VAL02;
            VAL[2] = VAL03;
            VAL[3] = VAL04;
            VAL[4] = VAL05;
            VAL[5] = VAL06;
            VAL[6] = VAL07;
            VAL[7] = VAL08;
            VAL[8] = VAL09;
            VAL[9] = VAL10;
            VAL[10] = VAL11;
            VAL[11] = VAL12;
            VAL[12] = VAL13;
            VAL[13] = VAL14;
            VAL[14] = VAL15;
            VAL[15] = VAL16;
            VAL[16] = VAL17;
            VAL[17] = VAL18;
            VAL[18] = VAL19;
            VAL[19] = VAL20;
            VAL[20] = VAL21;
            VAL[21] = VAL22;
            VAL[22] = VAL23;
            VAL[23] = VAL24;
            VAL[24] = VAL25;

            SET[0] = SET01;
            SET[1] = SET02;
            SET[2] = SET03;
            SET[3] = SET04;
            SET[4] = SET05;
            SET[5] = SET06;
            SET[6] = SET07;
            SET[7] = SET08;
            SET[8] = SET09;
            SET[9] = SET10;
            SET[10] = SET11;
            SET[11] = SET12;
            SET[12] = SET13;
            SET[13] = SET14;
            SET[14] = SET15;
            SET[15] = SET16;
            SET[16] = SET17;
            SET[17] = SET18;
            SET[18] = SET19;
            SET[19] = SET20;
            SET[20] = SET21;
            SET[21] = SET22;
            SET[22] = SET23;
            SET[23] = SET24;
            SET[24] = SET25;

            UNT[0] = UNT01;
            UNT[1] = UNT02;
            UNT[2] = UNT03;
            UNT[3] = UNT04;
            UNT[4] = UNT05;
            UNT[5] = UNT06;
            UNT[6] = UNT07;
            UNT[7] = UNT08;
            UNT[8] = UNT09;
            UNT[9] = UNT10;
            UNT[10] = UNT11;
            UNT[11] = UNT12;
            UNT[12] = UNT13;
            UNT[13] = UNT14;
            UNT[14] = UNT15;
            UNT[15] = UNT16;
            UNT[16] = UNT17;
            UNT[17] = UNT18;
            UNT[18] = UNT19;
            UNT[19] = UNT20;
            UNT[20] = UNT21;
            UNT[21] = UNT22;
            UNT[22] = UNT23;
            UNT[23] = UNT24;
            UNT[24] = UNT25;

            TME[0] = TME01;
            TME[1] = TME02;
            TME[2] = TME03;
            TME[3] = TME04;
            TME[4] = TME05;
            TME[5] = TME06;
            TME[6] = TME07;
            TME[7] = TME08;
            TME[8] = TME09;
            TME[9] = TME10;
            TME[10] = TME11;
            TME[11] = TME12;
            TME[12] = TME13;
            TME[13] = TME14;
            TME[14] = TME15;
            TME[15] = TME16;
            TME[16] = TME17;
            TME[17] = TME18;
            TME[18] = TME19;
            TME[19] = TME20;
            TME[20] = TME21;
            TME[21] = TME22;
            TME[22] = TME23;
            TME[23] = TME24;
            TME[24] = TME25;

        }

    }


    /// <summary>
    /// OWS 각각에서 관리되는 경보리스트 아이템
    /// </summary>
    public class AlarmViewItem
    {

        bool _isFirstOut = false;

        public alarmItem _alarmItem = null;
        DateTime _time = DateTime.Now;

        private void ColorCoding()
        {
            if (_isFirstOut)
            {
                TextColor = Color.FromArgb(255, 0, 0, 0);
                FillColor = Color.FromArgb(255, 255, 0, 0);

                BlinkTextColor = Color.FromArgb(255, 0, 0, 0);
                BlinkFillColor = Color.FromArgb(255, 255, 255, 255);
            }
            else if (_alarmItem.AlarmPriority == 1)
            {
                TextColor = Color.FromArgb(255, 0, 0, 0);
                FillColor = Color.FromArgb(255, 255, 0, 0);

                BlinkTextColor = Color.FromArgb(255, 0, 0, 0);
                BlinkFillColor = Color.FromArgb(255, 255, 255, 255);
            }
            else if (_alarmItem.AlarmPriority == 2)
            {
                TextColor = Color.FromArgb(255, 0, 0, 0);
                FillColor = Color.FromArgb(255, 255, 255, 0);

                BlinkTextColor = Color.FromArgb(255, 0, 0, 0);
                BlinkFillColor = Color.FromArgb(255, 255, 255, 255);
            }
            else
            {
                TextColor = Color.FromArgb(255, 0, 0, 0);
                FillColor = Color.FromArgb(255, 255, 255, 255);

                BlinkTextColor = Color.FromArgb(255, 0, 0, 0);
                BlinkFillColor = Color.FromArgb(255, 255, 255, 0);
            }

            if (_alarmItem.AlarmTranStatus == "R")
            {
                TextColor = Color.FromArgb(255, 0, 0, 0);
                FillColor = Color.FromArgb(255, 255, 0, 0);

                BlinkTextColor = Color.FromArgb(255, 0, 0, 0);
                BlinkFillColor = Color.FromArgb(255, 255, 255, 255);
            }

            if (_alarmItem.AlarmTranStatus == "C")
            {
                TextColor = Color.FromArgb(255, 0, 0, 0);
                FillColor = Color.FromArgb(255, 255, 255, 255);

                BlinkTextColor = Color.FromArgb(255, 0, 0, 0);
                BlinkFillColor = Color.FromArgb(255, 255, 255, 0);
            }

        }

        public bool IsBlink = false;
        public bool IsSlowBlink = false;
        public bool BlinkFlag = false;

        public Color TextColor = Color.FromArgb(255, 0, 0, 0);
        public Color FillColor = Color.FromArgb(255, 255, 255, 255);
        public Color BlinkTextColor = Color.FromRgb(255, 255, 0);
        public Color BlinkFillColor = Color.FromRgb(255, 0, 0);

        public AlarmViewItem(alarmItem lSrcAlarm)
        {
            _alarmItem = lSrcAlarm;

            if (_alarmItem.AlarmTranStatus == "N")
            {
                if (_alarmItem.AlarmPriority == 2)
                {
                    IsBlink = false;
                    IsSlowBlink = true;
                }
                else
                {
                    IsBlink = true;
                    IsSlowBlink = false;
                }
            }
            else if (_alarmItem.AlarmTranStatus == "R")
            {
                IsBlink = false;
                IsSlowBlink = true;
            }
            else
            {
                IsBlink = false;
                IsSlowBlink = false;
            }

            ColorCoding();
        }
    }
}


   
