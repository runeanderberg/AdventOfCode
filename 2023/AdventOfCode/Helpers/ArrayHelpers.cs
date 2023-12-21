namespace Helpers
{
    public static class ArrayHelpers
    {
        public static (int Row, int Col) IndexOf<T>(this T?[,] input, T? value)
        {
            for (var row = 0; row < input.GetLength(0); row++)
            {
                for (var col = 0; col < input.GetLength(1); col++)
                {
                    if (input[row, col]?.Equals(value) ?? false)
                        return (row, col);
                }
            }

            return (-1, -1);
        }

        public static T?[,] Transpose<T>(this T?[,] input)
        {
            var rowLength = input.GetLength(0);
            var colLength = input.GetLength(1);

            var result = new T?[colLength, rowLength];

            for (var row = 0; row < rowLength; row++)
            {
                for (var col = 0; col < colLength; col++)
                {
                    result[col, row] = input[row, col];
                }
            }

            return result;
        }
    }
}