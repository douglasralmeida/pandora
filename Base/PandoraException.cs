using System;
using System.Runtime.Serialization;

[Serializable()]
class PandoraException : Exception
{
    protected PandoraException() : base()
    {

    }

    public PandoraException(string mensagem) : base(mensagem)
    {

    }

    public PandoraException(string message, Exception innerException) : base(message, innerException)
    {
        
    }

    protected PandoraException(SerializationInfo info, StreamingContext context) : base(info, context)
    {

    }
}
