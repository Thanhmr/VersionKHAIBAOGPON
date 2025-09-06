using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace KHAIBAO_GPON
{
    /// <summary>
    /// Repository class for managing TSL Channel data access
    /// Lớp quản lý truy cập dữ liệu kênh TSL
    /// </summary>
    public class KenhTSLRepository
    {
        private string connectionString;

        public KenhTSLRepository(string connString)
        {
            connectionString = connString;
        }

        /// <summary>
        /// Lấy danh sách kênh TSL
        /// Get list of TSL channels
        /// </summary>
        public List<KenhTSL> GetAllKenhTSL()
        {
            List<KenhTSL> danhSach = new List<KenhTSL>();
            
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
            
            return danhSach;
        }

        /// <summary>
        /// Thêm kênh TSL mới
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
                throw new Exception($"Lỗi khi thêm kênh TSL: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật kênh TSL
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
                throw new Exception($"Lỗi khi cập nhật kênh TSL: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa kênh TSL
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
                throw new Exception($"Lỗi khi xóa kênh TSL: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// TSL Channel model class
    /// Lớp mô hình dữ liệu kênh TSL
    /// </summary>
    public class KenhTSL
    {
        public int ID { get; set; }
        public string TenKenh { get; set; } = "";
        public string LoaiKenh { get; set; } = "";
        public string TrangThai { get; set; } = "";
        public DateTime NgayTao { get; set; }
    }
}