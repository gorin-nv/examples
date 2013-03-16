using System.Collections.Generic;

namespace UnitTests.DiffMerge.Lib
{
    public interface ICommonSubstringsFinder
    {
        IEnumerable<CommonPart> CommonSubstrings(string text1, string text2);
    }
}