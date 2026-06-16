using HUCE_DALTUD_LOPNV90_2026_0053867;
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

namespace HUCE_DALTUD_LOPNV90_2026_0053867.View
{
    /// <summary>
    /// Interaction logic for ViewDangNhap.xaml
    /// </summary>
    public partial class ViewDangNhap : Window
    {
        public ViewDangNhap()
        {
            InitializeComponent();
        }
        private void btnDangNhap_Click(object sender, RoutedEventArgs e)
        {
            HomePage homePage = new HomePage();
            homePage.Show();
            this.Close();
        }
    }
}
