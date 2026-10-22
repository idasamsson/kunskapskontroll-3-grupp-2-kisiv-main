# KISIV Blackjack

## Vad programmet gör och hur man startar det
Ett tjugoett i konsolen för en till fem spelare mot en givare. Spelarnas resultat sparas i `players.txt` och finns kvar nästa gång programmet startas.

Starta med `dotnet run` i projektmappen.

## Klasser och hur de hänger ihop
*(fylls i gemensamt)*

## Vem som gjort vad

### Kim
- **Grund:** `Game` (meny, spelare och spelloop), `Round` (en giv: utdelning, köpregel, givarens tur och avgörande), `GameStorage` (sparar och läser `players.txt`) och `Outcome`.
- **Tillval 3, husregler från konfigurationsfil:** `RuleSet` och `RuleSetLoader` i `XRules/`.

### Ida
*(fylls i av Ida)*

### Sandra
*(fylls i av Sandra)*

### Veronica
*(fylls i av Veronica)*

### Ibuzor
*(fylls i av Ibuzor)*

## Regler
Spelet följer reglerna i uppgiften, med följande tillägg:

- Spelarnamn får inte vara tomma, längre än 20 tecken eller innehålla semikolon (;), eftersom sparfilen använder semikolon som avgränsare.

### Husregler (`rules.txt`)
Reglerna kan ändras i `rules.txt` utan att programmet byggs om. Filen läses när programmet startar.

| Inställning | Standard | Betyder |
|---|---|---|
| `DealerHitsSoft17` | `false` | Köper givaren på mjuk 17? |
| `NumberOfDecks` | `1` | Antal kortlekar |
| `ReshuffleThreshold` | `15` | Ny lek när färre än så här många kort är kvar |
| `NaturalBeatsThreeCard21` | `true` | Vinner naturlig 21 över en 21 av tre kort? Om `false` blir det lika |

Saknas filen används standardreglerna, och programmet säger till. En felstavad regel, en trasig rad eller ett ogiltigt värde (till exempel `NumberOfDecks=0`) ger en varning, och standardvärdet används för just den regeln. `ReshuffleThreshold` måste vara minst 15, så att korten inte tar slut mitt i en runda.

## Kända brister
- Med en lek och `ReshuffleThreshold` på 52 eller mer blandas leken om varje runda.
- Trasiga rader i `players.txt` hoppas över vid inläsning och försvinner när filen sparas igen.
- `Round.Settle()` räknar upp statistiken varje gång den anropas. Anropas den två gånger för samma spelare räknas resultatet dubbelt.
Räknaren kan nollställas mitt i en runda i nödfall, och räkningen uppdateras bara mellan rundorna.

## Klassdiagram

```mermaid
classDiagram
    direction TB

    class Game {
        -Deck deck
        -Dealer dealer
        -List~Player~ players
        -ConsoleUI ui
        -GameStorage storage
        +Run() void
        -NewGame() void
        -AddPlayer(string name) void
        -GameLoop() void
        -PlayRound() void
        -DetermineOutcome(Player p) Outcome
    }

    class ConsoleUI {
        +ShowStartMenu() int
        +ShowRules() void
        +ShowResults(List~Player~ players) void
        +AskPlayerName() string
        +AskMove() Move
        +ShowHand(Participant p) void
        +ShowOutcome(Player p, Outcome o) void
    }

    class GameStorage {
        -string filePath
        +SavePlayers(List~Player~ players) void
        +LoadPlayers() List~Player~
    }

    class Participant {
        <<abstract>>
        +string Name
        #Hand Hand
        +MakeMove()* Move
        +IsBust() bool
    }

    class Player {
        +int Wins
        +int Losses
        +int Pushes
        +MakeMove() Move
    }

    class Dealer {
        +MakeMove() Move
    }

    class Hand {
        -List~Card~ cards
        +AddCard(Card c) void
        +GetValue() int
        +IsBlackjack() bool
        +Clear() void
    }

    class Deck {
        -List~Card~ cards
        +Shuffle() void
        +Draw() Card
        +Reset() void
        +Count() int
    }

    class Card {
        +Rank Rank
        +Suit Suit
        +GetValue() int
        +ToString() string
    }

    class Outcome {
        <<enumeration>>
        Win
        Lose
        Push
        Blackjack
    }

    class Move {
        <<enumeration>>
        Hit
        Stand
    }

    class Rank {
        <<enumeration>>
        Two … Ten
        Jack
        Queen
        King
        Ace
    }

    class Suit {
        <<enumeration>>
        Hearts
        Diamonds
        Clubs
        Spades
    }

    %% Relationer
    Game --> ConsoleUI : använder
    Game --> GameStorage : sparar/laddar
    Game *-- "1..*" Participant
    Game *-- "1" Deck
    Game ..> Outcome : «use»

    Participant <|-- Player
    Participant <|-- Dealer
    Participant *-- "1" Hand

    Player ..> Move : «use»
    Dealer ..> Move : «use»

    Hand o-- "0..*" Card
    Deck *-- "52" Card
    Card --> Rank
    Card --> Suit

    %% Kommentarer
    note for Dealer "MakeMove(): köper på 16 eller lägre, stannar på 17"
    note for Move "Hit = Köpa<br>Stand = Stanna"
    note for Card "Rank = valör, Suit = färg"
```
