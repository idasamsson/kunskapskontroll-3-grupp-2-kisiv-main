using System.Text;

// Behövs för att ♥ ♦ ♣ ♠ ska visas rätt i terminalen
Console.OutputEncoding = Encoding.UTF8;

// En enda Random för hela programmet
var random = new Random();

var ui = new ConsoleUI();
var storage = new GameStorage("players.txt");
var game = new Game(random, ui, storage);

game.Run();