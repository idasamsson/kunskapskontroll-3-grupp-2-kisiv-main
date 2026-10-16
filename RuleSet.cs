class RuleSet
{
    public bool DealerHitsSoft17 { get; set; } = false;
    // Ska givaren köpa på mjuk 17? Grundreglerna säger nej, givaren stannar
    public int NumberOfDecks { get; set; } = 1;
    // Hur många kortlekar som blandas ihop
    public int ReshuffleThreshold { get; set; } = 15;
    // Ny lek när färre än så här många kort är kvar
    public bool NaturalBeatsThreeCard21 { get; set; } = true;
    // Vinner naturlig 21 över en 21 av tre kort?
}
