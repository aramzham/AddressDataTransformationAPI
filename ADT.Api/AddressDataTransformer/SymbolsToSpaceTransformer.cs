using System.Text;

namespace ADT.Api.AddressDataTransformer;

public class SymbolsToSpaceTransformer : IAddressDataTransformer
{
    private static readonly char[] _symbolsToRemove = { '.', '-', ',', '#' };

    public string Transform(string input)
    {
        var isPreviousCharacterSpace = false;
        var sb = new StringBuilder();

        foreach (var c in input)
        {
            if ((isPreviousCharacterSpace && char.IsWhiteSpace(c)) || Array.IndexOf(_symbolsToRemove, c) > -1)
                continue;

            isPreviousCharacterSpace = char.IsWhiteSpace(c);

            sb.Append(c);
        }

        return sb.ToString();
    }
}
