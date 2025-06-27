using ReactorControl.Classes;
using ReactorControl.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ReactorControl.Popup
{
	/// <summary>
	/// Popup_ProcessControl1.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class Popup_pumpControl : Window
	{

		/*
			// TIP: ? 의 의미는 Null 조건부 연산자로써 객체가 null이 아닌경우에만 호출됨
			_parentControl?.POPUP_CALL_BACK("RUN", "RUN");

		*/




		// 단계별 색상(대기)
		SolidColorBrush solidColorBrush_wait = new SolidColorBrush(Color.FromRgb(0xAA, 0xAA, 0xAA));
		// 단계별 색상(요청)
		//SolidColorBrush solidColorBrush_request = new SolidColorBrush(Color.FromRgb(0x30, 0x30, 0x30));
		SolidColorBrush solidColorBrush_request = new SolidColorBrush(Color.FromRgb(0x30, 0x30, 0xFF));
		// 단계별 색상(실패)
		SolidColorBrush solidColorBrush_fail = new SolidColorBrush(Color.FromRgb(0xFF, 0x30, 0x30));

		String requestCommand = "_BLANK_";
		String requestCommand_param1 = "_BLANK_";
		String requestCommand_param2 = "_BLANK_";
		String requestCommandStep = "wait";

		/// <summary>
		/// 서버와 통신 상태: 0=대기중, 1=요청, 2=응답, 3=결과
		/// </summary>
		int serverStatus = 0;

		/// <summary>
		/// 임시-통신 상태를 화면에 보여주기 위한 타이머
		/// </summary>
		DispatcherTimer timerStatusChange;

		ServerprocessSteps serverProcessSteps = new ServerprocessSteps();


		//public UC_ADS1 _UC_ProcessControl;

		UserControl userControl;
		UserControl parentUserControl;

		private readonly dynamic _parentControl; // dynamic으로 부모 참조

		public Popup_pumpControl(dynamic parentControl)
		{
			InitializeComponent();

			_parentControl = parentControl;
			

			/*
			parentUserControl = _userControl;

	        Console.WriteLine($"DEBUG:parent={parentUserControl}");
            Console.WriteLine($"DEBUG:parentName={parentUserControl.Name}");

            UserControl pc = this.parentUserControl;
            Console.WriteLine($"DEBUG:pc Name={pc.Name}");

			String pucName = parentUserControl.ToString();
            Console.WriteLine($"DEBUG:pucName={pucName}");

            if (pucName == "ReactorControl.View.UC_LRS1")
			{
                //UC_LRS1 uC_LRS1 = new UC_LRS1();
			}
			*/


            // Content RUN Style<설정 안 함> System.Windows.Controls.ContentControl
			




            //_UC_ProcessControl = uC_ProcessControl;

            timerStatusChange = new DispatcherTimer();
			timerStatusChange.Interval = TimeSpan.FromMilliseconds(500); // 500ms
			timerStatusChange.Tick += timer_status_change_Tick;
			timerStatusChange.Stop();
		}

		private void timer_status_change_Tick(object sender, EventArgs e)
		{
			serverStatus++;
			Console.WriteLine("server_status={0}", serverStatus);
			if (serverStatus > 4)
			{
				serverStatus = 0;
				requestCommandStep = "wait";
				timerStatusChange.Stop();


				//requestCommand = "VALVE_CONTROL";
				//requestCommand_param1 = "PP-001";
				//requestCommand_param2 = "CLOSE";
				//requestCommandStep = "wait";
				//ValveControl("PP-001", "close", ref error_code);
				//Console.WriteLine("ref error_code=", error_code);

				//requestCommand = "VALVE_CONTROL";
				//requestCommand_param1 = "PP-001";
				//requestCommand_param2 = "OPEN";

				//if (requestCommand_param2 == "OPEN")
				//{
				//	// 적용 성공 !!!
				//	_UC_ProcessControl.UpdatePumpImage("Images/obj_pump_on.png");

				//	MessageBox.Show("밸브 [열기] 명령 실행됨.", "정보", MessageBoxButton.OK, MessageBoxImage.Information);

				//	// 다른 xaml의 화면의 내용을 갱신하려고 했지만 실패 -> 새롭게 인스턴스를 만들면 변경 불가능 -> 당연한 예기
				//	//UC_ProcessControl uC_ProcessControl = new UC_ProcessControl();
				//	//uC_ProcessControl.ValueControl("PP-001", "open");

				//	//UC_ProcessControl.PUBLIC_pp001.Source = "";
				//	//UC_ProcessControl.PUBLIC_pp001.SetPumpImage("Images/obj_pump_on.png");

				//}
				//else
				//{
				//	// 적용 성공 !!!
				//	_UC_ProcessControl.UpdatePumpImage("Images/obj_pump_off.png");
				//	MessageBox.Show("밸브 [닫기] 명령 실행됨.", "정보", MessageBoxButton.OK, MessageBoxImage.Information);
				//}

				// POPUP_CALL_BACK(string _button_name, string _value)
				if (requestCommand_param2 == "OPEN")
				{
					_parentControl?.POPUP_CALL_BACK("RUN", "RUN");
				}
				if (requestCommand_param2 == "CLOSE")
				{
					_parentControl?.POPUP_CALL_BACK("RUN", "STOP");
				}


			}
			if (serverStatus == 1)
			{
				requestCommandStep = "request";
				lbl_server_request.Foreground = solidColorBrush_request;
				//lbl_server_request.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => { }));
			}
			if (serverStatus == 2)
			{
				requestCommandStep = "request";
				lbl_server_receive.Foreground = solidColorBrush_request;
				//lbl_server_receive.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => { }));
			}
			if (serverStatus == 3)
			{
				requestCommandStep = "process";
				lbl_server_process.Foreground = solidColorBrush_request;
				//lbl_server_result.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => { }));
			}
			if (serverStatus == 4)
			{
				requestCommandStep = "result";
				lbl_server_result.Foreground = solidColorBrush_request;
				//lbl_server_result.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => { }));
			}


			// txtTime.Text = DateTime.Now.ToString("HH:mm:ss");
		}


		private void Close_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
        }


		/// <summary>
		/// 
		/// </summary>
		/// <param name="_device_name">창치명(PP-001...)</param>
		/// <param name="_value">open:열기, close:닫기</param>
		/// <param name="_error_code"></param>
		/// <returns></returns>
		private Boolean ValveControl(String _device_name, String _value, ref int _error_code)
		{

			if (_value == "open")
			{
				_error_code = 0;

				serverStatus = 0;
				timerStatusChange.Start();


				// 임시로 서버와 통신 하는 것 처럼 표시
				/*
				lbl_server_request.Foreground = solidColorBrush_request;
				lbl_server_request.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => { }));
				System.Threading.Thread.Sleep(500);
				lbl_server_process.Foreground = solidColorBrush_request;
				lbl_server_process.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => { }));
				System.Threading.Thread.Sleep(500);
				lbl_server_receive.Foreground = solidColorBrush_request;
				lbl_server_receive.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => { }));
				System.Threading.Thread.Sleep(500);
				lbl_server_result.Foreground = solidColorBrush_fail;
				lbl_server_result.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => { }));
				*/
			}
			else if (_value == "close")
			{
				_error_code = -900;
				serverStatus = 0;
				timerStatusChange.Start();

				// 임시로 서버와 통신 하는 것 처럼 표시
				/*
				lbl_server_request.Foreground = solidColorBrush_request;
				System.Threading.Thread.Sleep(500);
				lbl_server_process.Foreground = solidColorBrush_request;
				System.Threading.Thread.Sleep(500);
				lbl_server_receive.Foreground = solidColorBrush_request;
				System.Threading.Thread.Sleep(500);
				lbl_server_result.Foreground = solidColorBrush_fail;
				*/
			}

			//MessageBox.Show("밸브 [열기] 명령 실행됨.", "정보", MessageBoxButton.OK, MessageBoxImage.Information);

			return true;
		}

		private void BtnOpen_Click(object sender, RoutedEventArgs e)
		{

			//_UC_ProcessControl.PUBLIC_pp001.Source = .SetPumpImage("Images/obj_pump_on.png");
			// 적용 성공 !!!
			//_UC_ProcessControl.UpdatePumpImage("Images/obj_pump_on.png");
			//return;

			MessageBoxResult result = MessageBox.Show("밸브를 [열기]로 요청 하시겠습니까?", "밸브 제어", MessageBoxButton.YesNo, MessageBoxImage.Question);

			int error_code = 0;
			if (result == MessageBoxResult.Yes)
			{
				/*
				lbl_server_request.Foreground = lbl_server_process.Foreground = lbl_server_receive.Foreground = lbl_server_result.Foreground = solidColorBrush_wait;
				lbl_server_request.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => { }));
				lbl_server_process.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => { }));
				lbl_server_receive.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => { }));
				lbl_server_result.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => { }));
				*/

				requestCommand = "VALVE_CONTROL";
				requestCommand_param1 = "PP-001";
				requestCommand_param2 = "OPEN";
				requestCommandStep = "wait";

				lbl_server_request.Foreground = lbl_server_process.Foreground = lbl_server_receive.Foreground = lbl_server_result.Foreground = solidColorBrush_wait;

				ValveControl("PP-001", "open", ref error_code);
				Console.WriteLine("ref error_code=", error_code);
			}
		}

		private void BtnClose_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show("밸브를 [닫기]로 요청 하시겠습니까?", "밸브 제어", MessageBoxButton.YesNo, MessageBoxImage.Question);

			int error_code = 0;
			if (result == MessageBoxResult.Yes)
			{
				/*
				lbl_server_request.Foreground = lbl_server_process.Foreground = lbl_server_receive.Foreground = lbl_server_result.Foreground = solidColorBrush_wait;
				lbl_server_request.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => { }));
				lbl_server_process.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => { }));
				lbl_server_receive.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => { }));
				lbl_server_result.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => { }));
				*/
				requestCommand = "VALVE_CONTROL";
				requestCommand_param1 = "PP-001";
				requestCommand_param2 = "CLOSE";
				requestCommandStep = "wait";
				lbl_server_request.Foreground = lbl_server_process.Foreground = lbl_server_receive.Foreground = lbl_server_result.Foreground = solidColorBrush_wait;
				ValveControl("PP-001", "close", ref error_code);
				Console.WriteLine("ref error_code=", error_code);
			}
		}
	}  // end of main class
}
