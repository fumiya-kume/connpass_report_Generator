using Reactive.Bindings;

namespace ConnpassReportGenerator.Services
{
    public interface IFilePickerService
    {
        ReactiveProperty<string> FileContent { get; set; }

        void ShowFilePicker();
    }
}