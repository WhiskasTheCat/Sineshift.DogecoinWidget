using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sineshift.DogecoinWidget.Common
{
    public static class CollectionExtensions
    {
		readonly static Random rand = new Random();

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
                collection.Add(item);
        }

        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
                collection.Remove(item);
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);

            return items;
        }

		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> items, Action<int, T> action)
		{
			int index = 0;

			foreach (var item in items)
			{
				action(index, item);
				index++;
			}

			return items;
		}

		public static bool None<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            return items.All(item => !predicate(item));
        }

		public static bool None<T>(this IEnumerable<T> items)
		{
			return !items.Any();
		}

		public static int IndexOf<T>(this IEnumerable<T> source, T value)
        {
            int index = 0;
            var comparer = EqualityComparer<T>.Default;

            foreach (T item in source)
            {
                if (comparer.Equals(item, value))
                    return index;
                index++;
            }

            return -1;
        }

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items)
		{
			var list = items.ToList();
			int n = list.Count;

			while (n > 1)
			{
				n--;
				int k = rand.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}

			return list;
		}
	
		public static IEnumerable<T> Except<T>(this IEnumerable<T> items, T item)
		{
			return items.Except(new List<T>() { item });
		}

		public static T GetRandomItem<T>(this List<T> list)
		{
			return list.Any() ? list[rand.Next(0, list.Count)] : default(T);
		}
    }
}
