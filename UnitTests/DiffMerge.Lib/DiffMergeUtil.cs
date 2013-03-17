using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.DiffMerge.Lib
{
    public class DiffMergeUtil
    {
        public Diff MakeDiff(ICommonSubstringsFinder finder, string text1, string text2)
        {
            var positions = finder.CommonSubstrings(text1, text2);
            return MakeDiff(positions, text1.Length, text2.Length);
        }

        public Diff MakeDiff(IEnumerable<CommonPart> substringPositions, int firstLength, int secondLength)
        {
            var commonParts = substringPositions as CommonPart[] ?? substringPositions.ToArray();
            var differenceParts = CreateDifference(commonParts, firstLength, secondLength).ToArray();
            return new Diff(differenceParts, commonParts);
        }

        private IEnumerable<DifferencePart> CreateDifference(CommonPart[] substringPositions, int firstLength, int secondLength)
        {
            if (substringPositions.Length == 0)
                yield break;

            var begin = BeginDifferencePart(substringPositions);
            if (begin != null)
                yield return begin;

            var neighbourCommonParts = new EnumerableHelper().SplitByPair(substringPositions);
            foreach (var pair in neighbourCommonParts)
            {
                var first = Range.Between(pair.First.First, pair.Second.First);
                var second = Range.Between(pair.First.Second, pair.Second.Second);
                yield return new DifferencePart(first, second);
            }

            var end = EndDifferencePart(substringPositions, firstLength, secondLength);
            if (end != null)
                yield return end;
        }

        private DifferencePart BeginDifferencePart(CommonPart[] substringPositions)
        {
            Func<Range, Range> beginRange =
                range => range.Start > 0
                             ? new Range(0, range.Start - 1)
                             : null;
            var firstBegin = beginRange(substringPositions[0].First);
            var secondBegin = beginRange(substringPositions[0].Second);
            return firstBegin != null || secondBegin != null
                    ? new DifferencePart(firstBegin, secondBegin)
                    : null;
        }

        private DifferencePart EndDifferencePart(CommonPart[] substringPositions, int firstLength, int secondLength)
        {
            Func<Range, int, Range> endRange =
                (range, maximum) => range.Stop < maximum
                                        ? new Range(range.Stop + 1, maximum)
                                        : null;
            var firstEnd = endRange(substringPositions.Last().First, firstLength - 1);
            var secondEnd = endRange(substringPositions.Last().Second, secondLength - 1);
            return firstEnd != null || secondEnd != null
                       ? new DifferencePart(firstEnd, secondEnd)
                       : null;
        }
    }
}