using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows; // Thêm thư viện này để hiển thị thông báo MessageBox khi lưu dữ liệu
using System.Windows.Input;
using HUCE_DALTUD_LOPNV90_2026_0053867.Class;
using HUCE_DALTUD_LOPNV90_2026_0053867.KetNoiEtabs;
using HUCE_DALTUD_LOPNV90_2026_0053867.Pages;

namespace HUCE_DALTUD_LOPNV90_2026_0053867.ViewModels
{
    public class ColumnParametersVM
    {
        public ICommand cmNhapEtab { get; set; }
        public ICommand cmLuuCot { get; set; }

        public ColumnParametersVM()
        {
            cmNhapEtab = new RelayCommand<pageColumnParameters>((parameter) => true, (parameter) => LayThongTinTuEtab(parameter));
            cmLuuCot = new RelayCommand<pageColumnParameters>((parameter) => true, (parameter) => LuuCot(parameter));
        }

        public void LayThongTinTuEtab(pageColumnParameters parameter)
        {
            if (parameter == null) return;

            // ĐỒNG BỘ TẠI ĐÂY: Bỏ hàm gọi cũ ở lớp Ctrinhcon. 
            // Gọi trực tiếp hàm XulyNhapTuEtab chuẩn hóa đã sửa lỗi ép kiểu (GetSapModel) ở file Code-behind
            parameter.XulyNhapTuEtab(parameter);

            // Nạp lại dữ liệu ComboBox hiển thị
            parameter.LoadDL();
        }

        public void LuuCot(pageColumnParameters parameter)
        {
            if (parameter == null) return;

            // 1. Kiểm tra an toàn dữ liệu: Không để trống tên cột
            if (string.IsNullOrWhiteSpace(parameter.txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên cột trước khi thực hiện lưu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. GIẢI QUYẾT LỖI FORMATEXCEPTION: Dùng TryParse chuyển đổi số an toàn
            // Nếu người dùng nhập sai, bỏ trống hoặc nhập ký tự, hệ thống tự gán = 0 thay vì làm sập ứng dụng
            double.TryParse(parameter.txtDaiCot.Text, out double daiCot);
            double.TryParse(parameter.txtTaiTrongchan.Text, out double taiTrongChan);
            double.TryParse(parameter.txtMoMenchan.Text, out double moMenChan);

            // Kiểm tra tính hợp lệ cơ bản của chiều dài cấu kiện
            if (daiCot <= 0)
            {
                MessageBox.Show("Chiều dài cột (L) phải là một số lớn hơn 0!", "Dữ liệu không hợp lệ", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 3. Tìm tiết diện được chọn từ ComboBox
            clsTietDien clsTietDien1 = null;
            foreach (clsTietDien clsTietDien in clsBienToanCuc.clsTietDien)
            {
                if (clsTietDien.Name == parameter.cbbTietDien.Text)
                {
                    clsTietDien1 = clsTietDien;
                    break; // Thoát nhanh vòng lặp khi đã tìm thấy đối tượng
                }
            }

            // 4. Tìm vật liệu được chọn từ ComboBox
            clsVatLieuThep clsVatLieuThep = null;
            foreach (clsVatLieuThep clsVatLieu in clsBienToanCuc.clsVatLieu)
            {
                if (clsVatLieu.TenVatLieu == parameter.cbbVatLieu.Text)
                {
                    clsVatLieuThep = clsVatLieu;
                    break; // Thoát nhanh vòng lặp khi đã tìm thấy đối tượng
                }
            }

            try
            {
                // 5. Khởi tạo đối tượng và nạp vào danh sách dữ liệu cột toàn cục
                clsBienToanCuc.clsColumn.Add
                (
                    new clsColumn(
                        parameter.txtName.Text,
                        daiCot,
                        clsVatLieuThep,
                        clsTietDien1,
                        taiTrongChan,
                        moMenChan
                    )
                );

                MessageBox.Show($"Đã lưu thành công cấu kiện cột '{parameter.txtName.Text}' vào danh sách tính toán!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                // 6. Làm sạch các TextBox sau khi lưu thành công để tiện nhập tiếp
                parameter.txtName.Clear();
                parameter.txtDaiCot.Clear();
                parameter.txtTaiTrongchan.Clear();
                parameter.txtMoMenchan.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi không xác định khi khởi tạo đối tượng cột: " + ex.Message, "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}