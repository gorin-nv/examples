using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace DiffMerge.Lib.Tests
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
                        new CommonPart(new Range(1,4), new Range(2,5)),
                        new CommonPart(new Range(7,8), new Range(9,10))
                    },
                15, 20);

            diff.DifferenceParts.Should().HaveCount(3);

            diff.DifferenceParts.ElementAt(0).FirstRange.Start.Should().Be(0);
            diff.DifferenceParts.ElementAt(0).FirstRange.Stop.Should().Be(0);
            diff.DifferenceParts.ElementAt(1).FirstRange.Start.Should().Be(5);
            diff.DifferenceParts.ElementAt(1).FirstRange.Stop.Should().Be(6);
            diff.DifferenceParts.ElementAt(2).FirstRange.Start.Should().Be(9);
            diff.DifferenceParts.ElementAt(2).FirstRange.Stop.Should().Be(14);

            diff.DifferenceParts.ElementAt(0).SecondRange.Start.Should().Be(0);
            diff.DifferenceParts.ElementAt(0).SecondRange.Stop.Should().Be(1);
            diff.DifferenceParts.ElementAt(1).SecondRange.Start.Should().Be(6);
            diff.DifferenceParts.ElementAt(1).SecondRange.Stop.Should().Be(8);
            diff.DifferenceParts.ElementAt(2).SecondRange.Start.Should().Be(11);
            diff.DifferenceParts.ElementAt(2).SecondRange.Stop.Should().Be(19);
        }

        [Test]
        public void MakeDiff_should_use_sended_common_parts()
        {
            var diff = new DiffMergeUtil().MakeDiff(
                new[]
                    {
                        new CommonPart(new Range(1,4), new Range(2,5)),
                        new CommonPart(new Range(7,8), new Range(9,10))
                    },
                15, 20);

            diff.CommonParts.Should().HaveCount(2);

            diff.CommonParts.ElementAt(0).First.Start.Should().Be(1);
            diff.CommonParts.ElementAt(0).First.Stop.Should().Be(4);
            diff.CommonParts.ElementAt(1).First.Start.Should().Be(7);
            diff.CommonParts.ElementAt(1).First.Stop.Should().Be(8);

            diff.CommonParts.ElementAt(0).Second.Start.Should().Be(2);
            diff.CommonParts.ElementAt(0).Second.Stop.Should().Be(5);
            diff.CommonParts.ElementAt(1).Second.Start.Should().Be(9);
            diff.CommonParts.ElementAt(1).Second.Stop.Should().Be(10);
        }
    }
}