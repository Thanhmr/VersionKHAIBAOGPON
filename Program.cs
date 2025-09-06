using System;

namespace KHAIBAO_GPON
{
    /// <summary>
    /// Main program entry point
    /// Điểm vào chính của chương trình
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Điểm vào chính cho ứng dụng.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            try
            {
                Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║        KHAI BÁO GPON - QUẢN LÝ KÊNH TSL v3.0.0.13           ║");
                Console.WriteLine("║      Repository đã được gộp vào QuanLyKenhTSLForm            ║");
                Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
                Console.WriteLine();
                
                // Khởi tạo và chạy ứng dụng quản lý kênh TSL
                // Initialize and run TSL channel management application
                QuanLyKenhTSLManager manager = new QuanLyKenhTSLManager();
                manager.ShowMainMenu();
                
                Console.WriteLine("Cảm ơn bạn đã sử dụng hệ thống!");
                Console.WriteLine("Thank you for using the system!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khởi tạo ứng dụng: {ex.Message}");
                Console.WriteLine("Nhấn Enter để thoát...");
                Console.ReadLine();
            }
        }
    }
}