using System.Collections.Generic;
using DiffMerge.Lib.DiffStructure;

namespace DiffMerge.Lib
{
    public interface ICommonSubstringsFinder
    {
        IEnumerable<CommonPart> CommonSubstrings(string text1, string text2);
    }
}