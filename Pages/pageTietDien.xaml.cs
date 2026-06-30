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
        private void dtgTietDien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtgTietDien.SelectedItem == null)
                return;

            clsTietDien td = dtgTietDien.SelectedItem as clsTietDien;

            if (td == null)
                return;

            txtName.Text = td.Name;
            txtRongCanh.Text = td.ChieuRongCanh.ToString();
            txtDayCanh.Text = td.DoDayCanh.ToString();
            txtCaoBung.Text = td.ChieuCaoBung.ToString();
            txtDayBung.Text = td.DoDayBung.ToString();

            VeTietDien(td);
        }
        private void VeTietDien(clsTietDien td)
        {
            double B = td.ChieuRongCanh;
            double tf = td.DoDayCanh;
            double h = td.ChieuCaoBung;
            double tw = td.DoDayBung;

            double H = h + 2 * tf;

            // Kích thước vùng vẽ
            double canvasW = 200;
            double canvasH = 300;

            // Scale để tiết diện luôn vừa khung
            double scale = Math.Min(
                canvasW * 0.7 / B,
                canvasH * 0.7 / H);

            B *= scale;
            tf *= scale;
            h *= scale;
            tw *= scale;

            H = h + 2 * tf;

            double cx = canvasW / 2;
            double cy = canvasH / 2;

            // Cánh trên
            geoCanhTren.Rect = new Rect(
                cx - B / 2,
                cy - H / 2,
                B,
                tf);

            // Bụng
            geoBung.Rect = new Rect(
                cx - tw / 2,
                cy - H / 2 + tf,
                tw,
                h);

            // Cánh dưới
            geoCanhDuoi.Rect = new Rect(
                cx - B / 2,
                cy + H / 2 - tf,
                B,
                tf);
        }
    }

}