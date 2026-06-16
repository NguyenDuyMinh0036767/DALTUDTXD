using HUCE_DALTUD_LOPNV90_2026_0053867.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace HUCE_DALTUD_LOPNV90_2026_0053867.Pages
{
    /// <summary>
    /// Interaction logic for pageTietDien.xaml
    /// </summary>
    public partial class pageTietDien : Page
    {
        public pageTietDien()
        {
            InitializeComponent();
            loadDaTa();
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra tính hợp lệ của dữ liệu đầu vào chống lỗi crash app
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                !double.TryParse(txtRongCanh.Text, out double b) ||
                !double.TryParse(txtDayCanh.Text, out double tf) ||
                !double.TryParse(txtCaoBung.Text, out double h) ||
                !double.TryParse(txtDayBung.Text, out double tw))
            {
                System.Windows.MessageBox.Show("Vui lòng nhập đầy đủ tên và các thông số kích thước phải là số hợp lệ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            clsBienToanCuc.clsTietDien.Add(new clsTietDien(txtName.Text, b, tf, h, tw));
            loadDaTa();
        }

        // Tự động tính toán lại tọa độ hình khối mỗi khi người dùng thay đổi số ở TextBox
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (geoCanhTren != null && geoBung != null && geoCanhDuoi != null &&
                txtRongCanh != null && txtDayCanh != null && txtCaoBung != null && txtDayBung != null)
            {
                if (double.TryParse(txtRongCanh.Text, out double b) &&
                    double.TryParse(txtDayCanh.Text, out double tf) &&
                    double.TryParse(txtCaoBung.Text, out double h) &&
                    double.TryParse(txtDayBung.Text, out double tw))
                {
                    // 1. Cánh trên
                    geoCanhTren.Rect = new Rect(0, 0, b, tf);

                    // 2. Thân bụng (Căn giữa theo cánh bằng công thức dịch trục X)
                    double webX = (b - tw) / 2;
                    geoBung.Rect = new Rect(webX, tf, tw, h);

                    // 3. Cánh dưới
                    geoCanhDuoi.Rect = new Rect(0, tf + h, b, tf);
                }
            }
        }

        public void loadDaTa()
        {
            List<clsTietDien> list = new List<clsTietDien>();
            HashSet<string> tenTietDienDaCo = new HashSet<string>();

            foreach (clsTietDien TietDienThep in clsBienToanCuc.clsTietDien)
            {
                if (!tenTietDienDaCo.Contains(TietDienThep.Name))
                {
                    tenTietDienDaCo.Add(TietDienThep.Name);
                    list.Add(TietDienThep);
                }
            }

            dtgTietDien.ItemsSource = list;
        }
    }
}