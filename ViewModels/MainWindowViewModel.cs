using ConnpassReportGenerator.DataStore;
using ConnpassReportGenerator.Model;
using ConnpassReportGenerator.Services;
using ConnpassReportGenerator.Translator;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Reactive.Bindings.Notifiers;

namespace ConnpassReportGenerator.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IConnpassClient _connpassClient;
        private readonly IFilePickerService _filePickerService;
        private readonly IClipBoardService _clipBoardService;
        private readonly IArticleTemplateEngine _articleTemplateEngine;
        public ReactiveProperty<string> ConnpassUrl { get; set; } = new ReactiveProperty<string>("");
        public ReactiveProperty<string> TemplateContent { get; set; } = new ReactiveProperty<string>("");
        public ReactiveProperty<string> ArticleContent { get; set; }
        public ReactiveProperty<string> SelectedTag { get; set; } = new ReactiveProperty<string>("");
        public BooleanNotifier IsOpenTagView { get; set; } = new BooleanNotifier(false);
        public List<string> TagCollection { get; set; } = Model.ArticleData.PropertyList();
        public ReactiveProperty<ArticleData> ArticleData { get; set; }
        public ReactiveCommand OpenTemplateFileFromLocal { get; set; } = new ReactiveCommand();
        public ReactiveCommand CopyArticleContentCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand TranslateCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand SwitchTagView { get; set; } = new ReactiveCommand();
        public ReactiveCommand CopyTag { get; set; } = new ReactiveCommand();
        public ReactiveCommand AddTag { get; set; } = new ReactiveCommand();
        public MainWindowViewModel(IConnpassClient connpassClient, IFilePickerService filePickerService, IClipBoardService clipBoardService, IArticleTemplateEngine articleTemplateEngine)
        {
            _connpassClient = connpassClient;
            _filePickerService = filePickerService;
            _clipBoardService = clipBoardService;
            _articleTemplateEngine = articleTemplateEngine;

            ConnpassUrl.Subscribe(s => _connpassClient.ConnpassURL.Value = s);

            ArticleData = _connpassClient.Article;

            ArticleContent = TemplateContent.Merge(TranslateCommand)
                //.Where(s => ArticleData != null && !string.IsNullOrWhiteSpace(TemplateContent.Value) && string.IsNullOrWhiteSpace(ConnpassUrl.Value))
                .Select(s => _articleTemplateEngine.ReplaceArticleData(TemplateContent.Value, ArticleData.Value))
                .ToReactiveProperty();

            CopyArticleContentCommand.Subscribe(() => _clipBoardService.CopyToClipBoard(ArticleContent.Value));

            TemplateContent = _filePickerService.FileContent;

            OpenTemplateFileFromLocal.Subscribe(() => _filePickerService.ShowFilePicker());

            SwitchTagView.Subscribe(() => IsOpenTagView.SwitchValue());

            CopyTag.Subscribe(() => _clipBoardService.CopyToClipBoard($"{{{SelectedTag.Value}}}"));

            AddTag.Subscribe(() => TemplateContent.Value += $"{{{SelectedTag.Value}}}");
        }
    }
}
