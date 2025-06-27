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

namespace ReactorControl.View
{
	/// <summary>
	/// UC_ProcessContrtol_Main.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class UC_PC : UserControl
    {

        //UC_ADS1 uC_ADS1 = new UC_ADS1();
        //UC_LRS1 uC_LSR1 = new UC_LRS1();

        public UC_PC()
		{
			InitializeComponent();
		}

		private void Btn_CLD_Click(object sender, RoutedEventArgs e)
		{

        }

		private void PumpObject_Click(object sender, MouseButtonEventArgs e)
		{

		}

		private void Btn_SLD_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Btn_PID_Click(object sender, RoutedEventArgs e)
		{

		}

        private void PC_Click(object sender, RoutedEventArgs e)
        {
			if (sender is Button button)
			{
				string tagName = button.Tag.ToString();
				Console.WriteLine("DEBUG:tagName={0}", tagName);

				if (tagName == "LRS")
				{
					this.Content = new UC_LRS1();
				}
				else if (tagName == "PWMS")
				{
					this.Content = new UC_PWMS1();
				}
			}

		}
    }
}
