using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using ETABSv1;
using HUCE_DALTUD_LOPNV90_2026_0053867.Pages;

namespace HUCE_DALTUD_LOPNV90_2026_0053867.KetNoiEtabs
{
    public static class EtabsAPI_Ctrinhcon
    {
        // Thêm tham số (pageColumnParameters page) vào đây
        public static void DocETABSAPI_GetallFrame(pageColumnParameters page)
        {
            if (page == null) return;

            dynamic myETABSObject = null;
            dynamic mySapModel = null;

            try
            {
                myETABSObject = System.Runtime.InteropServices.Marshal.GetActiveObject("CSI.ETABS.API.ETABSObject");
                mySapModel = myETABSObject.GetSapModel();
                mySapModel.SetPresentUnits(eUnits.N_m_C);

                int numberNames = 0;
                string[] frameNames = null;
                int ret = mySapModel.FrameObj.GetNameList(ref numberNames, ref frameNames);

                if (ret != 0 || numberNames == 0) return;

                // Tìm kiếm lưới đồ họa trực tiếp trên giao diện của trang được truyền sang
                var container = page.FindName("modelContainer") as ModelVisual3D;
                var view3D = page.FindName("view3D") as HelixViewport3D;

                if (container != null)
                {
                    container.Children.Clear();
                    var khungEtabs = new LinesVisual3D { Color = Colors.DodgerBlue, Thickness = 2.5 };

                    foreach (string frameName in frameNames)
                    {
                        string point1 = "", point2 = "";
                        mySapModel.FrameObj.GetPoints(frameName, ref point1, ref point2);

                        double x1 = 0, y1 = 0, z1 = 0;
                        double x2 = 0, y2 = 0, z2 = 0;

                        mySapModel.PointObj.GetCoordXYZ(point1, ref x1, ref y1, ref z1);
                        mySapModel.PointObj.GetCoordXYZ(point2, ref x2, ref y2, ref z2);

                        khungEtabs.Points.Add(new Point3D(x1, y1, z1));
                        khungEtabs.Points.Add(new Point3D(x2, y2, z2));
                    }

                    container.Children.Add(khungEtabs);
                    view3D?.ZoomExtents();
                    MessageBox.Show($"Đồng bộ thành công {numberNames} cấu kiện thanh!", "Thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối API ETABS: " + ex.Message, "Thông báo lỗi");
            }
        }
    }
}