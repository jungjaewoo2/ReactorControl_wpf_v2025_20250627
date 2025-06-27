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

namespace ProcessValue
{
    /// <summary>
    /// PIDInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PIDInfo : Window
    {
        public PIDInfo()
        {
            InitializeComponent();
            
        }

        public void updateValue(string pid, string descrpition)
        {
            PID.Text = pid;
            Descpriton.Text = descrpition;
        }

       
    }
}
