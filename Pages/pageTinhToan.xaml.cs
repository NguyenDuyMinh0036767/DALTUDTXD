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
            if (string.IsNullOrEmpty(selectedColumnName)) return;

            foreach (clsColumn column in clsBienToanCuc.clsColumn)
            {
                if (column.Name == selectedColumnName)
                {
                    txtDaiCot.Text = column.ChieuCao.ToString();
                    txtTaiTrong.Text = column.LucDoc.ToString();
                    cbbTietDien.Text = column.TietDien.Name;
                    cbbVatLieu.SelectedItem = column.VatLieu;
                    cbbVatLieu.SelectionChanged -= cbbVatLieu_SelectionChanged;
                    cbbVatLieu.SelectedItem = column.VatLieu;
                    cbbVatLieu.SelectionChanged += cbbVatLieu_SelectionChanged;

                    // Vẽ lại mặt cắt khi chọn cột
                    VeMatCatTietDien(column.TietDien.Name);
                    break;
                }
            }
            TinhVaHienKetQua();
        }

        private void cbbTietDien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbTietDien.SelectedItem == null) return;

            VeMatCatTietDien(cbbTietDien.SelectedItem as string);
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

            cbbVatLieu.ItemsSource = clsBienToanCuc.clsVatLieu;
            cbbVatLieu.DisplayMemberPath = "TenVatLieu";
            

            List<clsTietDien> listtd = new List<clsTietDien>();
            HashSet<string> tenTDDaCo = new HashSet<string>();

            foreach (clsTietDien clstietdien in clsBienToanCuc.clsTietDien)
            {
                if (!tenTDDaCo.Contains(clstietdien.Name))
                {
                    tenTDDaCo.Add(clstietdien.Name);
                    listtd.Add(clstietdien);
                }
            }
            cbbTietDien.ItemsSource = tenTDDaCo;
        }

        /// <summary>
        /// Hàm vẽ tự động mặt cắt chữ I động lên Canvas
        /// </summary>
        private void VeMatCatTietDien(string tenTietDien)
        {
            if (canvasTietDien == null) return;
            canvasTietDien.Children.Clear();

            clsTietDien td = clsBienToanCuc.clsTietDien.FirstOrDefault(x => x.Name == tenTietDien);
            if (td == null) return;

            // 1. Lấy thông số hình học hình chữ I từ Class của bạn
            double h = td.ChieuCaoBung + 2 * td.DoDayCanh;
            double b = td.ChieuRongCanh;
            double tw = td.DoDayBung;
            double tf = td.DoDayCanh;

            // 2. Tính toán tỷ lệ scale an toàn cho Canvas (Width=200, Height=170)
            double maxVisualWidth = 150;
            double maxVisualHeight = 130;

            double scaleX = maxVisualWidth / b;
            double scaleY = maxVisualHeight / h;
            double scale = Math.Min(scaleX, scaleY);

            double drawW = b * scale;
            double drawH = h * scale;
            double drawTw = tw * scale;
            double drawTf = tf * scale;

            double offsetX = (canvasTietDien.Width - drawW) / 2;
            double offsetY = (canvasTietDien.Height - drawH) / 2;

            // 3. Khởi tạo nét vẽ Polygon bao quanh hình chữ I
            Polygon iSection = new Polygon();
            iSection.Stroke = Brushes.Black;
            iSection.StrokeThickness = 1.5;
            iSection.Fill = new SolidColorBrush(Color.FromRgb(200, 200, 200));

            double xLeft = offsetX;
            double xRight = offsetX + drawW;
            double xMidLeft = offsetX + (drawW - drawTw) / 2;
            double xMidRight = offsetX + (drawW + drawTw) / 2;

            double yTop = offsetY;
            double yTopFlangeBottom = offsetY + drawTf;
            double yBottomFlangeTop = offsetY + drawH - drawTf;
            double yBottom = offsetY + drawH;

            // Thêm các điểm vẽ khép kín chữ I
            iSection.Points.Add(new Point(xLeft, yTop));
            iSection.Points.Add(new Point(xRight, yTop));
            iSection.Points.Add(new Point(xRight, yTopFlangeBottom));
            iSection.Points.Add(new Point(xMidRight, yTopFlangeBottom));
            iSection.Points.Add(new Point(xMidRight, yBottomFlangeTop));
            iSection.Points.Add(new Point(xRight, yBottomFlangeTop));
            iSection.Points.Add(new Point(xRight, yBottom));
            iSection.Points.Add(new Point(xLeft, yBottom));
            iSection.Points.Add(new Point(xLeft, yBottomFlangeTop));
            iSection.Points.Add(new Point(xMidLeft, yBottomFlangeTop));
            iSection.Points.Add(new Point(xMidLeft, yTopFlangeBottom));
            iSection.Points.Add(new Point(xLeft, yTopFlangeBottom));

            canvasTietDien.Children.Add(iSection);

            // 4. Thêm nhãn kích thước text ở tâm tiết diện
            TextBlock txtName = new TextBlock();
            txtName.Text = $"{h}X{b}";
            txtName.FontWeight = FontWeights.Bold;
            txtName.FontSize = 12;
            txtName.Foreground = Brushes.Black;

            txtName.UpdateLayout();
            Canvas.SetLeft(txtName, (canvasTietDien.Width / 2) - 22);
            Canvas.SetTop(txtName, (canvasTietDien.Height / 2) - 8);

            canvasTietDien.Children.Add(txtName);
        }

        private void TinhVaHienKetQua()
        {
            if (cbbTietDien.SelectedItem == null || cbbVatLieu.SelectedItem == null)
                return;

            ChuongTrinhCon ct = new ChuongTrinhCon();
            double N = double.Parse(txtTaiTrong.Text);
            double L = double.Parse(txtDaiCot.Text) * 1000.0;   // đổi m → mm

            string tdName = cbbTietDien.SelectedItem as string;

            clsTietDien td = clsBienToanCuc.clsTietDien
                .First(x => x.Name == tdName);
            clsVatLieuThep vl = cbbVatLieu.SelectedItem as clsVatLieuThep;
            if (vl == null) return;

            double A = td.TinhDienTichTietDien();
            double Ix = td.TinhIx();
            double Iy = td.TinhIy();

            double ix = Math.Sqrt(Ix / A);
            double iy = Math.Sqrt(Iy / A);

            // lấy bán kính quán tính nhỏ hơn
            double imin = Math.Min(ix, iy);

            // độ mảnh của cột
            double Lamda = L / imin;
            double hw = td.ChieuCaoBung;
            double tw = td.DoDayBung;
            double bo = (td.ChieuRongCanh - td.DoDayBung) / 2.0;
            double tf = td.DoDayCanh;

            double f = vl.CuongDoTinhToanF;
            double E = vl.MoDunDanHoiE;
            double gamaC = 1.1;

            bool ktBen = ct.TinhToanBen(N, A, f, gamaC);
            bool ktODTT = ct.TinhToanOnDinhTongThe(N, A, f, gamaC, Lamda, E);
            bool ktODCB = ct.TinhToanOnDinhCucBo(hw, tw, Lamda, f, E, bo, tf);
            double knChiuNen = ct.TTKhaNangChiuNenLechtam(N, f, gamaC, A, Lamda, E);


            lblKTB.Content = ktBen ? "ĐẠT" : "KHÔNG ĐẠT";
            lblKTODTH.Content = ktODTT ? "ĐẠT" : "KHÔNG ĐẠT";
            lblKTODCB.Content = ktODCB ? "ĐẠT" : "KHÔNG ĐẠT";
            lblKNCN.Content = knChiuNen.ToString("0.##") + " kN";

            lblKTB.Foreground = ktBen ? Brushes.Green : Brushes.Red;
            lblKTODTH.Foreground = ktODTT ? Brushes.Green : Brushes.Red;
            lblKTODCB.Foreground = ktODCB ? Brushes.Green : Brushes.Red;
            
        }
        private void cbbVatLieu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;

            var vl = cbbVatLieu.SelectedItem as clsVatLieuThep;
            if (vl == null) return;

            string selectedColumnName = cbbCotCanTinh.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedColumnName)) return;

            var column = clsBienToanCuc.clsColumn
                .FirstOrDefault(x => x.Name == selectedColumnName);

            if (column == null) return;

            // update dữ liệu gốc
            column.VatLieu = vl;

            TinhVaHienKetQua();
        }
    }
}