using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HUCE_DALTUD_LOPNV90_2026_0053867.Class
{
    public class clsTietDien
    {
        private string _name;
        private double _chieuRongCanh;
        private double _doDayCanh;
        private double _chieuCaoBung;
        private double _doDayBung;
        private Point _origin;
        private Path _matcat;

        public Point Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }
        public Path MatCat
        {
            get { return _matcat; }
            set { _matcat = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public double ChieuRongCanh
        {
            get { return _chieuRongCanh; }
            set { _chieuRongCanh = value; }
        }

        public double DoDayCanh
        {
            get { return _doDayCanh; }
            set { _doDayCanh = value; }
        }

        public double ChieuCaoBung
        {
            get { return _chieuCaoBung; }
            set { _chieuCaoBung = value; }
        }

        public double DoDayBung
        {
            get { return _doDayBung; }
            set { _doDayBung = value; }
        }


        public double TinhDienTichTietDien()
        {
            double dienTichCanh = ChieuRongCanh * DoDayCanh;
            double dienTichBung = ChieuCaoBung * DoDayBung;
            return 2 * dienTichCanh + dienTichBung;
        }
        public bool XacThucTietDien()
        {
            if (ChieuCaoBung > 0 && ChieuRongCanh > 0 && DoDayBung > 0 && DoDayCanh > 0)
            {
                if (DoDayBung < DoDayCanh)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Chiều dày bản bụng phải nhỏ hơn chiều dày bản cánh.");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Thuộc tính cột không hợp lệ.");
                return false;
            }
        }


        public clsTietDien(string name, double chieuRongCanh, double doDayCanh, double chieuCaoBung, double doDayBung, Point origin)
        {
            Name = name;
            ChieuRongCanh = chieuRongCanh;
            DoDayCanh = doDayCanh;
            ChieuCaoBung = chieuCaoBung;
            DoDayBung = doDayBung;
            Origin = origin;
            _matcat = VeMC_Cot(Origin, ChieuRongCanh, DoDayCanh, ChieuCaoBung, DoDayBung);
            // Kiểm tra giá trị
            //if (!XacThucTietDien())
            //{
            //    ResetValues();
            //}
        }
        public clsTietDien(string name, double chieuRongCanh, double doDayCanh, double chieuCaoBung, double doDayBung)
        {
            Name = name;
            ChieuRongCanh = chieuRongCanh;
            DoDayCanh = doDayCanh;
            ChieuCaoBung = chieuCaoBung;
            DoDayBung = doDayBung;
            // Kiểm tra giá trị
            //if (!XacThucTietDien())
            //{
            //    ResetValues();
            //}
        }
        // Hàm để reset các giá trị nếu không hợp lệ
        private void ResetValues()
        {
            ChieuRongCanh = 0;
            DoDayCanh = 0;
            ChieuCaoBung = 0;
            DoDayBung = 0;
        }
        public Path VeMC_Cot(Point Origin, double rongcanh, double daycanh, double caobung, double daybung)
        {
            double B = rongcanh;
            double H = caobung + daycanh * 2;
            //1. Khởi tạo đối tượng MC thông Path
            Path Path_MC = new Path();
            Path_MC.Name = "Cot";

            //2. Khởi tạo Path.Data
            PathGeometry path_geo_MC = new PathGeometry();
            PathFigure pathFigure_MC = new PathFigure();
            pathFigure_MC.StartPoint = new Point(Origin.X - B / 2, Origin.Y - H / 2); //điểm 0
            PolyLineSegment PolySeg_MC = new PolyLineSegment();

            //3. Thêm các điểm cho polyline
            //PolySeg_MC.Points.Add(new Point(Origin.X - B/2, Origin.Y - H/2));  //điểm 1
            PolySeg_MC.Points.Add(new Point(Origin.X + B / 2, Origin.Y - H / 2));  //điểm 1
            PolySeg_MC.Points.Add(new Point(Origin.X + B / 2, Origin.Y - H / 2 + daycanh));  //điểm 2
            PolySeg_MC.Points.Add(new Point(Origin.X + daybung / 2, Origin.Y - H / 2 + daycanh));  //điểm 3
            PolySeg_MC.Points.Add(new Point(Origin.X + daybung / 2, Origin.Y + H / 2 - daycanh));  //điểm 4
            PolySeg_MC.Points.Add(new Point(Origin.X + B / 2, Origin.Y + H / 2 - daycanh));  //điểm 5
            PolySeg_MC.Points.Add(new Point(Origin.X + B / 2, Origin.Y + H / 2)); //điểm 6
            PolySeg_MC.Points.Add(new Point(Origin.X - B / 2, Origin.Y + H / 2)); //điểm 7
            PolySeg_MC.Points.Add(new Point(Origin.X - B / 2, Origin.Y + H / 2 - daycanh)); //điểm 8
            PolySeg_MC.Points.Add(new Point(Origin.X - daybung / 2, Origin.Y + H / 2 - daycanh)); //điểm 9
            PolySeg_MC.Points.Add(new Point(Origin.X - daybung / 2, Origin.Y - H / 2 + daycanh)); //điểm 10
            PolySeg_MC.Points.Add(new Point(Origin.X - B / 2, Origin.Y - H / 2 + daycanh)); //điểm 11
            PolySeg_MC.Points.Add(new Point(Origin.X - B / 2, Origin.Y - H / 2));
            PolySeg_MC.Points.Add(new Point(Origin.X + B / 2, Origin.Y - H / 2));


            //4. Thêm Polyline vào PathFigure
            pathFigure_MC.Segments.Add(PolySeg_MC);

            //5. Thêm PathFigure vào PathGeometry
            path_geo_MC.Figures.Add(pathFigure_MC);

            //6. Thêm PathGeometry vào Path.Data
            Path_MC.Data = path_geo_MC;

            //7. Tô
            Path_MC.Fill = Brushes.Gray;
            Path_MC.StrokeThickness = 1;
            Path_MC.Stroke = Brushes.Blue;
            return Path_MC;
        }
    }
}
