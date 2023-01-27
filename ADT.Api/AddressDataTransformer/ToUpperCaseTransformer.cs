namespace ADT.Api.AddressDataTransformer;

public class ToUpperCaseTransformer : IAddressDataTransformer
{
    public string Transform(string input)
    {
        return input.ToUpperInvariant();
    }
}