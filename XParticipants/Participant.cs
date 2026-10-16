abstract class Participant
{
    public string Name;
    public Hand Hand = new Hand();

    public abstract Move MakeMove();

}
