using System.Collections.Generic;

namespace UnitTests.DiffMerge.Lib
{
    public class CommonSubstringsFinder : ICommonSubstringsFinder
    {
        public IEnumerable<CommonPart> CommonSubstrings(string text1, string text2)
        {
            var currentFirstPosition = 0;
            var currentSecondPosition = 0;
            while (currentFirstPosition < text1.Length && currentSecondPosition < text2.Length)
            {
                var position = FindFirstSubstribngPosition(
                    text1, currentFirstPosition,
                    text2, currentSecondPosition);
                if (position == null)
                    yield break;
                yield return position;
                currentFirstPosition = position.First.Stop + 1;
                currentSecondPosition = position.Second.Stop + 1;
            }
        }

        public CommonPart FindFirstSubstribngPosition(string text1, int position1, string text2, int position2)
        {
            int start1;
            int start2;

            if (!FindFirstCommonChar(
                text1, position1,
                text2, position2,
                out start1, out start2))
                return null;

            var length = CommonCharsLength(
                text1, start1,
                text2, start2);

            return new CommonPart(start1, start2, length);
        }

        public bool FindFirstCommonChar(string text1, int position1, string text2, int position2, out int start1, out int start2)
        {
            start1 = position1;
            while (start1 < text1.Length)
            {
                var currentChar = text1[start1];
                start2 = text2.IndexOf(currentChar, position2);
                if (start2 >= 0)
                    return true;
                start1 += 1;
            }

            start1 = default(int);
            start2 = default(int);
            return false;
        }

        public int CommonCharsLength(string text1, int start1, string text2, int start2)
        {
            var length = 0;
            while (
                text1.Length > start1 + length &&
                text2.Length > start2 + length &&
                text1[start1 + length] == text2[start2 + length]
                )
            {
                length += 1;
            }
            return length;
        }
    }
}