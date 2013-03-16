using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using UnitTests.DiffMerge.Lib;

namespace UnitTests.DiffMerge.Tests
{
    [TestFixture]
    public class DiffMergeUtilTests
    {
        [Test]
        public void MakeDiff_should_make_diff()
        {
            var diff = new DiffMergeUtil().MakeDiff(
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
    }
}