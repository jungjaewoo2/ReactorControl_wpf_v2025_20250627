﻿#pragma checksum "..\..\..\Popup\Popup_pumpControl2.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "63E9113A18E736D487071803166F95FCC14649F6C9E65C36B5D662BC31F4B0B6"
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using ReactorControl.Popup;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ReactorControl.Popup {
    
    
    /// <summary>
    /// Popup_pumpControl2
    /// </summary>
    public partial class Popup_pumpControl2 : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 39 "..\..\..\Popup\Popup_pumpControl2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbl_server_request;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\Popup\Popup_pumpControl2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbl_server_receive;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\Popup\Popup_pumpControl2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbl_server_process;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\Popup\Popup_pumpControl2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbl_server_result;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\Popup\Popup_pumpControl2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnRun;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\Popup\Popup_pumpControl2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnStop;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ReactorControl;component/popup/popup_pumpcontrol2.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Popup\Popup_pumpControl2.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 27 "..\..\..\Popup\Popup_pumpControl2.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Btn_WindowClose_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.lbl_server_request = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.lbl_server_receive = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.lbl_server_process = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.lbl_server_result = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.BtnRun = ((System.Windows.Controls.Button)(target));
            
            #line 49 "..\..\..\Popup\Popup_pumpControl2.xaml"
            this.BtnRun.Click += new System.Windows.RoutedEventHandler(this.BtnRun_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.BtnStop = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\..\Popup\Popup_pumpControl2.xaml"
            this.BtnStop.Click += new System.Windows.RoutedEventHandler(this.BtnStop_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

