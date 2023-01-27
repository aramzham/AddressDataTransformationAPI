namespace ADT.Api.AddressDataTransformer;

public class StreetDesignationsTransformer : IAddressDataTransformer
{
    private static readonly Dictionary<string, string> _streetAbbreviations = new()
    {
        { "STREET", "ST" },
        { "AVENUE", "AVE" },
        { "AVE", "AVE" },
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
        var list = new List<string>();

        for (int i = 0; i < splitBySpace.Length; i++)
        {
            var upperCase = splitBySpace[i].ToUpperInvariant();

            if (IsDirection(upperCase))
                list.Add(_directionAbbreviations[upperCase]);
            else if (IsNumber(upperCase) && i != splitBySpace.Length - 1)
            {
                var nextUpper = splitBySpace[i + 1].ToUpperInvariant();
                if (IsStreet(nextUpper))
                {
                    list.Add(TransformNumber(splitBySpace[i]));
                    HandleStreet(i + 1 == splitBySpace.Length - 1, nextUpper, list, splitBySpace);
                    break;
                }

                list.Add(upperCase);
            }
            else if (IsStreet(upperCase))
            {
                HandleStreet(i == splitBySpace.Length - 1, upperCase, list, splitBySpace);
                break;
            }
            else
                list.Add(upperCase);
        }

        return string.Join(' ', list);
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

    private bool IsStreet(string s) => _streetAbbreviations.ContainsKey(s);

    private bool IsDirection(string s) => _directionAbbreviations.ContainsKey(s);

    private bool IsNumber(string s) => s.All(char.IsNumber);

    private void HandleStreet(bool isEnd, string street, ICollection<string> list, IReadOnlyList<string> splitBySpace)
    {
        list.Add(_streetAbbreviations[street]);

        if (isEnd)
            return;

        list.Add(splitBySpace[^1].ToUpperInvariant());
    }
}
