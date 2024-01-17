using System.Runtime.Serialization;

namespace SolveChess.Logic.Exceptions;

[Serializable]
public class UsernameGenerationException : SolveChessBaseException
{

    public UsernameGenerationException()
    {
    }

    public UsernameGenerationException(string? message) : base(message)
    {
    }

    public UsernameGenerationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected UsernameGenerationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

}