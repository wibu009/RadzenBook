namespace RadzenBook.Common.Exceptions;

public class ServiceException : Exception
{
    private ServiceException(string message) : base(message)
    {
    }

    private ServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static ServiceException Create(string methodName, string className, string message, Exception innerException) 
        => new ServiceException($"Error in {className}.{methodName}: {message}", innerException);
    
    public static ServiceException Create(string methodName, string className, string message)
        => new ServiceException($"Error in {className}.{methodName}: {message}");
}