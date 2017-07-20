using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConnpassReportGenerator.Translator
{
    public class ArticleTemplateEngine : IArticleTemplateEngine
    {
        public string ReplaceArticleData<T>(string templateText, T data)
        {
            if (string.IsNullOrWhiteSpace(templateText) || data == null) return "";
            var Tags = TagStringCollection(templateText);

            foreach (var tag in Tags)
            {
                var PropertyValue = GetPropertyFromIdentify(tag, data);
                if (PropertyValue == null) continue;
                templateText = templateText.Replace($"{tag}", PropertyValue);
            };
            return templateText;
        }

        private string GetPropertyFromIdentify<T>(string TagName, T data)
        {
            var dataType = (object)data;

            var PathList = IdentifyList(TagName);
            var ValuePropertyName = PathList.Last();
            PathList.RemoveAt(PathList.Count - 1);

            foreach (var path in PathList)
            {
                dataType = dataType.GetType().GetProperty(path)?.GetValue(data);
            }

            return dataType.GetType().GetProperty(ValuePropertyName)?.GetValue(dataType) as string;
        }

        private List<string> TagStringCollection(string templateText) => new Regex(@"\{(.+?)\}").Matches(templateText).Cast<Match>().Select(match => match.Value).ToList();

        private List<string> IdentifyList(string TagName) =>
            TagName.Split('.').Select(s => s.Replace(".", "").Replace("{", "").Replace("}", "")).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
    }
}
