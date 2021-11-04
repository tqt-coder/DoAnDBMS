CREATE DATABASE DoAn
GO
USE DoAn
GO
CREATE TABLE NhaXuatBan 
(
	MaNXB VARCHAR (20) NOT NULL UNIQUE,
	TenNXB NVARCHAR (20) NULL,
	CONSTRAINT pk_nxb PRIMARY KEY (MaNXB)
)
GO
CREATE TABLE NguoiQuanLy
(
	MaNQL VARCHAR(20) NOT NULL UNIQUE,
	Ten NVARCHAR(20) NULL,
	MaKH VARCHAR(20) NOT NULL,
	CONSTRAINT pk_nql PRIMARY KEY (MaNQL)
)
GO
CREATE TABLE DonHang
(
	MaDH VARCHAR(20) NOT NULL UNIQUE,
	MaKH VARCHAR(20) NOT NULL,
	MaSach VARCHAR(20) NOT NULL,
	MaNXB VARCHAR(20) NOT NULL,
	DiaChi NVARCHAR(50) NOT NULL,
	NgayDat DATE NOT NULL,
	NgayNhan DATE,
	SoLuong INT NOT NULL,
	ThanhTien MONEY,
	CONSTRAINT pk_dh PRIMARY KEY (MaDH),
	CONSTRAINT c_ngay	CHECK(NgayNhan>NgayDat),
	CONSTRAINT c_sl	CHECK(SoLuong > 0)
)
GO
CREATE TABLE Sach
(
	MaSach VARCHAR(20) NOT NULL UNIQUE,	
	TenSach NVARCHAR(20)  NULL,
	TenTacGia NVARCHAR(20)  NULL,
	TheLoai NVARCHAR(20)  NULL,
	GiaBan MONEY,
	CONSTRAINT pk_s PRIMARY KEY (MaSach),
	CONSTRAINT c_gia CHECK ( GiaBan > 0),
);
GO
-- Quan hệ N:M giữa Sach và NhaXuatBan
CREATE TABLE QuanLySach
(
	MaNXB VARCHAR (20) NOT NULL,
	MaSach VARCHAR(20) NOT NULL,
	SoLuong INT ,
	CONSTRAINT c_soluong CHECK ( SoLuong > 0),
	CONSTRAINT pk_qls PRIMARY KEY (MaNXB, MaSach)
)
GO
CREATE TABLE KhachHang 
(
	MaKH VARCHAR(20) NOT NULL,
	HovaTen NVARCHAR(20),
	SoDienThoai VARCHAR(10),
	PassWord varchar(20) NOT NULL,
	Gmail varchar(50),
	Quyen nvarchar(20) NOT NULL,	
	CONSTRAINT pk_kh PRIMARY KEY (MaKH)
)
GO
-- Foreign key
ALTER TABLE QuanLySach ADD CONSTRAINT fk_qls_nxb FOREIGN KEY (MaNXB) REFERENCES NhaXuatBan (MaNXB) ON DELETE CASCADE
GO
ALTER TABLE QuanLySach ADD CONSTRAINT fk_qls_sach FOREIGN KEY (MaSach) REFERENCES Sach (MaSach)  ON DELETE CASCADE
GO
ALTER TABLE DonHang ADD CONSTRAINT fk_dh_kh FOREIGN KEY (MaKH) REFERENCES KhachHang (MaKH)  ON DELETE CASCADE
GO
ALTER TABLE DonHang ADD CONSTRAINT fk_dh_sach FOREIGN KEY (MaSach) REFERENCES Sach (MaSach) ON DELETE CASCADE
GO
ALTER TABLE DonHang ADD CONSTRAINT fk_dh_nxb FOREIGN KEY (MaNXB) REFERENCES NhaXuatBan (MaNXB) ON DELETE CASCADE
GO
ALTER TABLE NguoiQuanLy ADD CONSTRAINT fk_nql_sach FOREIGN KEY (MaKH) REFERENCES KhachHang (MaKH) ON DELETE CASCADE
GO

-- Trigger
CREATE TRIGGER t_huydon ON DonHang AFTER DELETE
AS
	
BEGIN
	UPDATE QuanLySach 
	SET SoLuong = qls1.SoLuong + (SELECT del.SoLuong 
		FROM deleted as del, QuanLySach as qls 
		WHERE del.MaSach = qls.MaSach AND del.MaNXB = qls.MaNXB)
	FROM QuanLySach as qls1 
	JOIN deleted as del1 ON del1.MaSach = qls1.MaSach AND del1.MaNXB = qls1.MaNXB
END;

--Đặt sách 
CREATE TRIGGER t_themdh ON DonHang
AFTER INSERT AS
DECLARE @new INT,@rest INT
SELECT @new=ne.soluong , @rest=qls.Soluong
FROM INSERTED ne,QuanLySach qls
WHERE ne.MaSach = qls.MaSach
	AND ne.MaNXB = qls.maNXB
IF(@new >@rest)
	ROLLBACK;
ELSE
BEGIN
	UPDATE QuanLySach
	set soluong=SoLuong-@new
END
-- View
GO
CREATE VIEW DatHang AS
	SELECT MaDH,HovaTen,NgayDat,ThanhTien 
	FROM DonHang,KhachHang
	WHERE DonHang.MaKH=KhachHang.MaKH
Go

CREATE VIEW view_thongtinKH AS
	SELECT DonHang.MaKH, HovaTen, SoDienThoai, DiaChi, MaDH
	FROM KhachHang, DonHang
	WHERE DonHang.MaKH=KhachHang.MaKH;

CREATE VIEW ViewQuanLySach AS
	SELECT s.TenSach, s.TenTacGia , s.GiaBan, nxb.TenNXB, qls.SoLuong 
	FROM QuanLySach as qls, Sach as s, NhaXuatBan as nxb
	WHERE qls.MaSach = s.MaSach and qls.MaNXB = nxb.MaNXB

CREATE TRIGGER TINHTIEN ON DONHANG
	AFTER InSERT,UPDATE AS
	DECLARE @Tien money
	SELECT @TIEN=s.GiaBan 
	FROM Sach s,inserted ne
	WHERE ne.MaSach=s.MaSach
	BEGIN
		UPDATE DONHANG
		SET ThanhTien=SoLuong*@Tien
	END


	
INSERT INTO DONHANG(MaDH, MaKH, MaSach, MaNXB, DiaChi,NgayDat,NgayNhan,SoLuong) VALUES
('DH0001','KH0001','S0001','NXB0001',N'Hồ Chí minh','2018-12-13',Null,12);