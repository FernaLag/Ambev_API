namespace Ambev.Ominia.Domain.Exceptions;

public class AggregateNotFoundException(string message) : Exception(message)
{
    }