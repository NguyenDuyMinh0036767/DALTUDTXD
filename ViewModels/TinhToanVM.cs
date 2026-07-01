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
            double N = Math.Abs(Convert.ToDouble(parameter.txtTaiTrong.Text)) ;
            //double L = Convert.ToDouble(parameter.txtDaiCot.Text);    
            double f = Column.VatLieu.CuongDoTinhToanF;
            double E = Column.VatLieu.MoDunDanHoiE;
            double gamaC = 1;
            double A = 2 * (daycanh * caocanh) + (daybung * caobung);
            ChuongTrinhCon chuongTrinhCon = new ChuongTrinhCon();
            double ix = Column.TietDien.Tinhix();
            double iy = Column.TietDien.Tinhiy();

            double L = Column.ChieuCao * 1000.0;

            double lambdaX = L / ix;
            double lambdaY = L / iy;

            double DoManhLamDa = Math.Max(lambdaX, lambdaY);

            parameter.lblKTB.Content = chuongTrinhCon.TinhToanBen(N, A, f, gamaC) ? "Thỏa Mãn" : "Không Thỏa Mãn";

            parameter.lblKTODTH.Content = chuongTrinhCon.TinhToanOnDinhTongThe(N, A, f, gamaC, DoManhLamDa, E) ? "Thỏa Mãn" : "Không Thỏa Mãn";

            parameter.lblKTODCB.Content = chuongTrinhCon.TinhToanOnDinhCucBo(caobung, daybung, DoManhLamDa, f, E, (caocanh - daybung) / 2, daycanh) ? "Thỏa Mãn" : "Không Thỏa Mãn";

            parameter.lblKNCN.Content = chuongTrinhCon.TTKhaNangChiuNenLechtam(N, f, gamaC, A, DoManhLamDa, E);
        }
    }
}
