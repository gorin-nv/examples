using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace UnitTests
{
    public class SubstringPosition
    {
        private readonly string _firstSource;
        private int _length;

        public SubstringPosition(int firstStart, int secondStart, int length, string firstSource)
        {
            _firstSource = firstSource;
            _length = length;
            SecondStart = secondStart;
            FirstStart = firstStart;
        }

        public int FirstStart { get; private set; }
        public int SecondStart { get; private set; }
        public string Value
        {
            get { return _firstSource.Substring(FirstStart, _length); }
        }
    }

    public static class TextCompareUtil
    {
        public static IEnumerable<SubstringPosition> CommonSubstrings(string text1, string text2)
        {
            throw new System.NotImplementedException();
        }
    }

    [TestFixture]
    public class DiffMergeTests
    {
        [Test]
        public void CommonSubstrings_should_find_common_substrings()
        {
            var text1 = "123abc789def";
            var text2 = "012a3c78ghe9iej";

            var substrings = TextCompareUtil.CommonSubstrings(text1, text2);

            substrings.Should().HaveCount(5);

            substrings.ElementAt(0).FirstStart.Should().Be(0);
            substrings.ElementAt(0).SecondStart.Should().Be(1);
            substrings.ElementAt(0).Value.Should().Be("12");

            substrings.ElementAt(1).FirstStart.Should().Be(3);
            substrings.ElementAt(1).SecondStart.Should().Be(3);
            substrings.ElementAt(1).Value.Should().Be("a");

            substrings.ElementAt(2).FirstStart.Should().Be(5);
            substrings.ElementAt(2).SecondStart.Should().Be(5);
            substrings.ElementAt(2).Value.Should().Be("c78");

            substrings.ElementAt(3).FirstStart.Should().Be(8);
            substrings.ElementAt(3).SecondStart.Should().Be(11);
            substrings.ElementAt(3).Value.Should().Be("9");

            substrings.ElementAt(4).FirstStart.Should().Be(10);
            substrings.ElementAt(4).SecondStart.Should().Be(13);
            substrings.ElementAt(4).Value.Should().Be("e");
        }
    }
}