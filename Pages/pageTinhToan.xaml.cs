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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HUCE_DALTUD_LOPNV90_2026_0053867.Pages
{
    /// <summary>
    /// Interaction logic for pageTinhToan.xaml
    /// </summary>
    public partial class pageTinhToan : Page
    {
        public pageTinhToan()
        {
            InitializeComponent();
            LoadDL();
        }


        private void sl(object sender, SelectionChangedEventArgs e)
        {
            string selectedColumnName = cbbCotCanTinh.SelectedItem as string;

            foreach (clsColumn column in clsBienToanCuc.clsColumn)
            {
                if (column.Name == selectedColumnName)
                {
                    txtDaiCot.Text = column.ChieuCao.ToString();
                    txtTaiTrong.Text = column.LucDoc.ToString();
                    cbbTietDien.Text = column.TietDien.Name;
                    cbbVatLieu.Text = column.VatLieu.TenVatLieu;
                    break;
                }
            }
            TinhVaHienKetQua();

        }
        public void LoadDL()
        {
            List<string> listcl = new List<string>();
            foreach (clsColumn column in clsBienToanCuc.clsColumn)
            {
                listcl.Add(column.Name);
            }
            cbbCotCanTinh.ItemsSource = listcl;

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

        private void TinhVaHienKetQua()
        {
            ChuongTrinhCon ct = new ChuongTrinhCon();

            // 1. Lấy dữ liệu từ giao diện
            double N = double.Parse(txtTaiTrong.Text);   // lực dọc (kN)
            double Lamda = double.Parse(txtDaiCot.Text); // TẠM dùng chiều dài làm độ mảnh

            clsTietDien td = clsBienToanCuc.clsTietDien
                .First(x => x.Name == cbbTietDien.Text);

            clsVatLieuThep vl = clsBienToanCuc.clsVatLieu
                .First(x => x.TenVatLieu == cbbVatLieu.Text);

            // 2. Thông số tiết diện
            double A = td.TinhDienTichTietDien();

            double hw = td.ChieuCaoBung;
            double tw = td.DoDayBung;
            double bo = td.ChieuRongCanh / 2.0;
            double tf = td.DoDayCanh;

            // 3. Thông số vật liệu
            double f = vl.CuongDoChiuKeo;     // f
            double E = vl.MoDunDanHoi;        // E

            // Hệ số vật liệu γc (chưa có trong class → gán theo TCVN)
            double gamaC = 1.1;               // hoặc 1.0 tùy bài


            // 4. Tính toán
            bool ktBen = ct.TinhToanBen(N, A, f, gamaC);

            bool ktODTT = ct.TinhToanOnDinhTongThe(
                N, A, f, gamaC, Lamda, E);

            bool ktODCB = ct.TinhToanOnDinhCucBo(
                hw, tw, Lamda, f, E, bo, tf);

            double knChiuNen = ct.TTKhaNangChiuNenLechtam(
                N, f, gamaC, A, Lamda, E);

            // 5. Hiển thị kết quả
            lblKTB.Content = ktBen ? "ĐẠT" : "KHÔNG ĐẠT";
            lblKTODTH.Content = ktODTT ? "ĐẠT" : "KHÔNG ĐẠT";
            lblKTODCB.Content = ktODCB ? "ĐẠT" : "KHÔNG ĐẠT";
            lblKNCN.Content = knChiuNen.ToString("0.##") + " kN";

            lblKTB.Foreground = ktBen ? Brushes.Green : Brushes.Red;
            lblKTODTH.Foreground = ktODTT ? Brushes.Green : Brushes.Red;
            lblKTODCB.Foreground = ktODCB ? Brushes.Green : Brushes.Red;
        }

    }
}
