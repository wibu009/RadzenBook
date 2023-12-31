﻿namespace RadzenBook.Application.Common.Exceptions;

public class ServiceException : Exception
{
    public ServiceException()
    {
    }

    public ServiceException(string message)
        : base(message)
    {
    }

    public ServiceException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
    
    public static ServiceException Create(string methodName, string className, string message, Exception innerException) 
        => new ServiceException($"Exception in {className}.{methodName}: {message}", innerException);
    
    public static ServiceException Create(string methodName, string className, string message) 
        => new ServiceException($"Exception in {className}.{methodName}: {message}");
}