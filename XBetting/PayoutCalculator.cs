using System.Linq.Expressions;

class PayoutCalculator
{
    public static int Calculate(Outcome outcome, int bet)
    {
        return outcome switch
        {
            Outcome.Lose => 0,
            Outcome.Push => bet,
            Outcome.Win => bet * 2,
            Outcome.Blackjack => bet + (bet * 3 + 1) / 2,
            _ => throw new ArgumentOutOfRangeException(nameof(outcome), outcome, "Utfallet är okänt. Lägg till en regel i PayoutCalculator."),
        };
    }
}