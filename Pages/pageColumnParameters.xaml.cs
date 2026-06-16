using HUCE_DALTUD_LOPNV90_2026_0053867.Class;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Xml.Linq;

namespace HUCE_DALTUD_LOPNV90_2026_0053867.Pages
{
    /// <summary>
    /// Interaction logic for pageColumnParameters.xaml
    /// </summary>
    public partial class pageColumnParameters : Page
    {
        public pageColumnParameters()
        {
            InitializeComponent();

            // Kết nối DataGrid với danh sách cột
            dtgColumn.ItemsSource = clsBienToanCuc.clsColumn;

            // Load dữ liệu cho các ComboBox
            LoadDL();
        }

        // Xử lý khi chọn một hàng trong DataGrid
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtgColumn.SelectedItem is clsColumn selectedItem)
            {
                cbbTietDien.SelectedItem = selectedItem.TietDien.Name;
                cbbVatLieu.SelectedItem = selectedItem.VatLieu.TenVatLieu;
                txtDaiCot.Text = selectedItem.ChieuCao.ToString();
                txtMoMenchan.Text = selectedItem.MoMent.ToString();
                txtTaiTrongchan.Text = selectedItem.LucDoc.ToString();
                txtName.Text = selectedItem.Name;
            }
        }

        public void LoadDL()
        {
            List<clsVatLieuThep> listvl = new List<clsVatLieuThep>();
            HashSet<string> tenVatLieuDaCo = new HashSet<string>();

            foreach (clsVatLieuThep vatLieuThep in clsBienToanCuc.clsVatLieu)
            {
                if (!tenVatLieuDaCo.Contains(vatLieuThep.TenVatLieu))
                {
                    tenVatLieuDaCo.Add(vatLieuThep.TenVatLieu);
                    listvl.Add(vatLieuThep);
                }
            }
            cbbVatLieu.ItemsSource = tenVatLieuDaCo;

            List<clsTietDien> listtd = new List<clsTietDien>();
            HashSet<string> tenTDDaCo = new HashSet<string>();

            foreach (clsTietDien clstietdien in clsBienToanCuc.clsTietDien)
            {
                if (!tenVatLieuDaCo.Contains(clstietdien.Name))
                {
                    tenTDDaCo.Add(clstietdien.Name);
                    listtd.Add(clstietdien);
                }
            }
            cbbTietDien.ItemsSource = tenTDDaCo;
        }
    }
}
