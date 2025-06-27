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
    /// XAML 파일에서 이 사용자 지정 컨트롤을 사용하려면 1a 또는 1b단계를 수행한 다음 2단계를 수행하십시오.
    ///
    /// 1a단계) 현재 프로젝트에 있는 XAML 파일에서 이 사용자 지정 컨트롤 사용.
    /// 이 XmlNamespace 특성을 사용할 마크업 파일의 루트 요소에 이 특성을 
    /// 추가합니다.
    ///
    ///     xmlns:MyNamespace="clr-namespace:ProcessValue"
    ///
    ///
    /// 1b단계) 다른 프로젝트에 있는 XAML 파일에서 이 사용자 지정 컨트롤 사용.
    /// 이 XmlNamespace 특성을 사용할 마크업 파일의 루트 요소에 이 특성을 
    /// 추가합니다.
    ///
    ///     xmlns:MyNamespace="clr-namespace:ProcessValue;assembly=ProcessValue"
    ///
    /// 또한 XAML 파일이 있는 프로젝트의 프로젝트 참조를 이 프로젝트에 추가하고
    /// 다시 빌드하여 컴파일 오류를 방지해야 합니다.
    ///
    ///     솔루션 탐색기에서 대상 프로젝트를 마우스 오른쪽 단추로 클릭하고
    ///     [참조 추가]->[프로젝트]를 차례로 클릭한 다음 이 프로젝트를 찾아서 선택합니다.
    ///
    ///
    /// 2단계)
    /// 계속 진행하여 XAML 파일에서 컨트롤을 사용합니다.
    ///
    ///     <MyNamespace:KJPageLink/>
    ///
    /// </summary>

    [TemplatePart(Name = "canvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "LinkText", Type = typeof(TextBlock))]
    public class KJPageLink : Control
    {
        static string defaultValue = "LINK";
        public static DependencyProperty linkTextProperty =
           DependencyProperty.Register("LinkText", typeof(string), typeof(KJPageLink), new PropertyMetadata(defaultValue, OnLinkTextChanged));
        public static DependencyProperty DirectionProperty =
           DependencyProperty.Register("Direction", typeof(bool), typeof(KJPageLink), new PropertyMetadata((bool)false, OnDirectionChanged));

        public string LinkText {
            set {
                SetValue(linkTextProperty, value);
            }
            get { return (string)GetValue(linkTextProperty); }
        }

        public bool Direction
        {
            set {
                SetValue(DirectionProperty, value);
            }
            get { return (bool)GetValue(DirectionProperty); }
        }

        private Canvas canvas;
        private TextBlock linkText;

        static KJPageLink()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KJPageLink), new FrameworkPropertyMetadata(typeof(KJPageLink)));
        }

      
        private static void OnLinkTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            KJPageLink kjPageLink = (KJPageLink)d;
            kjPageLink.LinkText = (string)e.NewValue;
        }

        private static void OnDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            KJPageLink kjPageLink = (KJPageLink)d;
            kjPageLink.Direction = (bool)e.NewValue;
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            canvas = GetTemplateChild("canvas") as Canvas;
            linkText = GetTemplateChild("LinkText") as TextBlock;
            linkText.Text = LinkText;
            draw();
        }

        public void setLinkText()
        {
            linkText = GetTemplateChild("LinkText") as TextBlock;
            linkText.Text = LinkText;
        }

        public void draw()
        {
            canvas.Children.Clear();
            PointCollection myPointCollection = new PointCollection();

            if (Direction)
            {
                myPointCollection.Add(new Point(2, 0));
                myPointCollection.Add(new Point(12, 0));
                myPointCollection.Add(new Point(10, 1));
                myPointCollection.Add(new Point(12, 2));
                myPointCollection.Add(new Point(2, 2));
                myPointCollection.Add(new Point(0, 1));
            }
            else
            {
                myPointCollection.Add(new Point(0, 0));
                myPointCollection.Add(new Point(10, 0));
                myPointCollection.Add(new Point(12, 1));
                myPointCollection.Add(new Point(10, 2));
                myPointCollection.Add(new Point(0, 2));
                myPointCollection.Add(new Point(2, 1));
            }
             
        
            Polygon myPolygon = new Polygon();
            myPolygon.Points = myPointCollection;
            myPolygon.Fill = Brushes.LightGray;
            myPolygon.Width = 120;
            myPolygon.Height = 30;
            myPolygon.Stretch = Stretch.Fill;
            myPolygon.Stroke = Brushes.Black;
            myPolygon.StrokeThickness = 1;

            

            Canvas.SetTop(myPolygon, 0.0);
            canvas.Children.Add(myPolygon);

        }
    }
}
