﻿namespace RadzenBook.Application.Common.Exceptions;

public class RepositoryException : Exception
{
    public RepositoryException()
    {
    }

    public RepositoryException(string message) : base(message)
    {
    }

    private RepositoryException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static RepositoryException Create(string methodName, string className, string message, Exception innerException) 
        => new RepositoryException($"Exception in {className}.{methodName}: {message}", innerException);
}