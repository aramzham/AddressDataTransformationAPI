namespace ADT.Api.AddressDataTransformer;

public class StreetDesignationsTransformer : IAddressDataTransformer
{
    private static readonly Dictionary<string, string> _streetAbbreviations = new()
    {
        { "STREET", "ST" },
        { "AVENUE", "AVE" },
        { "ROAD", "RD" },
        { "BOULEVARD", "BLVD" },
        { "LANE", "LN" },
    };

    private static readonly Dictionary<string, string> _directionAbbreviations = new()
    {
        { "SOUTH", "S" },
        { "North", "N" },
        { "NO", "N" },
        { "WEST", "W" },
        { "EAST", "E" },
    };

    public string Transform(string input)
    {
        var splitBySpace = input.Split();

        for (int i = 0; i < splitBySpace.Length; i++)
        {
            if (IsDirection(splitBySpace[i]))
            {
                splitBySpace[i] = _directionAbbreviations[splitBySpace[i]];
                if (NextIsNumber(splitBySpace, i) && SecondNextIsStreet(splitBySpace, i))
                    splitBySpace[i] = TransformNumber(splitBySpace[i]);
            }
            else if (IsStreet(splitBySpace[i]))
                splitBySpace[i] = _streetAbbreviations[splitBySpace[i]];
        }

        return string.Join(' ', splitBySpace);
    }

    private string TransformNumber(string s)
    {
        return s[^1] switch
        {
            '1' => s == "11" ? "11TH" : $"{s}ST",
            '2' => s == "12" ? "12TH" : $"{s}ND",
            '3' => s == "13" ? "13TH" : $"{s}RD",
            _ => $"{s}TH"
        };
    }

    private bool IsStreet(string s) => _streetAbbreviations.ContainsKey(s.ToUpperInvariant());
    
    private bool IsDirection(string s) => _directionAbbreviations.ContainsKey(s.ToUpperInvariant());

    private bool NextIsNumber(string[] arr, int index) => index + 1 < arr.Length && arr[index + 1].All(char.IsNumber);

    private bool SecondNextIsStreet(string[] arr, int index) => index + 2 < arr.Length && IsStreet(arr[index + 2]);
}