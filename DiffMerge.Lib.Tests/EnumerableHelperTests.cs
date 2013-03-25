using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace DiffMerge.Lib.Tests
{
    [TestFixture]
    public class EnumerableHelperTests
    {
        [Test]
        public void SplitByPair_should_split_enumerable_by_two()
        {
            var testedEnumerable = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var expectedEnumerable = new[]
                                         {
                                             new Pair<int>(1, 2),
                                             new Pair<int>(2, 3),
                                             new Pair<int>(3, 4),
                                             new Pair<int>(4, 5),
                                             new Pair<int>(5, 6),
                                             new Pair<int>(6, 7),
                                             new Pair<int>(7, 8)
                                         };

            var enumerable = new EnumerableHelper().SplitByPair(testedEnumerable).ToList();

            enumerable.Should().HaveSameCount(expectedEnumerable);

            for (var i = 0; i < enumerable.Count; i++)
            {
                var actual = enumerable.ElementAt(i);
                var expected = expectedEnumerable.ElementAt(i);

                actual.First.Should().Be(expected.First);
                actual.Second.Should().Be(expected.Second);
            }
        }
    }
}