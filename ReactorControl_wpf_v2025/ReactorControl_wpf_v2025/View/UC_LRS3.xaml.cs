using ReactorControl.Classes;
using ReactorControl.Popup;
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
using static ReactorControl.View.UC_LRS3;

namespace ReactorControl.View
{
	/// <summary>
	/// UC_LRS1.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class UC_LRS3 : UserControl, IParentControl
	{

		public PumpInfoDetailListClass pumpInfoDetailList = new PumpInfoDetailListClass();


		public interface IParentControl
		{
			void POPUP_CALL_BACK(string _button_name, string _value);
		}

		/// <summary>
		/// Image 태그 이미지 변경
		/// </summary>
		/// <param name="_button_name"></param>
		/// <param name="_value"></param>
		//public BitmapImage UpdatePumpImage(string imagePath)
		//{
		//	Console.WriteLine("UpdatePumpImage()-imagePath={0}", imagePath);
		//	var bitmap = new BitmapImage();
		//	try
		//	{
		//		bitmap.BeginInit();
		//		bitmap.UriSource = new Uri(imagePath, UriKind.Relative);
		//		bitmap.CacheOption = BitmapCacheOption.OnLoad; // 즉시 로드
		//		bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache; // 캐시 무시
		//		bitmap.EndInit();
		//		pp_001.Source = bitmap;
		//		return bitmap;
		//	}
		//	catch (Exception ex)
		//	{
		//		MessageBox.Show("이미지 로딩 실패: " + ex.Message);
		//		return bitmap;
		//	}
		//}

		public void POPUP_CALL_BACK(string _button_name, string _value)
		{
			MessageBox.Show($"UC_LRS1-Pupup 에서 받은 메시지: button={_button_name}, value={_value}");

			SolidColorBrush brush_run_bg = new SolidColorBrush(Color.FromRgb(0x0A, 0x91, 0x0A));
			SolidColorBrush brush_stop_bg = new SolidColorBrush(Color.FromRgb(0xFF, 0x00, 0x00));
			SolidColorBrush brush_fail_bg = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0xFF));

			// TODO: 상태에 따라 색상,텍스트 변경 -> 스타일에 정의 하여 사용 !!!!
			// 배경색만 바뀌고 텍스트 색상은 동일함
			if (_value == "RUN")
			{
				// 0A910A
				pump_pp_001.Background = brush_run_bg;      // new SolidColorBrush(Colors.Green);
				LBL_PP_001.Content = "RUN";
				//LBL_PP_001.Foreground = new SolidColorBrush(Color.FromRgb(0x0A, 0x91, 0x0A));
				// 0A910A, 00FF00

			}
			else if (_value == "STOP")
			{
				// FF0000
				pump_pp_001.Background = brush_stop_bg;     // new SolidColorBrush(Colors.Red);
				LBL_PP_001.Content = "STOP";
				//LBL_PP_001.Foreground = new SolidColorBrush(Colors.Yellow);
			}
			else if (_value == "FAIL")
			{
				// FF0000
				pump_pp_001.Background = brush_fail_bg;     // new SolidColorBrush(Colors.Blue);
				LBL_PP_001.Content = "FAIL";
				//LBL_PP_001.Foreground = new SolidColorBrush(Color.FromRgb(0x0A, 0x91, 0x0A));
			}


			//	_UC_ProcessControl.UpdatePumpImage("Images/obj_pump_off.png");
			//	MessageBox.Show("밸브 [닫기] 명령 실행됨.", "정보", MessageBoxButton.OK, MessageBoxImage.Information);


			/*
			Console.WriteLine("_objectName={0},_change image OFF", _objectName);
			BitmapImage bitmapImage = UpdatePumpImage("Images/obj_pump_off.png");
			Dispatcher.Invoke(() =>
			{
				pp_001.Source = bitmapImage;
				pp_001.InvalidateVisual(); // 수동으로 다시 그리기 요청
			});
			*/

		}


		public UC_LRS3()
		{
			InitializeComponent();

			//PumpInfoDetailClass pumpInfoDetailClass = new PumpInfoDetailClass();
			//pumpInfoDetailClass.Name = "PP-001";
			//pumpInfoDetailClass.Status = "RUN";
			//pumpInfoDetailClass.ChangeDatetime = DateTime.Now;

			//pumpInfoDetailList.LIST.Add(pumpInfoDetailClass);

			// 각 장치의 상태는 서버로 부터 값을 수신
			List<string> pumpOperateStatus = new List<string> { "RUN", "STOP", "FAIL" };
			List<string> valveOperateStatus = new List<string> { "OPEN", "CLOSE", "FAIL" };

			// Name, status(open,close,fail), receive date(2025.06.20 10:00:00)
			List<string> valveOperateStatusTags = new List<string> { "pp_001", "CLOSE", "FAIL" };
		}

		private void Btn_CLD_Click(object sender, RoutedEventArgs e)
		{

		}

		private void PumpObject_Click(object sender, MouseButtonEventArgs e)
		{
			// Popup3 ->

			//Label label = (Label)sender;
			//Label label = sender as Label;

			if (sender.GetType() == typeof(Label))
			{

				Popup_pumpControl popup_ProcessControl1 = new Popup_pumpControl(this);
				//popup_ProcessControl1.Owner = this;
				popup_ProcessControl1.ShowDialog();

				Label label = sender as Label;
				Console.WriteLine("DEBUG:Label Name={0}", label.Name);

				string tooltip = label.ToolTip?.ToString(); // "펌프 제어"
				Console.WriteLine($"ToolTip: {tooltip}");

				// 클릭한 UIElement(레이블 등)를 PlacementTarget으로 지정
				if (sender is UIElement target)
				{

					// 클릭한 object-즉 클릭한 위치에 표시 -> 성공
					/*
                    PumpStatusPopup.PlacementTarget = target;
                    PumpStatusPopup.PlacementTarget = WorkArea;               // 기준 대상을 윈도우나 UserControl로
                    //PumpStatusPopup.Placement = PlacementMode.Center;
                    PumpStatusPopup.IsOpen = true;
					*/

					/*
                    //PumpStatusPopup2.PlacementTarget = target;
                    PumpStatusPopup2.PlacementTarget = RootControl;               // 기준 대상을 윈도우나 UserControl로
                    PumpStatusPopup2.Placement = PlacementMode.Center;
                    //PumpStatusPopup2.LostFocus += PumpStatusPopup2_LostFocus;
                    PumpStatusPopup2.IsOpen = true;
                    //PumpStatusPopup2.Focus();
                    return;
					*/

					// 1) 팝업을 띄우고자 하는 대상을 화면 중앙 기준으로 설정
					//    this는 UserControl, Window.GetWindow(this)는 최상위 Window입니다.
					//var host = Window.GetWindow(this) ?? (UIElement)this;

					//PumpStatusPopup.PlacementTarget = WorkArea;               // 기준 대상을 윈도우나 UserControl로
					//PumpStatusPopup.Placement = PlacementMode.Center;     // 중앙 배치 모드
					//PumpStatusPopup.HorizontalOffset = 0;                 // 필요시 오프셋 조정
					//PumpStatusPopup.VerticalOffset = 0;

					//PumpStatusPopup.IsOpen = true;
				}

				// case2. 미리 작성한 xaml사용
				//Popup_pumpControl popup_PumpControl = new Popup_pumpControl(this);
				//popup_PumpControl.ShowDialog();

				//Popup_ProcessControl1 popup_ProcessControl1 = new Popup_ProcessControl1(this);
				//popup_ProcessControl1.ShowDialog();


			}

		}

		/// <summary>
		/// 포커스를 벗어날때 -> 하지만 동작 하지 않음
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PumpStatusPopup2_LostFocus(object sender, RoutedEventArgs e)
		{

			// StayOpen = "False"로 지정하면 창이 열리고 바로 닫힘 ㅠㅠ

			//throw new NotImplementedException();
			// 왜 Popup이 닫히지 않는거지??? -> 이벤트가 발생 하지 않음 -> 왜???
			//PumpStatusPopup2.IsOpen = false;
		}

		//      public void CALL_BACK_BUTTON_1(String _string1, int _value1)
		//{
		//	Console.WriteLine($"_string1={_string1}, _value1={_value1}");
		//}


		private void Btn_SLD_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Btn_PID_Click(object sender, RoutedEventArgs e)
		{

		}


		private void PumpObject_Click(object sender, RoutedEventArgs e)
		{
			// case2. 미리 작성한 xaml사용
			Popup_pumpControl2 popup = new Popup_pumpControl2(this);
			popup.ShowDialog();
		}

		/// <summary>
		/// 페이지이동-처음
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Btn_goFirstPage_Click(object sender, MouseButtonEventArgs e)
		{
			this.Content = new UC_LRS1();
		}

		/// <summary>
		/// 페이지이동-이전
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PBtn_goPrevPage__Click(object sender, MouseButtonEventArgs e)
		{
			this.Content = new UC_LRS2();
		}

		/// <summary>
		/// 페이지이동-다음
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Btn_goNextPage_Click(object sender, MouseButtonEventArgs e)
		{
			//this.Content = new UC_LRS2();
		}

		/// <summary>
		/// 페이지이동-마지막
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Btn_goEndPage_Click(object sender, MouseButtonEventArgs e)
		{
			//this.Content = new UC_LRS3();
		}

		private void btnRun_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnStop_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnClosePopup_Click(object sender, RoutedEventArgs e)
		{
			//PumpStatusPopup.StaysOpen = true;

			// Popup 닫기
			PumpStatusPopup.IsOpen = false;

		}
	}
}
