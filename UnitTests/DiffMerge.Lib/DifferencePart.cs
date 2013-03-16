namespace UnitTests.DiffMerge.Lib
{
    public class DifferencePart
    {
        public DifferencePart(Interval firstInterval, Interval secondInterval)
        {
            SecondInterval = secondInterval;
            FirstInterval = firstInterval;
        }

        public Interval FirstInterval { get; private set; }
        public Interval SecondInterval { get; private set; }
    }
}