// Testbädd: kör enkla tester av klasserna innan Game är klart.
// Varje test skriver ut OK eller FEL. Metoder som inte är byggda än visas som "EJ KLAR".

static class Tests
{
    public static void RunAll()
    {
        int passed = 0;
        int failed = 0;
        int notDone = 0;

        void Check(string name, bool condition)
        {
            if (condition)
            {
                passed++;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  OK      {name}");
            }
            else
            {
                failed++;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  FEL     {name}");
            }
            Console.ResetColor();
        }

        // Kör ett test och fångar NotImplementedException, så att testbädden fortsätter
        void Run(string name, Action test)
        {
            try
            {
                test();
            }
            catch (NotImplementedException)
            {
                notDone++;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  EJ KLAR {name}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                failed++;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  FEL     {name}: {ex.GetType().Name} - {ex.Message}");
                Console.ResetColor();
            }
        }

        Hand MakeHand(params Rank[] ranks)
        {
            Hand hand = new Hand();
            foreach (Rank rank in ranks)
            {
                hand.AddCard(new Card(rank, Suit.Hearts));
            }
            return hand;
        }

        // ---------- Card ----------
        Console.WriteLine("Card");
        Run("GetValue", () =>
        {
            Check("2 är värd 2", new Card(Rank.Two, Suit.Hearts).GetValue() == 2);
            Check("10 är värd 10", new Card(Rank.Ten, Suit.Hearts).GetValue() == 10);
            Check("Knekt är värd 10", new Card(Rank.Jack, Suit.Hearts).GetValue() == 10);
            Check("Dam är värd 10", new Card(Rank.Queen, Suit.Hearts).GetValue() == 10);
            Check("Kung är värd 10", new Card(Rank.King, Suit.Hearts).GetValue() == 10);
            Check("Ess är värt 11", new Card(Rank.Ace, Suit.Hearts).GetValue() == 11);
        });
        Run("ToString", () =>
        {
            Check("Spader Ess", new Card(Rank.Ace, Suit.Spades).ToString() == "Spader Ess");
            Check("Hjärter 7", new Card(Rank.Seven, Suit.Hearts).ToString() == "Hjärter 7");
            Check("Klöver Dam", new Card(Rank.Queen, Suit.Clubs).ToString() == "Klöver Dam");
        });

        // ---------- Deck ----------
        Console.WriteLine("\nDeck");
        Run("Ny lek", () =>
        {
            Deck deck = new Deck(new Random(1));
            Check("Ny lek har 52 kort", deck.Count() == 52);

            HashSet<string> unique = new HashSet<string>();
            for (int i = 0; i < 52; i++)
            {
                unique.Add(deck.Draw().ToString());
            }
            Check("Alla 52 kort är unika", unique.Count == 52);
        });
        Run("Draw", () =>
        {
            Deck deck = new Deck(new Random(1));
            deck.Draw();
            Check("Draw minskar leken med 1", deck.Count() == 51);

            for (int i = 0; i < 51; i++)
            {
                deck.Draw();
            }
            Check("Tom lek efter 52 dragningar", deck.Count() == 0);

            deck.Draw();
            Check("Draw på tom lek blandar ny lek (51 kvar)", deck.Count() == 51);
        });
        Run("Shuffle", () =>
        {
            // Samma seed ska ge samma ordning, olika seed (nästan alltid) olika ordning
            string Order(int seed)
            {
                Deck deck = new Deck(new Random(seed));
                List<string> cards = new List<string>();
                for (int i = 0; i < 52; i++)
                {
                    cards.Add(deck.Draw().ToString());
                }
                return string.Join(",", cards);
            }
            Check("Samma seed ger samma ordning", Order(42) == Order(42));
            Check("Olika seed ger olika ordning", Order(1) != Order(2));
        });
        Run("ResetIfLow", () =>
        {
            Deck deck = new Deck(new Random(1));
            for (int i = 0; i < 37; i++)
            {
                deck.Draw();
            }
            deck.ResetIfLow();
            Check("15 kort kvar: ingen ny lek", deck.Count() == 15);

            deck.Draw();
            deck.ResetIfLow();
            Check("14 kort kvar: ny lek med 52 kort", deck.Count() == 52);
        });

        // ---------- Hand ----------
        Console.WriteLine("\nHand");
        Run("GetHandValue", () =>
        {
            Check("Tom hand = 0", new Hand().GetHandValue() == 0);
            Check("7 + 9 = 16", MakeHand(Rank.Seven, Rank.Nine).GetHandValue() == 16);
            Check("Kung + Dam = 20", MakeHand(Rank.King, Rank.Queen).GetHandValue() == 20);
            Check("Ess + Kung = 21", MakeHand(Rank.Ace, Rank.King).GetHandValue() == 21);
            Check("Ess + Ess = 12", MakeHand(Rank.Ace, Rank.Ace).GetHandValue() == 12);
            Check("Ess + 6 + 9 = 16 (ess räknas som 1)", MakeHand(Rank.Ace, Rank.Six, Rank.Nine).GetHandValue() == 16);
            Check("Ess + Ess + 9 = 21", MakeHand(Rank.Ace, Rank.Ace, Rank.Nine).GetHandValue() == 21);
            Check("4 ess + 7 = 21", MakeHand(Rank.Ace, Rank.Ace, Rank.Ace, Rank.Ace, Rank.Seven).GetHandValue() == 21);
            Check("Kung + Dam + 5 = 25 (tjock)", MakeHand(Rank.King, Rank.Queen, Rank.Five).GetHandValue() == 25);
        });
        Run("AddCard / Clear", () =>
        {
            Hand hand = MakeHand(Rank.Two, Rank.Three);
            Check("AddCard lägger till kort", hand.Cards.Count == 2);
            hand.Clear();
            Check("Clear tömmer handen", hand.Cards.Count == 0 && hand.GetHandValue() == 0);
        });
        Run("IsBlackjack", () =>
        {
            Check("Ess + Kung är blackjack", MakeHand(Rank.Ace, Rank.King).IsBlackjack());
            Check("7 + 7 + 7 är inte blackjack", !MakeHand(Rank.Seven, Rank.Seven, Rank.Seven).IsBlackjack());
            Check("Kung + Dam är inte blackjack", !MakeHand(Rank.King, Rank.Queen).IsBlackjack());
        });

        // ---------- Participant / Dealer ----------
        Console.WriteLine("\nParticipant / Dealer");
        Run("IsBust", () =>
        {
            Player player = new Player { Name = "Test", Hand = MakeHand(Rank.King, Rank.Queen, Rank.Five) };
            Check("25 är tjock", player.Hand.IsBust());
            player.Hand = MakeHand(Rank.King, Rank.Ace);
            Check("21 är inte tjock", !player.Hand.IsBust());
        });
        Run("Dealer.MakeMove", () =>
        {
            Dealer dealer = new Dealer { Name = "Dealer", Hand = MakeHand(Rank.King, Rank.Six) };
            Check("Dealer köper på 16", dealer.MakeMove() == Move.Hit);
            dealer.Hand = MakeHand(Rank.King, Rank.Seven);
            Check("Dealer stannar på 17", dealer.MakeMove() == Move.Stand);
        });

        // ---------- GameStorage ----------
        Console.WriteLine("\nGameStorage");
        Run("Spara och ladda", () =>
        {
            string path = Path.Combine(Path.GetTempPath(), "blackjack_testbed_players.txt");
            File.Delete(path);
            GameStorage storage = new GameStorage(path);

            Check("Saknad fil ger tom lista", storage.LoadPlayers().Count == 0);

            List<Player> players = new List<Player>
            {
                new Player { Name = "Anna", Wins = 3, Losses = 1, Pushes = 2 },
                new Player { Name = "Bertil", Wins = 0, Losses = 5, Pushes = 0 },
            };
            storage.SavePlayers(players);
            List<Player> loaded = storage.LoadPlayers();

            Check("Två spelare laddas", loaded.Count == 2);
            Check("Namn och statistik stämmer",
                loaded.Count == 2 &&
                loaded[0].Name == "Anna" && loaded[0].Wins == 3 && loaded[0].Losses == 1 && loaded[0].Pushes == 2 &&
                loaded[1].Name == "Bertil" && loaded[1].Losses == 5);

            File.WriteAllLines(path, ["Anna;3;1;2", "trasig rad", ";1;1;1", "Cecilia;x;1;1"]);
            Check("Felaktiga rader hoppas över", storage.LoadPlayers().Count == 1);

            File.Delete(path);
        });

        // ---------- PayoutCalculator ----------
        Console.WriteLine("\nPayoutCalculator");
        Run("Calculate", () =>
        {
            Check("Insats 5 ska ge 0 vid förlust", PayoutCalculator.Calculate(Outcome.Lose, 5) == 0);
            Check("Insats 5 ska ge 5 vid lika", PayoutCalculator.Calculate(Outcome.Push, 5) == 5);
            Check("Insats 5 ska ge 10 vid vinst", PayoutCalculator.Calculate(Outcome.Win, 5) == 10);
            Check("Insats 5 ska ge 13 vid blackjack", PayoutCalculator.Calculate(Outcome.Blackjack, 5) == 13);
            Check("Insats 10 ska ge 0 vid förlust", PayoutCalculator.Calculate(Outcome.Lose, 10) == 0);
            Check("Insats 10 ska ge 10 vid lika", PayoutCalculator.Calculate(Outcome.Push, 10) == 10);
            Check("Insats 10 ska ge 20 vid vinst", PayoutCalculator.Calculate(Outcome.Win, 10) == 20);
            Check("Insats 10 ska ge 25 vid blackjack", PayoutCalculator.Calculate(Outcome.Blackjack, 10) == 25);
        });

        // ---------- Summering ----------
        Console.WriteLine($"\nResultat: {passed} OK, {failed} FEL, {notDone} EJ KLARA");
    }
}
