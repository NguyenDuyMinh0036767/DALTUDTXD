using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUCE_DALTUD_LOPNV90_2026_0053867.Class
{
    public class clsColumn
    {
        private string _Name;
        private double _chieuCao;
        private clsVatLieuThep _vatLieu;
        private clsTietDien _tietDien;
        private string _Story;
        private double _lucDoc;
        private double _moMent;


        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public string Story
        {
            get { return _Story; }
            set { _Story = value; }
        }
        public double ChieuCao
        {
            get { return _chieuCao; }
            set { _chieuCao = value; }
        }


        public clsVatLieuThep VatLieu
        {
            get { return _vatLieu; }
            set { _vatLieu = value; }
        }
        public clsTietDien TietDien
        {
            get { return _tietDien; }
            set { _tietDien = value; }
        }
        public double LucDoc
        {
            get { return _lucDoc; }
            set { _lucDoc = value; }
        }

        public double MoMent
        {
            get { return _moMent; }
            set { _moMent = value; }
        }

        public clsColumn(string name, double chieucao, clsVatLieuThep vatlieu, clsTietDien tietdien, double lucdoc, double moment, string story)
        {
            Name = name;
            ChieuCao = chieucao;
            VatLieu = vatlieu;
            TietDien = tietdien;
            Story = story;
            LucDoc = lucdoc;
            MoMent = moment;
        }

        public clsColumn(string name, double chieucao, clsVatLieuThep vatlieu, clsTietDien tietdien, double lucdoc, double moment)
        {
            Name = name;
            ChieuCao = chieucao;
            VatLieu = vatlieu;
            TietDien = tietdien;
            LucDoc = lucdoc;
            MoMent = moment;
        }
    }
}
