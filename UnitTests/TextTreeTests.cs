using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace UnitTests
{
    internal class Node
    {
        public string Value { get; set; }
        public IEnumerable<Node> Children { get; set; }
    }

    [TestFixture]
    public class TextTreeTests
    {
        [Test]
        public void int_should_be_ref()
        {
            var source = "ЭНИ-601[2к[10Вт];4к[10Вт;20Вт;30Вт];8к[70Вт;80Вт]];";
            const string jsonPattern = @"
{
    ""ЭНИ-601"":{
        ""2к"":{
            ""10Вт"":{}
        },
        ""4к"":{
            ""10Вт"":{},
            ""20Вт"":{},
            ""30Вт"":{}
        },
        ""8к"":{
            ""70Вт"":{},
            ""80Вт"":{}
        }
}";
            var expectedResult = jsonPattern.Replace(Environment.NewLine, "").Replace(" ", "");

            var jsonText = ConvertToJson(source);
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonText) as JObject;
            var node = ConvertToNode(json);

            node.Value.Should().BeNullOrEmpty();
            node.Children.Should().HaveCount(1);

            node.Children.ElementAt(0).Value.Should().Be("ЭНИ-601");
            node.Children.ElementAt(0).Children.Should().HaveCount(3);
        }

        private string ConvertToJson(string source)
        {
            source = source.TrimEnd(';');
            source = Regex.Replace(source, @"([а-яА-Я\w\d-]+)", @"""$1"":{");
            source = source.Replace("[", "");
            source = source.Replace("]", "}");
            source = source.Replace(";", "},");
            source = "{" + source + "}";
            return source;
        }

        private Node ConvertToNode(JObject json)
        {
            throw new NotImplementedException();
        }
    }
}