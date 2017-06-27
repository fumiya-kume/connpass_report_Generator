using ConnpassReportGenerator.Model;
using Reactive.Bindings;

namespace ConnpassReportGenerator.DataStore
{
    public interface IConnpassClient
    {
        ReactiveProperty<ArticleData> Article { get; set; }
        ReactiveProperty<string> ConnpassURL { get; set; }
    }
}