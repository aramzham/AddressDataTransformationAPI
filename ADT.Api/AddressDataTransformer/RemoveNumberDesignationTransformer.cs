using System.Text;

namespace ADT.Api.AddressDataTransformer;

public class RemoveNumberDesignationTransformer : IAddressDataTransformer
{
    public string Transform(string input)
    {
        var sb = new StringBuilder(input);
        sb.Replace("#", string.Empty);
        sb.Replace("Number", string.Empty);

        return sb.ToString();
    }
}