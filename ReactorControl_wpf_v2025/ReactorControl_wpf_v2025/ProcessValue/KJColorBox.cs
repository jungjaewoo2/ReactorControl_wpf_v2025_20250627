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
    ///     <MyNamespace:KJColorBox/>
    ///
    /// </summary>
    public class KJColorBox : PDBControl
    {
        // Border 기반의 칼라 박스
        public Brush ResetColor
        {
            set
            {
                SetValue(ResetColorProperty, value);
            }
            get
            {
                return (Brush)GetValue(ResetColorProperty);
            }
        }
        public static DependencyProperty ResetColorProperty =
            DependencyProperty.Register("ResetColor", typeof(Brush), typeof(KJColorBox), new UIPropertyMetadata(null));



        public Brush SetColor
        {
            set
            {
                SetValue(SetColorProperty, value);
            }
            get
            {
                return (Brush)GetValue(SetColorProperty);
            }
        }
        public static DependencyProperty SetColorProperty =
            DependencyProperty.Register("SetColor", typeof(Brush), typeof(KJColorBox), new UIPropertyMetadata(null));


        public bool MoveHorizontal
        {
            set
            {
                SetValue(MoveHorizontalProperty, value);
            }
            get
            {
                return (bool)GetValue(MoveHorizontalProperty);
            }
        }
        public static DependencyProperty MoveHorizontalProperty =
            DependencyProperty.Register("MoveHorizontal", typeof(bool), typeof(KJColorBox), new UIPropertyMetadata(null));



        public double LeftMax
        {
            set
            {
                SetValue(LeftMaxProperty, value);
            }
            get
            {
                return (double)GetValue(LeftMaxProperty);
            }
        }
        public static DependencyProperty LeftMaxProperty =
            DependencyProperty.Register("LeftMax", typeof(double), typeof(KJColorBox), new UIPropertyMetadata(null));

        public double LeftPoisition
        {
            set
            {
                SetValue(LeftPoisitionProperty, value);
            }
            get
            {
                return (double)GetValue(LeftPoisitionProperty);
            }
        }
        public static DependencyProperty LeftPoisitionProperty =
            DependencyProperty.Register("LeftPoisition", typeof(double), typeof(KJColorBox), new UIPropertyMetadata(null));

        public double TopMax
        {
            set
            {
                SetValue(TopMaxProperty, value);
            }
            get
            {
                return (double)GetValue(TopMaxProperty);
            }
        }
        public static DependencyProperty TopMaxProperty =
            DependencyProperty.Register("TopMax", typeof(double), typeof(KJColorBox), new UIPropertyMetadata(null));


        public double TopPoisition
        {
            set
            {
                SetValue(TopPoisitionProperty, value);
            }
            get
            {
                return (double)GetValue(TopPoisitionProperty);
            }
        }
        public static DependencyProperty TopPoisitionProperty =
            DependencyProperty.Register("TopPoisition", typeof(double), typeof(KJColorBox), new UIPropertyMetadata(null));


        static KJColorBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KJColorBox), new FrameworkPropertyMetadata(typeof(KJColorBox)));
        }

        public KJColorBox()
        {
            SetColor = Brushes.Red;
            ResetColor = Brushes.White;
            //BorderBrush = SetColor;
            Background = SetColor;
            LeftPoisition = 0;
            TopPoisition = 0;
        }

        // Main Page에 의해 PDBControl.Update 가 호출되고, OnUpdate는 PDBControl.Update에 의해 호출됨.
        public override void OnUpdate()
        {
            // 입력된 PID의 공정값이 0보다 크면 셋, 작으면 리셋
            if (PValue > 0) Background = SetColor;
            else Background = ResetColor;

            if (MoveHorizontal)
            {
                try
                {
                    LeftPoisition = LeftMax * (PValue - EngLow) / (EngHigh - EngLow);
                }
                catch
                {
                    LeftPoisition = 0;
                }

            } else
            {
                try
                {
                    TopPoisition = TopMax * (PValue - EngLow) / (EngHigh - EngLow);
                }
                catch
                {
                    TopPoisition = 0;
                }

            }

         }

    }
}
