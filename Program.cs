using System.Text;
using XConsoleUI;

// Manuell testbädd: kör de riktiga klasserna och visar faktiska värden,
// så att man ser vad funktionerna returnerar och inte bara OK/FEL.
// De automatiska testerna ligger i Test.cs (val 1 i menyn).
// Metoder som inte är byggda än visas som "EJ KLAR" istället för att krascha.

// Behövs för att ♥ ♦ ♣ ♠ ska visas rätt i terminalen
Console.OutputEncoding = Encoding.UTF8;

// En enda Random för hela programmet (krav i uppgiften)
Random random = new Random();



while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== Testmeny ===");
    Console.WriteLine("1. Kör automatiska tester (OK/FEL)");
    Console.WriteLine("2. Dra kort från leken");
    Console.WriteLine("3. Bygg en egen hand");
    Console.WriteLine("4. Spela en testhand mot dealern");
    Console.WriteLine("5. Visa hela leken (blandad)");
    Console.WriteLine("6. Testa spara/ladda spelare");
    Console.WriteLine("7. Starta riktiga spelet (Game.Run)");
    Console.WriteLine("8. Avsluta");
    int choice = ConsoleInput.ReadInt("Välj (1-8): ", 1, 8);
    Console.WriteLine();

    switch (choice)
    {
        case 1:
            Tests.RunAll();
            break;
        case 2:
            DrawCards();
            break;
        case 3:
            BuildHand();
            break;
        case 4:
            PlayTestRound();
            break;
        case 5:
            ShowDeck();
            break;
        case 6:
            TestStorage();
            break;
        case 7:
            StartGame();
            break;
        case 8:
            return;
    }
}

// Startar det riktiga spelet, samma kod som ska ligga kvar när testmenyn tas bort
void StartGame()
{
    ConsoleUI ui = new ConsoleUI();
    GameStorage storage = new GameStorage("players.txt");
    Game game = new Game(random, ui, storage);

    game.Run();
}

// Anropar en funktion och returnerar resultatet som text, eller "EJ KLAR"
string Try<T>(Func<T> function)
{
    try
    {
        return function()?.ToString() ?? "null";
    }
    catch (NotImplementedException)
    {
        return "EJ KLAR";
    }
    catch (Exception ex)
    {
        return $"FEL: {ex.GetType().Name} - {ex.Message}";
    }
}

void ShowHand(string label, Hand hand)
{
    Console.WriteLine($"{label}: {string.Join(", ", hand.Cards)}");
    Console.WriteLine($"  GetHandValue() = {hand.GetHandValue()}");
    Console.WriteLine($"  IsBlackjack()  = {Try(() => hand.IsBlackjack())}");
}

void DrawCards()
{
    Deck deck = new Deck(random);
    Hand hand = new Hand();
    int count = ConsoleInput.ReadInt("Hur många kort vill du dra (1-52)? ", 1, 52);

    for (int i = 0; i < count; i++)
    {
        Card card = deck.Draw();
        hand.AddCard(card);
        Console.WriteLine($"  Drog {card,-12} GetValue() = {card.GetValue(),2}   handens värde = {hand.GetHandValue()}");
    }

    Console.WriteLine();
    ShowHand("Handen", hand);
    Console.WriteLine($"Kort kvar i leken: deck.Count() = {deck.Count()}");
}

void BuildHand()
{
    Console.WriteLine("Skriv kort separerade med mellanslag, t.ex: ess kung 5");
    Console.WriteLine("Giltiga: 2-10, knekt (kn), dam (d), kung (k), ess (e)");
    string input = ConsoleInput.ReadLine("Kort: ");

    Hand hand = new Hand();
    foreach (string word in input.Split(' ', StringSplitOptions.RemoveEmptyEntries))
    {
        Rank? rank = ParseRank(word.ToLower());
        if (rank == null)
        {
            Console.WriteLine($"  Okänt kort '{word}', hoppas över");
            continue;
        }
        hand.AddCard(new Card(rank.Value, Suit.Hearts));
    }

    Console.WriteLine();
    ShowHand("Handen", hand);

    Player player = new Player { Name = "Test", Hand = hand };
    Console.WriteLine($"  IsBust()       = {Try(() => player.IsBust())}");

    Dealer dealer = new Dealer { Name = "Dealer", Hand = hand };
    Console.WriteLine($"  Dealer.MakeMove() med denna hand = {Try(() => dealer.MakeMove())}");
}

Rank? ParseRank(string word)
{
    switch (word)
    {
        case "e":
        case "ess":
            return Rank.Ace;
        case "k":
        case "kung":
            return Rank.King;
        case "d":
        case "dam":
            return Rank.Queen;
        case "kn":
        case "knekt":
            return Rank.Jack;
    }

    if (int.TryParse(word, out int number) && number >= 2 && number <= 10)
    {
        return (Rank)number;
    }
    return null;
}

void PlayTestRound()
{
    Deck deck = new Deck(random);
    ConsoleUI ui = new ConsoleUI();
    Player player = new Player { Name = "Testspelare", Hand = new Hand() };
    Dealer dealer = new Dealer { Name = "Dealer", Hand = new Hand() };

    player.Hand.AddCard(deck.Draw());
    dealer.Hand.AddCard(deck.Draw());
    player.Hand.AddCard(deck.Draw());
    dealer.Hand.AddCard(deck.Draw());

    Console.WriteLine($"Dealerns synliga kort: {dealer.Hand.Cards[0]}");
    ShowHand("Din hand", player.Hand);

    // Spelarens tur
    while (player.Hand.GetHandValue() < 21 && ui.AskMove() == Move.Hit)
    {
        Card card = deck.Draw();
        player.Hand.AddCard(card);
        Console.WriteLine($"Du fick {card}");
        ShowHand("Din hand", player.Hand);
    }
    Console.WriteLine($"  IsBust()       = {Try(() => player.IsBust())}");

    int playerValue = player.Hand.GetHandValue();
    if (playerValue > 21)
    {
        Console.WriteLine("Du blev tjock, dealern vinner.");
        return;
    }

    // Dealerns tur
    Console.WriteLine();
    ShowHand("Dealerns hand", dealer.Hand);
    while (true)
    {
        Move move;
        try
        {
            move = dealer.MakeMove();
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("  Dealer.MakeMove() EJ KLAR, använder regeln: köp på 16, stanna på 17");
            move = dealer.Hand.GetHandValue() <= 16 ? Move.Hit : Move.Stand;
        }

        Console.WriteLine($"  Dealern väljer: {move}");
        if (move == Move.Stand)
        {
            break;
        }

        Card card = deck.Draw();
        dealer.Hand.AddCard(card);
        Console.WriteLine($"Dealern fick {card}");
        ShowHand("Dealerns hand", dealer.Hand);
    }

    int dealerValue = dealer.Hand.GetHandValue();
    Console.WriteLine();
    Console.WriteLine($"Du: {playerValue}   Dealer: {dealerValue}");
    if (dealerValue > 21 || playerValue > dealerValue)
    {
        Console.WriteLine("Du vinner!");
    }
    else if (playerValue < dealerValue)
    {
        Console.WriteLine("Dealern vinner.");
    }
    else
    {
        Console.WriteLine("Oavgjort (push).");
    }
}

void ShowDeck()
{
    Deck deck = new Deck(random);
    Console.WriteLine($"deck.Count() = {deck.Count()}");

    for (int i = 1; deck.Count() > 0; i++)
    {
        Console.Write($"{deck.Draw(),-14}");
        if (i % 4 == 0)
        {
            Console.WriteLine();
        }
    }
    Console.WriteLine($"deck.Count() efter att alla dragits = {deck.Count()}");
}

void TestStorage()
{
    string path = Path.Combine(Path.GetTempPath(), "blackjack_manual_players.txt");
    GameStorage storage = new GameStorage(path);

    List<Player> players = storage.LoadPlayers();
    Console.WriteLine($"Laddade {players.Count} spelare från {path}");

    string name = ConsoleInput.ReadLine("Namn på ny spelare: ");
    int wins = ConsoleInput.ReadInt("Vinster: ", 0, 1000);
    int losses = ConsoleInput.ReadInt("Förluster: ", 0, 1000);
    int pushes = ConsoleInput.ReadInt("Oavgjorda: ", 0, 1000);
    players.Add(new Player { Name = name, Wins = wins, Losses = losses, Pushes = pushes });

    storage.SavePlayers(players);

    Console.WriteLine();
    Console.WriteLine("Filens innehåll efter SavePlayers:");
    foreach (string line in File.ReadAllLines(path))
    {
        Console.WriteLine($"  {line}");
    }

    Console.WriteLine();
    Console.WriteLine("Spelare efter LoadPlayers:");
    foreach (Player p in storage.LoadPlayers())
    {
        Console.WriteLine($"  {p.Name}: {p.Wins} vinster, {p.Losses} förluster, {p.Pushes} oavgjorda");
    }
}
