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
using HUCE_DALTUD_LOPNV90_2026_0053867.Class;
using ClosedXML.Excel;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace HUCE_DALTUD_LOPNV90_2026_0053867.Pages
{
    public partial class pageBaoCaoExcel : Page
    {
        public pageBaoCaoExcel()
        {
            InitializeComponent();

            LoadBaoCao();
        }

        private void LoadBaoCao()
        {
            List<clsBaoCaoDong> ds =
                new List<clsBaoCaoDong>();

            ChuongTrinhCon ct =
                new ChuongTrinhCon();

            foreach (clsColumn column in clsBienToanCuc.clsColumn)
            {
                clsTietDien td = column.TietDien;
                clsVatLieuThep vl = column.VatLieu;

                double N = column.LucDoc;
                double Lamda = column.ChieuCao;

                double A = td.TinhDienTichTietDien();

                double hw = td.ChieuCaoBung;
                double tw = td.DoDayBung;
                double bo = td.ChieuRongCanh / 2.0;
                double tf = td.DoDayCanh;

                double f = vl.CuongDoChiuKeo;
                double E = vl.MoDunDanHoi;

                double gamaC = 1.1;

                bool ktBen =
                    ct.TinhToanBen(
                        N, A, f, gamaC);

                bool ktODTT =
                    ct.TinhToanOnDinhTongThe(
                        N, A, f, gamaC,
                        Lamda, E);

                bool ktODCB =
                    ct.TinhToanOnDinhCucBo(
                        hw, tw, Lamda,
                        f, E, bo, tf);

                double kncn =
                    ct.TTKhaNangChiuNenLechtam(
                        N, f, gamaC,
                        A, Lamda, E);

                ds.Add(new clsBaoCaoDong()
                {
                    TenCot = column.Name,
                    TietDien = td.Name,
                    KTBen = ktBen ? "ĐẠT" : "KHÔNG ĐẠT",
                    KTODTT = ktODTT ? "ĐẠT" : "KHÔNG ĐẠT",
                    KTODCB = ktODCB ? "ĐẠT" : "KHÔNG ĐẠT",
                    KNCN = kncn
                });
            }

            dgBaoCao.ItemsSource = ds;

            // Thống kê
            txtTongCot.Text = ds.Count.ToString();

            int soDat = ds.Count(x =>
                x.KTBen == "ĐẠT" &&
                x.KTODTT == "ĐẠT" &&
                x.KTODCB == "ĐẠT");

            txtDat.Text = soDat.ToString();

            txtKhongDat.Text =
                (ds.Count - soDat).ToString();
        }

        private void btnXuatExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                    FileName = "BaoCaoCotThep.xlsx"
                };

                if (saveFileDialog.ShowDialog() != true)
                    return;

                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("BaoCao");

                    // ===== TIÊU ĐỀ =====
                    ws.Cell(1, 1).Value = "BÁO CÁO KIỂM TRA CỘT THÉP CHỮ I";
                    ws.Range(1, 1, 1, 6).Merge();
                    ws.Cell(1, 1).Style.Font.Bold = true;
                    ws.Cell(1, 1).Style.Font.FontSize = 16;
                    ws.Cell(1, 1).Style.Alignment.Horizontal =
                        XLAlignmentHorizontalValues.Center;

                    // ===== THỐNG KÊ =====
                    ws.Cell(3, 1).Value = "Tổng số cột";
                    ws.Cell(3, 2).Value = txtTongCot.Text;

                    ws.Cell(3, 3).Value = "Đạt";
                    ws.Cell(3, 4).Value = txtDat.Text;

                    ws.Cell(3, 5).Value = "Không đạt";
                    ws.Cell(3, 6).Value = txtKhongDat.Text;

                    // ===== HEADER =====
                    int row = 5;

                    ws.Cell(row, 1).Value = "Tên cột";
                    ws.Cell(row, 2).Value = "Tiết diện";
                    ws.Cell(row, 3).Value = "Kiểm tra bền";
                    ws.Cell(row, 4).Value = "Ổn định tổng thể";
                    ws.Cell(row, 5).Value = "Ổn định cục bộ";
                    ws.Cell(row, 6).Value = "Khả năng chịu nén";

                    var headerRange = ws.Range(row, 1, row, 6);

                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.Gray;
                    headerRange.Style.Font.FontColor = XLColor.White;
                    headerRange.Style.Alignment.Horizontal =
                        XLAlignmentHorizontalValues.Center;

                    // ===== DỮ LIỆU =====
                    row++;

                    foreach (dynamic item in dgBaoCao.ItemsSource)
                    {
                        ws.Cell(row, 1).Value = item.TenCot;
                        ws.Cell(row, 2).Value = item.TietDien;
                        ws.Cell(row, 3).Value = item.KTBen;
                        ws.Cell(row, 4).Value = item.KTODTT;
                        ws.Cell(row, 5).Value = item.KTODCB;
                        ws.Cell(row, 6).Value = item.KNCN;

                        row++;
                    }

                    // ===== KẺ BẢNG =====
                    var dataRange = ws.Range(5, 1, row - 1, 6);

                    dataRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    // ===== TỰ ĐỘNG CĂN CỘT =====
                    ws.Columns().AdjustToContents();

                    wb.SaveAs(saveFileDialog.FileName);
                    if (MessageBox.Show(
                            "Xuất Excel thành công!\nBạn có muốn mở file ngay không?",
                            "Thông báo",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = saveFileDialog.FileName,
                            UseShellExecute = true
                        });
                    }
                }

                MessageBox.Show(
                    "Xuất Excel thành công!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}