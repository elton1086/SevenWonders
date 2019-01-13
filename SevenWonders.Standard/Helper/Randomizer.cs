using System;
using System.Collections.Generic;

namespace SevenWonders.Helper
{
    public static class Randomizer
    {
        /// <summary>
        /// Shuffles all the items of a list as many times as requested.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemList">the original unshuffled list.</param>
        /// <param name="timesToShuffle">The number of times it will shuffle the items of the list.</param>
        /// <returns></returns>
        public static void Shuffle<T>(IList<T> itemList, int timesToShuffle)
        {
            var random = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < timesToShuffle; i++)
            {
                int position = random.Next(itemList.Count);
                var item = itemList[position];
                itemList.Remove(item);
                itemList.Add(item);
            }
        }

        public static T SelectOne<T>(IList<T> itemList)
        {
            var random = new Random(DateTime.Now.Millisecond);
            return itemList[random.Next(itemList.Count)];
        }
    }
}
