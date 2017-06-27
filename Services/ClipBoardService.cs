using System.Windows;

namespace ConnpassReportGenerator.Services
{
    public class ClipBoardService : IClipBoardService
    {
        public void CopyToClipBoard(string Content)
        {
            if (Content == null) return;
            Clipboard.SetText(Content);
        }
    }
}
