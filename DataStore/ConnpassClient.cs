using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using Reactive.Bindings;
using System.Reactive.Linq;
using ConnpassReportGenerator.Model;

namespace ConnpassReportGenerator.DataStore
{
    public class ConnpassClient : IConnpassClient
    {
        public ReactiveProperty<string> ConnpassURL { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<ArticleData> Article { get; set; } = new ReactiveProperty<ArticleData>();
        public ConnpassClient()
        {
            ConnpassURL
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Subscribe(async s =>
            {
                Article.Value = await GetArchcleData(s);
            });
        }

        public static async Task<ArticleData> GetArchcleData(string URL)
        {
            if (string.IsNullOrWhiteSpace(URL)) throw new ArgumentException("URL がおかしいかも");
            var html = "";
            using (var client = new HttpClient())
            {
                try
                {
                    var result = await client.GetAsync(URL);
                    html = await result.Content.ReadAsStringAsync();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            if (string.IsNullOrWhiteSpace(html)) throw new ArgumentException("アクセスが正常にできませんでした");
            var doc = await new HtmlParser().ParseAsync(html);

            var title = doc.Title.Replace("connpass", "").Replace("-", "");

            var memberCount = doc.QuerySelectorAll(".amount span").Select(element => int.Parse(element.InnerHtml))
                .Aggregate((newValue, oldValue) => newValue + oldValue);

            var textUrl = URL + "/presentation/";

            var ymd = doc.QuerySelector(".ymd").InnerHtml;
            var startTime = doc.QuerySelector(".hi").InnerHtml;
            var endTime = doc.QuerySelector(".dtend").TextContent.Trim();

            var dateAndTime = $"{ymd} {startTime} - {endTime}";

            var meetupDescription = doc.QuerySelector("#editor_area").TextContent.Replace(" ","").Substring(0,256);

            return new ArticleData() { Title = title, MemberCount = memberCount.ToString(), TextURL = textUrl, DateAndTime = dateAndTime, MeetupDescription = meetupDescription, MeetupURL = URL};
        }
    }
}
