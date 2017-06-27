using System.Collections.Generic;
using System.Linq;

namespace ConnpassReportGenerator.Model
{
    public class ArticleData
    {
        public static List<string> PropertyList() => typeof(ArticleData).GetProperties().Select(info => info.Name).ToList();

        public string DateAndTime { get; set; } = "";
        public string Title { get; set; } = "";
        public string MemberCount { get; set; } = "";
        public string TextURL { get; set; } = "";
        public string MeetupDescription { get; set; } = "";
        public string MeetupURL { get; set; } = "";
    }
}
