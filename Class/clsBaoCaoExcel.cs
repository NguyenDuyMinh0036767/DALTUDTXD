using OfficeOpenXml;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Windows;


namespace HUCE_DALTUD_LOPNV90_2026_0053867.Class
{
    public class clsBaoCaoExcel
    {
        public static void XuatExcel(List<clsBaoCaoDong> ds)
        {
            ExcelPackage.License.SetNonCommercialOrganization("HUCE");

            SaveFileDialog save =
                new SaveFileDialog();

            save.Filter = "Excel File|*.xlsx";
            save.FileName = "BaoCaoCotThep.xlsx";

            if (save.ShowDialog() != true)
                return;

            using (ExcelPackage package = new ExcelPackage())
            {
                var ws =
                    package.Workbook.Worksheets.Add("BaoCao");

                ws.Cells[1, 1].Value = "Tên cột";
                ws.Cells[1, 2].Value = "Tiết diện";
                ws.Cells[1, 3].Value = "Kiểm tra bền";
                ws.Cells[1, 4].Value = "Ổn định tổng thể";
                ws.Cells[1, 5].Value = "Ổn định cục bộ";
                ws.Cells[1, 6].Value = "Khả năng chịu nén";

                int row = 2;

                foreach (var item in ds)
                {
                    ws.Cells[row, 1].Value = item.TenCot;
                    ws.Cells[row, 2].Value = item.TietDien;
                    ws.Cells[row, 3].Value = item.KTBen;
                    ws.Cells[row, 4].Value = item.KTODTT;
                    ws.Cells[row, 5].Value = item.KTODCB;
                    ws.Cells[row, 6].Value = item.KNCN;

                    row++;
                }

                ws.Cells.AutoFitColumns();

                File.WriteAllBytes(
                    save.FileName,
                    package.GetAsByteArray());

                MessageBox.Show("Xuất Excel thành công!");
            }
        }
    }
}