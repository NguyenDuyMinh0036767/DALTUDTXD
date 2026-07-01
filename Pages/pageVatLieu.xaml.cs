using HUCE_DALTUD_LOPNV90_2026_0053867.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace HUCE_DALTUD_LOPNV90_2026_0053867.Pages
{
    /// <summary>
    /// Interaction logic for pageVatLieu.xaml
    /// </summary>
    public partial class pageVatLieu : Page
    {
        public static ObservableCollection<Material> Materials { get; set; } = new ObservableCollection<Material>();

        private Material selectedMaterial = null;

        public pageVatLieu()
        {
            InitializeComponent();
            MaterialDataGrid.ItemsSource = Materials;
            loadDaTa2();
            // Chọn 1 dòng trong DataGrid
            MaterialDataGrid.SelectionChanged += MaterialDataGrid_SelectionChanged;
        }

        private void MaterialDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedMaterial = MaterialDataGrid.SelectedItem as Material;
            if (selectedMaterial != null)
            {
                txtName.Text = selectedMaterial.TenVatLieu;
                txtMoDunDanHoi.Text = selectedMaterial.MoDunDanHoi;
                txtCuongDoKeoChay.Text = selectedMaterial.CuongDoChiuKeo;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtMoDunDanHoi.Text) || string.IsNullOrWhiteSpace(txtCuongDoKeoChay.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Thêm mới vật liệu
            Materials.Add(new Material
            {
                TenVatLieu = txtName.Text,
                MoDunDanHoi = txtMoDunDanHoi.Text,
                CuongDoChiuKeo = txtCuongDoKeoChay.Text
            });

            ClearInputs();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedMaterial == null)
            {
                MessageBox.Show("Vui lòng chọn vật liệu cần sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            selectedMaterial.TenVatLieu = txtName.Text;
            selectedMaterial.MoDunDanHoi = txtMoDunDanHoi.Text;
            selectedMaterial.CuongDoChiuKeo = txtCuongDoKeoChay.Text;

            // Refresh DataGrid
            MaterialDataGrid.Items.Refresh();

            ClearInputs();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedMaterial == null)
            {
                MessageBox.Show("Vui lòng chọn vật liệu cần xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Materials.Remove(selectedMaterial);
            ClearInputs();
        }

        private void ClearInputs()
        {
            txtName.Clear();
            txtMoDunDanHoi.Clear();
            txtCuongDoKeoChay.Clear();
            MaterialDataGrid.SelectedItem = null;
            selectedMaterial = null;
        }
        public void loadDaTa2()
        {
            Materials.Add(new Material
            {
                TenVatLieu = "CCT34",
                MoDunDanHoi = "210 N/mm2",
                CuongDoChiuKeo = "340 N/mm2"
            });

            Materials.Add(new Material
            {
                TenVatLieu = "CCT38",
                MoDunDanHoi = "230 N/mm2",
                CuongDoChiuKeo = "380 N/mm2"
            });

            Materials.Add(new Material
            {
                TenVatLieu = "CCT42",
                MoDunDanHoi = "245 N/mm2",
                CuongDoChiuKeo = "420 N/mm2"
            });

        
        }

    }
    public class Material
    {
        public string TenVatLieu { get; set; }
        public string MoDunDanHoi { get; set; }
        public string CuongDoChiuKeo { get; set; }
    }
}
