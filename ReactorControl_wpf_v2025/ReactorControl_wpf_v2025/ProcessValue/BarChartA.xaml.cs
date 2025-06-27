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

namespace ProcessValue
{
    /// <summary>
    /// BarChartA.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BarChartA : UserControl
    {

       public float ValueA
       {
            get { return (float)GetValue(ValueAProperty); }
            set { SetValue(ValueAProperty, value); }
        }
        public float ValueB
        {
            get { return (float)GetValue(ValueBProperty); }
            set { SetValue(ValueBProperty, value); }
        }

        public float ValueC
        {
            get { return (float)GetValue(ValueCProperty); }
            set { SetValue(ValueCProperty, value); }
        }

        public float MINValue
        {
            get { return (float)GetValue(MinEngProperty); }
            set { SetValue(MinEngProperty, value); }
        }

        public float MAXValue
        {
            get { return (float)GetValue(MaxEngProperty); }
            set { SetValue(MaxEngProperty, value); }
        }

        
        public static  DependencyProperty ValueAProperty =
            DependencyProperty.Register("ValueA", typeof(float),typeof(BarChartA), new PropertyMetadata((float)30.0, OnValueChanged));

        public static  DependencyProperty ValueBProperty =
           DependencyProperty.Register("ValueB", typeof(float), typeof(BarChartA), new PropertyMetadata((float)20.0, OnValueChanged));
           
        public static  DependencyProperty ValueCProperty =
           DependencyProperty.Register("ValueC", typeof(float), typeof(BarChartA), new PropertyMetadata((float)50.0, OnValueChanged));
         
        public static  DependencyProperty MinEngProperty =
           DependencyProperty.Register("Min_Value", typeof(float), typeof(BarChartA), new PropertyMetadata((float)0.0, OnValueChanged));

        public static  DependencyProperty MaxEngProperty =
           DependencyProperty.Register("Max_Value", typeof(float), typeof(BarChartA), new PropertyMetadata((float)100.0, OnValueChanged));

     

        public BarChartA()
        {
            InitializeComponent();
            DependencyPropertyChangedEventArgs e = new DependencyPropertyChangedEventArgs();
            OnValueChanged(this, e);
        }


        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
            var barchadrtA = (BarChartA)d;

            double startValue = barchadrtA.MINValue;
            double EndValue = barchadrtA.MAXValue;
            double Value1 = barchadrtA.ValueA;
            double Value2 = barchadrtA.ValueB;
            double Value3 = barchadrtA.ValueC;

            // 위쪽 사각형을 그린다.
            double originX = 0.0;
            double originY = 0.0;

            double ValueHeight = 30;
            double Height = 200.0;
            double Width = 80.0;

            try
            {
                barchadrtA.CanV.Children.Clear();
                Path myRed = new Path();
                RectangleGeometry myRectangleGeometry = new RectangleGeometry();
                myRed.Stroke = Brushes.Black;
                myRed.StrokeThickness = 2;
                myRectangleGeometry.Rect = new Rect(originX, originY, Width, ValueHeight);
                myRed.Data = myRectangleGeometry;
                barchadrtA.CanV.Children.Add(myRed);

                // 위쪽 사각형의 값을 표시한다.  
                barchadrtA.Text(originX + 15, originY + 6, Value2.ToString("0.00"), 18, Color.FromRgb(0, 0, 0), barchadrtA.CanV);

                // BAR 1 사각형을 그린다.
                double leftMargin = 5;
                double RightMargin = 10;
                double leftPosition = originX + leftMargin;
                Rectangle rect = new Rectangle();
                rect.Width = Width / 3.0 - leftMargin * 2;
                rect.Height = Height * Value1 / (EndValue - startValue) + 1;
                rect.Fill = Brushes.LawnGreen;
                Canvas.SetLeft(rect, leftPosition);
                Canvas.SetTop(rect, ValueHeight + Height - rect.Height);
                barchadrtA.CanV.Children.Add(rect);

                // BAR 2 사각형을 그린다.
                leftPosition = originX + 5 + Width / 3.0;
                rect = new Rectangle();
                rect.Width = Width / 3.0 - leftMargin * 2;
                rect.Height = Height * Value2 / (EndValue - startValue);
                rect.Fill = Brushes.LawnGreen;
                Canvas.SetLeft(rect, leftPosition);
                Canvas.SetTop(rect, ValueHeight + Height - rect.Height);
                barchadrtA.CanV.Children.Add(rect);

                // BAR 3 사각형을 그린다.
                leftPosition = originX + 5 + Width * 2.0 / 3.0;
                rect = new Rectangle();
                rect.Width = Width / 3.0 - leftMargin * 2;
                rect.Height = Height * Value3 / (EndValue - startValue);
                rect.Fill = Brushes.DarkRed;
                Canvas.SetLeft(rect, leftPosition);
                Canvas.SetTop(rect, ValueHeight + Height - rect.Height);
                barchadrtA.CanV.Children.Add(rect);



                // 아래쪽 사각형을 그린다.
                myRed = new Path();
                myRectangleGeometry = new RectangleGeometry();
                myRed.Stroke = Brushes.Black;
                myRed.StrokeThickness = 2;
                myRectangleGeometry.Rect = new Rect(originX, originY + ValueHeight, Width, Height);
                myRed.Data = myRectangleGeometry;
                barchadrtA.CanV.Children.Add(myRed);



                // 안쪽 선을 그린다.
                Path myPath = new Path();
                myPath.Stroke = Brushes.Black;
                myPath.StrokeThickness = 1;
                StreamGeometry geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;
                using (StreamGeometryContext ctx = geometry.Open())
                {
                    ctx.BeginFigure(new Point(originX + Width / 3.0, ValueHeight + 5.0), true /* is filled */, true /* is closed */);
                    ctx.LineTo(new Point(originX + Width / 3, Height + ValueHeight - 5.0), true /* is stroked */, false /* is smooth join */);

                    ctx.BeginFigure(new Point(originX + Width * 2.0 / 3.0, ValueHeight + 5.0), true /* is filled */, true /* is closed */);
                    ctx.LineTo(new Point(originX + Width * 2.0 / 3.0, Height + ValueHeight - 5.0), true /* is stroked */, false /* is smooth join */);
                }
                geometry.Freeze();
                myPath.Data = geometry;
                barchadrtA.CanV.Children.Add(myPath);

                // Main Grid 를 그린다.

                double GridDiff = 50.0;
                double GirdLength = 5.0;
                myPath = new Path();
                myPath.Stroke = Brushes.Black;
                myPath.StrokeThickness = 1;
                geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;
                using (StreamGeometryContext ctx = geometry.Open())
                {
                    int num = (int)((EndValue - startValue) / GridDiff);
                    for (int i = 0; i <= num; i++)
                    {
                        ctx.BeginFigure(new Point(originX + Width + 5, (ValueHeight + Height) - i * (Height / num)), true /* is filled */, true /* is closed */);
                        ctx.LineTo(new Point(originX + Width + GirdLength + 5, (ValueHeight + Height) - i * (Height / num)), true /* is stroked */, false /* is smooth join */);
                        barchadrtA.Text(originX + Width + GirdLength + 12,
                            (ValueHeight + Height) - i * (Height / num) - 7,
                            (startValue + GridDiff * i).ToString("0"),
                            10,
                            Color.FromRgb(0, 0, 0), barchadrtA.CanV);
                    }
                }
                geometry.Freeze();
                myPath.Data = geometry;
                barchadrtA.CanV.Children.Add(myPath);

                // Small Grid 를 그린다.
                startValue = 0.0;
                EndValue = 150.0;
                GridDiff = 10.0;
                GirdLength = 4.0;
                myPath = new Path();
                myPath.Stroke = Brushes.Black;
                myPath.StrokeThickness = 0.5;
                geometry = new StreamGeometry();
                geometry.FillRule = FillRule.EvenOdd;

                using (StreamGeometryContext ctx = geometry.Open())
                {
                    int num = (int)((EndValue - startValue) / GridDiff);
                    for (int i = 0; i <= num; i++)
                    {
                        ctx.BeginFigure(new Point(originX + Width + 3, (ValueHeight + Height) - i * (Height / num)), true /* is filled */, true /* is closed */);
                        ctx.LineTo(new Point(originX + Width + GirdLength + 3, (ValueHeight + Height) - i * (Height / num)), true /* is stroked */, false /* is smooth join */);

                    }
                }
                geometry.Freeze();
                myPath.Data = geometry;
                barchadrtA.CanV.Children.Add(myPath);
            } catch
            {

            }

        }

        private void Text(double x, double y, string text, double fontSize, Color color, Canvas can)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Foreground = new SolidColorBrush(color);
            textBlock.FontSize = fontSize;
            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);
            can.Children.Add(textBlock);
        }

    }
}
