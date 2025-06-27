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
    /// MenuMainButton.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MenuMainButton : UserControl
    {
        public string TextContent
        {
            get
            {
                string z = (string)GetValue(TextContentProperty);
                if (z != null)
                {
                    z = z.Replace(';', (char)(10));
                }
                return z;
            }
            set
            {
                string z = value.Replace((char)(10),';' );
                SetValue(TextContentProperty, value);
            }
        }

        public static readonly DependencyProperty TextContentProperty =
           DependencyProperty.Register("TextContents", typeof(string), typeof(MenuMainButton), new UIPropertyMetadata(null));


        public SolidColorBrush HoverBrush
        {
            get { return (SolidColorBrush)GetValue(HoverBrushProperty); }
            set { SetValue(HoverBrushProperty, value); }
        }
        public static readonly DependencyProperty HoverBrushProperty =
            DependencyProperty.Register("HoverBrush", typeof(SolidColorBrush), typeof(MenuMainButton), new UIPropertyMetadata(null));

        public SolidColorBrush PressedBrush
        {
            get { return (SolidColorBrush)GetValue(PressedBrushProperty); }
            set { SetValue(PressedBrushProperty, value); }
        }
        public static readonly DependencyProperty PressedBrushProperty =
            DependencyProperty.Register("PressedBrush", typeof(SolidColorBrush), typeof(MenuMainButton), new UIPropertyMetadata(null));

        public event RoutedEventHandler Click;
        private void button_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }



        Brush BorderBrush;
        public MenuMainButton()
        {
            InitializeComponent();
        }


        
        
    }
}
