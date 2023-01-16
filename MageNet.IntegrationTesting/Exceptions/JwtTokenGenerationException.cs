using System;

namespace MageNet.IntegrationTesting.Exceptions;

public class JwtTokenGenerationException : ApplicationException
{
    public JwtTokenGenerationException(string message) : base(message: message)
    {
        
    }
}