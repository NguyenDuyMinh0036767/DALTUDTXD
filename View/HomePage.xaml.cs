using HUCE_DALTUD_LOPNV90_2026_0053867.View;
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

namespace HUCE_DALTUD_LOPNV90_2026_0053867
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class HomePage : Window
    {
        public HomePage()
        {
            InitializeComponent();
        }
        private void btnTaoMoi_Click(object sender, RoutedEventArgs e)
        {
            ViewTrangChu viewTrangChu = new ViewTrangChu();
            viewTrangChu.Show();
            this.Close();
        }
    }
}
