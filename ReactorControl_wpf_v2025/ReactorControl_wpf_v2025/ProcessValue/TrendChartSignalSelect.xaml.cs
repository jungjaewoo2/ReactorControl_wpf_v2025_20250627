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
using PDBLib;
using System.Collections.ObjectModel;

namespace ProcessValue
{
    /// <summary>
    /// TrendChartSignalSelect.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TrendChartSignalSelect : Window
    {
        public bool isOk = false;
        PDBManager _pdbManager;
        public TrendChartSignalSelect(ref PDBManager pdbManager)
        {
            InitializeComponent();
            _pdbManager = pdbManager;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PIDLists pidlist = new PIDLists();
            if (_pdbManager != null)
            {
                long last = _pdbManager.get_PID_LastIndex();
                for (long i = 0; i < last; i++)
                {
                    string zz = _pdbManager.getPIDStringfromPDB(i);

                    if (zz != "")
                    {
                        string dsc = _pdbManager.getDescStringfromPDB(i);
                        pidlist.Add(new PIDListItem(zz, dsc));
                    }
                }
            }
            PIDListView.ItemsSource = pidlist;
        }

        private void PIDListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListView sen = (ListView)sender;
            PIDListItem pid = (PIDListItem)sen.SelectedValue;
            if (pid == null) return;
            if (PID01.Text == "-") PID01.Text = pid.PointID;
            else if (PID02.Text == "-") PID02.Text = pid.PointID;
            else if (PID03.Text == "-") PID03.Text = pid.PointID;
            else if (PID04.Text == "-") PID04.Text = pid.PointID;
        }

        public string[] getSeletedPointID()
        {
            string[] pointIDs = new string[4];
            for (int i=0;i<4;i++)
            {
                pointIDs[i] = string.Empty;
            }
            if (PID01.Text != "-") pointIDs[0] = PID01.Text;
            if (PID02.Text != "-") pointIDs[1] = PID02.Text;
            if (PID03.Text != "-") pointIDs[2] = PID03.Text;
            if (PID04.Text != "-") pointIDs[3] = PID04.Text;
            return pointIDs;
        }

        private void PID01_Clear(object sender, RoutedEventArgs e)
        {
            PID01.Text = string.Empty;
        }

        private void PID02_Clear(object sender, RoutedEventArgs e)
        {
            PID02.Text = string.Empty;
        }

        private void PID03_Clear(object sender, RoutedEventArgs e)
        {
            PID03.Text = string.Empty;
        }

        private void PID04_Clear(object sender, RoutedEventArgs e)
        {
            PID04.Text = string.Empty;
        }

        private void PID01_Select(object sender, RoutedEventArgs e)
        {
            PIDListItem pid = (PIDListItem)PIDListView.SelectedValue;
            if (pid == null) return;
            PID01.Text = pid.PointID;
        }

        private void PID02_Select(object sender, RoutedEventArgs e)
        {
            PIDListItem pid = (PIDListItem)PIDListView.SelectedValue;
            if (pid == null) return;
            PID02.Text = pid.PointID;
        }

        private void PID03_Select(object sender, RoutedEventArgs e)
        {
            PIDListItem pid = (PIDListItem)PIDListView.SelectedValue;
            if (pid == null) return;
            PID03.Text = pid.PointID;
        }

        private void PID04_Select(object sender, RoutedEventArgs e)
        {
            PIDListItem pid = (PIDListItem)PIDListView.SelectedValue;
            if (pid == null) return;
            PID04.Text = pid.PointID;
        }

        private void OnOK(object sender, RoutedEventArgs e)
        {
            isOk = true;
            Close();
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            isOk = false;
            Close();
        }
    }



    public class PIDLists : ObservableCollection<PIDListItem> { }

    public class PIDListItem 
    {
        public PIDListItem ( string pid, string description)
        {
            PointID = pid;
            Description = description;
        }
        public string PointID { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return PointID;
        }
    }
}
