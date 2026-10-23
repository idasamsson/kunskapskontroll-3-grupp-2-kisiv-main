// Rank = valör. Ändrar värdet för Knekt, dam o kung från 10 till egna värden 
// då C# inte klarar av att se skillnaden mellan korten om de har samma värde. 
//Enum-medlemmar med samma värde blir lika (Rank.Jack == Rank.Ten) och går inte att skilja åt.
public enum Rank 
{
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13,
    Ace = 14,
}
