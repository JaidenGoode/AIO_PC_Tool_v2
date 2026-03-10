using System.Windows;
using System.Windows.Controls;
using AIO_PC_Tool_v2.Services;

namespace AIO_PC_Tool_v2.Views
{
    public partial class CleanerPage : Page
    {
        private readonly CleanerService _cleanerService;
        private long _totalFound = 0;

        public CleanerPage()
        {
            InitializeComponent();
            _cleanerService = new CleanerService();
        }

        private async void Scan_Click(object sender, RoutedEventArgs e)
        {
            ScanButton.IsEnabled = false;
            ScanStatus.Text = "Scanning...";
            ScanDescription.Text = "Please wait while we analyze your system...";
            
            try
            {
                var categories = _cleanerService.GetCategories();
                _totalFound = 0;

                foreach (var category in categories)
                {
                    await _cleanerService.ScanCategoryAsync(category);
                    _totalFound += category.Size;
                }

                // Update UI with found sizes
                var temp = categories.FirstOrDefault(c => c.Name.Contains("Temp"));
                var browser = categories.FirstOrDefault(c => c.Name.Contains("Browser"));
                var update = categories.FirstOrDefault(c => c.Name.Contains("Update"));
                var thumb = categories.FirstOrDefault(c => c.Name.Contains("Thumbnail"));
                var recycle = categories.FirstOrDefault(c => c.Name.Contains("Recycle"));

                TempSize.Text = FormatSize(temp?.Size ?? 0);
                BrowserSize.Text = FormatSize(browser?.Size ?? 0);
                UpdateSize.Text = FormatSize(update?.Size ?? 0);
                ThumbSize.Text = FormatSize(thumb?.Size ?? 0);
                RecycleSize.Text = FormatSize(recycle?.Size ?? 0);

                ScanStatus.Text = $"Found {FormatSize(_totalFound)}";
                ScanDescription.Text = "Click Clean Now to remove selected junk files";
                CleanButton.IsEnabled = _totalFound > 0;
            }
            catch (Exception ex)
            {
                ScanStatus.Text = "Scan failed";
                ScanDescription.Text = ex.Message;
            }
            
            ScanButton.IsEnabled = true;
        }

        private async void Clean_Click(object sender, RoutedEventArgs e)
        {
            CleanButton.IsEnabled = false;
            ScanStatus.Text = "Cleaning...";
            
            try
            {
                long cleaned = 0;
                var categories = _cleanerService.GetCategories();

                foreach (var category in categories)
                {
                    cleaned += await _cleanerService.CleanCategoryAsync(category);
                }

                TotalFreed.Text = FormatSize(cleaned);
                LastCleanedSize.Text = FormatSize(cleaned);
                LastCleanedTime.Text = DateTime.Now.ToString("MMM dd, yyyy 'at' HH:mm");
                
                ScanStatus.Text = "Cleaning Complete!";
                ScanDescription.Text = $"Successfully freed {FormatSize(cleaned)} of disk space";
                
                // Reset sizes
                TempSize.Text = "0 MB";
                BrowserSize.Text = "0 MB";
                UpdateSize.Text = "0 MB";
                ThumbSize.Text = "0 MB";
                RecycleSize.Text = "0 MB";
                LogSize.Text = "0 MB";
            }
            catch (Exception ex)
            {
                ScanStatus.Text = "Clean failed";
                ScanDescription.Text = ex.Message;
            }
        }

        private string FormatSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double size = bytes;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }
            return $"{size:0.##} {sizes[order]}";
        }
    }
}
