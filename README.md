# KISIV Blackjack

## Vad programmet gör och hur man startar det
Ett tjugoett i konsolen för en till fem spelare mot en givare. Spelarnas resultat sparas i `players.txt` och finns kvar nästa gång programmet startas.

Starta med `dotnet run` i projektmappen.

## Klasser och hur de hänger ihop
*(fylls i gemensamt)*

## Vem som gjort vad

### Kim

- **Grund:** `Game` (meny, 1–5 spelare där samma namn inte kan sitta vid bordet två gånger, sparade spelare återanvänds, spelloop som spelar runda efter runda och sparar efter varje runda), `Round` (utbruten ur `Game`; en giv: utdelning, köpregel, givarens tur och avgörande, utan någon in- eller utmatning), `GameStorage` (sparar och läser `players.txt`; en saknad fil ger en tom lista och trasiga rader hoppas över) och `Outcome`.

- **Tillval 3, husregler från konfigurationsfil:** `RuleSet` (håller de fyra reglerna, med grundreglerna som standardvärden), `RuleSetLoader` (läser `rules.txt` rad för rad; en saknad fil, en felstavad regel eller ett ogiltigt värde ger en varning och standardvärdet används, så programmet kraschar aldrig på grund av filen), `RuleTable` (ett bord: namn och regler). Varje fil som heter `rules-*.txt` blir ett eget bord som man väljer vid nytt spel (`Game.ChooseTable`, `ConsoleUI.AskTable`), och bordets husregler visas direkt. Sju tester för `RuleSetLoader` i `Test.cs`.

- **Ändrat i gemensam kod för tillvalet:** `Dealer` (köper på mjuk 17 om regeln säger det), `Deck.ResetIfLow` (gränsen för omblandning skickas in i stället för det fasta talet 15), `Round` (regeln om naturlig 21 mot 21 av tre kort), `Game` (bygger lek eller sko och givare efter bordets regler i `ApplyRules`) och `Program.cs` (läser in borden när programmet startar).

### Ida

- **Grund:** `Hand` (håller varje spelares kort, inklusive givarens; räknar ut värdet, mjuk hand, spräckt hand och naturlig 21), `IMoveChooser` (gränssnitt som används vid val av drag, tar in spelarens hand och givarens öppna kort), `HumanMoveChooser` (skickar frågan om drag till den mänskliga spelaren), `Player` (håller spelarens namn, statistik och marker; frågar sin beslutsfattare när det är dags att välja drag).

- **Tillval 4, satsningar och marker:** `PayoutCalculator` (räknar ut utbetalningen utifrån utgången i spelet), `Player.PlaceBet` (drar insatsen från markerna, nekas om den är 0 eller överstiger spelarens marker), `Player.ReceivePayout` (ger spelaren utbetalningen efter rundan), `ConsoleUI.AskBet` (frågar den mänskliga spelaren hur mycket hen vill satsa), `Game.AskForBets` (frågar spelarna om deras insatser inför varje runda), `Game.RemoveBrokePlayers` (tar bort spelare utan marker innan nästa runda), `Round.Settle` (utökad från Kim; räknar ut utbetalningen med `PayoutCalculator` och ger den till spelaren), `GameStorage` (utökad från Kim; sparar och läser marker i `players.txt`).

### Sandra
*(fylls i av Sandra)*

### Veronica
*(fylls i av Veronica)*

### Ibuzor
*(fylls i av Ibuzor)*

## Regler
Spelet följer reglerna i uppgiften, med följande tillägg:

- Spelarnamn får inte vara tomma, längre än 20 tecken eller innehålla semikolon (;), eftersom sparfilen använder semikolon som avgränsare.
- Naturlig tjugoett betalar 3:2. Vinsten avrundas uppåt när insatsen är udda. En insats på 5 marker ger vinst 7,5, som blir 8.
- En sparad spelare med 0 marker tilldelas nya startmarker (200, `Player.StartingChips`) när hen sätter sig vid bordet.

### Husregler (`rules.txt`)

Reglerna kan ändras i `rules.txt` utan att programmet byggs om. Filen läses när programmet startar.

| Inställning | Standard | Betyder |
|---|---|---|
| `DealerHitsSoft17` | `false` | Köper givaren på mjuk 17? |
| `NumberOfDecks` | `1` | Antal kortlekar |
| `ReshuffleThreshold` | `15` | Ny lek när färre än så här många kort är kvar |
| `NaturalBeatsThreeCard21` | `true` | Vinner naturlig 21 över en 21 av tre kort? Om `false` blir det lika |

Saknas filen används standardreglerna, och programmet säger till. En felstavad regel, en trasig rad eller ett ogiltigt värde (till exempel `NumberOfDecks=0`) ger en varning, och standardvärdet används för just den regeln. `ReshuffleThreshold` måste vara minst 15, så att korten inte tar slut mitt i en runda.

#### Bord (regelpaket)
Varje fil som heter `rules-*.txt` blir ett eget bord. Man väljer bord när man startar ett nytt spel, och reglerna för bordet visas direkt. Ett nytt bord är bara en ny textfil, utan ändringar i koden.

| Bord | Fil | Regler |
|---|---|---|
| Standard | `rules.txt` | Grundreglerna |
| Tuff | `rules-tuff.txt` | 8 lekar, givaren köper på mjuk 17, 21 mot 21 blir lika, ny sko redan under 60 kort |
| Vegas | `rules-vegas.txt` | 6 lekar, givaren köper på mjuk 17 |

Vid Vegas avgör skiljekortet när skon blandas om, som på ett riktigt kasino, och `ReshuffleThreshold` är bara ett skydd. Vid Tuff blandas skon om redan under 60 kort. Då spelas en mindre del av skon, och korträkning ger mindre fördel.

## Kända brister

- Med en lek och `ReshuffleThreshold` på 52 eller mer blandas leken om varje runda.
- En runda med många spelare kan i sällsynta fall använda fler än 15 kort. Då byggs en ny lek mitt i rundan, och samma kort kan finnas två gånger på bordet.
- Trasiga rader i `players.txt` hoppas över vid inläsning och försvinner när filen sparas igen.
- `Round.Settle()` räknar upp statistiken varje gång den anropas. Anropas den två gånger för samma spelare räknas resultatet dubbelt.
- `Player.Chips` är ett publikt fält, så andra klasser kan ändra markerna direkt, till exempel `Game.AddPlayer` när en spelare får nya startmarker.

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
