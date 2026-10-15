abstract class Participant
{
    public string Name;
    public Hand Hand = new Hand();

    public abstract Move MakeMove();

    public bool IsBust()
    {
        throw new NotImplementedException();
    }
}
