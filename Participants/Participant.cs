abstract class Participant
{
    public string Name;
    protected Hand Hand;

    public abstract Move MakeMove();

    public bool IsBust()
    {
        throw new NotImplementedException();
    }
}
