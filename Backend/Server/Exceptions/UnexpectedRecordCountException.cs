namespace Server.Exceptions;

[Serializable]
public class UnexpectedRecordCountException : Exception
{
    public UnexpectedRecordCountException() { }
    public UnexpectedRecordCountException(string message) : base(message) { }
    public UnexpectedRecordCountException(string message, Exception inner) : base(message, inner) { }
    protected UnexpectedRecordCountException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
