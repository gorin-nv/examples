namespace DiffMerge.Lib.DiffStructure
{
    public class DifferencePart
    {
        public DifferencePart(Range original, Range changed)
        {
            Original = original;
            Changed = changed;
        }

        public Range Original { get; private set; }
        public Range Changed { get; private set; }
    }
}