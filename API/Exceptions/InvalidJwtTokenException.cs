﻿using System.Runtime.Serialization;

namespace SolveChess.API.Exceptions;

[Serializable]
internal class InvalidJwtTokenException : Exception
{
    public InvalidJwtTokenException()
    {
    }

    public InvalidJwtTokenException(string? message) : base(message)
    {
    }

    public InvalidJwtTokenException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected InvalidJwtTokenException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}