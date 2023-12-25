namespace Helpers
{
    public static class DictionaryExtensions
    {
        public static void AddOrIncrement<TKey>(this Dictionary<TKey, int> dictionary, TKey key, int value)
            where TKey : notnull
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] += value;
            else
                dictionary[key] = value;
        }
    }
}