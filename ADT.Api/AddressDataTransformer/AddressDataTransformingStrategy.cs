namespace ADT.Api.AddressDataTransformer;

public class AddressDataTransformingStrategy : IAddressDataTransformingStrategy
{
    private readonly List<IAddressDataTransformer> _transformers = new();

    public IAddressDataTransformingStrategy AddType(Type type)
    {
        _transformers.Add((IAddressDataTransformer)Activator.CreateInstance(type));

        return this;
    }

    public string Transform(string address)
    {
        return _transformers.Aggregate(address, (current, transformer) => transformer.Transform(current));
    }
}