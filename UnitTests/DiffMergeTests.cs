using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace UnitTests
{
    public class CommonPart
    {
        public CommonPart(int firstStart, int secondStart, int length)
        {
            FirstStart = firstStart;
            SecondStart = secondStart;
            Length = length;
        }

        public int FirstStart { get; private set; }
        public int SecondStart { get; private set; }
        public int Length { get; private set; }
    }

    public class Interval
    {
        public Interval(int start, int length)
        {
            Length = length;
            Start = start;
        }

        public int Start { get; private set; }
        public int Length { get; private set; }
    }

    public class DifferencePart
    {
        public DifferencePart(Interval firstInterval, Interval secondInterval)
        {
            SecondInterval = secondInterval;
            FirstInterval = firstInterval;
        }

        public Interval FirstInterval { get; private set; }
        public Interval SecondInterval { get; private set; }
    }

    public class Diff
    {
        public Diff(IEnumerable<DifferencePart> differenceParts, IEnumerable<CommonPart> commonParts)
        {
            CommonParts = commonParts;
            DifferenceParts = differenceParts;
        }

        public IEnumerable<DifferencePart> DifferenceParts { get; private set; }
        public IEnumerable<CommonPart> CommonParts { get; private set; }
    }

    public static class DiffMergeUtil
    {
        public static Diff MakeDiff(string text1, string text2)
        {
            var positions = CommonSubstrings(text1, text2);
            return MakeDiff(positions, text1.Length, text2.Length);
        }

        public static Diff MakeDiff(IEnumerable<CommonPart> substringPositions, int firstLength, int secondLength)
        {
            var firstPosition = 0;
            var secondPosition = 0;
            var differenceParts = substringPositions.Aggregate(
                seed: new List<DifferencePart>(),
                func: (diffs, common) =>
                {
                    var length1 = common.FirstStart - firstPosition;
                    var length2 = common.SecondStart - secondPosition;
                    DifferencePart newDiff = null;
                    if (length1 != 0 || length2 != 0)
                    {
                        //newDiff = new DifferencePart(firstPosition, firstLength, secondPosition, secondLength);
                        //diffs.Add(newDiff);
                        //firstPosition = common.FirstStart + common.Length;
                        //secondPosition = common.SecondStart + common.Length;
                    }
                    if (newDiff != null)
                        diffs.Add(newDiff);
                    return diffs;
                });

            return new Diff(differenceParts, substringPositions);
        }

        public static IEnumerable<CommonPart> CommonSubstrings(string text1, string text2)
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
                currentFirstPosition = position.FirstStart + position.Length;
                currentSecondPosition = position.SecondStart + position.Length;
            }
        }

        public static CommonPart FindFirstSubstribngPosition(string text1, int position1, string text2, int position2)
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

            return new CommonPart(start1, start2, length);
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
    public class DiffMergeUtilTests
    {
        [Test]
        public void MakeDiff_should_make_diff()
        {
            var diff = DiffMergeUtil.MakeDiff(
                new[]
                    {
                        new CommonPart(1, 2, 4),
                        new CommonPart(7, 9, 2)
                    },
                15, 20);

            diff.DifferenceParts.Should().HaveCount(3);

            diff.DifferenceParts.ElementAt(0).FirstInterval.Start.Should().Be(0);
            diff.DifferenceParts.ElementAt(0).FirstInterval.Length.Should().Be(1);
            diff.DifferenceParts.ElementAt(0).SecondInterval.Start.Should().Be(0);
            diff.DifferenceParts.ElementAt(0).SecondInterval.Length.Should().Be(2);

            diff.DifferenceParts.ElementAt(1).FirstInterval.Start.Should().Be(5);
            diff.DifferenceParts.ElementAt(1).FirstInterval.Length.Should().Be(2);
            diff.DifferenceParts.ElementAt(1).SecondInterval.Start.Should().Be(6);
            diff.DifferenceParts.ElementAt(1).SecondInterval.Length.Should().Be(3);

            diff.DifferenceParts.ElementAt(2).FirstInterval.Start.Should().Be(9);
            diff.DifferenceParts.ElementAt(2).FirstInterval.Length.Should().Be(6);
            diff.DifferenceParts.ElementAt(2).SecondInterval.Start.Should().Be(11);
            diff.DifferenceParts.ElementAt(2).SecondInterval.Length.Should().Be(9);

            diff.CommonParts.Should().HaveCount(2);

            diff.CommonParts.ElementAt(0).FirstStart.Should().Be(1);
            diff.CommonParts.ElementAt(0).SecondStart.Should().Be(2);
            diff.CommonParts.ElementAt(0).Length.Should().Be(4);

            diff.CommonParts.ElementAt(0).FirstStart.Should().Be(7);
            diff.CommonParts.ElementAt(0).SecondStart.Should().Be(9);
            diff.CommonParts.ElementAt(0).Length.Should().Be(2);
        }

        [Test]
        public void CommonSubstrings_should_find_common_substrings()
        {
            var text1 = "123abc789def";
            var text2 = "012a3c78ghe9iej";

            var substrings = DiffMergeUtil.CommonSubstrings(text1, text2);

            substrings.Should().HaveCount(5);

            substrings.ElementAt(0).FirstStart.Should().Be(0);
            substrings.ElementAt(0).SecondStart.Should().Be(1);
            substrings.ElementAt(0).Length.Should().Be(2);

            substrings.ElementAt(1).FirstStart.Should().Be(2);
            substrings.ElementAt(1).SecondStart.Should().Be(4);
            substrings.ElementAt(1).Length.Should().Be(1);

            substrings.ElementAt(2).FirstStart.Should().Be(5);
            substrings.ElementAt(2).SecondStart.Should().Be(5);
            substrings.ElementAt(2).Length.Should().Be(3);

            substrings.ElementAt(3).FirstStart.Should().Be(8);
            substrings.ElementAt(3).SecondStart.Should().Be(11);
            substrings.ElementAt(3).Length.Should().Be(1);

            substrings.ElementAt(4).FirstStart.Should().Be(10);
            substrings.ElementAt(4).SecondStart.Should().Be(13);
            substrings.ElementAt(4).Length.Should().Be(1);
        }

        [Test]
        public void FindFirstSubstribngPosition_find_first_position()
        {
            var position1 = DiffMergeUtil.FindFirstSubstribngPosition(
                "abc123abc456", 0,
                "qwebbccbce", 0);
            position1.FirstStart.Should().Be(1);
            position1.SecondStart.Should().Be(3);
            position1.Length.Should().Be(1);

            var position2 = DiffMergeUtil.FindFirstSubstribngPosition(
                "abc123abc456", 0,
                "qwebbccbce", 5);
            position2.FirstStart.Should().Be(1);
            position2.SecondStart.Should().Be(7);
            position2.Length.Should().Be(2);

            var position3 = DiffMergeUtil.FindFirstSubstribngPosition(
                "abc123abc456", 2,
                "qwebbccbce", 1);
            position3.FirstStart.Should().Be(2);
            position3.SecondStart.Should().Be(5);
            position3.Length.Should().Be(1);

            var position4 = DiffMergeUtil.FindFirstSubstribngPosition(
                "abc123abc456", 2,
                "qwebbccbce", 1);
            position4.FirstStart.Should().Be(2);
            position4.SecondStart.Should().Be(5);
            position4.Length.Should().Be(1);

            var position5 = DiffMergeUtil.FindFirstSubstribngPosition(
                "abc123abc456", 3,
                "qwebbccbce", 5);
            position5.FirstStart.Should().Be(7);
            position5.SecondStart.Should().Be(7);
            position5.Length.Should().Be(2);

            var position6 = DiffMergeUtil.FindFirstSubstribngPosition(
                "abc123abc456", 9,
                "qwebbccbce", 1);
            position6.Should().BeNull();
        }

        [Test]
        public void FindFirstCommonChar_should_find_first_common_char()
        {
            int start1, start2;

            DiffMergeUtil.FindFirstCommonChar(
                "012345678", 1,
                "_2____2__", 0,
                out start1, out start2).Should().BeTrue();
            start1.Should().Be(2);
            start2.Should().Be(1);

            DiffMergeUtil.FindFirstCommonChar(
                "012345678", 1,
                "_2____2__", 2,
                out start1, out start2).Should().BeTrue();
            start1.Should().Be(2);
            start2.Should().Be(6);

            DiffMergeUtil.FindFirstCommonChar(
                "012345679", 1,
                "_2____2__", 7,
                out start1, out start2).Should().BeFalse();

            DiffMergeUtil.FindFirstCommonChar(
                "012345679", 3,
                "_2____2__", 1,
                out start1, out start2).Should().BeFalse();
        }

        [Test]
        public void CommonCharsLength_should_calc_common_chars_sequence_length()
        {
            DiffMergeUtil.CommonCharsLength(
                "0123456", 1,
                "0123456", 1).Should().Be(6);

            DiffMergeUtil.CommonCharsLength(
                "0123456789", 0,
                "01234567cd", 0).Should().Be(8);

            DiffMergeUtil.CommonCharsLength(
                "0123456789", 2,
                "01234567cd", 2).Should().Be(6);

            DiffMergeUtil.CommonCharsLength(
                "0123456789", 7,
                "01234567cd", 7).Should().Be(1);
        }
    }
}