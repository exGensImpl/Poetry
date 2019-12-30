using System;
using System.Collections.Generic;
using System.Linq;

namespace ExGens.Poetry
{
    /// <summary>
    /// Contains all the extension methods in this project
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Generates an infinite sequence of values of type <typeparamref name="T"/>
        /// starting from the initial value specified and produses the next values
        /// by the <paramref name="mapper"/> function
        /// </summary>
        /// <typeparam name="T">The element type of the collection generated</typeparam>
        /// <param name="initial">The initial value</param>
        /// <param name="mapper">
        /// The function generating next values for each sequence element.
        /// If all the fuction calls return an empty sequence the generation will be ended.
        /// </param>
        /// <returns>An infinite sequence (a flatten tree) of values of type <typeparamref name="T"/></returns>
        public static IEnumerable<T> Unfold<T>(this T initial, Func<T, IEnumerable<T>> mapper)
        {
            foreach (var result in mapper(initial))
            {
                yield return result;
                foreach (var next in result.Unfold(mapper))
                {
                    yield return next;
                }
            }
        }

        public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
                yield return item;
            }
        }
        
        public static TOut To<T,TOut>(this T value, Func<T,TOut> func) => func(value);

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) => source.Shuffle(new Random());

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (rng == null) throw new ArgumentNullException("rng");

            return ShuffleIterator();

            IEnumerable<T> ShuffleIterator()
            {
                var buffer = source.ToList();
                for (int i = 0; i < buffer.Count; i++)
                {
                    int j = rng.Next(i, buffer.Count);
                    yield return buffer[j];

                    buffer[j] = buffer[i];
                }
            }
        }
    }
}
