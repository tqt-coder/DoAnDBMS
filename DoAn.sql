
-- Dùng xóa nếu database đã tồn tại
use master
GO
IF DB_ID('DoAn') IS NOT NULL
	Drop database DoAn
GO

Create DATABASE DoAn
GO
USE DoAn
GO
CREATE TABLE NhaXuatBan 
(
	MaNXB VARCHAR (20) NOT NULL UNIQUE,
	TenNXB NVARCHAR (50) NULL,
	CONSTRAINT pk_nxb PRIMARY KEY (MaNXB)
)
GO

CREATE TABLE NguoiQuanLy
(
	MaNQL INT IDENTITY(1,1),
	Ten NVARCHAR(50) NULL,
	MaKH INT,
	CONSTRAINT pk_nql PRIMARY KEY (MaNQL)
)
GO

CREATE TABLE DonHang(
	MaDH INT IDENTITY(1,1),
	MaKH INT NOT NULL,
	NgayDat DATE NOT NULL,
	NgayNhan DATE, 
	TongTien MONEY,
	CONSTRAINT pk_dh PRIMARY KEY (MaDH),
	CONSTRAINT c_ngay	CHECK(NgayNhan>NgayDat),
)
GO

CREATE TABLE ChiTietHoaDon(
	MaHD INT NOT NULL,
	MaSach VARCHAR(20) NOT NULL,
	SoLuong INT NOT NULL,
	ThanhTien MONEY,
	CONSTRAINT c_soluong CHECK (SoLuong > 0),
	CONSTRAINT pk_cthd PRIMARY KEY( MaHD, MaSach),
)
GO

CREATE TABLE Sach
(
	MaSach VARCHAR(20) NOT NULL UNIQUE,	
	TenSach NVARCHAR(50)  NULL,
	TenTacGia NVARCHAR(50)  NULL,
	MaNXB VARCHAR (20),
	TheLoai NVARCHAR(50)  NULL,
	SoLuong INT,
	GiaBan MONEY,
	HinhAnh varchar(MAX) NOT NULL,
	CONSTRAINT pk_s PRIMARY KEY (MaSach),
	CONSTRAINT c_gia CHECK ( GiaBan > 0)
)
GO

CREATE TABLE KhachHang 
(
	MaKH INT IDENTITY(1,1),
	HovaTen NVARCHAR(50),
	SoDienThoai VARCHAR(10),
	DiaChi NVARCHAR(100) NOT NULL,
	PassWord varchar(20) NOT NULL,
	Gmail varchar(50) NOT Null,
	Quyen nvarchar(20) NULL,	
	CONSTRAINT pk_kh PRIMARY KEY (MaKH)
)
GO

-- Foreign key
ALTER TABLE Sach ADD CONSTRAINT fk_s_nxb FOREIGN KEY (MaNXB) REFERENCES NhaXuatBan (MaNXB) ON DELETE CASCADE
GO
ALTER TABLE DonHang ADD CONSTRAINT fk_dh_kh FOREIGN KEY (MaKH) REFERENCES KhachHang (MaKH)  ON DELETE CASCADE
GO
ALTER TABLE ChiTietHoaDon ADD CONSTRAINT fk_ctdh_dh FOREIGN KEY (MaHD) REFERENCES DonHang (MaDH)
GO
ALTER TABLE ChiTietHoaDon ADD CONSTRAINT fk_ctdh_sach FOREIGN KEY (MaSach) REFERENCES Sach (MaSach)
GO
ALTER TABLE NguoiQuanLy ADD CONSTRAINT fk_nql_sach FOREIGN KEY (MaKH) REFERENCES KhachHang (MaKH) ON DELETE CASCADE
GO
-- Trigger xóa sách khi số lượng bằng 0
CREATE TRIGGER ChangeQuantity On Sach
AFTER Insert , Update 
As
	Declare @SoLuong int, @MaSach nvarchar(20)
Begin
	Declare  ChangeQuantityCursor Cursor for
	Select s.MaSach, i.SoLuong  From inserted as i, Sach as s Where s.MaSach = i.MaSach
	Open ChangeQuantityCursor
	Fetch next from ChangeQuantityCursor into @MaSach, @SoLuong
	while @@FETCH_STATUS = 0
	Begin
		if( @SoLuong = 0)
			Begin
				Delete Sach Where MaSach = @MaSach
			End;

	Fetch next from ChangeQuantityCursor into @MaSach, @SoLuong
	End
	Close ChangeQuantityCursor
	Deallocate ChangeQuantityCursor
END

Go
-- Trigger hủy chi tiết hàng và trả lại số lượng sách trong kho
CREATE TRIGGER t_huydon ON ChiTietHoaDon AFTER DELETE
AS
Declare @MaSach VARCHAR(20), @SoLuong int
BEGIN
	Declare  XoaHDCursor Cursor for
	Select de.MaSach,de.SoLuong From deleted de
	Open XoaHDCursor
	Fetch next from XoaHDCursor into @MaSach, @SoLuong
	while @@FETCH_STATUS = 0
	Begin
		UPDATE Sach 
		SET SoLuong = SoLuong + @SoLuong
		FROM Sach
		where MaSach=@MaSach

	Fetch next from XoaHDCursor into @MaSach, @SoLuong
	End
	Close XoaHDCursor
	Deallocate XoaHDCursor
END

GO
-- Trigger thêm tự động thêm tên người quản lý
CREATE TRIGGER  TenNguoiQuanLy ON NguoiQuanLy
AFTER INSERT
AS
	DECLARE @ten NVARCHAR(50), @MaNQL varchar(20)

BEGIN
	Declare  TenNguoiQuanLyCursor Cursor for

	Select  nql.MaNQL,kh.HovaTen  from KhachHang as kh ,inserted as i, NguoiQuanLy as nql 
	where kh.MaKH = nql.MaKH and nql.MaNQL = i.MaNQL
	
	Open TenNguoiQuanLyCursor

	Fetch next from TenNguoiQuanLyCursor into @MaNQL, @ten
	while @@FETCH_STATUS = 0
	Begin
		if(@MaNQL IS NOT NULL)
			begin
				Update NguoiQuanLy Set Ten = @ten Where NguoiQuanLy.MaNQL = @MaNQL
			end
	
	Fetch next from TenNguoiQuanLyCursor into @MaNQL, @ten
	End
	Close TenNguoiQuanLyCursor
	Deallocate TenNguoiQuanLyCursor
END;
GO

--Trigger đặt sách 
CREATE TRIGGER t_themdh ON ChiTietHoaDon
AFTER INSERT AS
DECLARE @new INT,@rest INT,@idsach nvarchar(20)
Declare TaoHDCursor Cursor for
SELECT ne.soluong ,s.Soluong,s.MaSach
FROM INSERTED ne,Sach s
WHERE ne.MaSach = s.MaSach
begin
	Open TaoHDCursor
	Fetch next from TaoHDCursor into @new, @rest,@idsach
	while @@FETCH_STATUS = 0
	BEGIN
		IF(@new >@rest)
		begin
			Rollback
			Close TaoHDCursor
		end
		ELSE
			UPDATE Sach
			set soluong=SoLuong-@new
			where MaSach=@idsach
	Fetch next from TaoHDCursor into @new, @rest,@idsach
	End
	Close TaoHDCursor
	Deallocate TaoHDCursor
end

go
-- Trigger thêm quyền tự động cho user khi khách hàng đăng kí tự thêm quyền vào luôn trừ admin đã được thêm trước
Create Trigger ThemQuyen On KhachHang
After Insert 
As
	Declare @maKH varchar(20), @quyen nvarchar(20)
Begin
	Declare  KhachHangCursor Cursor for

	Select  kh.MaKH,kh.Quyen  from KhachHang as kh inner join inserted as i On kh.MaKH = i.MaKH
	
	Open KhachHangCursor

	Fetch next from KhachHangCursor into @maKH, @quyen
	while @@FETCH_STATUS = 0
	Begin
		if(@quyen IS NULL)
			begin
				Update KhachHang Set Quyen = 'User' Where KhachHang.MaKH = @maKH
			end
	
	Fetch next from KhachHangCursor into @maKH, @quyen
	End
	Close KhachHangCursor
	Deallocate KhachHangCursor
End

go

-- View
Create VIEW Gio AS
	SELECT hd.MaHD,kh.MaKH,s.TenSach,s.GiaBan,s.MaSach,hd.SoLuong,hd.ThanhTien,HinhAnh 
	FROM ChiTietHoaDon as hd,Sach as s,DonHang dh,KhachHang kh
	WHERE hd.MaSach=s.MaSach
	and dh.MaDH=hd.MaHD
	and kh.MaKH=dh.MaKH
Go

CREATE VIEW view_thongtinKH AS
	SELECT DonHang.MaKH, HovaTen, SoDienThoai, MaDH
	FROM KhachHang, DonHang
	WHERE DonHang.MaKH=KhachHang.MaKH;
GO

--trigger tinhtien--
CREATE TRIGGER TinhTien ON ChitietHoaDon
AFTER insert,update AS
DECLARE @MaHD INT ,@Tien money,@MaSach varchar(20)
Declare TinhTienCursor Cursor for
	SELECT MaHD,s.GiaBan,ne.MaSach 
	FROM Sach s,inserted ne
	WHERE ne.MaSach=s.MaSach
BEGIN
	open TinhTienCursor
	Fetch next from TinhTienCursor into @MaHD, @Tien,@MaSach
	while @@FETCH_STATUS=0
	begin
		UPDATE ChiTietHoaDon
		SET ThanhTien=SoLuong*@Tien
		where MaHD=@MaHD and MaSach=@MaSach
	Fetch next from TinhTienCursor into @MaHD, @Tien,@MaSach
	end
	Close TinhTienCursor
	Deallocate TinhTienCursor
END
GO
--TongTien
CREATE proc TongTien 
@MaKH int
as
Declare @Tien money,@MaDH int,@HD int
Select @MaDH=MaDH from DonHang where MaKH=@MaKH and NgayNhan is null
Begin
	Select @HD=MaHD,@Tien=sum(ThanhTien) from ChiTietHoaDon group by MaHD Having MaHD=@MaDH
	if(@HD is null)
		update DonHang set TongTien = 0 where MaKH = @MaKH
	update DonHang set TongTien=@Tien where MaDH=@HD
end

go
-- Trigger cập nhập lại hóa đơn khi xóa chi tiết hóa đơn
Create Trigger XoaChiTietHoaDon On ChiTietHoaDon
After Delete
As
	Declare @HD int, @MaKH int
Begin 
	
Declare XoaChiTietHoaDonCursor Cursor For

	Select deleted.MaHD  from deleted

	Open XoaChiTietHoaDonCursor
	Fetch next from XoaChiTietHoaDonCursor into @HD
	While @@FETCH_STATUS = 0
	Begin
		if(@HD is not null)
			begin
				Select @MaKH = dh.MaKH from DonHang as dh Where dh.MaDH = @HD 
				exec TongTien @MaKH
			end

	Fetch next from XoaChiTietHoaDonCursor into @HD
	End
	Close XoaChiTietHoaDonCursor
	Deallocate XoaChiTietHoaDonCursor
END
GO

-- Trigger tính Tổng hóa đơn trong Đơn Hàng
Create Function SumMoney(
	@MaHD INT 
	)
returns  Table

return 
		Select  Sum(dh.ThanhTien) As TongTien
		from ChiTietHoaDon as dh
		Group By dh.MaHD
		Having dh.MaHD = @MaHD

GO
CREATE Trigger Total On ChiTietHoaDon
After Insert, Update
As
	Declare @ma INT, @cost MONEY
Begin
	Declare TotalCursor Cursor For

	Select dh.MaDH From ChiTietHoaDon as cthd, inserted as i, DonHang as dh
	Where  dh.MaDH = cthd.MaHD and i.MaHD = cthd.MaHD

	Open TotalCursor
	Fetch next from TotalCursor into @ma
	While @@FETCH_STATUS = 0
	Begin
		if(@ma is not null)
			begin
			select @cost = s.TongTien from  SumMoney(@ma) as s
			Update DonHang Set  TongTien = @cost Where MaDH = @ma
			end

	Fetch next from TotalCursor into @ma
	End
Close TotalCursor
Deallocate TotalCursor
END
GO

/*NXB*/
INSERT INTO NhaXuatBan(MaNXB, TenNXB) VALUES
('NXB0001', N'NXB Kim Đồng');

INSERT INTO NhaXuatBan(MaNXB, TenNXB) VALUES
('NXB0002', N'NXB ĐH Quốc Gia');

INSERT INTO NhaXuatBan(MaNXB, TenNXB) VALUES
('NXB0003', N'NXB NLS');

INSERT INTO NhaXuatBan(MaNXB, TenNXB) VALUES
('NXB0004', N'NXB KBS');

INSERT INTO NhaXuatBan(MaNXB, TenNXB) VALUES
('NXB0005', N'NXB NLS');
GO

/*KH*/


INSERT INTO KhachHang( HovaTen, SoDienThoai,DiaChi ,PassWord, Gmail) VALUES
(N'Trần An Bình', '0909244322' ,N'61 Tô Hiến Thành, Quận 10, TP HCM','333333' ,'Binh@gmail.com');
GO
INSERT INTO KhachHang( HovaTen, SoDienThoai,DiaChi ,PassWord, Gmail) VALUES
( N'Đinh Khánh An', '0345217892',N'61 Võ Văn Ngân, TP Thủ Đức', '444444', 'An@gmail.com');
GO
INSERT INTO KhachHang( HovaTen, SoDienThoai,DiaChi ,PassWord, Gmail,Quyen) VALUES
(N'Trần Quốc Tuấn', '0943071252',N'Mũi Côn Đại, Phước Hiệp, Củ Chi, TP HCM', 'quoctuan', 'tranquoctuan@gmail.com','admin');
GO
INSERT INTO KhachHang( HovaTen, SoDienThoai,DiaChi ,PassWord, Gmail,Quyen) VALUES
(N'Nguyễn Lâm Sơn', '0478348347',N'Đồng Nai', 'lamson', 'lamson@gmail.com','admin');
GO
INSERT INTO KhachHang( HovaTen, SoDienThoai,DiaChi ,PassWord, Gmail,Quyen) VALUES
(N'Trần Phát Đạt', '0478348347',N'Long An','phatdat', 'phatdat@gmail.com','admin');
GO
INSERT INTO KhachHang( HovaTen, SoDienThoai,DiaChi ,PassWord, Gmail,Quyen) VALUES
(N'Trần Công Tuấn Mạnh', '0478348347',N'1 Võ Văn Ngân, TP Thủ Đức','tuanmanh', 'tuanmanh@gmail.com','admin');
GO

/*Sach*/

INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) 
values( 'S0001', N'Dế mèn phiêu lưu ký', N'Tô Hoài','NXB0001', N'Phiêu lưu mạo hiểm',80, 175.56, 'https://cdn0.fahasa.com/media/catalog/product/cache/1/small_image/400x400/9df78eab33525d08d6e5fb8d27136e95/s/l/slam-dunk---tap-5.jpg');

INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh)  
values( 'S0002', N'Ký sinh thú', N'Albert Einstein','NXB0004', N'Khoa học',150, 220.00, 'https://cdn0.fahasa.com/media/catalog/product/cache/1/small_image/400x400/9df78eab33525d08d6e5fb8d27136e95/n/x/nxbtre_full_23322021_033256_2.jpg')

INSERT INTO  Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) 
values( 'S0003', N'Tớ thích cậu hơn cả Harvard', N'Lam Rùa','NXB0003', N'Ngôn tình',50, 100.00,'https://cdn0.fahasa.com/media/catalog/product/cache/1/small_image/400x400/9df78eab33525d08d6e5fb8d27136e95/i/m/image_182309.jpg')

INSERT INTO  Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) 
values( 'S0004', N'Nhà giả kim', N'Paulo Coelho','NXB0002', N'Tiểu thuyết',10,61.700,'https://cdn0.fahasa.com/media/catalog/product/cache/1/small_image/600x600/9df78eab33525d08d6e5fb8d27136e95/i/m/image_195509_1_36793.jpg')

INSERT INTO  Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) 
values( 'S0005', N'Trump - Đừng Bao Giờ Bỏ Cuộc', N'Donald J.Trump & Meredith','NXB0002', N'Chính trị',5,50.700,'https://cdn0.fahasa.com/media/catalog/product/cache/1/small_image/400x400/9df78eab33525d08d6e5fb8d27136e95/8/9/8934974148227.jpg')

INSERT INTO  Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) 
values( 'S0006', N'Thay đổi cuộc sống với nhân số học', N'Lê Đỗ Quỳnh Hương','NXB0003', N'Kỹ năng sống',16,212.100,'https://cdn0.fahasa.com/media/catalog/product/cache/1/small_image/400x400/9df78eab33525d08d6e5fb8d27136e95/t/d/tdcsvnsh.jpg')

INSERT INTO  Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) 
values( 'S0007', N'Hackers Ielts', N'Viện ngôn ngữ','NXB0003', N'Ngoại ngữ',8,155.700,'https://cdn0.fahasa.com/media/catalog/product/cache/1/small_image/400x400/9df78eab33525d08d6e5fb8d27136e95/h/a/hacker-ielts-reading-01.jpg')

INSERT INTO  Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) 
values( 'S0008', N'Tớ thích cậu hơn cả Harvard', N'Lam Rùa','NXB0003', N'Thiếu nhi',50, 100.00,'https://cdn0.fahasa.com/media/catalog/product/cache/1/small_image/400x400/9df78eab33525d08d6e5fb8d27136e95/i/m/image_182309.jpg')

INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values( 'S00012', N'Vũ Trụ song song', N'Khoa học Việt Nam','NXB0002', N'Khoa học',29,193.700,'https://wiibook.net/wp-content/uploads/2020/03/sach-vu-tru-song-song.png')

INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values( 'S0009', N'How the Earth was born', N'Unknown','NXB0002', N'Khoa học',29,300.700,'https://i.ebayimg.com/images/g/ASQAAOSwMglfHE74/s-l400.jpg')

INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values( 'S0010', N'A Black Hole is not a Hole', N'Charlesbridge Hardcover','NXB0002', N'Khoa học',29,100.700,'http://www.carolyndecristofano.com/test/wp-content/uploads/2012/03/BlackHole.hires_.cover_.jpg')

INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values( 'S0011', N'The Universe Builders', N'Steve LeBel','NXB0002', N'Khoa học',29,100.700,'https://4.bp.blogspot.com/-YsQKP8iuAeA/VDCDx194bHI/AAAAAAAAFyg/N1nVrYCnNCY/s1600/The%2BUniverse%2BBuilders.jpg')

--KyNangSong--

INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values( 'S0012', N'Đắc nhân tâm', N'Dale Carnegie','NXB0003', N'Kỹ năng sống',59,99.500,'https://firstnews.com.vn/public/uploads/products/dac-nhan-tam-biamem2019-76k-bia1.jpg')

INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values( 'S0013', N'Bài học diệu kỳ từ chiếc xe rác', N' David J.Pollay','NXB0005', N'Kỹ năng sống',29,100.700,'https://www.vanphongit.com/wp-content/uploads/2019/04/bai-hoc-dieu-ky-tu-chiec-xe-rac-ebook.gif')

INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values( 'S0014', N'Khéo ăn nói sẽ có được thiên hạ', N'Trác Nhã','NXB0005', N'Kỹ năng sống',23,150.700,'https://timsachdoc.com/wp-content/uploads/2020/11/kheo_an_noi_se_co_duoc_thien_ha-1.jpg')

INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values( 'S0015', N'Kỹ năng lãnh đạo', N'John C. Maxwell','NXB0005', N'Kỹ năng sống',109,100.700,'https://salt.tikicdn.com/cache/400x400/ts/product/41/a5/17/ee35e671d62e43e796a6700c40b11d9d.png')

--Bach Khoa--
INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values('S0016', N'Bách khoa toàn thư', N'Phan Huy Chú','NXB0002', N'Kiến Thức - Bách Khoa',28,120.500,'http://hocgioitoan.com.vn/wp-content/uploads/2020/08/KH2.png')
INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values('S0017', N'Science a children’s encyclopedia', N'DK','NXB0002', N'Kiến Thức - Bách Khoa',30,495.000,'http://hocgioitoan.com.vn/wp-content/uploads/2020/08/2-3.jpg')
INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values('S0018', N'Science Encyclopedia', N'National Geographic Kids','NXB0002', N'Kiến Thức - Bách Khoa',30,495.000,'http://hocgioitoan.com.vn/wp-content/uploads/2020/08/3-3.jpg')
INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values('S0019', N'Bách khoa cơ thể người', N'Dorling Kindersley','NXB0002', N'Kiến Thức - Bách Khoa',50,107.200,'https://cdn0.fahasa.com/media/catalog/product/cache/1/small_image/600x600/9df78eab33525d08d6e5fb8d27136e95/8/9/8936071672674_2.jpg')

--Lịch sử--
INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values('S0020', N'Sử việt 12 khúc tráng ca', N'Dũng Phan','NXB0001', N'Lịch sử',25,99.000,'https://toplist.vn/images/800px/su-viet-12-khuc-trang-ca-361536.jpg')
INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values('S0021', N'Vua Gia Long Và Người Pháp', N'Thụy Khê','NXB0001', N'Lịch sử',64,199.200,'https://newshop.vn/public/uploads/products/10769/vua-gia-long.gif')
INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values('S0022', N'Bão Táp Triều Trần', N'Hoàng Quốc Hải','NXB0002', N'Lịch sử',37,657.000,'https://toplist.vn/images/800px/bao-tap-trieu-tran-361550.jpg')
INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values('S0023', N'Hào Kiệt Lam Sơn', N'Vũ Ngọc Dĩnh','NXB0001', N'Lịch sử',31,200.000,'https://salt.tikicdn.com/cache/400x400/media/catalog/product/l/a/lam-son.jpg.webp')

--Trinh Thám--
INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values('S0024', N'Án Mạng Trên Chuyến Tàu Tốc Hành Phương Đông', N' Agatha Christie','NXB0003', N'Trinh Thám',19,110.000,'https://salt.tikicdn.com/cache/400x400/ts/product/f5/38/3f/6ce388fc1314314a69d4b87d6fb4bffa.jpg.webp')
INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values('S0025', N'Sherlock Holmes Toàn Tập', N'Sir Arthur Conan Doyle','NXB0003', N'Trinh Thám',50,242.000,'https://salt.tikicdn.com/cache/400x400/ts/product/07/56/49/d45d9887e53ea330eea1fea516313dd4.jpg.webp')
INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values('S0026', N'Thám Tử Lừng Danh Conan', N'Gosho Aoyama','NXB0004', N'Trinh Thám',18,35.000,'https://cdn0.fahasa.com/media/catalog/product/cache/1/small_image/600x600/9df78eab33525d08d6e5fb8d27136e95/i/m/image_195509_1_561.jpg')
INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values('S0027', N'Phía Sau Nghi Can X', N'Higashino Keigo','NXB0004', N'Trinh Thám',16,109.000,'https://salt.tikicdn.com/cache/400x400/ts/product/23/56/86/a538698ead7dc2f693d1e9778417317d.jpg.webp')

/*NguoiQuanLy*/
INSERT INTO NguoiQuanLy( MaKH) VALUES
(3);

INSERT INTO NguoiQuanLy( MaKH) VALUES
( 4);

INSERT INTO NguoiQuanLy( MaKH) VALUES
( 5);

INSERT INTO NguoiQuanLy(MaKH) VALUES
(6);
GO


-- Procedure cho chức năng tìm kiếm
Create Procedure searchBook
@tenSach nvarchar(200)
AS
Begin
	Select * from Sach Where Sach.TenSach like @tenSach
End


go
--Procedure cho danh mục--

create procedure recommend 
@theloai nvarchar(50) 
as 
begin 
select * from Sach 
where TheLoai Like @theloai end
GO

create view View_KH
as
select HovaTen,SoDienThoai,Gmail
from KhachHang

go


-- procedure dùng để thêm sản phảm vào giỏ hàng
Create proc Don
@MaKH int,@NgayDat Date,@MaSach VARCHAR(20),@SoLuong int
as
Declare @MaDH int
Select * from DonHang
begin tran
if(@MaKH not in (select MaKH from DonHang where MaKH=@MaKH and NgayNhan is null))
	insert into DonHang(MaKH,NgayDat) values (@MaKH,@NgayDat)
select @MaDH=MaDH from DonHang where MaKH=@MaKH and  NgayNhan is null
insert into ChiTietHoaDon(MaHD,MaSach,SoLuong) values (@MaDH,@MaSach,@SoLuong)
if(@MaDH not in (Select MaHD from ChiTietHoaDon where MaHD=@MaDH and MaSach=@MaSach))
	rollback
commit

go

--commit
go

--view dathang
CREATE VIEW DatHang AS
	SELECT MaDH,kh.MaKH,HovaTen,TongTien 
	FROM DonHang,KhachHang kh
	WHERE DonHang.MaKH=kh.MaKH
	and NgayNhan is null
Go
--TraTienDonHang, tạo transaction
create proc TraTien
@MaKH int,@Tien money,@NgayNhan Date
as
Declare @MaHD int,@TongTien money
select @MaHD=MaDH from DonHang where MaKH=@MaKH and NgayNhan is null
Begin tran
Select @TongTien=sum(ThanhTien),@MaHD=MaHD from ChiTietHoaDon group by MaHD having MaHD=@MaHD
delete from ChiTietHoaDon where MaHD=@MaHD
if(@Tien!=@TongTien)
	rollback
else
	Begin
		update DonHang set NgayNhan=@NgayNhan where MaDH=@MaHD

	End
	commit
go
-- procedure xóa chi tiết hóa đơn
Create proc XoaHoaDon
	@MaHD int, @MaSach nvarchar(20)
As
Begin
	Delete ChiTietHoaDon Where MaHD = @MaHD and MaSach = @MaSach
End;


-- procedure xem giỏ hàng
Create proc ViewCart
	@MaKH int
As
Begin
	Select * from Gio Where  MaKH = @MaKH
End;

