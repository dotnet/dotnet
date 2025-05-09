namespace Valleysoft.DockerCredsProvider;

internal static class ListHelper {
    public static List<T> Singleton<T>(T item) => new List<T>(1) { item };
}