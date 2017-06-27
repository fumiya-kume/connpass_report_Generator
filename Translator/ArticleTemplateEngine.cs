namespace ConnpassReportGenerator.Translator
{
    public class ArticleTemplateEngine : IArticleTemplateEngine
    {
        public string ReplaceArticleData<T>(string templateText, T data)
        {
            if (string.IsNullOrWhiteSpace(templateText) || data == null) return "";
            var properties = data.GetType().GetProperties();
            foreach (var info in properties)
            {
                var propertyName = info.Name;
                var dataPropertyInfo = data.GetType().GetProperty(propertyName);
                if (dataPropertyInfo == null) return "";
                var propertyValue = (string)dataPropertyInfo.GetValue(data, null);
                templateText = templateText.Replace($"{{{propertyName}}}", propertyValue);
            };
            return templateText;
        }
    }
}
