using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace UnitTests
{
    public interface ISubstringPosition
    {
        int FirstStart { get; }
        int SecondStart { get; }
        string Value { get; }
    }

    public class SubstringPosition : ISubstringPosition
    {
        private readonly string _firstSource;
        private readonly int _length;

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
        public static IEnumerable<ISubstringPosition> CommonSubstrings(string text1, string text2)
        {
            var currentFirstPosition = 0;
            var currentSecondPosition = 0;
            while (currentFirstPosition < text1.Length && currentSecondPosition < text2.Length)
            {
                var position = FindFirstSubstribngPosition(
                    text1, currentFirstPosition,
                    text2, currentSecondPosition);
                if (position == null)
                    yield break;
                yield return position;
                currentFirstPosition = position.FirstStart + position.Value.Length;
                currentSecondPosition = position.SecondStart + position.Value.Length;
            }
        }

        public static SubstringPosition FindFirstSubstribngPosition(string text1, int position1, string text2, int position2)
        {
            int start1;
            int start2;
            
            if(!FindFirstCommonChar(
                text1, position1,
                text2, position2,
                out start1, out start2))
                return null;

            var length = CommonCharsLength(
                text1, start1,
                text2, start2);

            return new SubstringPosition(start1, start2, length, text1);
        }

        public static bool FindFirstCommonChar(string text1, int position1, string text2, int position2, out int start1, out int start2)
        {
            start1 = position1;
            while (start1 < text1.Length)
            {
                var currentChar = text1[start1];
                start2 = text2.IndexOf(currentChar, position2);
                if (start2 >= 0)
                    return true;
                start1 += 1;
            }

            start1 = default(int);
            start2 = default(int);
            return false;
        }

        public static int CommonCharsLength(string text1, int start1, string text2, int start2)
        {
            var length = 0;
            while (
                text1.Length > start1+length &&
                text2.Length > start2+length &&
                text1[start1+length] == text2[start2+length]
                )
            {
                length += 1;
            }
            return length;
        }
    }

    [TestFixture]
    public class TextCompareUtilTests
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

            substrings.ElementAt(1).FirstStart.Should().Be(2);
            substrings.ElementAt(1).SecondStart.Should().Be(4);
            substrings.ElementAt(1).Value.Should().Be("3");

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

        [Test]
        public void FindFirstSubstribngPosition_find_first_position()
        {
            var position1 = TextCompareUtil.FindFirstSubstribngPosition(
                "abc123abc456", 0,
                "qwebbccbce", 0);
            position1.FirstStart.Should().Be(1);
            position1.SecondStart.Should().Be(3);
            position1.Value.Should().Be("b");

            var position2 = TextCompareUtil.FindFirstSubstribngPosition(
                "abc123abc456", 0,
                "qwebbccbce", 5);
            position2.FirstStart.Should().Be(1);
            position2.SecondStart.Should().Be(7);
            position2.Value.Should().Be("bc");

            var position3 = TextCompareUtil.FindFirstSubstribngPosition(
                "abc123abc456", 2,
                "qwebbccbce", 1);
            position3.FirstStart.Should().Be(2);
            position3.SecondStart.Should().Be(5);
            position3.Value.Should().Be("c");

            var position4 = TextCompareUtil.FindFirstSubstribngPosition(
                "abc123abc456", 2,
                "qwebbccbce", 1);
            position4.FirstStart.Should().Be(2);
            position4.SecondStart.Should().Be(5);
            position4.Value.Should().Be("c");

            var position5 = TextCompareUtil.FindFirstSubstribngPosition(
                "abc123abc456", 3,
                "qwebbccbce", 5);
            position5.FirstStart.Should().Be(7);
            position5.SecondStart.Should().Be(7);
            position5.Value.Should().Be("bc");

            var position6 = TextCompareUtil.FindFirstSubstribngPosition(
                "abc123abc456", 9,
                "qwebbccbce", 1);
            position6.Should().BeNull();
        }

        [Test]
        public void FindFirstCommonChar_should_find_first_common_char()
        {
            int start1, start2;

            TextCompareUtil.FindFirstCommonChar(
                "012345678", 1,
                "_2____2__", 0,
                out start1, out start2).Should().BeTrue();
            start1.Should().Be(2);
            start2.Should().Be(1);

            TextCompareUtil.FindFirstCommonChar(
                "012345678", 1,
                "_2____2__", 2,
                out start1, out start2).Should().BeTrue();
            start1.Should().Be(2);
            start2.Should().Be(6);

            TextCompareUtil.FindFirstCommonChar(
                "012345679", 1,
                "_2____2__", 7,
                out start1, out start2).Should().BeFalse();

            TextCompareUtil.FindFirstCommonChar(
                "012345679", 3,
                "_2____2__", 1,
                out start1, out start2).Should().BeFalse();
        }

        [Test]
        public void CommonCharsLength_sould_calc_common_chars_sequence_length()
        {
            TextCompareUtil.CommonCharsLength(
                "0123456", 1,
                "0123456", 1).Should().Be(6);

            TextCompareUtil.CommonCharsLength(
                "0123456789", 0,
                "01234567cd", 0).Should().Be(8);

            TextCompareUtil.CommonCharsLength(
                "0123456789", 2,
                "01234567cd", 2).Should().Be(6);

            TextCompareUtil.CommonCharsLength(
                "0123456789", 7,
                "01234567cd", 7).Should().Be(1);
        }
    }

    public class Replace
    {
    }

    public class Diff
    {
        public IEnumerable<Replace> Replaces { get; private set; }
    }

    public static class DiffMergeUtil
    {
        public static Diff MakeDiff(string text1, string text2)
        {
            var positions = TextCompareUtil.CommonSubstrings(text1, text2);
            return MakeDiff(positions, text1.Length, text2.Length);
        }

        public static Diff MakeDiff(IEnumerable<ISubstringPosition> substringPositions, int firstLength, int secondLength)
        {
            throw new NotImplementedException();
        }
    }

    [TestFixture]
    public class DiffMergeUtilTests
    {
        private class PositionForTesting : ISubstringPosition
        {
            public static IEnumerable<ISubstringPosition> Build(params PositionForTesting[] positions)
            {
                return positions;
            }

            public int FirstStart { get; set; }

            public int SecondStart { get; set; }

            public string Value { get; set; }
        }

        [Test]
        public void MakeDiff_should_make_diff()
        {
            throw new NotImplementedException();

            var diff1 = DiffMergeUtil.MakeDiff(
                PositionForTesting.Build(
                    new PositionForTesting {FirstStart = 1, SecondStart = 2, Value = "abc"}),
                10, 10);
            diff1.Replaces.Should().HaveCount(2);
            
        }
    }
}