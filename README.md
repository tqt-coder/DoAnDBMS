# DoAnDBMS




Nhanh thì copy dòng này vào

--Khoa học--

INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values( 'S0008', N'Vũ Trụ song song', N'Khoa học Việt Nam','NXB0002', N'Khoa học',29,193.700,'https://wiibook.net/wp-content/uploads/2020/03/sach-vu-tru-song-song.png')

INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values( 'S0009', N'How the Earth was born', N'Unknown','NXB0002', N'Khoa học',29,300.700,'https://i.ebayimg.com/images/g/ASQAAOSwMglfHE74/s-l400.jpg')

INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values( 'S0010', N'A Black Hole is not a Hole', N'Charlesbridge Hardcover','NXB0002', N'Khoa học',29,100.700,'http://www.carolyndecristofano.com/test/wp-content/uploads/2012/03/BlackHole.hires_.cover_.jpg')

INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values( 'S0011', N'The Universe Builders', N'Steve LeBel','NXB0002', N'Khoa học',29,100.700,'https://4.bp.blogspot.com/-YsQKP8iuAeA/VDCDx194bHI/AAAAAAAAFyg/N1nVrYCnNCY/s1600/The%2BUniverse%2BBuilders.jpg')

--KyNangSong--

INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values( 'S0012', N'Đắc nhân tâm', N'Dale Carnegie','NXB0001', N'Kỹ năng sống',59,99.500,'https://firstnews.com.vn/public/uploads/products/dac-nhan-tam-biamem2019-76k-bia1.jpg')

INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values( 'S0013', N'Bài học diệu kỳ từ chiếc xe rác', N' David J.Pollay','NXB0005', N'Kỹ năng sống',29,100.700,'https://www.vanphongit.com/wp-content/uploads/2019/04/bai-hoc-dieu-ky-tu-chiec-xe-rac-ebook.gif')

INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values( 'S0014', N'Khéo ăn nói sẽ có được thiên hạ', N'Trác Nhã','NXB0005', N'Kỹ năng sống',23,150.700,'https://timsachdoc.com/wp-content/uploads/2020/11/kheo_an_noi_se_co_duoc_thien_ha-1.jpg')

INSERT INTO Sach(MaSach, TenSach, TenTacGia,MaNXB, TheLoai, SoLuong, GiaBan, HinhAnh) values( 'S0015', N'Kỹ năng lãnh đạo', N'John C. Maxwell','NXB0005', N'Kỹ năng sống',109,100.700,'https://salt.tikicdn.com/cache/400x400/ts/product/41/a5/17/ee35e671d62e43e796a6700c40b11d9d.png')

--Procedure cho danh mục-- 

create procedure recommend @theloai nvarchar(50) as begin select * from Sach where TheLoai Like @theloai end
