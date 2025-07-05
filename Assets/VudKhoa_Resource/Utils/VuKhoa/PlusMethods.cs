namespace CustomUtils.Vukhoa
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class PlusMethods
    {
        public static void ShuffleList<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0) return;
            int n = list.Count;

            while (n > 0)
            {
                n--;
                int k = Random.Range(0, n + 1);

                T tmp = list[k];
                list[k] = list[n];
                list[n] = tmp;
            }
        }
    }
}