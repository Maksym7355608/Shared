namespace MaksiKo.Shared.Application.Extensions;

public static class MoreLinqExtensions
{
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var element in enumerable)
            action(element);
    }

    public static void ParallelForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        enumerable.AsParallel().ForEach(action);
    }

    public static IEnumerable<IEnumerable<T>> ToChunks<T>(this IEnumerable<T> enumerable, int chunkSize)
    {
        if (chunkSize <= 0)
        {
            throw new ArgumentException("Chunk size must be greater than zero", nameof(chunkSize));
        }

        var chunk = new List<T>(chunkSize);
        foreach (var item in enumerable)
        {
            chunk.Add(item);
            if (chunk.Count != chunkSize) continue;
            yield return chunk;
            chunk = new List<T>(chunkSize);
        }

        if (chunk.Count != 0)
            yield return chunk;
    }
}