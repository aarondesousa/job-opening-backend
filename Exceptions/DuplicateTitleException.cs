namespace JobOpeningBackend.Exceptions;

public class DuplicateTitleException : Exception
{
    public DuplicateTitleException() { }

    public DuplicateTitleException(string message)
        : base(message) { }

    public DuplicateTitleException(string message, Exception innerException)
        : base(message, innerException) { }
}
