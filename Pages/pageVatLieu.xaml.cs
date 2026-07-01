using HUCE_DALTUD_LOPNV90_2026_0053867.Class;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace HUCE_DALTUD_LOPNV90_2026_0053867.Pages
{
    public partial class pageVatLieu : Page
    {
        // Khai báo danh sách sử dụng đúng class clsVatLieuThep
      
        private clsVatLieuThep selectedMaterial = null;

        public pageVatLieu()
        {
            InitializeComponent();
            MaterialDataGrid.ItemsSource = clsBienToanCuc.clsVatLieu;
            loadData2();

            MaterialDataGrid.SelectionChanged += MaterialDataGrid_SelectionChanged;
        }

        // 1. Khi chọn 1 dòng dưới lưới -> Đổ dữ liệu lên các ô TextBox chuẩn tên mới
        private void MaterialDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedMaterial = MaterialDataGrid.SelectedItem as clsVatLieuThep;
            if (selectedMaterial != null)
            {
                txtTenVatLieu.Text = selectedMaterial.TenVatLieu;
                txtCuongDoF.Text = selectedMaterial.CuongDoTinhToanF.ToString();
                txtMoDunE.Text = selectedMaterial.MoDunDanHoiE.ToString();
            }
        }

        // 2. Click nút Lưu vật liệu (Thêm mới)
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenVatLieu.Text) ||
                !double.TryParse(txtCuongDoF.Text, out double f) ||
                !double.TryParse(txtMoDunE.Text, out double eModun))
            {
                MessageBox.Show("Vui lòng nhập đúng và đầy đủ thông tin (Cường độ và Mô-đun phải là số)!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Gọi đúng Constructor của clsVatLieuThep
            clsBienToanCuc.clsVatLieu.Add(new clsVatLieuThep(txtTenVatLieu.Text, f, eModun));

            ClearInputs();
        }

        // 3. Click nút Sửa vật liệu
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedMaterial == null)
            {
                MessageBox.Show("Vui lòng chọn vật liệu cần sửa từ danh sách!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenVatLieu.Text) ||
                !double.TryParse(txtCuongDoF.Text, out double f) ||
                !double.TryParse(txtMoDunE.Text, out double eModun))
            {
                MessageBox.Show("Dữ liệu sửa không hợp lệ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Giao diện tự động cập nhật nhờ có PropertyChanged trong clsVatLieuThep
            selectedMaterial.TenVatLieu = txtTenVatLieu.Text;
            selectedMaterial.CuongDoTinhToanF = f;
            selectedMaterial.MoDunDanHoiE = eModun;

            ClearInputs();
        }

        // 4. Click nút Xóa vật liệu
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedMaterial == null)
            {
                MessageBox.Show("Vui lòng chọn vật liệu cần xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            clsBienToanCuc.clsVatLieu.Remove(selectedMaterial);
            ClearInputs();
        }

        // 5. Làm sạch các ô nhập liệu
        private void ClearInputs()
        {
            txtTenVatLieu.Clear();
            txtCuongDoF.Clear();
            txtMoDunE.Clear();
            MaterialDataGrid.SelectedItem = null;
            selectedMaterial = null;
        }

        // 6. Nạp dữ liệu mặc định chuẩn kỹ thuật xây dựng (E = 210000 N/mm2)
        public void loadData2()
        {
            clsBienToanCuc.clsVatLieu.Clear();

            // Khởi tạo thông qua constructor của clsVatLieuThep
            clsBienToanCuc.clsVatLieu.Add(new clsVatLieuThep("CCT34", 210, 210000));
            clsBienToanCuc.clsVatLieu.Add(new clsVatLieuThep("CCT38", 230, 210000));
            clsBienToanCuc.clsVatLieu.Add(new clsVatLieuThep("CCT42", 250, 210000));
            clsBienToanCuc.clsVatLieu.Add(new clsVatLieuThep("SS400", 235, 210000));
            clsBienToanCuc.clsVatLieu.Add(new clsVatLieuThep("S235", 235, 210000));
            clsBienToanCuc.clsVatLieu.Add(new clsVatLieuThep("S275", 275, 210000));
        }
    }
}