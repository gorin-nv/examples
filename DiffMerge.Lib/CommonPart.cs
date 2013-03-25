namespace DiffMerge.Lib
{
    public class CommonPart
    {
        public CommonPart(Range first, Range second)
        {
            First = first;
            Second = second;
        }

        public CommonPart(int firstStart, int secondStart, int length)
        {
            First = new Range(firstStart, firstStart + length - 1);
            Second = new Range(secondStart, secondStart + length - 1);
        }

        public Range First { get; private set; }
        public Range Second { get; private set; }
    }
}