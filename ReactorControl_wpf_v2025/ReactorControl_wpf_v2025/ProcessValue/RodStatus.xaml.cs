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

namespace ProcessValue
{
    /// <summary>
    /// RodStatus.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RodStatus : UserControl
    {

        //AdjustGrid


        public int Position
        {
            get { return (int)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Position.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(int), typeof(RodStatus), new PropertyMetadata(0, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rodStatus = (RodStatus)d;
            double pos = (double)rodStatus.Position;
            GridLength gl1 = new GridLength(100.0-pos, GridUnitType.Star);
            GridLength gl2 = new GridLength(50.0, GridUnitType.Star);
            GridLength gl3 = new GridLength(100.0+pos, GridUnitType.Star);
           


        }
        

        public RodStatus()
        {
            InitializeComponent();
        }



    }
}
