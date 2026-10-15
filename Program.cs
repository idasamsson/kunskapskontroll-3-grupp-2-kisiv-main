var ui = new ConsoleUI();

Console.WriteLine($"Menyval: {ui.ShowStartMenu()}");
Console.WriteLine($"Antal spelare: {ui.AskPlayerCount()}");
Console.WriteLine($"Namn: {ui.AskPlayerName()}");
Console.WriteLine($"Drag: {ui.AskMove()}");