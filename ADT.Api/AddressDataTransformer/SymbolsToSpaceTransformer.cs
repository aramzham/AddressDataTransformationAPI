using System.Text;

namespace ADT.Api.AddressDataTransformer;

public class SymbolsToSpaceTransformer : IAddressDataTransformer
{
    private static readonly char[] _symbolsToRemove = { '.', '-', ',' };

    public string Transform(string input)
    {
        var isPreviousCharacterSpace = false;
        var sb = new StringBuilder();

        foreach (var c in input)
        {
            if(isPreviousCharacterSpace && (Array.IndexOf(_symbolsToRemove, c) > -1 || char.IsWhiteSpace(c)))
                continue;
            
            if (char.IsWhiteSpace(c))
                isPreviousCharacterSpace = true;

            sb.Append(c);
        }

        return sb.ToString();
    }
}