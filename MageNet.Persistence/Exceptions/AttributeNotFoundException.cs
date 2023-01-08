namespace MageNet.Persistence.Exceptions;

public class AttributeNotFoundException : ApplicationException
{
    public override string Message { get; } = "The requested attribute cannot be found. " +
                                              "Please recheck the provided identifier";
}