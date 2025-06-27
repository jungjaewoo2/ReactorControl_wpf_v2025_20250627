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
using System.Windows.Shapes;

namespace ReactorControl.Popup
{
	/// <summary>
	/// Popup_pumpControl.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class Popup_pumpControl2 : Window
	{

        UserControl userControl;
        UserControl parentUserControl;

        public Popup_pumpControl2(UserControl userControl)
		//public Popup_pumpControl(UserControl userControl)
		{
			InitializeComponent();

            parentUserControl = userControl;


		}


		private void Btn_WindowClose_Click(object sender, RoutedEventArgs e)
		{

		}

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            // 서버로 RUN요청
            // ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------


            // 응답 후 처리
            // ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
            // Parent의 정보를 사용하여 CallBack
            //UserControl parentControl = this.Parent;

            Console.WriteLine($"parentName={parentUserControl.Name}");

        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
