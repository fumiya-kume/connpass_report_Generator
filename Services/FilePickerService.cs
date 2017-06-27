using Microsoft.Win32;
using Reactive.Bindings;
using System.IO;

namespace ConnpassReportGenerator.Services
{
    public class FilePickerService : IFilePickerService
    {
        public ReactiveProperty<string> FileContent { get; set; } = new ReactiveProperty<string>("");
        public void ShowFilePicker()
        {
            var fileDialog = new OpenFileDialog();
            var result = fileDialog.ShowDialog();
            if (!result.HasValue || !result.Value) return;
            var fileAddress = fileDialog.FileName;
            if (!File.Exists(fileAddress)) return;
            var fileText = File.ReadAllText(fileAddress);
            if (string.IsNullOrWhiteSpace(fileText)) return;
            FileContent.Value = fileText;
        }
    }
}
