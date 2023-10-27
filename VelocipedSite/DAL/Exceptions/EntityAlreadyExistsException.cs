using System.Runtime.Serialization;

namespace VelocipedSite.DAL.Exceptions;

public class EntityAlreadyExistsException : InfrastructureException
{
    public EntityAlreadyExistsException()
    {
    }

    protected EntityAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public EntityAlreadyExistsException(string message) : base(message)
    {
    }

    public EntityAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}