﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Book.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class DoAnEntities1 : DbContext
    {
        public DoAnEntities1()
            : base("name=DoAnEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public virtual DbSet<DonHang> DonHangs { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<NguoiQuanLy> NguoiQuanLies { get; set; }
        public virtual DbSet<NhaXuatBan> NhaXuatBans { get; set; }
        public virtual DbSet<Sach> Saches { get; set; }
        public virtual DbSet<DatHang> DatHangs { get; set; }
        public virtual DbSet<Gio> Gios { get; set; }
        public virtual DbSet<View_KH> View_KH { get; set; }
        public virtual DbSet<view_thongtinKH> view_thongtinKH { get; set; }
    
        [DbFunction("DoAnEntities1", "SumMoney")]
        public virtual IQueryable<SumMoney_Result> SumMoney(Nullable<int> maHD)
        {
            var maHDParameter = maHD.HasValue ?
                new ObjectParameter("MaHD", maHD) :
                new ObjectParameter("MaHD", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<SumMoney_Result>("[DoAnEntities1].[SumMoney](@MaHD)", maHDParameter);
        }
    
        public virtual ObjectResult<BookNotOrder_Result> BookNotOrder()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BookNotOrder_Result>("BookNotOrder");
        }
    
        public virtual ObjectResult<bookyear2_Result> bookyear2()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<bookyear2_Result>("bookyear2");
        }
    
        public virtual int Don(Nullable<int> maKH, Nullable<System.DateTime> ngayDat, string maSach, Nullable<int> soLuong)
        {
            var maKHParameter = maKH.HasValue ?
                new ObjectParameter("MaKH", maKH) :
                new ObjectParameter("MaKH", typeof(int));
    
            var ngayDatParameter = ngayDat.HasValue ?
                new ObjectParameter("NgayDat", ngayDat) :
                new ObjectParameter("NgayDat", typeof(System.DateTime));
    
            var maSachParameter = maSach != null ?
                new ObjectParameter("MaSach", maSach) :
                new ObjectParameter("MaSach", typeof(string));
    
            var soLuongParameter = soLuong.HasValue ?
                new ObjectParameter("SoLuong", soLuong) :
                new ObjectParameter("SoLuong", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Don", maKHParameter, ngayDatParameter, maSachParameter, soLuongParameter);
        }
    
        public virtual ObjectResult<recommend_Result> recommend(string theloai)
        {
            var theloaiParameter = theloai != null ?
                new ObjectParameter("theloai", theloai) :
                new ObjectParameter("theloai", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<recommend_Result>("recommend", theloaiParameter);
        }
    
        public virtual ObjectResult<searchBook_Result> searchBook(string tenSach)
        {
            var tenSachParameter = tenSach != null ?
                new ObjectParameter("tenSach", tenSach) :
                new ObjectParameter("tenSach", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<searchBook_Result>("searchBook", tenSachParameter);
        }
    
        public virtual ObjectResult<SoSachBanTrongNam_Result> SoSachBanTrongNam(Nullable<System.DateTime> date)
        {
            var dateParameter = date.HasValue ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SoSachBanTrongNam_Result>("SoSachBanTrongNam", dateParameter);
        }
    
        public virtual ObjectResult<SoSachBanTrongThang_Result> SoSachBanTrongThang(Nullable<System.DateTime> date)
        {
            var dateParameter = date.HasValue ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SoSachBanTrongThang_Result>("SoSachBanTrongThang", dateParameter);
        }
    
        public virtual ObjectResult<SoSachBanTrongTuan_Result> SoSachBanTrongTuan(Nullable<System.DateTime> date)
        {
            var dateParameter = date.HasValue ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SoSachBanTrongTuan_Result>("SoSachBanTrongTuan", dateParameter);
        }
    
        public virtual ObjectResult<ThongKe_Result> ThongKe(Nullable<System.DateTime> dateStart, Nullable<System.DateTime> dateEnd, string chucnang)
        {
            var dateStartParameter = dateStart.HasValue ?
                new ObjectParameter("dateStart", dateStart) :
                new ObjectParameter("dateStart", typeof(System.DateTime));
    
            var dateEndParameter = dateEnd.HasValue ?
                new ObjectParameter("dateEnd", dateEnd) :
                new ObjectParameter("dateEnd", typeof(System.DateTime));
    
            var chucnangParameter = chucnang != null ?
                new ObjectParameter("chucnang", chucnang) :
                new ObjectParameter("chucnang", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ThongKe_Result>("ThongKe", dateStartParameter, dateEndParameter, chucnangParameter);
        }
    
        public virtual ObjectResult<ThongKe2_Result> ThongKe2(Nullable<System.DateTime> dateStart)
        {
            var dateStartParameter = dateStart.HasValue ?
                new ObjectParameter("dateStart", dateStart) :
                new ObjectParameter("dateStart", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ThongKe2_Result>("ThongKe2", dateStartParameter);
        }
    
        public virtual int TongTien(Nullable<int> maKH)
        {
            var maKHParameter = maKH.HasValue ?
                new ObjectParameter("MaKH", maKH) :
                new ObjectParameter("MaKH", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TongTien", maKHParameter);
        }
    
        public virtual int TraTien(Nullable<int> maKH, Nullable<decimal> tien, Nullable<System.DateTime> ngayNhan)
        {
            var maKHParameter = maKH.HasValue ?
                new ObjectParameter("MaKH", maKH) :
                new ObjectParameter("MaKH", typeof(int));
    
            var tienParameter = tien.HasValue ?
                new ObjectParameter("Tien", tien) :
                new ObjectParameter("Tien", typeof(decimal));
    
            var ngayNhanParameter = ngayNhan.HasValue ?
                new ObjectParameter("NgayNhan", ngayNhan) :
                new ObjectParameter("NgayNhan", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("TraTien", maKHParameter, tienParameter, ngayNhanParameter);
        }
    
        public virtual ObjectResult<trongnam_Result> trongnam(Nullable<int> nam)
        {
            var namParameter = nam.HasValue ?
                new ObjectParameter("nam", nam) :
                new ObjectParameter("nam", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<trongnam_Result>("trongnam", namParameter);
        }
    
        public virtual ObjectResult<trongthang_Result> trongthang(Nullable<System.DateTime> date)
        {
            var dateParameter = date.HasValue ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<trongthang_Result>("trongthang", dateParameter);
        }
    
        public virtual ObjectResult<trongtuan_Result> trongtuan(Nullable<System.DateTime> date)
        {
            var dateParameter = date.HasValue ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<trongtuan_Result>("trongtuan", dateParameter);
        }
    
        public virtual ObjectResult<ViewCart_Result> ViewCart(Nullable<int> maKH)
        {
            var maKHParameter = maKH.HasValue ?
                new ObjectParameter("MaKH", maKH) :
                new ObjectParameter("MaKH", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ViewCart_Result>("ViewCart", maKHParameter);
        }
    
        public virtual int XoaHoaDon(Nullable<int> maHD, string maSach)
        {
            var maHDParameter = maHD.HasValue ?
                new ObjectParameter("MaHD", maHD) :
                new ObjectParameter("MaHD", typeof(int));
    
            var maSachParameter = maSach != null ?
                new ObjectParameter("MaSach", maSach) :
                new ObjectParameter("MaSach", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("XoaHoaDon", maHDParameter, maSachParameter);
        }
    }
}
