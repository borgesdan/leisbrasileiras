using LawLibrary;


while (true)
{
    string? path;
    Console.Write("Digite o caminho do arquivo de lei desejado para análise: ");
    path = Console.ReadLine();

    if (path == null || !File.Exists(path))
    {
        Console.WriteLine("O caminho específicado não é válido.");
        Console.ReadKey();
        Console.Clear();
        continue;
    }

    LawParser parser = new LawParser(path);
    await parser.ParseAsync();

    Console.Write("O arquivo foi análisado, digite o caminho para salvar: ");
    string? savePath = Console.ReadLine();

    if (savePath == null)
    {
        Console.WriteLine("O caminho específicado não é válido.");
        Console.ReadKey();
        Console.Clear();
        continue;
    }

    parser.Save(LawParserSaveType.Json, savePath);
    break;
}