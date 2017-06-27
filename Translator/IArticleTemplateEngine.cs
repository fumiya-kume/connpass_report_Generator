namespace ConnpassReportGenerator.Translator
{
    public interface IArticleTemplateEngine
    {
        string ReplaceArticleData<T>(string TemplateText, T data);
    }
}