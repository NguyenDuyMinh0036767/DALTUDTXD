using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public bool TinhToanOnDinhCucBo(double hw,
                                double tw,
                                double Lamda,
                                double f,
                                double E,
                                double bo,
                                double tf)
        {
            // Độ mảnh quy ước
            double lambda0 = Lamda * Math.Sqrt(f / E);

            // Giới hạn bản bụng
            double gioiHanBung;

            if (lambda0 <= 2)
                gioiHanBung = (1.30 + 0.15 * lambda0 * lambda0) * Math.Sqrt(E / f);
            else
                gioiHanBung = (1.20 + 0.35 * lambda0) * Math.Sqrt(E / f);

            // Giới hạn bản cánh
            double gioiHanCanh = (0.36 + 0.10 * lambda0) * Math.Sqrt(E / f);

            // Tỷ số thực tế
            double tySoBung = hw / tw;
            double tySoCanh = bo / tf;

            return (tySoBung <= gioiHanBung &&
                    tySoCanh <= gioiHanCanh);
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

        public double TTKhaNangChiuNenLechtam(double N, double f, double gamaC,
                                     double A, double Lamda, double E)
        {
            double hsUonDoc = TinhPhiMin(Lamda, f, E);

            double KNTheoDKBen = A * f * gamaC;
            double KNTheoOnDinhTongThe = hsUonDoc * A * f * gamaC;

            return Math.Min(KNTheoDKBen, KNTheoOnDinhTongThe) / 1000.0; // đổi sang kN
        }

        public double TinhLambda(double L, double i)
        {
            return L / i;
        }

        public double TinhPhiMin(double Lamda, double f, double E)
        {
            double lambdaBar = Lamda * Math.Sqrt(f / E);

            double phi;

            if (lambdaBar <= 0.2)
                phi = 1;

            else if (lambdaBar <= 1.0)
                phi = 1 - 0.25 * (lambdaBar - 0.2);

            else if (lambdaBar <= 2.0)
                phi = 0.8 - 0.25 * (lambdaBar - 1.0);

            else
                phi = 0.55 / (1 + 0.15 * (lambdaBar - 2));

            return phi;
        }
    }
}
