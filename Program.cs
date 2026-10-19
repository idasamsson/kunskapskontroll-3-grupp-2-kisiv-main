using System.Text;

// Behövs för att ♥ ♦ ♣ ♠ ska visas rätt i terminalen
Console.OutputEncoding = Encoding.UTF8;

// En enda Random för hela programmet
var random = new Random();

var ui = new ConsoleUI();
var storage = new GameStorage("players.txt");
var loader = new RuleSetLoader("rules.txt");
var rules = loader.Load();
var game = new Game(random, ui, storage, rules);

foreach (string warning in loader.Warnings)
{
    ui.ShowMessage(warning);
}

game.Run();