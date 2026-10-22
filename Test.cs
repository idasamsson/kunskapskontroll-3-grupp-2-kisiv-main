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
            deck.ResetIfLow(15);
            Check("15 kort kvar: ingen ny lek", deck.Count() == 15);

            deck.Draw();
            deck.ResetIfLow(15);
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

        // ---------- Participants / Dealer ----------
        Console.WriteLine("\nParticipants / Dealer");
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

        // ---------- Player: marker och insats ----------
        Console.WriteLine("\nPlayer: marker och insats");
        Run("Ny spelare", () =>
        {
            Player player = new Player { Name = "Anna" };
            Check("Ny spelare har marker", player.Chips > 0);
            Check("Ny spelare har ingen insats", player.Bet == 0);
        });
        Run("PlaceBet nekas", () =>
        {
            Player player = new Player { Name = "Anna", Chips = 100 };
            Check("Insats 0 nekas", !player.PlaceBet(0));
            Check("Negativ insats nekas", !player.PlaceBet(-5));
            Check("Insats större än markerna nekas", !player.PlaceBet(101));
            Check("Markerna är oförändrade efter nekade insatser", player.Chips == 100);
            Check("Insatsen är oförändrad efter nekade insatser", player.Bet == 0);
        });
        Run("PlaceBet godkänns", () =>
        {
            Player player = new Player { Name = "Anna", Chips = 100 };
            Check("Insats 30 godkänns", player.PlaceBet(30));
            Check("Insatsen är sparad", player.Bet == 30);
            Check("Markerna har minskat", player.Chips == 70);
        });
        Run("PlaceBet alla marker", () =>
        {
            Player player = new Player { Name = "Anna", Chips = 100 };
            Check("Alla marker kan satsas", player.PlaceBet(100));
            Check("Inga marker kvar", player.Chips == 0);
        });
        Run("ReceivePayout", () =>
        {
            Player player = new Player { Name = "Anna", Chips = 100 };
            player.PlaceBet(30);
            player.ReceivePayout(60);
            Check("Markerna ökar med utbetalningen", player.Chips == 130);
            Check("Insatsen nollställs efter utbetalning", player.Bet == 0);
        });
        Run("ReceivePayout vid förlust", () =>
        {
            Player player = new Player { Name = "Anna", Chips = 100 };
            player.PlaceBet(30);
            player.ReceivePayout(0);
            Check("Markerna är kvar på 70 efter förlust", player.Chips == 70);
            Check("Insatsen nollställs även vid förlust", player.Bet == 0);
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
                    new Player { Name = "Anna", Wins = 3, Losses = 1, Pushes = 2, Chips = 150 },
                    new Player { Name = "Bertil", Wins = 0, Losses = 5, Pushes = 0, Chips = 40 },
                };
            storage.SavePlayers(players);
            List<Player> loaded = storage.LoadPlayers();

            Check("Två spelare laddas", loaded.Count == 2);
            Check("Namn och statistik stämmer",
                loaded.Count == 2 &&
                loaded[0].Name == "Anna" && loaded[0].Wins == 3 && loaded[0].Losses == 1 && loaded[0].Pushes == 2 &&
                loaded[1].Name == "Bertil" && loaded[1].Losses == 5);
            Check("Marker sparas och laddas",
                loaded.Count == 2 &&
                loaded[0].Chips == 150 &&
                loaded[1].Chips == 40);

        // Gammalt filformat utan marker ska fortfarande gå att läsa
            File.WriteAllLines(path, ["Anna;3;1;2"]);
            List<Player> oldFormat = storage.LoadPlayers();
            Check("Gammal rad (4 fält) laddas", oldFormat.Count == 1);
            Check("Gammal rad får startmarker",
                oldFormat.Count == 1 && oldFormat[0].Chips == Player.StartingChips);

            // Trasiga rader ska hoppas över
            File.WriteAllLines(path, ["Anna;3;1;2;80", "trasig rad", ";1;1;1;50", "Cecilia;x;1;1", "Dan;1;1;1;abc", "Eva;1;1;1;50;7", "Bertil;2;1;0;-50", ]);
            List<Player> mixed = storage.LoadPlayers();
            Check("Felaktiga rader hoppas över", mixed.Count == 1);
            Check("Giltig ny rad behåller sina marker",
                mixed.Count == 1 && mixed[0].Chips == 80);

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

                // ---------- RuleSetLoader ----------
        Console.WriteLine("\nRuleSetLoader");
        Run("Läsa husregler", () =>
        {
            string path = Path.Combine(Path.GetTempPath(), "blackjack_testbed_rules.txt");

            File.Delete(path);
            RuleSetLoader missingLoader = new RuleSetLoader(path);
            RuleSet defaults = missingLoader.Load();
            Check("Saknad fil ger standardregler",
                !defaults.DealerHitsSoft17 && defaults.NumberOfDecks == 1 &&
                defaults.ReshuffleThreshold == 15 && defaults.NaturalBeatsThreeCard21);
            Check("Saknad fil ger en varning", missingLoader.Warnings.Count == 1);

            File.WriteAllLines(path, ["DealerHitsSoft17=True", "NumberOfDecks=6", "ReshuffleThreshold=20", "NaturalBeatsThreeCard21=false"]);
            RuleSetLoader validLoader = new RuleSetLoader(path);
            RuleSet rules = validLoader.Load();
            Check("Giltiga värden läses in",
                rules.DealerHitsSoft17 && rules.NumberOfDecks == 6 &&
                rules.ReshuffleThreshold == 20 && !rules.NaturalBeatsThreeCard21);
            Check("Giltig fil ger inga varningar", validLoader.Warnings.Count == 0);

            File.WriteAllLines(path, ["NumberOfDecks=abc", "ReshuffleThreshold=5", "Decks=2", "bara text"]);
            RuleSetLoader invalidLoader = new RuleSetLoader(path);
            RuleSet fallback = invalidLoader.Load();
            Check("Ogiltigt antal lekar ger standardvärdet", fallback.NumberOfDecks == 1);
            Check("Gräns under 15 ger standardvärdet", fallback.ReshuffleThreshold == 15);
            Check("Fyra fel ger fyra varningar", invalidLoader.Warnings.Count == 4);

            File.Delete(path);
        });

         // ---------- Datorstrategier ----------
        Console.WriteLine("\nDatorstrategier");

        Card DealerCard(Rank rank) => new Card(rank, Suit.Spades);

        Player Computer(string name, IMoveChooser strategy)
        {
            // Många marker så att insatsen aldrig tar slut under simuleringen
            Player player = new Player { Name = name, Type = PlayerType.Computer, Chips = 1_000_000 };
            player.ReceiveMoveChooser(strategy);
            return player;
        }

        Run("Gränsfall: spräckt hand", () =>
        {
            Hand bust = MakeHand(Rank.King, Rank.Queen, Rank.Five); // 25
            Card six = DealerCard(Rank.Six);
            Check("Enkel stannar på spräckt 25", new SimpleStrategy().ChooseMove(bust, six) == Move.Stand);
            Check("Grundstrategi stannar på spräckt 25", new BasicStrategy().ChooseMove(bust, six) == Move.Stand);
            Check("Försiktig stannar på spräckt 25", new CautiousStrategy().ChooseMove(bust, six) == Move.Stand);
            Check("Djärv stannar på spräckt 25", new BoldStrategy().ChooseMove(bust, six) == Move.Stand);
        });

        Run("Gränsfall: naturlig tjugoett", () =>
        {
            Hand natural = MakeHand(Rank.Ace, Rank.King);
            Card ace = DealerCard(Rank.Ace);
            Check("Ess + Kung är naturlig tjugoett", natural.IsBlackjack());
            Check("Enkel stannar på naturlig 21", new SimpleStrategy().ChooseMove(natural, ace) == Move.Stand);
            Check("Grundstrategi stannar på naturlig 21", new BasicStrategy().ChooseMove(natural, ace) == Move.Stand);
            Check("Djärv stannar på naturlig 21", new BoldStrategy().ChooseMove(natural, ace) == Move.Stand);
        });

        Run("Gränsfall: mjuk hand med flera ess", () =>
        {
            Hand aceAceSix = MakeHand(Rank.Ace, Rank.Ace, Rank.Six); // mjuk 18
            Check("Ess + Ess + 6 är mjuk 18", aceAceSix.GetHandValue() == 18 && aceAceSix.IsSoft());
            Check("Grundstrategi stannar på mjuk 18 mot 7", new BasicStrategy().ChooseMove(aceAceSix, DealerCard(Rank.Seven)) == Move.Stand);
            Check("Grundstrategi köper på mjuk 18 mot 9", new BasicStrategy().ChooseMove(aceAceSix, DealerCard(Rank.Nine)) == Move.Hit);
            Check("Enkel stannar på mjuk 18", new SimpleStrategy().ChooseMove(aceAceSix, DealerCard(Rank.Nine)) == Move.Stand);

            Hand aceAce = MakeHand(Rank.Ace, Rank.Ace); // mjuk 12
            Check("Ess + Ess är mjuk 12", aceAce.GetHandValue() == 12 && aceAce.IsSoft());
            Check("Grundstrategi köper på mjuk 12", new BasicStrategy().ChooseMove(aceAce, DealerCard(Rank.Six)) == Move.Hit);
        });

        Run("Grundstrategi läser tabellen", () =>
        {
            BasicStrategy basic = new BasicStrategy();
            Check("Hård 13 mot 6: stanna", basic.ChooseMove(MakeHand(Rank.King, Rank.Three), DealerCard(Rank.Six)) == Move.Stand);
            Check("Hård 13 mot 7: köp", basic.ChooseMove(MakeHand(Rank.King, Rank.Three), DealerCard(Rank.Seven)) == Move.Hit);
            Check("Hård 12 mot 3: köp", basic.ChooseMove(MakeHand(Rank.King, Rank.Two), DealerCard(Rank.Three)) == Move.Hit);
            Check("Hård 12 mot 4: stanna", basic.ChooseMove(MakeHand(Rank.King, Rank.Two), DealerCard(Rank.Four)) == Move.Stand);
            Check("Hård 16 mot ess: köp", basic.ChooseMove(MakeHand(Rank.King, Rank.Six), DealerCard(Rank.Ace)) == Move.Hit);
        });

        Run("Simulering 1000 rundor", () =>
        {
            const int Rounds = 1000;
            RuleSet rules = new RuleSet();
            // Fast frö (2026) ger samma resultat varje körning, så siffrorna i rapporten går att återskapa
            ICardSource deck = new Deck(new Random(2026));
            Dealer dealer = new Dealer(rules.DealerHitsSoft17);

            List<Player> players = new List<Player>
            {
                Computer("Enkel (som givaren)", new SimpleStrategy()),
                Computer("Grundstrategi", new BasicStrategy()),
                Computer("Försiktig", new CautiousStrategy()),
                Computer("Djärv", new BoldStrategy()),
            };

            for (int i = 0; i < Rounds; i++)
            {
                foreach (Player player in players)
                {
                    player.PlaceBet(10);
                }

                Round round = new Round(deck, dealer, players, rules);
                round.Deal();

                foreach (Player player in players)
                {
                    while (round.CanHit(player) && player.MakeMove() == Move.Hit)
                    {
                        round.Hit(player);
                    }
                }

                round.PlayDealerTurn();

                foreach (Player player in players)
                {
                    round.Settle(player);
                }
            }

            foreach (Player player in players)
            {
                Console.WriteLine($"          {player.Name,-20} vinster {player.Wins,4}  förluster {player.Losses,4}  lika {player.Pushes,4}");
                Check($"{player.Name}: alla {Rounds} rundor avgjordes", player.Wins + player.Losses + player.Pushes == Rounds);
            }
        });

        // ---------- Summering ----------
        Console.WriteLine($"\nResultat: {passed} OK, {failed} FEL, {notDone} EJ KLARA");
    }
}
