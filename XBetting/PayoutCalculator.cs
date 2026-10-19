static class PayoutCalculator
{
    public static int Calculate(Outcome outcome, int bet)
    {
        if (bet <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(bet), bet, "Insatsen får inte vara 0 eller negativt.");
        }
        
        return outcome switch
        {
            Outcome.Lose => 0,
            Outcome.Push => bet,
            Outcome.Win => bet * 2,
            // Naturlig 21 betalar 3:2. Vinsten är 1,5 gånger insatsen avrundat uppåt:
            // + 1 före divisionen gör att heltalsdivisionen avrundar uppåt vid udda insats.
            // Insatsen kommer tillbaka, därför bet + vinsten.
            Outcome.Blackjack => bet + (bet * 3 + 1) / 2,
            _ => throw new ArgumentOutOfRangeException(nameof(outcome), outcome, "Utfallet är okänt. Lägg till en regel i PayoutCalculator."),
        };
    }
}