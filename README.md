## BlackJack

Gruppen lämnar in en README.md i repots rot som innehåller:

* Vad programmet gör och hur man startar det

* En kort översikt över klasserna och hur de hänger ihop

* Vem som gjort vad — vilket tillval var och en har byggt

* Vilka regler som gäller, om ni avvikit från specifikationen på någon punkt

* Kända brister

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
