namespace FirstBlazorProject_BookStore.Common.Exceptions;

public class RepositoryException : Exception
{
    public RepositoryException()
    {
    }

    public RepositoryException(string message) : base(message)
    {
    }

    public RepositoryException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static RepositoryException Create(string methodName, string className, string message, Exception innerException)
    {
        return new RepositoryException($"Error in {className}.{methodName}: {message}", innerException);
    }
}