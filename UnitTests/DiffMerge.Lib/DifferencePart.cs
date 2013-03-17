namespace UnitTests.DiffMerge.Lib
{
    public class DifferencePart
    {
        public DifferencePart(Range firstRange, Range secondRange)
        {
            SecondRange = secondRange;
            FirstRange = firstRange;
        }

        public Range FirstRange { get; private set; }
        public Range SecondRange { get; private set; }
    }
}