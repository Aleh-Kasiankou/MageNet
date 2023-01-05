namespace MageNetServices.Exceptions;

public class AttributeAssemblyDataTypeMismatchException : NotSupportedException
{
    public override string Message { get; } = "Attribute assembly failed. Attribute can only be combined with data " +
                                              "of the same type: price attribute with price data, text attribute " +
                                              "with text data etc.";
}