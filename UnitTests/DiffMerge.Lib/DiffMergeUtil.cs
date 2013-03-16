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
    }
}