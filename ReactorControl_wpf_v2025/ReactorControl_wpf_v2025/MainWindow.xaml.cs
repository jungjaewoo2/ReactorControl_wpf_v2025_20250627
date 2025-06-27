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
using ReactorControl.View;

using ProcessValue;
using ReactorControl.Popup;

namespace ReactorControl
{

	//public interface IParentControl
	//{
	//	void POPUP_CALL_BACK(string _button_name, string _value);
	//}


	/// <summary>
	/// MainWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class MainWindow : Window
    {



		String CRLF = Environment.NewLine;

        public MainWindow()
        {
            InitializeComponent();

			// 전체화면으로 전환
			//this.WindowState = WindowState.Maximized;

			// 1920,1080이지만 전체 화면이 아니기 때문에 임시로 1930으로 변경 -> 하지만 2K이상 모니터에서는 정상으로 표시 되지만 1920x1080모니터는???
			this.Width = 1930;
			this.Height = 1080;

			// 나머지 메뉴는 텍스트 표시
			ContentsArea.Content = new TextBlock
			{
				Text = "화면 크기: 전체=1920 x 1080 기준, ",
				FontSize = 24,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			};


			Console.WriteLine("하단 영역: {0} x {1}", ContentsArea.Width, ContentsArea.Height);


		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				this.DragMove();
			}
		}


		public UC_PC uC_PC = new UC_PC();
        public UC_LRS1 uC_LRS1 = new UC_LRS1();
        public UC_LRS2 uC_LRS2 = new UC_LRS2();
        public UC_LRS3 uC_LRS3 = new UC_LRS3();



        private void Button_Click(object sender, RoutedEventArgs e)
        {

			if (sender is Button button)
            {
                string menuNumber = button.Tag.ToString();
				Console.WriteLine("DEBUG:menuNumber={0}", menuNumber);


				if (menuNumber == "Popup1")
				{



				}
				/*
				if (_PDBManager != null)
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
				*/


				if (menuNumber == "1")
                {
					// 나머지 메뉴는 텍스트 표시
					ContentsArea.Content = new TextBlock
					{
						Text = $"하단메뉴{menuNumber}",
						FontSize = 24,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center
					};
					// 메뉴1 클릭 시 RxPowerControl 표시
					//ContentsArea.Content = new UC_RxPowerControl();
                }
                else
                {
					// 나머지 메뉴는 텍스트 표시
					ContentsArea.Content = new TextBlock
                    {
                        Text = $"하단메뉴{menuNumber} ",
                        FontSize = 24,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                }
            }
        }


        private void MMenu1_Click(object sender, RoutedEventArgs e)
        {

			String msg1 = String.Format("모니터 해상도-1 = {0} x {1}", this.ActualWidth, this.ActualHeight);
			String msg2 = String.Format("모니터 해상도-2 = {0} x {1}", SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);
			String msg3 = String.Format("실제 표현 영역 = {0} x {1}", SystemParameters.WorkArea.Width, SystemParameters.WorkArea.Height);
			String msg4 = String.Format("컨텐츠 영역 = {0} x {1}", ContentsArea.ActualWidth,  ContentsArea.ActualHeight);

			string menuName = "_BLANK_";

			if (sender.GetType() == typeof(Button))
			{

				Button button = sender as Button;

				menuName = button.Tag.ToString();
			}


			//if (sender is ProcessValue.MenuMainButton processValue) {
			//             menuName = processValue.Tag.ToString();
			//         }

			if (menuName == "PowerControl")
			{
				ContentsArea.Content = new UC_RxPowerControl();
			}
			else if (menuName == "ProcessControl")
			{
				// 1회만 동작함
				//ContentsArea.Content = uC_PC;       // new UC_PC();
				ContentsArea.Content = new UC_PC();
			}
			//else if (menuName == "PowerControl3")
			//{
			//	ContentsArea.Content = new UC_ADS1();
			//}
			else
			{
				// 나머지 메뉴는 텍스트 표시
				ContentsArea.Content = new TextBlock
				{
					Text = $"하단메뉴{menuName} " + msg1 + CRLF + msg2 + CRLF + msg3 + CRLF + msg4,
					FontSize = 24,
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center
				};
			}



			// sender.ToString(); -> "ProcessValue.MenuMainButton"


			Console.WriteLine("MMenu1_Click, menuName={0}", menuName);
			//openPage(0);
		}

		private void OpenPopupButton_Click(object sender, RoutedEventArgs e)
		{

			// case2. 미리 작성한 xaml사용
			//Popup_ProcessControl1 popup = new Popup_ProcessControl1(UC_ProcessControl);
			//popup.ShowDialog();




			// case1. 새로운 popup window 생성 -> 정상동작
			/*
			Window popup = new Window
			{
				Title = "팝업 창",
				Width = 300,
				Height = 200,
				WindowStartupLocation = WindowStartupLocation.CenterScreen
			};

			// 팝업 창에 콘텐츠 추가
			StackPanel panel = new StackPanel();
			panel.Children.Add(new TextBlock { Text = "팝업 창입니다!", Margin = new Thickness(10) });
			popup.Content = panel;

			//popup.Show(); // 비모달 팝업
			popup.ShowDialog(); // 모달 팝업
			*/


		}
	}  // end of main class
}
