abstract class Participant
{
    public string Name;
    public Hand Hand;

    public abstract Move MakeMove();

    public bool IsBust()
    {
        throw new NotImplementedException();
    }
}
