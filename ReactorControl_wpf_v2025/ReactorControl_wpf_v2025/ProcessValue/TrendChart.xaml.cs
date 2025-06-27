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
using LibHistorian;
using PDBLib;
using LibUtility;

namespace ProcessValue
{
    /// <summary>
    /// TrendChart.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 


    public partial class TrendChart : UserControl
    {
        public PDBManager _PDBManager = null;

        double baseX = 30.0;
        double baseY = 30.0;
     
        Series[] _dataSeries = new Series[4];
        HistorianClient historianClient = new HistorianClient();
        double chartWidth = 1050;
        double chartHeight = 500;
        Brush[] ChartColorBrush;

        DateTime chartStartTIme;
        DateTime chartEndTime;
        static int _MAXPointID = 4;

        public TextBlock[] PID = new TextBlock[_MAXPointID];
        public TextBlock[] DSC = new TextBlock[_MAXPointID];
        public TextBlock[] MIN = new TextBlock[_MAXPointID];
        public TextBlock[] MAX = new TextBlock[_MAXPointID];
        public TextBlock[] UNT = new TextBlock[_MAXPointID];
        public TextBlock[] CSR = new TextBlock[_MAXPointID];
        public CheckBox[] VIS = new CheckBox[_MAXPointID];

        public TrendChart()
        {
            InitializeComponent();
            ChartColorBrush = new Brush[4];
            ChartColorBrush[0] = Brushes.Red;
            ChartColorBrush[1] = Brushes.Green;
            ChartColorBrush[2] = Brushes.Magenta;
            ChartColorBrush[3] = Brushes.Blue;

            this.SizeChanged += OnSizeChanged;
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnLoaded;
            for (int i = 0; i < 4; i++)
            {
                _dataSeries[i] = null;
            }
            AssignPointer();


            DateTime now = DateTime.Now;
            TimeSpan m1min = new TimeSpan(2, 0, 0);
            DateTime end = now - m1min;

            chartStartTIme = end;
            chartEndTime = now;

        }

        public void AssignPointer()
        {
            PID[0] = PID01;
            PID[1] = PID02;
            PID[2] = PID03;
            PID[3] = PID04;

            DSC[0] = DSC01;
            DSC[1] = DSC02;
            DSC[2] = DSC03;
            DSC[3] = DSC04;

            MIN[0] = MIN01;
            MIN[1] = MIN02;
            MIN[2] = MIN03;
            MIN[3] = MIN04;

            MAX[0] = MAX01;
            MAX[1] = MAX02;
            MAX[2] = MAX03;
            MAX[3] = MAX04;

            UNT[0] = UNT01;
            UNT[1] = UNT02;
            UNT[2] = UNT03;
            UNT[3] = UNT04;

            CSR[0] = CSR01;
            CSR[1] = CSR02;
            CSR[2] = CSR03;
            CSR[3] = CSR04;

            VIS[0] = VIS01;
            VIS[1] = VIS02;
            VIS[2] = VIS03;
            VIS[3] = VIS04;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs args)
        {

            drawChart(_dataSeries);
            DrawYAxisText(_dataSeries);
            updateTable();
        }

        public void updateTable()
        {

            for (int i = 0; i < 4; i++)
            {
                if (_dataSeries[i] != null)
                {
                    PID[i].Text = _dataSeries[i].PID;
                    if ((_PDBManager != null))
                    {
                        long pidIndex = _PDBManager.getIndex(_dataSeries[i].PID);
                        if (pidIndex >= 0)
                        {
                            DSC[i].Text = _PDBManager.getDescStringfromPDB(pidIndex);
                            UNT[i].Text = _PDBManager.getUnitString(pidIndex);
                        }
                        MIN[i].Text =  DataConvertor.RealToString(_dataSeries[i].MinValue);
                        MAX[i].Text = DataConvertor.RealToString(_dataSeries[i].MaxValue);
                        VIS[i].Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    DSC[i].Text = "";
                    UNT[i].Text = "";
                    MIN[i].Text = "";
                    MAX[i].Text = "";
                    VIS[i].Visibility = Visibility.Hidden;

                }
            }
        }
          

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            Console.WriteLine("OnLoad");
        }

        private void OnUnLoaded(object sender, RoutedEventArgs args)
        {
            Console.WriteLine("OnUnloaded");
        }

        private Point convtCord(double x, double y, double minX, double maxX, double minY, double maxY, double tWidth, double tHeight)
        {
            Point pout = new Point();
            pout.X = (x - minX) / (maxX - minX) * tWidth;
            pout.Y = tHeight - ((y - minY) / (maxY - minY)) * tHeight;
            Console.WriteLine(y.ToString() + " " + pout.Y.ToString());
            return pout;
        }


        private void DrawText(Grid YaxisValaueTextGrid, int index, string[] valueText)
        {

            TextBlock txt1 = new TextBlock();
            txt1.Text = valueText[0];
            txt1.FontSize = 12;
            txt1.Foreground = ChartColorBrush[0];
            txt1.TextAlignment = TextAlignment.Right;
            Grid.SetColumn(txt1, 0);
            Grid.SetRow(txt1, 0);
            YaxisValaueTextGrid.Children.Add(txt1);

            txt1 = new TextBlock();
            txt1.Text = valueText[1];
            txt1.FontSize = 12;
            txt1.Foreground = ChartColorBrush[1];
            txt1.TextAlignment = TextAlignment.Right;
            Grid.SetColumn(txt1, 0);
            Grid.SetRow(txt1, 1);
            YaxisValaueTextGrid.Children.Add(txt1);

            txt1 = new TextBlock();
            txt1.Text = valueText[2];
            txt1.FontSize = 12;
            txt1.Foreground = ChartColorBrush[2];
            txt1.TextAlignment = TextAlignment.Right;
            Grid.SetColumn(txt1, 0);
            Grid.SetRow(txt1, 2);
            YaxisValaueTextGrid.Children.Add(txt1);

            txt1 = new TextBlock();
            txt1.Text = valueText[3];
            txt1.FontSize = 12;
            txt1.Foreground = ChartColorBrush[3];
            txt1.TextAlignment = TextAlignment.Right;
            Grid.SetColumn(txt1, 0);
            Grid.SetRow(txt1, 3);
            YaxisValaueTextGrid.Children.Add(txt1);

            Canvas.SetLeft(YaxisValaueTextGrid, baseX - 50);
            Canvas.SetTop(YaxisValaueTextGrid, baseY + chartHeight * 0.25 * index - (18 * index));

        }

        private void DrawYAxisText(Series[] series)
        {
            
            string[][] axValue = new string[5][];
            axValue[0] = new string[4];
            axValue[1] = new string[4];
            axValue[2] = new string[4];
            axValue[3] = new string[4];
            axValue[4] = new string[4];


            for (int i = 0; i < 5; i++)
            {
                Grid YaxisValaueTextGrid = new Grid();
                YaxisValaueTextGrid.Width = 40;
                YaxisValaueTextGrid.Height = 68;
                YaxisValaueTextGrid.ShowGridLines = false;
                RowDefinition rowDef1 = new RowDefinition();
                RowDefinition rowDef2 = new RowDefinition();
                RowDefinition rowDef3 = new RowDefinition();
                RowDefinition rowDef4 = new RowDefinition();
                YaxisValaueTextGrid.RowDefinitions.Add(rowDef1);
                YaxisValaueTextGrid.RowDefinitions.Add(rowDef2);
                YaxisValaueTextGrid.RowDefinitions.Add(rowDef3);
                YaxisValaueTextGrid.RowDefinitions.Add(rowDef4);


                if (series[0] != null) axValue[i][0] = DataConvertor.RealToString((series[0].chartMaxValue + (series[0].chartMinValue - series[0].chartMaxValue) * (double)i * 0.25));
                else axValue[i][0] = "";

                if (series[1] != null) axValue[i][1] = DataConvertor.RealToString((series[1].chartMaxValue + (series[1].chartMinValue - series[1].chartMaxValue) * (double)i * 0.25));
                else axValue[i][1] = "";

                if (series[2] != null) axValue[i][2] = DataConvertor.RealToString((series[2].chartMaxValue + (series[2].chartMinValue - series[2].chartMaxValue) * (double)i * 0.25));
                else axValue[i][2] = "";

                if (series[3] != null) axValue[i][3] = DataConvertor.RealToString((series[3].chartMaxValue + (series[3].chartMinValue - series[3].chartMaxValue) * (double)i * 0.25));
                else axValue[i][3] = "";

                DrawText(YaxisValaueTextGrid, i, axValue[i]);
                TrendCanvas.Children.Add(YaxisValaueTextGrid);
            }
        }



        private void drawSeries(int index, Series series)
        {
            if (series == null) return;
            if (series._dataSet.Count == 0) return;

            series.setChartRange();

            double maxY = series.chartMaxValue;
            double minY = series.chartMinValue;


            Path path = new Path();
            StreamGeometry geometry = new StreamGeometry();

            DateTime minTime = series.GetMinDateTime();
            DateTime maxTime = series.GetMaxDateTime();
            TimeSpan timeDiff = maxTime - minTime;

            // 시리즈에서 그래프를 생성한다.
            foreach (seriesPoint sp in series._dataSet)
            {
                TimeSpan tsp = sp._DateTime - minTime;
                sp.displayCoord = convtCord((double)tsp.Ticks, sp._Value, 0, (double)timeDiff.Ticks, minY, maxY, chartWidth, chartHeight);
            }

            path = new Path();
            path.Stroke = ChartColorBrush[index];
            path.StrokeThickness = 1;
            geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;
            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(new Point(baseX + series._dataSet[0].displayCoord.X, baseY + series._dataSet[0].displayCoord.Y), false, false);
                foreach (seriesPoint sp in series._dataSet)
                {
                    ctx.LineTo(new Point(baseX + sp.displayCoord.X, baseY + sp.displayCoord.Y), true, false);
                }
            }
            geometry.Freeze();
            path.Data = geometry;
            TrendCanvas.Children.Add(path);
        }

        // Type : 10 Sec, 1Min
        // Type : 30 Sec, 5Min
        // Type : 1  Min, 10Min
        // Type : 5 Min, 1 Hour
        // Type : 10 Min, 3 Hour
        // Type : 30 Min, 12 Hour
        // Type : 1 Hour, 24 Hour
        enum DisplayMode
        {
            T10S_1M,
            T30S_5M,
            T1M_10M,
            T5M_1H,
            T10M_3H,
            T30M_12H,
            T1H_24H
        }

        private void drawChart(Series[] series)
        {

            TrendCanvas.Children.Clear();

            // Chart 배경 박스를 그린다
            SolidColorBrush solidColor = new SolidColorBrush(Color.FromRgb(215, 215, 200));
            Rectangle rec = new Rectangle();
            rec.Width = chartWidth;
            rec.Height = chartHeight;
            rec.Fill = solidColor;
            rec.Stroke = Brushes.Black;
            Canvas.SetLeft(rec, baseX);
            Canvas.SetTop(rec, baseY);
            TrendCanvas.Children.Add(rec);

            // X축 시간 및 전체 그래프를 그린다.
            DateTime minTime = chartStartTIme; // series[0].GetMinDateTime();
            DateTime maxTime = chartEndTime; // series[0].GetMaxDateTime();

            TimeSpan timeDiff = maxTime - minTime;


            Path path = new Path();
            StreamGeometry geometry = new StreamGeometry();

            // 보조  X축을 그린다. //TODO:

            DisplayMode CDM;
            if ((timeDiff.TotalSeconds / 30.0) < 6) CDM = DisplayMode.T10S_1M;
            else if ((timeDiff.TotalSeconds / 30.0) < 10) CDM = DisplayMode.T30S_5M;  // 3분 이하
            else if ((timeDiff.TotalSeconds / 30.0) < 70) CDM = DisplayMode.T1M_10M;  // 5분 이하
            else if ((timeDiff.TotalSeconds / 30.0) < 300) CDM = DisplayMode.T5M_1H;  // 1시간 30분 이하
            else CDM = DisplayMode.T10M_3H;

            double axDiffSecons = 10.0;
            TimeSpan MainXaxisInterval = new TimeSpan(0, 0, 300); // 메인 축 간격 300초
            TimeSpan SubXaxisInterval = new TimeSpan(0, 0, 10); // 메인 축 간격 300초
            if (CDM == DisplayMode.T10S_1M) { SubXaxisInterval = new TimeSpan(0, 0, 10); MainXaxisInterval = new TimeSpan(0, 1, 0); }
            if (CDM == DisplayMode.T30S_5M) { SubXaxisInterval = new TimeSpan(0, 0, 30); MainXaxisInterval = new TimeSpan(0, 5, 0); }
            if (CDM == DisplayMode.T1M_10M) { SubXaxisInterval = new TimeSpan(0, 1, 0); MainXaxisInterval = new TimeSpan(0, 10, 0); }
            if (CDM == DisplayMode.T5M_1H) { SubXaxisInterval = new TimeSpan(0, 5, 0); MainXaxisInterval = new TimeSpan(1, 0, 0); }
            if (CDM == DisplayMode.T10M_3H) { SubXaxisInterval = new TimeSpan(0, 10, 0); MainXaxisInterval = new TimeSpan(3, 0, 0); }
            if (CDM == DisplayMode.T30M_12H) { SubXaxisInterval = new TimeSpan(0, 30, 0); MainXaxisInterval = new TimeSpan(12, 0, 0); }
            if (CDM == DisplayMode.T1H_24H) { SubXaxisInterval = new TimeSpan(1, 0, 0); MainXaxisInterval = new TimeSpan(24, 0, 0); }


            ////////////////////////////
            // 그래프의 보조선을 그린다. 
            DateTime firstIndicator;
            DateTime endIndicator;
            firstIndicator = new DateTime(
                                         minTime.Ticks +
                                         ((SubXaxisInterval.Ticks - 1) - (minTime.Ticks - 1) % SubXaxisInterval.Ticks)
                                       );
            endIndicator = new DateTime(
                                            maxTime.Ticks -
                                            (maxTime.Ticks) % (SubXaxisInterval.Ticks)
                                       );

            TimeSpan displayDiff = endIndicator - firstIndicator;
            TimeSpan axDiff = firstIndicator - minTime;

            int axCnt = (int)(displayDiff.TotalSeconds / SubXaxisInterval.TotalSeconds);
            for (int i = 0; i < axCnt + 1; i++)
            {
                DateTime currentAxisTime = new DateTime(firstIndicator.Ticks + SubXaxisInterval.Ticks * i);

                Line subGrid = new Line();
                DoubleCollection dc = new DoubleCollection();
                dc.Add(2.0);
                dc.Add(4.0);
                subGrid.StrokeDashArray = dc;

                Point cc = convtCord((double)currentAxisTime.Ticks, 1, (double)minTime.Ticks, (double)maxTime.Ticks, 1, 2, chartWidth, chartHeight);
                subGrid.X1 = baseX + cc.X;
                subGrid.Y1 = baseY + 0;
                subGrid.X2 = baseX + cc.X;
                subGrid.Y2 = baseY + chartHeight;
                subGrid.StrokeThickness = 0.5;
                subGrid.Stroke = Brushes.Gray;

                TrendCanvas.Children.Add(subGrid);
            }

            ////////////////////////////
            // 그래프의 X 메인축을 그린다.
            firstIndicator = new DateTime(
                                            minTime.Ticks +
                                            ((MainXaxisInterval.Ticks - 1) - (minTime.Ticks - 1) % MainXaxisInterval.Ticks)
                                          );
            endIndicator = new DateTime(
                                            maxTime.Ticks -
                                            (maxTime.Ticks) % (MainXaxisInterval.Ticks)
                                       );

            displayDiff = endIndicator - firstIndicator;
            axDiff = firstIndicator - minTime;
            
            axCnt = (int)(displayDiff.TotalSeconds / MainXaxisInterval.TotalSeconds);

            for (int i = 0; i < axCnt + 1; i++)
            {
                DateTime currentAxisTime = new DateTime(firstIndicator.Ticks + MainXaxisInterval.Ticks * i);

                Point cc = convtCord((double)currentAxisTime.Ticks, 1, (double)minTime.Ticks, (double)maxTime.Ticks, 1, 2, chartWidth, chartHeight);
                Line MainGrid = new Line();
                MainGrid.X1 = baseX + cc.X;
                MainGrid.Y1 = baseY + cc.Y;
                MainGrid.X2 = baseX + cc.X;
                MainGrid.Y2 = baseY + cc.Y + 10;
                MainGrid.StrokeThickness = 1.0;
                MainGrid.Stroke = Brushes.Black;
                TrendCanvas.Children.Add(MainGrid);

                TextBlock dateText = new TextBlock();
                dateText.Text = currentAxisTime.ToString("yyyy/MM/dd");
                dateText.Foreground = Brushes.Black;
                dateText.HorizontalAlignment = HorizontalAlignment.Center;
                dateText.FontSize = 10;
                Canvas.SetLeft(dateText, baseX + cc.X - 30);
                Canvas.SetTop(dateText, baseY + cc.Y + 10);
                TrendCanvas.Children.Add(dateText);

                TextBlock timeText = new TextBlock();
                timeText.Text = currentAxisTime.ToString("HH:mm:ss");
                timeText.Foreground = Brushes.Black;
                timeText.HorizontalAlignment = HorizontalAlignment.Center;
                timeText.FontSize = 12;
                Canvas.SetLeft(timeText, baseX + cc.X - 30);
                Canvas.SetTop(timeText, baseY + cc.Y + 25);
                TrendCanvas.Children.Add(timeText);

                if (i == axCnt)
                {
                    TimeSpan IntervalFromLastAxix = maxTime - currentAxisTime;
                    if (IntervalFromLastAxix.TotalSeconds > (MainXaxisInterval.TotalSeconds * 0.6))
                    {
                        cc = convtCord((double)maxTime.Ticks, 1, (double)minTime.Ticks, (double)maxTime.Ticks, 1, 2, chartWidth, chartHeight);

                        MainGrid = new Line();
                        MainGrid.X1 = baseX + cc.X;
                        MainGrid.Y1 = baseY + cc.Y;
                        MainGrid.X2 = baseX + cc.X;
                        MainGrid.Y2 = baseY + cc.Y + 10;
                        MainGrid.StrokeThickness = 1.0;
                        MainGrid.Stroke = Brushes.Black;
                        TrendCanvas.Children.Add(MainGrid);

                        dateText = new TextBlock();
                        dateText.Text = maxTime.ToString("yyyy/MM/dd");
                        dateText.Foreground = Brushes.Black;
                        dateText.HorizontalAlignment = HorizontalAlignment.Center;
                        Canvas.SetLeft(dateText, baseX + cc.X - 30);
                        Canvas.SetTop(dateText, baseY + cc.Y + 10);
                        TrendCanvas.Children.Add(dateText);

                        timeText = new TextBlock();
                        timeText.Text = maxTime.ToString("HH:mm:ss");
                        timeText.Foreground = Brushes.Black;
                        timeText.HorizontalAlignment = HorizontalAlignment.Center;
                        Canvas.SetLeft(timeText, baseX + cc.X - 30);
                        Canvas.SetTop(timeText, baseY + cc.Y + 25);
                        TrendCanvas.Children.Add(timeText);
                    }
                }
            }

            // Y축을 그린다.
            int nGrid = 3; // 최상단 및 최하단 제외한 화면 분할수
            for (int i = 0; i < nGrid; i++)
            {
                double dy = chartHeight / ((double)nGrid + 1.0);

                Line MainGrid = new Line();

                DoubleCollection dc = new DoubleCollection();
                dc.Add(2.0);
                dc.Add(4.0);
                MainGrid.StrokeDashArray = dc;

                MainGrid.X1 = baseX + 0;
                MainGrid.Y1 = baseY + dy * i + dy;
                MainGrid.X2 = baseX + chartWidth;
                MainGrid.Y2 = baseY + dy * i + dy;
                MainGrid.StrokeThickness = 0.5;
                MainGrid.Stroke = Brushes.Black;

                TrendCanvas.Children.Add(MainGrid);
            }

            // Chart 외곽 박스를 그린다
            Path myRed = new Path();
            myRed.Stroke = Brushes.Black;
            myRed.StrokeThickness = 1;
            RectangleGeometry myRectangleGeometry = new RectangleGeometry();
            myRectangleGeometry.Rect = new Rect(baseX, baseY, chartWidth, chartHeight);
            myRed.Data = myRectangleGeometry;
            TrendCanvas.Children.Add(myRed);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_PDBManager!=null)
            {
                TrendChartSignalSelect selectWindow = new TrendChartSignalSelect(ref _PDBManager);
                selectWindow.ShowDialog();
                if (selectWindow.isOk)
                {
                    string[] pointIDs = selectWindow.getSeletedPointID();
                    loadHistoian(pointIDs);

                }
            }
        }

        private async void loadHistoian(string[] pointIDs)
        {
            DateTime now = DateTime.Now;
            TimeSpan m1min = new TimeSpan(2, 0, 0);
            DateTime end = now - m1min;

            chartStartTIme = end;
            chartEndTime = now;

            for (int i = 0; i < 4; i++)
            {
                _dataSeries[i] = await historianClient.getHistorian(pointIDs[i], end, now, HistorianClient.DataGroup.MIN, HistorianClient.DataSelection.MEAN);
                if (_dataSeries[i] != null)
                {
                    PID[i].Text = _dataSeries[i].PID;
                    if (_dataSeries[i]._dataSet.Count == 0) _dataSeries[i] = null;
                } 
            }
            
            drawChart(_dataSeries);
            DrawYAxisText(_dataSeries);

            for (int i = 0; i < 4; i++)
            {

                drawSeries(i, _dataSeries[i]);
            }

            updateTable();
        }
    }
}
