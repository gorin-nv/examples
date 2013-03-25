using System.Collections.Generic;
using System.Linq;
using DiffMerge.Lib.DiffStructure;

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

        private IEnumerable<DifferencePart> CreateDifference(IEnumerable<CommonPart> commonParts, int originalLength, int changedLength)
        {
            return from pair in BorderCommonParts(commonParts, originalLength, changedLength).SplitByPair()
                   let original = Range.Between(pair.First.Original, pair.Second.Original)
                   let changed = Range.Between(pair.First.Changed, pair.Second.Changed)
                   where original != null || changed != null
                   select new DifferencePart(original, changed);
        }

        private IEnumerable<CommonPart> BorderCommonParts(IEnumerable<CommonPart> commonParts, int originalLength, int changedLength)
        {
            var lowBorder = new Range(-1, -1);
            yield return new CommonPart(lowBorder, lowBorder);

            foreach (var commonPart in commonParts)
            {
                yield return commonPart;
            }

            var originalHighBorder = new Range(originalLength, originalLength);
            var changedHighBorder = new Range(changedLength, changedLength);
            yield return new CommonPart(originalHighBorder, changedHighBorder);
        }
    }
}