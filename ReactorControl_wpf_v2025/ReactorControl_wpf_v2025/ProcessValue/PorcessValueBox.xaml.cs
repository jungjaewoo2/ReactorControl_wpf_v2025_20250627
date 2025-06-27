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
using System.Windows.Threading;

namespace ProcessValue
{
    /// <summary>
    /// UserControl1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ProcessValueBox : UserControl
    {
        DispatcherTimer timer = new DispatcherTimer();
        
        public void InitTimer()
        {
           // timer.Interval = TimeSpan.FromMilliseconds(200);    //시간간격 설정
           // timer.Tick += new EventHandler(timer_Tick);          //이벤트 추가
           // timer.Start();                                       //타이머 시작. 종료는 timer.Stop(); 으로 한다
        }

        double testValue = 0.0;

        private void timer_Tick(object sender, EventArgs e)
        {
            testValue = testValue + 0.01;
            Value = (float)testValue;//(float)Math.s.Sin(testValue /  ( 2 * Math.PI));
            try
            {
                if ((FormatStr==null) || (FormatStr.Trim() == ""))
                {
                    ValueText.Text = Value.ToString();
                }
                else
                {
                    ValueText.Text = Value.ToString(FormatStr);
                }
            } catch
            {
                ValueText.Text = "Invalid Format";

            }
        }


        //float value;
        /*
        public static DependencyProperty PIDProperty =
             DependencyProperty.Register("PID", typeof(string),
             typeof(string));

        public static DependencyProperty FormatProperty =
             DependencyProperty.Register("Format", typeof(string),
             typeof(string));

      /*  public static DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(float),
            typeof(float));
            */

        public string PID;
        /*{
            get { return (string)GetValue(PIDProperty); }
            set { SetValue(PIDProperty, PID); }
        }*/

        public string FormatStr;
        /*{
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, FormatStr); }
        }*/

        public float Value;
        //{
        //    get { return (float)GetValue(ValueProperty); }
          //  set { SetValue(ValueProperty, Value); }
        //}

        
        public ProcessValueBox()
        {
            InitializeComponent();
            InitTimer();
          
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timer = null;
        }

        
        private void UserControl_Unloaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Unloaded_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
