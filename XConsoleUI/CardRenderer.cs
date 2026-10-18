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
}