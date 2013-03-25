using System;
using System.Collections.Generic;
using System.Linq;

namespace DiffMerge.Lib
{
    public class DiffMergeUtil
    {
        public Diff MakeDiff(ICommonSubstringsFinder finder, string original, string changed)
        {
            var positions = finder.CommonSubstrings(original, changed);
            return MakeDiff(positions, original.Length, changed.Length);
        }

        public Diff MakeDiff(IEnumerable<CommonPart> commonParts, int originalLength, int changedLength)
        {
            var enumeratedCommonParts = commonParts as CommonPart[] ?? commonParts.ToArray();
            var differenceParts = CreateDifference(enumeratedCommonParts, originalLength, changedLength).ToArray();
            return new Diff(differenceParts, enumeratedCommonParts);
        }

        private IEnumerable<DifferencePart> CreateDifference(CommonPart[] commonParts, int originalLength, int changedLength)
        {
            if (commonParts.Length == 0)
                yield break;

            var firstCommonPart = commonParts[0];
            var originalBegin = FirstRange(firstCommonPart.Original);
            var changedBegin = FirstRange(firstCommonPart.Changed);
            if (originalBegin != null || changedBegin != null)
                yield return new DifferencePart(originalBegin, changedBegin);

            var neighbourCommonParts = new EnumerableHelper().SplitByPair(commonParts);
            foreach (var pair in neighbourCommonParts)
            {
                var original = Range.Between(pair.First.Original, pair.Second.Original);
                var changed = Range.Between(pair.First.Changed, pair.Second.Changed);
                yield return new DifferencePart(original, changed);
            }

            var lastCommonPart = commonParts.Last();
            var originalEnd = LastRange(lastCommonPart.Original, originalLength);
            var changedEnd = LastRange(lastCommonPart.Changed, changedLength);
            if(originalEnd != null || changedEnd != null)
                yield return new DifferencePart(originalEnd, changedEnd);
        }

        private Range FirstRange(Range range)
        {
            var lowBorder = new Range(-1, -1);
            return Range.Between(lowBorder, range);
        }

        private Range LastRange(Range range, int length)
        {
            var highBorder = new Range(length, length);
            return Range.Between(range, highBorder);
        }
    }
}