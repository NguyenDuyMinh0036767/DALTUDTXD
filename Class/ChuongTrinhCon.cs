using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUCE_DALTUD_LOPNV90_2026_0053867.Class
{
    public class ChuongTrinhCon
    {
        public bool TinhToanBen(double N, double An, double f, double gamaC)
        {
            if (N / An <= f * gamaC)
            {
                return true;
            }
            return false;
        }

        public bool TinhToanOnDinhTongThe(double N, double A, double f, double gamaC, double Lamda, double E)
        {
            double hsUonDoc = TinhPhiMin(Lamda, f, E);
            double dfdssdfds = f * gamaC * 2;
            double dddfsdfdsf = N / (hsUonDoc * A);
            if (N / (hsUonDoc * A) <= f * gamaC)
            {
                return true;
            }
            return false;
        }

        public bool TinhToanOnDinhCucBo(double hw, double tw, double Lamda, double f, double E, double bo, double tf)
        {
            double domanhquyuoc = Lamda * Math.Sqrt(f / E);
            double domanhgioihanchophep = TTDoManhGioiHan(Lamda, f, E);
            double doManhGHBanNho = (0.36 + 0.1 * domanhquyuoc) * Math.Sqrt(E / f);
            if (hw / tw <= domanhgioihanchophep && bo / tf <= doManhGHBanNho)
            {
                return true;
            }
            return false;
        }

        public double TTDoManhGioiHan(double Lamda, double f, double E)
        {
            double domanhquyuoc = Lamda * Math.Sqrt(f / E);
            if (2 <= domanhquyuoc)
            {
                return (1.2 + 0.35 * domanhquyuoc) * Math.Sqrt(E / f);
            }
            else if (domanhquyuoc < 2)
            {
                return (1.3 + 0.15 * Math.Pow(domanhquyuoc, 2)) * Math.Sqrt(E / f);
            }
            return 0;
        }

        public double TTKhaNangChiuNenLechtam(double N, double f, double gamaC, double A, double Lamda, double E)
        {
            double KNTheoDKBen = A * f * gamaC;
            double hsUonDoc = TinhPhiMin(Lamda, f, E);
            double KNTheoOnDinhTongThe = hsUonDoc * A * f * gamaC;
            if (KNTheoDKBen < KNTheoOnDinhTongThe)
            {
                return KNTheoDKBen;
            }
            else if (KNTheoDKBen > KNTheoOnDinhTongThe)
            {
                return KNTheoOnDinhTongThe;
            }
            return 0;
        }

        public double DoManh(double tBanCanh, double tBanBung, double hBanCanh, double hBanBung)
        {
            double Ix = (hBanCanh * Math.Pow((hBanBung + tBanCanh * 2), 3) - (hBanCanh - tBanBung) * Math.Pow(hBanBung, 3)) / 12;
            double Iy = (2 * tBanCanh * Math.Pow(hBanCanh, 3) + hBanBung * Math.Pow(tBanBung, 3)) / 12;
            double A = 2 * (tBanCanh * hBanCanh) + (tBanBung * hBanBung);
            double lamdaX = Math.Sqrt(Ix / A);
            double lamdaY = Math.Sqrt(Iy / A);
            return Math.Max(lamdaX, lamdaY);
        }

        public double TinhPhiMin(double Lamda, double f, double E)
        {
            double domanhquyuoc = Lamda * Math.Sqrt(f / E);
            if (0 < domanhquyuoc && domanhquyuoc <= 2.5)
            {
                return 1 - (0.073 - 5.53 * f / E);
            }
            else if (2.5 < domanhquyuoc && domanhquyuoc <= 4.5)
            {
                return 1.47 - 13 * f / E - (0.371 - 27.3 * f / E) * domanhquyuoc + (0.0275 - 5.53f / E) * Math.Pow(domanhquyuoc, 2);
            }
            else if (domanhquyuoc > 4.5)
            {
                return 332 / Math.Pow(domanhquyuoc, 2) * (51 - domanhquyuoc);
            }
            return 0;
        }
    }
}
