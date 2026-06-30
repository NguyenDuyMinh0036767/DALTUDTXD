using ETABSv1;
using HelixToolkit.Wpf;
using HUCE_DALTUD_LOPNV90_2026_0053867.Class;
using HUCE_DALTUD_LOPNV90_2026_0053867.KetNoiEtabs;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace HUCE_DALTUD_LOPNV90_2026_0053867.Pages
{
    // BẮT BUỘC: Hàm bổ trợ xử lý sự kiện UI phải nằm trong partial class của Page
    public partial class pageColumnParameters : Page
    {
        public pageColumnParameters()
        {
            InitializeComponent();
        }

        // ĐÃ THÊM: Định nghĩa hàm LoadDL ở chế độ public để file ViewModel gọi được
        public void LoadDL()
        {
            // Nếu có lệnh nạp ItemSource cho các ComboBox vật liệu/tiết diện thì viết ở đây.
            // Nếu không cần xử lý gì thêm, bạn có thể để trống hàm này để tránh lỗi compile.
        }

        // ĐÃ CẬP NHẬT: Xử lý hiển thị dữ liệu chi tiết lên các TextBox/ComboBox phía trên khi click chọn dòng
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtgColumn.SelectedItem != null)
            {
                try
                {
                    clsColumn selectedCot = dtgColumn.SelectedItem as clsColumn;

                    if (selectedCot != null)
                    {
                        txtName.Text = selectedCot.Name;
                        txtDaiCot.Text = selectedCot.ChieuCao.ToString();
                        txtTaiTrongchan.Text = selectedCot.LucDoc.ToString();
                        txtTaiTrongdinh.Text = "0";
                        txtMoMenchan.Text = selectedCot.MoMent.ToString();

                        // ĐÃ SỬA: Chọn theo SelectedItem (vì ItemsSource bây giờ là danh sách chuỗi string)
                        if (selectedCot.TietDien != null)
                        {
                            cbbTietDien.SelectedItem = selectedCot.TietDien.Name;
                        }

                        if (selectedCot.VatLieu != null)
                        {
                            cbbVatLieu.SelectedItem = selectedCot.VatLieu.TenVatLieu;
                        }

                        cbbLK.SelectedIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi khi hiển thị thông số chi tiết: " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        // Hàm xử lý logic nhận lệnh từ nút bấm Nhập dữ liệu từ phần mềm ETABS
        public void XulyNhapTuEtab(object parameter)
        {
            var page = parameter as pageColumnParameters;
            if (page == null) return;

            cOAPI myETABSObject = null;
            cSapModel mySapModel = null;

            try
            {
                myETABSObject = (cOAPI)System.Runtime.InteropServices.Marshal
                    .GetActiveObject("CSI.ETABS.API.ETABSObject");
                mySapModel = myETABSObject.SapModel;

                // Đồng bộ hóa đơn vị sang dạng chuẩn (Newton, mét)
                mySapModel.SetPresentUnits(ETABSv1.eUnits.N_m_C);
                mySapModel.Results.Setup.DeselectAllCasesAndCombosForOutput();

                // Thay COMB1 bằng đúng tên tổ hợp trong ETABS của bạn
                mySapModel.Results.Setup.SetComboSelectedForOutput("BAO");
                int numberNames = 0;
                string[] frameNames = null;
                int ret = mySapModel.FrameObj.GetNameList(ref numberNames, ref frameNames);

                if (ret != 0 || numberNames == 0) return;

                var container = page.FindName("modelContainer") as ModelVisual3D;
                var view3D = page.FindName("view3D") as HelixViewport3D;

                if (container != null)
                {
                    container.Children.Clear();
                    clsBienToanCuc.clsColumn.Clear();
                    var khungEtabsThucTe = new LinesVisual3D { Color = System.Windows.Media.Colors.DodgerBlue, Thickness = 2.5 };

                    foreach (string frameName in frameNames)
                    {
                        int NumberResults = 0;

                        string[] Obj = null;
                        double[] ObjSta = null;
                        string[] Elm = null;
                        double[] ElmSta = null;
                        string[] LoadCase = null;
                        string[] StepType = null;
                        double[] StepNum = null;

                        double[] P = null;
                        double[] V2 = null;
                        double[] V3 = null;
                        double[] T = null;
                        double[] M2 = null;
                        double[] M3 = null;

                        int retForce = mySapModel.Results.FrameForce(
                            frameName,
                            eItemTypeElm.ObjectElm,
                            ref NumberResults,
                            ref Obj,
                            ref ObjSta,
                            ref Elm,
                            ref ElmSta,
                            ref LoadCase,
                            ref StepType,
                            ref StepNum,
                            ref P,
                            ref V2,
                            ref V3,
                            ref T,
                            ref M2,
                            ref M3);

                        double lucDocChan = 0;
                        double lucDocDinh = 0;
                        double momentChan = 0;

                        if (retForce == 0 && NumberResults > 0)
                        {
                            // Lấy vị trí mặt cắt cuối cùng làm mốc chiều dài thanh để xác định đỉnh cột
                            double chieuDaiThanh = ElmSta[NumberResults - 1];

                            for (int i = 0; i < NumberResults; i++)
                            {
                                // 1. Lấy nội lực tại vị trí Chân Cột (vị trí bằng 0)
                                if (Math.Abs(ElmSta[i]) < 0.001)
                                {
                                    if (Math.Abs(P[i]) > Math.Abs(lucDocChan)) lucDocChan = P[i];
                                    if (Math.Abs(M3[i]) > Math.Abs(momentChan)) momentChan = M3[i];
                                }
                                // 2. Lấy nội lực tại vị trí Đỉnh Cột (vị trí bằng chiều dài thanh)
                                else if (Math.Abs(ElmSta[i] - chieuDaiThanh) < 0.001)
                                {
                                    if (Math.Abs(P[i]) > Math.Abs(lucDocDinh)) lucDocDinh = P[i];
                                }
                            }
                        }
                        string point1 = "", point2 = "";
                        mySapModel.FrameObj.GetPoints(frameName, ref point1, ref point2);

                        double x1 = 0, y1 = 0, z1 = 0;
                        double x2 = 0, y2 = 0, z2 = 0;

                        string csys = "Global";

                        // Lấy tọa độ điểm đầu
                        mySapModel.PointObj.GetCoordCartesian(
                            point1, ref x1, ref y1, ref z1, csys);

                        // Lấy tọa độ điểm cuối
                        mySapModel.PointObj.GetCoordCartesian(
                            point2, ref x2, ref y2, ref z2, csys);

                        // Kiểm tra có phải cột không
                        bool laCot =
                               Math.Abs(x1 - x2) < 0.01 &&
                               Math.Abs(y1 - y2) < 0.01 &&
                               Math.Abs(z1 - z2) > 0.01;

                        if (!laCot)
                            continue;
                        khungEtabsThucTe.Points.Add(new Point3D(x1, y1, z1));
                        khungEtabsThucTe.Points.Add(new Point3D(x2, y2, z2));
                        double chieuCao = Math.Abs(z2 - z1);
                        string label = "";
                        string story = "";

                        mySapModel.FrameObj.GetLabelFromName(
                            frameName,
                            ref label,
                            ref story);

                        string tenTietDien = "";
                        string autoSelect = "";

                        mySapModel.FrameObj.GetSection(
                            frameName,
                            ref tenTietDien,
                            ref autoSelect);
                        //==================== ĐỌC KÍCH THƯỚC TIẾT DIỆN ====================

                        double h = 0;
                        double b = 0;
                        double tf = 0;
                        double tw = 0;
                        double t2b = 0;
                        double tfb = 0;

                        string fileName = "";
                        string tenVatLieuTietDien = "";
                        string notes = "";
                        string guid = "";
                        int color = 0;

                        int retSection = mySapModel.PropFrame.GetISection(
                            tenTietDien,
                            ref fileName,
                            ref tenVatLieuTietDien,
                            ref h,
                            ref b,
                            ref tf,
                            ref tw,
                            ref t2b,
                            ref tfb,
                            ref color,
                            ref notes,
                            ref guid);


                        //==================== TIẾT DIỆN ====================

                        clsTietDien tietDien = null;

                        foreach (clsTietDien td in clsBienToanCuc.clsTietDien)
                        {
                            if (td.Name == tenTietDien)
                            {
                                tietDien = td;
                                break;
                            }
                        }

                        if (tietDien == null)
                        {
                            tietDien = new clsTietDien(
                                tenTietDien,
                                b * 1000,
                                tf * 1000,
                                h * 1000,
                                tw * 1000);

                            clsBienToanCuc.clsTietDien.Add(tietDien);
                        }

                        //==================== VẬT LIỆU ====================

                        string tenVatLieu = "";

                        mySapModel.PropFrame.GetMaterial(
                            tenTietDien,
                            ref tenVatLieu);

                        clsVatLieuThep vatLieu = null;

                        foreach (clsVatLieuThep vl in clsBienToanCuc.clsVatLieu)
                        {
                            if (vl.TenVatLieu == tenVatLieu)
                            {
                                vatLieu = vl;
                                break;
                            }
                        }

                        if (vatLieu == null)
                        {
                            vatLieu = new clsVatLieuThep(
                                tenVatLieu,
                                0,
                                0);

                            clsBienToanCuc.clsVatLieu.Add(vatLieu);
                        }

                        //==================== TẠO CỘT ====================

                        // Sử dụng hàm dựng (constructor) mới đã bổ sung thêm biến lực dọc đỉnh
                        clsColumn cot = new clsColumn(
                                 label,
                                 chieuCao,
                                 vatLieu,
                                 tietDien,
                                 Math.Abs(lucDocChan), // Lực dọc chân
                                 Math.Abs(lucDocDinh), // Lực dọc đỉnh (Đã map vào thuộc tính mới)
                                 Math.Abs(momentChan), // Momen chân
                                 story
                            );

                        clsBienToanCuc.clsColumn.Add(cot);
                    }

                    container.Children.Add(khungEtabsThucTe);

                    page.dtgColumn.ItemsSource = null;
                    page.dtgColumn.ItemsSource = clsBienToanCuc.clsColumn;

                    // Tietdien
                    if (clsBienToanCuc.clsColumn != null && clsBienToanCuc.clsColumn.Count > 0)
                    {
                        // Tạo 2 danh sách phụ dạng chuỗi (string) để chứa tên tiết diện và vật liệu duy nhất
                        System.Collections.Generic.List<string> dsTietDien = new System.Collections.Generic.List<string>();
                        System.Collections.Generic.List<string> dsVatLieu = new System.Collections.Generic.List<string>();

                        foreach (var c in clsBienToanCuc.clsColumn)
                        {
                            // Nếu cột có tiết diện và tên tiết diện này chưa có trong danh sách phụ thì mới thêm vào
                            if (c.TietDien != null && !dsTietDien.Contains(c.TietDien.Name))
                            {
                                dsTietDien.Add(c.TietDien.Name);
                            }

                            // Tương tự với vật liệu
                            if (c.VatLieu != null && !dsVatLieu.Contains(c.VatLieu.TenVatLieu))
                            {
                                dsVatLieu.Add(c.VatLieu.TenVatLieu);
                            }
                        }

                        // Đổ dữ liệu đã lọc trùng vào ItemsSource của 2 ô ComboBox trên giao diện
                        page.cbbTietDien.ItemsSource = dsTietDien;
                        page.cbbVatLieu.ItemsSource = dsVatLieu;
                    }

                    view3D?.ZoomExtents();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối ETABS API: " + ex.Message);
            }
        }
    }
}