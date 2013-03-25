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

        public Node GetDescendant(params int[] descendants)
        {
            if (descendants.Count() == 0)
                return this;
            return Children
                .ElementAt(descendants.First())
                .GetDescendant(descendants.Skip(1).ToArray());
        }
    }

    [TestFixture]
    public class TextTreeTests
    {
        const string Source = "ЭНИ-601[2к[10Вт];4к[10Вт;20Вт;30Вт];8к[70Вт;80Вт]];";
        const string JsonPattern = @"
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

        [Test]
        public void ConvertToNode_should_convert_json_to_tree()
        {
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject(JsonPattern) as JObject;
            var node = ConvertToNode(json);

            node.Value.Should().BeNullOrEmpty();
            node.Children.Should().HaveCount(1);

            node.GetDescendant(0).Value.Should().Be("ЭНИ-601");
            node.GetDescendant(0).Children.Should().HaveCount(3);

            node.GetDescendant(0, 0).Value.Should().Be("2к");
            node.GetDescendant(0, 1).Value.Should().Be("4к");
            node.GetDescendant(0, 2).Value.Should().Be("8к");
            node.GetDescendant(0, 0).Children.Should().HaveCount(1);
            node.GetDescendant(0, 1).Children.Should().HaveCount(3);
            node.GetDescendant(0, 2).Children.Should().HaveCount(2);

            node.GetDescendant(0, 0, 0).Value.Should().Be("10Вт");
            node.GetDescendant(0, 1, 0).Value.Should().Be("10Вт");
            node.GetDescendant(0, 1, 1).Value.Should().Be("20Вт");
            node.GetDescendant(0, 1, 2).Value.Should().Be("30Вт");
            node.GetDescendant(0, 2, 0).Value.Should().Be("70Вт");
            node.GetDescendant(0, 2, 1).Value.Should().Be("80Вт");
            node.GetDescendant(0, 0, 0).Children.Should().HaveCount(0);
            node.GetDescendant(0, 1, 0).Children.Should().HaveCount(0);
            node.GetDescendant(0, 1, 1).Children.Should().HaveCount(0);
            node.GetDescendant(0, 1, 2).Children.Should().HaveCount(0);
            node.GetDescendant(0, 2, 0).Children.Should().HaveCount(0);
            node.GetDescendant(0, 2, 1).Children.Should().HaveCount(0);
        }

        [Test]
        public void ConvertToJson_should_conver_source_to_json_text()
        {
            var jsonText = ConvertToJsonText(Source);
            var expectedResult = JsonPattern.Replace(Environment.NewLine, "").Replace(" ", "");

            jsonText.Should().Be(expectedResult);
        }

        private string ConvertToJsonText(string source)
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