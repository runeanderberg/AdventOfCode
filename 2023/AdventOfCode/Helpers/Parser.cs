namespace Helpers
{
    public static class Parser
    {
        public static T[,] To2DArray<T>(this IEnumerable<string> input, Func<char, T> typeConverter)
        {
            var inputArray = input.ToArray();
            var result = new T[inputArray.Length, inputArray[0].Length];

            for (var row = 0; row < inputArray.Length; row++)
            {
                for (var col = 0; col < inputArray[row].Length; col++)
                {
                    result[row, col] = typeConverter.Invoke(inputArray[row][col]);
                }
            }

            return result;
        }
    }
}