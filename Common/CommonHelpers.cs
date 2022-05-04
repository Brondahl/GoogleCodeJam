namespace Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class StringHelpers
    {
        public static int[] SplitToIntArray(this string input)
            => input.SplitToTypedArray(int.Parse);

        public static long[] SplitToLongArray(this string input)
            => input.SplitToTypedArray(long.Parse);

        public static T[] SplitToTypedArray<T>(this string input, Func<string, T> converter)
            => input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(converter).ToArray();

        public static string StringJoin(this IEnumerable<string> input, string separator = ",")
            => string.Join(separator, input);

        public static char[] Alphabet => "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    }

    public static class MathsHelpers
    {

    }

    public static class IEnumerableHelpers
    {

    }
}
