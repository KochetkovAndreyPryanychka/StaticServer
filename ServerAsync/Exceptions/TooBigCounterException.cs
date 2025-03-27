namespace ServerAsync.Exceptions;

public class TooBigCounterException : Exception
{
    public TooBigCounterException(string message) : base(message) {}
}