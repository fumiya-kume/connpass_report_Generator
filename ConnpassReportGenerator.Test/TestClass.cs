﻿using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using ConnpassReportGenerator.Model;
using ConnpassReportGenerator.Translator;

namespace ConnpassReportGenerator.Test
{
    [TestFixture]
    public class TestClass
    {
        private static ArticleData DemoData()
        {
            return new ArticleData()
            {
                DateAndTime = "2017/04/08 18:30",
                MeetupDescription = "勉強会の詳細",
                MemberCount = "20",
                Title = "タイトル",
                URL = new ArticleData.Url()
                {
                    MeetupURL = "http://www/Meetup.com",
                    TextURL = "http://www/Text.com",
                }
            };
        }

        public static class タグを抜き出すテスト
        {
            [TestCase("{Title}", "{Title}")]

            public static void _1つのタグを抜き出すテスト(string TemplateText, string resultText)
            {
                var templateEngine = new ArticleTemplateEngine();
                var TagCollection = ArticleTemplateEngine.TagStringCollection(TemplateText);
                Assert.NotNull(TagCollection);
                Assert.AreEqual(resultText, TagCollection.First());
            }

            [TestCase("{Title}{MeetupURL}", "{Title}", "{MeetupURL}")]
            public static void _２つのタグを抜き出すテスト(string TemplateText, string resultFirstText, string resultSecoundText)
            {
                var templateEngine = new ArticleTemplateEngine();
                var TagCollection = ArticleTemplateEngine.TagStringCollection(TemplateText);
                Assert.NotNull(TagCollection);
                Assert.AreEqual(resultFirstText, TagCollection.First());
                Assert.AreEqual(resultSecoundText, TagCollection.ToList()[1]);
            }
        }

        public static class パスからクラスのプロパティの値を吊り上げるテスト
        {
            [TestCase("Title", "タイトル")]
            public static void プロパティの名前からプロパティの値を吊り上げる(string path, string ExpectedValue)
            {
                var templateEngine = new ArticleTemplateEngine();
                var tags = ArticleTemplateEngine.IdentifyList(path);
                var propertyinfo = ArticleTemplateEngine.GetPropertyFromIdentify(tags, DemoData());
                Assert.AreEqual(ExpectedValue, propertyinfo);
            }

            [TestCase("{URL.MeetupURL}", "http://www/Meetup.com")]
            public static void パスからプロパティの値を吊り上げる(string path, string ExpectedValue)
            {
                var templateEngine = new ArticleTemplateEngine();
                var tags = ArticleTemplateEngine.IdentifyList(path);
                var propertyInfo = ArticleTemplateEngine.GetPropertyFromIdentify(tags, DemoData());
                Assert.AreEqual(ExpectedValue, propertyInfo);
            }
        }

        public static class テンプレートテキストを変換するテスト
        {
            [TestCase("{Title}","タイトル")]
            [TestCase("{URL.MeetupURL}", "http://www/Meetup.com")]
            [TestCase("{Title} {MemberCount}", "タイトル 20")]
            public static void テンプレートテキストから変換の正常系のテスト(string TemplateText, string ExpectedValue)
            {
                var templateEngine = new ArticleTemplateEngine();
                var AfterText = templateEngine.ReplaceArticleData(TemplateText, DemoData());
                Assert.AreEqual(ExpectedValue, AfterText);
            }
            [TestCase("Hello World", "Hello World")]
            [TestCase("{Hello}", "{Hello}")]
            public static void テンプレートテキストから変換の異常系のテスト(string TemplateText, string ExpectedValue)
            {
                var templateEngine = new ArticleTemplateEngine();
                var AfterText = templateEngine.ReplaceArticleData(TemplateText, DemoData());
                Assert.AreEqual(ExpectedValue, AfterText);
            }
        }
    }
}
