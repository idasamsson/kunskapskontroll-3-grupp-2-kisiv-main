// Ritar kort som textrader. Klassen skriver aldrig ut något själv (ingen Console):
// den returnerar raderna som string[] och ConsoleUI skriver ut dem och sköter färgerna.
class CardRenderer
{
    // Kortets mått: ramen är 9 tecken bred och 7 rader hög.
    public const int InnerWidth = 7;
    public const int Width = InnerWidth + 2;
    public const int Height = 7;

    public string[] Render(Card card)
    {
        string rank = RankText(card.Rank);
        string suit = SuitSymbol(card.Suit);
        string face = FaceGlyph(card.Rank); // tom sträng för kort med siffra

        return new[]
        {
            "┌" + new string('─', InnerWidth) + "┐",
            "│" + rank.PadRight(InnerWidth) + "│",
            "│" + Center(face) + "│",
            "│" + Center(suit) + "│",
            "│" + new string(' ', InnerWidth) + "│",
            "│" + rank.PadLeft(InnerWidth) + "│",
            "└" + new string('─', InnerWidth) + "┘"
        };
    }

    // Baksidan har samma mått som framsidan, så att den kan stå bredvid andra kort.
    public string[] RenderBack()
    {
        var rows = new string[Height];
        rows[0] = "┌" + new string('─', InnerWidth) + "┐";

        for (int i = 1; i < Height - 1; i++)
        {
            rows[i] = "│" + new string('▒', InnerWidth) + "│";
        }

        rows[Height - 1] = "└" + new string('─', InnerWidth) + "┘";
        return rows;
    }

    // Lägger texten i mitten av kortet och fyller ut med mellanslag till full bredd.
    private static string Center(string text)
    {
        if (text.Length == 0)
        {
            return new string(' ', InnerWidth);
        }

        int left = (InnerWidth - text.Length) / 2;
        int right = InnerWidth - text.Length - left;
        return new string(' ', left) + text + new string(' ', right);
    }

    private static string RankText(Rank rank)
    {
        return rank switch
        {
            Rank.Ace => "A",
            Rank.Two => "2",
            Rank.Three => "3",
            Rank.Four => "4",
            Rank.Five => "5",
            Rank.Six => "6",
            Rank.Seven => "7",
            Rank.Eight => "8",
            Rank.Nine => "9",
            Rank.Ten => "10",
            Rank.Jack => "J",
            Rank.Queen => "Q",
            Rank.King => "K",
            _ => throw new ArgumentOutOfRangeException(nameof(rank), rank, null)
        };
    }
}