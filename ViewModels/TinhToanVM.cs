using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HUCE_DALTUD_LOPNV90_2026_0053867.Class;
using HUCE_DALTUD_LOPNV90_2026_0053867.Pages;

namespace HUCE_DALTUD_LOPNV90_2026_0053867.ViewModels
{
    public class TinhToanVM : BaseViewModel
    {

        public ICommand cmTinhToan { get; set; }

        public TinhToanVM()
        {
            cmTinhToan = new RelayCommand<pageTinhToan>((parameter) => true, (parameter) => TinhToan(parameter));

        }

        public void TinhToan(pageTinhToan parameter)
        {
            clsColumn Column = null;
            foreach (clsColumn column in clsBienToanCuc.clsColumn)
            {
                if (column.Name == parameter.cbbCotCanTinh.Text)
                {
                    Column = column;
                }
            }
            double caocanh = Column.TietDien.DoDayCanh;
            double daycanh = Column.TietDien.DoDayCanh;
            double caobung = Column.TietDien.ChieuCaoBung;
            double daybung = Column.TietDien.DoDayBung;
            double N = Math.Abs(Convert.ToDouble(parameter.txtTaiTrong.Text)) * 10000;
            //double L = Convert.ToDouble(parameter.txtDaiCot.Text);
            double f = Column.VatLieu.CuongDoChiuKeo;
            double E = Column.VatLieu.MoDunDanHoi;
            double gamaC = 1;
            double A = 2 * (daycanh * caocanh) + (daybung * caobung);
            ChuongTrinhCon chuongTrinhCon = new ChuongTrinhCon();
            double DoManhLamDa = chuongTrinhCon.DoManh(daycanh, daybung, caocanh, caobung);

            parameter.lblKTB.Content = chuongTrinhCon.TinhToanBen(N, A, f, gamaC) ? "Thỏa Mãn" : "Không Thỏa Mãn";

            parameter.lblKTODTH.Content = chuongTrinhCon.TinhToanOnDinhTongThe(N, A, f, gamaC, DoManhLamDa, E) ? "Thỏa Mãn" : "Không Thỏa Mãn";

            parameter.lblKTODCB.Content = chuongTrinhCon.TinhToanOnDinhCucBo(caobung, daybung, DoManhLamDa, f, E, (caocanh - daybung) / 2, daycanh) ? "Thỏa Mãn" : "Không Thỏa Mãn";

            parameter.lblKNCN.Content = chuongTrinhCon.TTKhaNangChiuNenLechtam(N, f, gamaC, A, DoManhLamDa, E);
        }
    }
}
