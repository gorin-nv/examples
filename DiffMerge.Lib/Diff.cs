using System.Collections.Generic;

namespace DiffMerge.Lib
{
    public class Diff
    {
        public Diff(IEnumerable<DifferencePart> differenceParts, IEnumerable<CommonPart> commonParts)
        {
            CommonParts = commonParts;
            DifferenceParts = differenceParts;
        }

        public IEnumerable<DifferencePart> DifferenceParts { get; private set; }
        public IEnumerable<CommonPart> CommonParts { get; private set; }
    }
}