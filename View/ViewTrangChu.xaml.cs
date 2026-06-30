using HUCE_DALTUD_LOPNV90_2026_0053867.Pages;
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
    /// Interaction logic for ViewTrangChu.xaml
    /// </summary>
    public partial class ViewTrangChu : Window
    {
        public ViewTrangChu()
        {
            InitializeComponent();
        }
        private void btnVatLieu_Click(object sender, RoutedEventArgs e)
        {
            frame_Body.Content = new pageVatLieu();

            ResetButtonStyle();
            btnVatLieu.Background = Brushes.LightSkyBlue;

            txtTrangThai.Text = "Đang làm việc: Vật liệu";
        }

        private void btnTietDien_Click(object sender, RoutedEventArgs e)
        {
            frame_Body.Content = new pageTietDien();

            ResetButtonStyle();
            btnTietDien.Background = Brushes.LightSkyBlue;

            txtTrangThai.Text = "Đang làm việc: Tiết diện";
        }

        private void btnTinhToanKNCNLechTam_Click(object sender, RoutedEventArgs e)
        {
            frame_Body.Content = new pageTinhToan();

            ResetButtonStyle();
            btnTinhToanKNCNLechTam.Background = Brushes.LightSkyBlue;

            txtTrangThai.Text = "Đang làm việc: Kiểm tra nén lệch tâm";
        }

        private void btnThongSoCot_Click(object sender, RoutedEventArgs e)
        {
            frame_Body.Content = new pageColumnParameters();

            ResetButtonStyle();
            btnThongSoCot.Background = Brushes.LightSkyBlue;

            txtTrangThai.Text = "Đang làm việc: Thông số cột";
        }
        private void btnDoBenKeo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Thông số: Độ bền kéo của thép.", "Tensile Strength", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnGioiHanChay_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Thông số: Giới hạn chảy của thép.", "Yield Strength", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnLuuDuAn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dữ liệu dự án đã được lưu thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void btnXuatExcel_Click(object sender, RoutedEventArgs e)
        {
            frame_Body.Content = new pageBaoCaoExcel();

            ResetButtonStyle();
            btnExportExcel.Background = Brushes.LightSkyBlue;

            txtTrangThai.Text = "Đang làm việc: Xuất Excel";
        }
        private void ResetButtonStyle()
        {
            btnVatLieu.ClearValue(BackgroundProperty);
            btnTietDien.ClearValue(BackgroundProperty);
            btnThongSoCot.ClearValue(BackgroundProperty);
            btnTinhToanKNCNLechTam.ClearValue(BackgroundProperty);
            btnExportExcel.ClearValue(BackgroundProperty);
        }
    }
}
