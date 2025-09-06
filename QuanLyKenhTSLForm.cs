using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace KHAIBAO_GPON
{
    /// <summary>
    /// Quản lý kênh TSL với repository được tích hợp (Console version)
    /// TSL Channel management with integrated repository functionality
    /// </summary>
    public class QuanLyKenhTSLManager
    {
        private string connectionString;
        private List<KenhTSL> danhSachKenh;

        public QuanLyKenhTSLManager()
        {
            // Khởi tạo connection string từ config hoặc default
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString 
                              ?? "Data Source=.;Initial Catalog=KHAIBAO_GPON;Integrated Security=True";
            danhSachKenh = new List<KenhTSL>();
        }

        #region Repository Functions - Các hàm repository được tích hợp từ KenhTSLRepository

        /// <summary>
        /// Lấy danh sách kênh TSL - Integrated from KenhTSLRepository
        /// Get list of TSL channels
        /// </summary>
        public List<KenhTSL> GetAllKenhTSL()
        {
            List<KenhTSL> danhSach = new List<KenhTSL>();
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM KenhTSL ORDER BY TenKenh";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        KenhTSL kenh = new KenhTSL
                        {
                            ID = reader["ID"] as int? ?? 0,
                            TenKenh = reader["TenKenh"] as string ?? "",
                            LoaiKenh = reader["LoaiKenh"] as string ?? "",
                            TrangThai = reader["TrangThai"] as string ?? "",
                            NgayTao = reader["NgayTao"] as DateTime? ?? DateTime.Now
                        };
                        danhSach.Add(kenh);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tải danh sách kênh TSL: {ex.Message}");
            }
            
            return danhSach;
        }

        /// <summary>
        /// Thêm kênh TSL mới - Integrated from KenhTSLRepository
        /// Add new TSL channel
        /// </summary>
        public bool ThemKenhTSL(KenhTSL kenh)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO KenhTSL (TenKenh, LoaiKenh, TrangThai, NgayTao) 
                                   VALUES (@TenKenh, @LoaiKenh, @TrangThai, @NgayTao)";
                    
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TenKenh", kenh.TenKenh);
                    cmd.Parameters.AddWithValue("@LoaiKenh", kenh.LoaiKenh);
                    cmd.Parameters.AddWithValue("@TrangThai", kenh.TrangThai);
                    cmd.Parameters.AddWithValue("@NgayTao", DateTime.Now);
                    
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm kênh TSL: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Cập nhật kênh TSL - Integrated from KenhTSLRepository
        /// Update TSL channel
        /// </summary>
        public bool CapNhatKenhTSL(KenhTSL kenh)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE KenhTSL SET 
                                   TenKenh = @TenKenh, 
                                   LoaiKenh = @LoaiKenh, 
                                   TrangThai = @TrangThai 
                                   WHERE ID = @ID";
                    
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", kenh.ID);
                    cmd.Parameters.AddWithValue("@TenKenh", kenh.TenKenh);
                    cmd.Parameters.AddWithValue("@LoaiKenh", kenh.LoaiKenh);
                    cmd.Parameters.AddWithValue("@TrangThai", kenh.TrangThai);
                    
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật kênh TSL: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Xóa kênh TSL - Integrated from KenhTSLRepository
        /// Delete TSL channel
        /// </summary>
        public bool XoaKenhTSL(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM KenhTSL WHERE ID = @ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", id);
                    
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xóa kênh TSL: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region Manager Functions - Các chức năng quản lý

        /// <summary>
        /// Hiển thị menu chính
        /// Display main menu
        /// </summary>
        public void ShowMainMenu()
        {
            bool thoat = false;
            
            while (!thoat)
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
                Console.WriteLine("║              QUẢN LÝ KÊNH TSL - KHAI BÁO GPON           ║");
                Console.WriteLine("║            (Repository được tích hợp vào Form)           ║");
                Console.WriteLine("╠══════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ 1. Xem danh sách kênh TSL                               ║");
                Console.WriteLine("║ 2. Thêm kênh TSL mới                                    ║");
                Console.WriteLine("║ 3. Cập nhật kênh TSL                                    ║");
                Console.WriteLine("║ 4. Xóa kênh TSL                                         ║");
                Console.WriteLine("║ 5. Thoát                                                ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
                Console.Write("Chọn chức năng (1-5): ");
                
                string luaChon = Console.ReadLine();
                
                switch (luaChon)
                {
                    case "1":
                        XemDanhSachKenh();
                        break;
                    case "2":
                        ThemKenhMoi();
                        break;
                    case "3":
                        CapNhatKenh();
                        break;
                    case "4":
                        XoaKenh();
                        break;
                    case "5":
                        thoat = true;
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ! Nhấn Enter để tiếp tục...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        /// <summary>
        /// Xem danh sách kênh TSL
        /// View TSL channel list
        /// </summary>
        private void XemDanhSachKenh()
        {
            Console.Clear();
            Console.WriteLine("DANH SÁCH KÊNH TSL");
            Console.WriteLine(new string('=', 80));
            
            try
            {
                danhSachKenh = GetAllKenhTSL();
                
                if (danhSachKenh.Count == 0)
                {
                    Console.WriteLine("Không có dữ liệu kênh TSL.");
                }
                else
                {
                    Console.WriteLine($"{"ID",-5} {"Tên Kênh",-25} {"Loại Kênh",-15} {"Trạng Thái",-15} {"Ngày Tạo",-20}");
                    Console.WriteLine(new string('-', 80));
                    
                    foreach (var kenh in danhSachKenh)
                    {
                        Console.WriteLine($"{kenh.ID,-5} {kenh.TenKenh,-25} {kenh.LoaiKenh,-15} {kenh.TrangThai,-15} {kenh.NgayTao:dd/MM/yyyy HH:mm,-20}");
                    }
                    
                    Console.WriteLine(new string('-', 80));
                    Console.WriteLine($"Tổng số: {danhSachKenh.Count} kênh");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
            
            Console.WriteLine("\nNhấn Enter để quay lại menu chính...");
            Console.ReadLine();
        }

        /// <summary>
        /// Thêm kênh TSL mới
        /// Add new TSL channel
        /// </summary>
        private void ThemKenhMoi()
        {
            Console.Clear();
            Console.WriteLine("THÊM KÊNH TSL MỚI");
            Console.WriteLine(new string('=', 40));
            
            try
            {
                Console.Write("Tên kênh: ");
                string tenKenh = Console.ReadLine();
                
                Console.Write("Loại kênh (GPON/XGSPON/TSL): ");
                string loaiKenh = Console.ReadLine();
                
                Console.Write("Trạng thái (Hoạt động/Dừng/Bảo trì): ");
                string trangThai = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(tenKenh) || string.IsNullOrWhiteSpace(loaiKenh) || string.IsNullOrWhiteSpace(trangThai))
                {
                    Console.WriteLine("Vui lòng nhập đầy đủ thông tin!");
                }
                else
                {
                    KenhTSL kenhMoi = new KenhTSL
                    {
                        TenKenh = tenKenh.Trim(),
                        LoaiKenh = loaiKenh.Trim(),
                        TrangThai = trangThai.Trim()
                    };
                    
                    if (ThemKenhTSL(kenhMoi))
                    {
                        Console.WriteLine("Thêm kênh TSL thành công!");
                    }
                    else
                    {
                        Console.WriteLine("Thêm kênh TSL thất bại!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
            
            Console.WriteLine("\nNhấn Enter để quay lại menu chính...");
            Console.ReadLine();
        }

        /// <summary>
        /// Cập nhật kênh TSL
        /// Update TSL channel
        /// </summary>
        private void CapNhatKenh()
        {
            Console.Clear();
            Console.WriteLine("CẬP NHẬT KÊNH TSL");
            Console.WriteLine(new string('=', 40));
            
            try
            {
                Console.Write("Nhập ID kênh cần cập nhật: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.Write("Tên kênh mới: ");
                    string tenKenh = Console.ReadLine();
                    
                    Console.Write("Loại kênh mới (GPON/XGSPON/TSL): ");
                    string loaiKenh = Console.ReadLine();
                    
                    Console.Write("Trạng thái mới (Hoạt động/Dừng/Bảo trì): ");
                    string trangThai = Console.ReadLine();
                    
                    if (string.IsNullOrWhiteSpace(tenKenh) || string.IsNullOrWhiteSpace(loaiKenh) || string.IsNullOrWhiteSpace(trangThai))
                    {
                        Console.WriteLine("Vui lòng nhập đầy đủ thông tin!");
                    }
                    else
                    {
                        KenhTSL kenhCapNhat = new KenhTSL
                        {
                            ID = id,
                            TenKenh = tenKenh.Trim(),
                            LoaiKenh = loaiKenh.Trim(),
                            TrangThai = trangThai.Trim()
                        };
                        
                        if (CapNhatKenhTSL(kenhCapNhat))
                        {
                            Console.WriteLine("Cập nhật kênh TSL thành công!");
                        }
                        else
                        {
                            Console.WriteLine("Cập nhật kênh TSL thất bại!");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("ID không hợp lệ!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
            
            Console.WriteLine("\nNhấn Enter để quay lại menu chính...");
            Console.ReadLine();
        }

        /// <summary>
        /// Xóa kênh TSL
        /// Delete TSL channel
        /// </summary>
        private void XoaKenh()
        {
            Console.Clear();
            Console.WriteLine("XÓA KÊNH TSL");
            Console.WriteLine(new string('=', 40));
            
            try
            {
                Console.Write("Nhập ID kênh cần xóa: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.Write($"Bạn có chắc chắn muốn xóa kênh ID {id}? (y/n): ");
                    string xacNhan = Console.ReadLine();
                    
                    if (xacNhan?.ToLower() == "y" || xacNhan?.ToLower() == "yes")
                    {
                        if (XoaKenhTSL(id))
                        {
                            Console.WriteLine("Xóa kênh TSL thành công!");
                        }
                        else
                        {
                            Console.WriteLine("Xóa kênh TSL thất bại!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Đã hủy thao tác xóa.");
                    }
                }
                else
                {
                    Console.WriteLine("ID không hợp lệ!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
            
            Console.WriteLine("\nNhấn Enter để quay lại menu chính...");
            Console.ReadLine();
        }

        #endregion
    }
}