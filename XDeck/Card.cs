class Card
{
    public Rank Rank;
    public Suit Suit;

    public int GetValue()
    {
        string card = rank switch
{
    10 => "Kneckt",
    10 => "Drottning",
    10 => "Kung",
    _ => "vardag"     
};
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }
}
