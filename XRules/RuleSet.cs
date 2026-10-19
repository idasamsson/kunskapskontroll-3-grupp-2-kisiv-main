class RuleSet
{
     // Ska givaren köpa på mjuk 17? Grundreglerna säger nej, givaren stannar
    public bool DealerHitsSoft17 { get; set; } = false;
    // Hur många kortlekar som blandas ihop
    public int NumberOfDecks { get; set; } = 1;
    // Ny lek när färre än så här många kort är kvar
    public int ReshuffleThreshold { get; set; } = 15;
    // Vinner naturlig 21 över en 21 av tre kort?
    public bool NaturalBeatsThreeCard21 { get; set; } = true;
}
