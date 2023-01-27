namespace ADT.Api.AddressDataTransformer;

public interface IAddressDataTransformingStrategy
{
    IAddressDataTransformingStrategy AddType(Type transformerType);
    string Transform(string address);
}