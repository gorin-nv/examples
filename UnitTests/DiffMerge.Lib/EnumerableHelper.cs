using System.Collections.Generic;

namespace UnitTests.DiffMerge.Lib
{
    public class EnumerableHelper
    {
        public IEnumerable<Pair<T>> SplitByPair<T>(IEnumerable<T> enumerable)
        {
            var iterator = enumerable.GetEnumerator();
            if (!iterator.MoveNext())
                yield break;
            var first = iterator.Current;
            if (!iterator.MoveNext())
                yield break;
            do
            {
                var second = iterator.Current;
                yield return new Pair<T>(first, second);
                first = second;
            } while (iterator.MoveNext());
        }
    }

    public class Pair<T>
    {
        public Pair(T first, T second)
        {
            First = first;
            Second = second;
        }

        public T First { get; private set; }
        public T Second { get; private set; }
    }
}