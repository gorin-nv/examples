namespace DiffMerge.Lib.DiffStructure
{
    public class CommonPart
    {
        public CommonPart(Range origianl, Range changed)
        {
            Original = origianl;
            Changed = changed;
        }

        public CommonPart(int originalStart, int changedStart, int commonLength)
        {
            Original = new Range(originalStart, originalStart + commonLength - 1);
            Changed = new Range(changedStart, changedStart + commonLength - 1);
        }

        public Range Original { get; private set; }
        public Range Changed { get; private set; }
    }
}