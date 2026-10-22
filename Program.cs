using System.Text;

// Behövs för att ♥ ♦ ♣ ♠ ska visas rätt i terminalen
Console.OutputEncoding = Encoding.UTF8;

// En enda Random för hela programmet
var random = new Random();

var ui = new ConsoleUI();
var storage = new GameStorage("players.txt");
List<RuleTable> tables = LoadTables();
var game = new Game(random, ui, storage, tables);

game.Run();

// Läser in alla bord: rules.txt (Standard) och alla regelpaket som heter rules-*.txt.
List<RuleTable> LoadTables()
{
    List<string> files = new List<string> { "rules.txt" };

    foreach (string path in Directory.GetFiles(".", "rules-*.txt").Order())
    {
        files.Add(Path.GetFileName(path));
    }

    List<RuleTable> tables = new List<RuleTable>();

    foreach (string file in files)
    {
        var loader = new RuleSetLoader(file);
        RuleSet rules = loader.Load();

        foreach (string warning in loader.Warnings)
        {
            ui.ShowMessage(warning);
        }

        tables.Add(new RuleTable(TableName(file), rules));
    }

    return tables;
}

// "rules-vegas.txt" blir "Vegas", och rules.txt blir "Standard".
string TableName(string file)
{
    string name = Path.GetFileNameWithoutExtension(file).Replace("rules-", "");
    if (name == "rules")
    {
        name = "standard";
    }
    return char.ToUpper(name[0]) + name.Substring(1);
}
