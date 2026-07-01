using System;
using System.Collections.Generic;
using System.ComponentModel; // Bắt buộc phải có để dùng INotifyPropertyChanged
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUCE_DALTUD_LOPNV90_2026_0053867.Class
{
    public class clsVatLieuThep : INotifyPropertyChanged
    {
        private string _tenVatLieu;
        private double _cuongDoTinhToanF;
        private double _moDunDanHoiE;

        public string TenVatLieu
        {
            get { return _tenVatLieu; }
            set
            {
                _tenVatLieu = value;
                OnPropertyChanged(nameof(TenVatLieu));
            }
        }

        public double CuongDoTinhToanF
        {
            get { return _cuongDoTinhToanF; }
            set
            {
                _cuongDoTinhToanF = value;
                OnPropertyChanged(nameof(CuongDoTinhToanF));
            }
        }

        public double MoDunDanHoiE
        {
            get { return _moDunDanHoiE; }
            set
            {
                _moDunDanHoiE = value;
                OnPropertyChanged(nameof(MoDunDanHoiE));
            }
        }

        // Hàm khởi tạo (Constructor)
        public clsVatLieuThep(string tenVatLieu, double cuongDoTinhToanF, double moDunDanHoiE)
        {
            TenVatLieu = tenVatLieu;
            CuongDoTinhToanF = cuongDoTinhToanF;
            MoDunDanHoiE = moDunDanHoiE;
        }

        // Sự kiện thông báo thay đổi thuộc tính cho WPF Binding
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}