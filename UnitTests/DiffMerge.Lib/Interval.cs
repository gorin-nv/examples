namespace UnitTests.DiffMerge.Lib
{
    public class Interval
    {
        public Interval(int start, int length)
        {
            Length = length;
            Start = start;
        }

        public int Start { get; private set; }
        public int Length { get; private set; }
    }
}