namespace Wcwidth;

internal static class IntegerExtensions
{
    public static bool Exist(this int[,] table, int value)
    {
        return Find(table, value) != 0;
    }

    public static int Find(this int[,] table, int value)
    {
        if (table is null)
        {
            throw new ArgumentNullException(nameof(table));
        }

        var min = 0;
        var max = table.GetUpperBound(0);
        int mid;

        if (value < table[0, 0] || value > table[max, 1])
        {
            return 0;
        }

        while (max >= min)
        {
            mid = (min + max) / 2;
            if (value > table[mid, 1])
            {
                min = mid + 1;
            }
            else if (value < table[mid, 0])
            {
                max = mid - 1;
            }
            else
            {
                return 1;
            }
        }

        return 0;
    }
}