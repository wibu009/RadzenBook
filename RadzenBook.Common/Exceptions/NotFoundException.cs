namespace RadzenBook.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException()
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public NotFoundException(string name, object key) : base($"Data \"{name}\" ({key}) was not found.")
    {
    }
}