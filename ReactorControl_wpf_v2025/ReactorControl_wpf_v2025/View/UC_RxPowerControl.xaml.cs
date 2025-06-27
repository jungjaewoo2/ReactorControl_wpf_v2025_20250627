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
    /// UC_RxPowerControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UC_RxPowerControl : UserControl
    {
		/// <summary>
		/// SSR1 - 실린더 상/하 이동시 값
		/// </summary>
		public int SSR_one_value = 0;
		/// <summary>
		/// SSR1 - 실린더 상/하 이동시 값 - 최대
		/// </summary>
		public int SSR_one_valueMax = 100;





		public UC_RxPowerControl()
        {
            InitializeComponent();
        }

		private void SSR_one_btn_up_Click(object sender, RoutedEventArgs e)
		{

			//SSR_one_cylinder_up.top
			//Label label = Label(sender);
			//label.Margin.Top -= 10;

			// 위로 이동
			Thickness currentMargin = SSR_one_cylinder_up.Margin;
			currentMargin.Top = currentMargin.Top - 2;
			SSR_one_cylinder_up.Margin = currentMargin;

			// 위로 이동
			currentMargin = SSR_one_cylinder_middle_small.Margin;
			currentMargin.Top = currentMargin.Top - 2;
			SSR_one_cylinder_middle_small.Margin = currentMargin;


			// 위로 이동
			//currentMargin = SSR_one_cylinder_middle_large.Margin;
			//currentMargin.Top = currentMargin.Top - 2;
			//SSR_one_cylinder_middle_large.Margin = currentMargin;
			// 높이 늘림
			//double currentHeight = SSR_one_cylinder_middle_large.Height;
			//currentHeight -= 2.0;
			//SSR_one_cylinder_middle_large.Height = currentHeight;
		}

        private void SSR_one_btn_up_Click(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
