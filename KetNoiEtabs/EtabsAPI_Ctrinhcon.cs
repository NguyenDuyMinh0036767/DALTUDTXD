using HUCE_DALTUD_LOPNV90_2026_0053867.Class;
using ETABSv1;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HUCE_DALTUD_LOPNV90_2026_0053867.KetNoiEtabs
{
    public class EtabsAPI_Ctrinhcon
    {
        public static double chieuCao = 0;
        public static clsVatLieuThep VatLieu = null;
        public static HashSet<clsVatLieuThep> vatLieuTheps = new HashSet<clsVatLieuThep>();


        public static void DocETABSAPI_GetallFrame()
        {
            // Khởi tạo biến API
            cOAPI etabsApi = null;
            cSapModel sapModel = null;

            try
            {
                // Kết nối với phiên bản ETABS đang chạy
                cHelper helper = new Helper();
                etabsApi = helper.GetObject("CSI.ETABS.API.ETABSObject") as cOAPI;

                if (etabsApi == null)
                {
                    MessageBox.Show("Không thể kết nối với ETABS.");
                    return;
                }

                // Lấy mô hình đang mở
                sapModel = etabsApi.SapModel;

                // Lấy danh sách frame trong mô hình
                int numFrames = 0;
                string[] frameNames = new string[0];
                sapModel.FrameObj.GetNameList(ref numFrames, ref frameNames);

                foreach (var frame in frameNames)
                {
                    //GetLoad(sapModel, frame);
                    string Name = "";
                    string sAuto = "";

                    // Lấy tiết diện của phần tử
                    int retSection = sapModel.FrameObj.GetSection(frame, ref Name, ref sAuto);
                    if (retSection != 0) continue;

                    // Biến chứa thông số chữ I
                    string fileName = "", matProp = "", notes = "", guid = "";
                    double caobung = 0, rongcanh = 0, daycanh = 0, daybung = 0, T2b = 0, Tfb = 0;
                    int color = 0;
                    // Lấy thông số tiết diện chữ I
                    int retISection = sapModel.PropFrame.GetISection(Name, ref fileName, ref matProp, ref caobung, ref rongcanh, ref daycanh, ref daybung, ref T2b, ref Tfb, ref color, ref notes, ref guid);

                    string label = "";
                    string story = "";
                    int ret = sapModel.FrameObj.GetLabelFromName(frame, ref label, ref story);

                    (string loai, double lucdoc, double moment) = GetLoad(sapModel, frame);

                    GetMaterial(sapModel, matProp);
                    GetHeight(sapModel, frame);
                    if (retISection == 0)
                    {
                        clsTietDien tietDien = new clsTietDien(Name, rongcanh, daycanh, caobung, daybung);
                        clsBienToanCuc.clsTietDien.Add(tietDien);
                        clsBienToanCuc.clsColumn.Add(new clsColumn(label, chieuCao, VatLieu, tietDien, lucdoc, moment, story));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                // Đóng kết nối
                if (etabsApi != null)
                {
                    etabsApi = null;
                }
            }
        }
        public static (string loai, double lucdoc, double moment) GetLoad(cSapModel sapModel, string frame)
        {
            string loai = "";
            double lucdoc = 0;
            double moment = 0;
            int numLoads = 0;
            string[] frameNamesOut = new string[0];
            string[] loadPatterns = new string[0];
            int[] myType = new int[0];
            string[] coordSystem = new string[0];
            int[] dir = new int[0];
            double[] relDist = new double[0];
            double[] dist = new double[0];
            double[] values = new double[0];

            // Lấy tải trọng tập trung gán lên cột
            int retLoad = sapModel.FrameObj.GetLoadPoint(frame, ref numLoads, ref frameNamesOut, ref loadPatterns, ref myType, ref coordSystem, ref dir, ref relDist, ref dist, ref values);

            if (retLoad == 0 && numLoads > 0)
            {
                for (int i = 0; i < numLoads; i++)
                {
                    string loadType = myType[i] == 1 ? "Force" : "Moment";
                    if (loadType == "Moment")
                    {
                        loai = loadType;
                        moment = values[i];
                    }
                    else if (loadType == "Force")
                    {
                        loai = loadType;
                        lucdoc = values[i];
                    }
                }
            }
            return (loai, lucdoc, moment);
        }
        public static void GetMaterial(cSapModel sapModel, string matProp)
        {
            //Lấy vật liệu

            double Fy = 0, Fu = 0, EFy = 0, EFu = 0;
            int SSType = 0, SSHysType = 0;
            double StrainAtHardening = 0, StrainAtMaxStress = 0, StrainAtRupture = 0;
            double Temp = 0; // Bạn có thể để là 0 nếu không xét theo nhiệt độ

            int ret = sapModel.PropMaterial.GetOSteel(
                matProp,
                ref Fy,
                ref Fu,
                ref EFy,
                ref EFu,
                ref SSType,
                ref SSHysType,
                ref StrainAtHardening,
                ref StrainAtMaxStress,
                ref StrainAtRupture,
                Temp
            );
            if (ret == 0)
            {
                VatLieu = new clsVatLieuThep(matProp, Fy, Fu);
                vatLieuTheps.Add(VatLieu);
                foreach (var vatLieu in vatLieuTheps)
                {
                    clsBienToanCuc.clsVatLieu.Add(vatLieu);
                }
            }
        }
        public static void GetHeight(cSapModel sapModel, string frame)
        {
            // Lấy toạ độ hai đầu của phần tử
            double x1 = 0, y1 = 0, z1 = 0;
            double x2 = 0, y2 = 0, z2 = 0;
            string point1 = "";
            string point2 = "";
            //Lấy chiều cao
            int retPoints = sapModel.FrameObj.GetPoints(frame, ref point1, ref point2);
            if (retPoints == 0)
            {
                sapModel.PointObj.GetCoordCartesian(point1, ref x1, ref y1, ref z1);
                sapModel.PointObj.GetCoordCartesian(point2, ref x2, ref y2, ref z2);

                // Tính chiều cao (theo Z)
                chieuCao = Math.Abs(z2 - z1);
            }
        }
    }
}
