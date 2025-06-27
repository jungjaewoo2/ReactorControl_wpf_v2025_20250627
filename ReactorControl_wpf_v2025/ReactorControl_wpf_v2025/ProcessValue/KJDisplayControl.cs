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
    ///     <MyNamespace:KJDisplayControl/>
    ///
    /// </summary>
    /// 
  
   

    public class KJDisplayControl : PDBControl
    {

        public static DependencyProperty KJ_VALUEProperty =
            DependencyProperty.Register("KJ_VALUE", typeof(string), typeof(KJDisplayControl), new UIPropertyMetadata(null));
        public static DependencyProperty KJ_TAG_TypeProperty =
          DependencyProperty.Register("KJ_TAG_Type", typeof(string), typeof(KJDisplayControl), new UIPropertyMetadata(null));
        public static DependencyProperty KJ_TAG_NumberProperty =
           DependencyProperty.Register("KJ_TAG_Number", typeof(string), typeof(KJDisplayControl), new UIPropertyMetadata(null));
        public static DependencyProperty KJ_UNITProperty =
          DependencyProperty.Register("KJ_UNIT", typeof(string), typeof(KJDisplayControl), new UIPropertyMetadata(null));
  
        
        public string KJ_TAG_Type
        {
            set
            {
                SetValue(KJ_TAG_NumberProperty, value);
            }
            get
            {
                return (string)GetValue(KJ_TAG_NumberProperty);
            }

        }


        public string KJ_TAG_Number
        {
            set
            {
                SetValue(KJ_TAG_TypeProperty, value);
            }
            get
            {
                return (string)GetValue(KJ_TAG_TypeProperty);
            }
        }

        public string KJ_UNIT
        {
            set
            {
                SetValue(KJ_UNITProperty, value);
            }
            get
            {
                return (string)GetValue(KJ_UNITProperty);
            }
        }


        public string KJ_VALUE
        {
            set
            {
                SetValue(KJ_VALUEProperty, value);
            }
            get
            {
                return (string)GetValue(KJ_VALUEProperty);
            }
        }



        public override void OnUpdate()
        {
            KJ_VALUE  = PValue.ToString();
            KJ_UNIT = Unit;
   
        }



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

        }




        static KJDisplayControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KJDisplayControl), new FrameworkPropertyMetadata(typeof(KJDisplayControl)));
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
