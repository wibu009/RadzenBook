namespace RadzenBook.Common.Exceptions;

public class ServiceException : Exception
{
    public ServiceException(string message) : base(message)
    {
    }

    public ServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static ServiceException Create(string methodName, string className, string message, Exception innerException)
    {
        return new ServiceException($"Error in {className}.{methodName}: {message}", innerException);
    }
}