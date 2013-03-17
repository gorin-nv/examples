namespace UnitTests.DiffMerge.Lib
{
    public class Range
    {
        public Range(int start, int stop)
        {
            Stop = stop;
            Start = start;
        }

        public int Start { get; private set; }
        public int Stop { get; private set; }

        public int Length
        {
            get { return Stop - Start + 1; }
        }

        public static Range Between(Range first, Range second)
        {
            return new Range(first.Stop + 1, second.Start - 1);
        }
    }
}