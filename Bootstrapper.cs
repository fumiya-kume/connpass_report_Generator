using Microsoft.Practices.Unity;
using Prism.Unity;
using ConnpassReportGenerator.Views;
using System.Windows;
using ConnpassReportGenerator.Services;
using ConnpassReportGenerator.DataStore;
using ConnpassReportGenerator.Translator;

namespace ConnpassReportGenerator
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            Container.RegisterType<IFilePickerService, FilePickerService>();
            Container.RegisterType<IClipBoardService, ClipBoardService>();
            Container.RegisterType<IConnpassClient, ConnpassClient>();
            Container.RegisterType<IArticleTemplateEngine, ArticleTemplateEngine>();

            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
