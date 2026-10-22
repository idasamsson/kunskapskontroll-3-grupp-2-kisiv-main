// Datorstrategi som följer grundstrategin för tjugoett.
// Draget slås upp i en tabell.
// Handens värde, hård eller mjuk hand, givarens öppna kort.
class BasicStrategy : IMoveChooser
{
    // Varje rad är ett handvärde. Varje tecken i strängen är en kolumn.
    // Givarens öppna kort i ordningen:
    //     2 3 4 5 6 7 8 9 10 E
    // H = köp (Hit) S = stanna (Stand)

    // Hårda händer (inget ess räknas som 11)
    private static readonly Dictionary<int, string> HardTable = new Dictionary<int,string>
    {
        { 12, "HHSSSHHHHH" },
        { 13, "SSSSSHHHHH" },
        { 14, "SSSSSHHHHH" },
        { 15, "SSSSSHHHHH" },
        { 16, "SSSSSHHHHH" },
    };

    // Mjuka händer (ess räknas som 11)
    private static readonly Dictionary<int, string> SoftTable = new Dictionary<int, string>
    {
        { 18, "SSSSSSSHHH" },
    };

    public Move ChooseMove(Hand myHand, Card dealerCard)
    {
        int handValue = myHand.GetHandValue();
        bool isSoft = myHand.IsSoft();

        // 1. Välj tabell: mjuk eller hård.
        Dictionary<int, string> table = isSoft ? SoftTable : HardTable;

        // 2. Hitta raden för handens värde.
        if (table.TryGetValue(handValue, out string? row))
        {
            // 3. Hitta kolumnen för givarens kort.
            // Kort 2 hamnar i kolumn 0, kort 3 i kolumn 1... tiokort i kolumn 8.
            // Esset har värdet 11 och hamnar i kolumn 9.
            int column = dealerCard.GetValue() -2;

            return row[column] == 'S' ? Move.Stand : Move.Hit;
        }
        // Värden som inte finns i tabellen följer samma regel, oavsett givarens kort
        // Hård hand stannar på 17 och uppåt, mjuk hand på 19 och uppåt, annars köp.
        int standFrom = isSoft ? 19 : 17;
        return handValue >= standFrom ? Move.Stand : Move.Hit;
    }
}