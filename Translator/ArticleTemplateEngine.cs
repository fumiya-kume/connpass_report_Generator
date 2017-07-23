using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConnpassReportGenerator.Translator
{
    public class ArticleTemplateEngine : IArticleTemplateEngine
    {
        public string ReplaceArticleData<T>(string templateText, T data)
        {
            if (string.IsNullOrWhiteSpace(templateText) || data == null)
                return string.Empty;

            return TagStringCollection(templateText)
                .Aggregate(templateText, (processing, tag) =>
                {
                    var propertyValue = GetPropertyFromIdentify(IdentifyList(tag), data)?.ToString();
                    return propertyValue != null ? processing.Replace(tag, propertyValue) : processing;
                });
        }

        internal static object GetPropertyFromIdentify(IEnumerable<string> paths, object data)
        {
            if (data == null || paths.Count() == 0)
                return data;
            else
            {
                var child = data.GetType().GetProperty(paths.First())?.GetValue(data);
                return GetPropertyFromIdentify(paths.Skip(1), child);
            }
        }

        internal static IEnumerable<string> TagStringCollection(string templateText) =>
            new Regex(@"\{(.+?)\}")
                .Matches(templateText)
                .Cast<Match>()
                .Select(match => match.Value);

        internal static IEnumerable<string> IdentifyList(string TagName) =>
            TagName.Replace("{", string.Empty).Replace("}", string.Empty)
                .Split('.')
                .Where(s => !string.IsNullOrWhiteSpace(s));
    }
}
