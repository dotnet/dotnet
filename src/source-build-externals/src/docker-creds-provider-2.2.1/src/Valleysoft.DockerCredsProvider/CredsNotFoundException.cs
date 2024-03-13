namespace Valleysoft.DockerCredsProvider;

public class CredsNotFoundException : Exception
{
    public CredsNotFoundException()
    {
    }

    public CredsNotFoundException(string message)
        : base(message)
    {
    }

    public CredsNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
