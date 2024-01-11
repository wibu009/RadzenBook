namespace RadzenBook.Application.Common.Exceptions;

public class HandleRequestException : Exception
{
    private HandleRequestException(string message) : base(message)
    {
    }

    private HandleRequestException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static HandleRequestException Create(string methodName, string className, string message, Exception innerException) 
        => new HandleRequestException($"Exception in {className}.{methodName}: {message}", innerException);
    
    public static HandleRequestException Create(string methodName, string className, string message)
        => new HandleRequestException($"Exception in {className}.{methodName}: {message}");
}