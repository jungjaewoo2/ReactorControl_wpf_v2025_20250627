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
using LibUtility;

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
    ///     <MyNamespace:KJDigitalValue/>
    ///
    /// </summary>
    /// 
    public enum DataType
    {
        Defalut,
        Value,
        DigitalValue,
        Desciprtion,
        Unit,
        Instrument
    }

    public class KJValue : PDBControl
    {
        public static DependencyProperty KJ_ValueStringProperty =
          DependencyProperty.Register("KJ_ValueString", typeof(string), typeof(KJValue), new UIPropertyMetadata(null));

        public static DependencyProperty KJ_TypeProperty =
          DependencyProperty.Register("KJ_Type", typeof(DataType), typeof(KJValue), new UIPropertyMetadata(null));

        public string KJ_ValueString
        {
            set
            {
                SetValue(KJ_ValueStringProperty, value);
            }
            get
            {
                return (string)GetValue(KJ_ValueStringProperty);
            }
        }

        public DataType KJ_Type
        {
            set
            {
                SetValue(KJ_TypeProperty, value);
            }
            get
            {
                return (DataType)GetValue(KJ_TypeProperty);
            }
        }

        // Main Page에 의해 PDBControl.Update 가 호출되고, OnUpdate는 PDBControl.Update에 의해 호출됨.
        public override void OnUpdate()
        {

            if (KJ_Type == DataType.Desciprtion)
            {
                KJ_ValueString = Description;
            }
            else

            if (KJ_Type == DataType.DigitalValue)
            {
                KJ_ValueString = DigitalString;
            }
            else

            if (KJ_Type == DataType.Unit)
            {
                KJ_ValueString = Unit;
            } else
            if (KJ_Type == DataType.Value)
            {
                KJ_ValueString = RealString;

            }
            else
            {
                KJ_ValueString = ValueStr;
            }
        }


        public KJValue()
        {
            KJ_ValueString = "--";
        }

        static KJValue()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KJValue), new FrameworkPropertyMetadata(typeof(KJValue)));
        }
    }
}
