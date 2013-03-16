using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using UnitTests.DiffMerge.Lib;

namespace UnitTests.DiffMerge.Tests
{
    [TestFixture]
    public class CommonSubstringsFinderTests
    {
        [Test]
        public void CommonSubstrings_should_find_common_substrings()
        {
            var text1 = "123abc789def";
            var text2 = "012a3c78ghe9iej";

            var substrings = new CommonSubstringsFinder().CommonSubstrings(text1, text2);

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
            var position1 = new CommonSubstringsFinder().FindFirstSubstribngPosition(
                "abc123abc456", 0,
                "qwebbccbce", 0);
            position1.FirstStart.Should().Be(1);
            position1.SecondStart.Should().Be(3);
            position1.Length.Should().Be(1);

            var position2 = new CommonSubstringsFinder().FindFirstSubstribngPosition(
                "abc123abc456", 0,
                "qwebbccbce", 5);
            position2.FirstStart.Should().Be(1);
            position2.SecondStart.Should().Be(7);
            position2.Length.Should().Be(2);

            var position3 = new CommonSubstringsFinder().FindFirstSubstribngPosition(
                "abc123abc456", 2,
                "qwebbccbce", 1);
            position3.FirstStart.Should().Be(2);
            position3.SecondStart.Should().Be(5);
            position3.Length.Should().Be(1);

            var position4 = new CommonSubstringsFinder().FindFirstSubstribngPosition(
                "abc123abc456", 2,
                "qwebbccbce", 1);
            position4.FirstStart.Should().Be(2);
            position4.SecondStart.Should().Be(5);
            position4.Length.Should().Be(1);

            var position5 = new CommonSubstringsFinder().FindFirstSubstribngPosition(
                "abc123abc456", 3,
                "qwebbccbce", 5);
            position5.FirstStart.Should().Be(7);
            position5.SecondStart.Should().Be(7);
            position5.Length.Should().Be(2);

            var position6 = new CommonSubstringsFinder().FindFirstSubstribngPosition(
                "abc123abc456", 9,
                "qwebbccbce", 1);
            position6.Should().BeNull();
        }

        [Test]
        public void FindFirstCommonChar_should_find_first_common_char()
        {
            int start1, start2;

            new CommonSubstringsFinder().FindFirstCommonChar(
                "012345678", 1,
                "_2____2__", 0,
                out start1, out start2).Should().BeTrue();
            start1.Should().Be(2);
            start2.Should().Be(1);

            new CommonSubstringsFinder().FindFirstCommonChar(
                "012345678", 1,
                "_2____2__", 2,
                out start1, out start2).Should().BeTrue();
            start1.Should().Be(2);
            start2.Should().Be(6);

            new CommonSubstringsFinder().FindFirstCommonChar(
                "012345679", 1,
                "_2____2__", 7,
                out start1, out start2).Should().BeFalse();

            new CommonSubstringsFinder().FindFirstCommonChar(
                "012345679", 3,
                "_2____2__", 1,
                out start1, out start2).Should().BeFalse();
        }

        [Test]
        public void CommonCharsLength_should_calc_common_chars_sequence_length()
        {
            new CommonSubstringsFinder().CommonCharsLength(
                "0123456", 1,
                "0123456", 1).Should().Be(6);

            new CommonSubstringsFinder().CommonCharsLength(
                "0123456789", 0,
                "01234567cd", 0).Should().Be(8);

            new CommonSubstringsFinder().CommonCharsLength(
                "0123456789", 2,
                "01234567cd", 2).Should().Be(6);

            new CommonSubstringsFinder().CommonCharsLength(
                "0123456789", 7,
                "01234567cd", 7).Should().Be(1);
        }
    }
}