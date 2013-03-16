namespace UnitTests.DiffMerge.Lib
{
    public class CommonPart
    {
        public CommonPart(int firstStart, int secondStart, int length)
        {
            FirstStart = firstStart;
            SecondStart = secondStart;
            Length = length;
        }

        public int FirstStart { get; private set; }
        public int SecondStart { get; private set; }
        public int Length { get; private set; }
    }
}