using LawLibrary;
string filePath = @"C:\Users\Danilo\Desktop\leis\8112-90.txt";
//XmlTextParser textParser = new XmlTextParser(filePath);
//textParser.Read();

LawParser parser = new LawParser(filePath);
await parser.ParseAsync();
//await parser.SaveAsync(ObjectParserSaveType.Xml, @"D:\file.xml", true);
//await parser.SaveAsync(ObjectParserSaveType.Text, @"D:\file.txt", true);
await parser.SaveAsync(LawParserSaveType.Json, @"D:\file.json", true);

var p = LawParser.Load(LawParserSaveType.Json, @"D:\file.json");

if (parser != null)
{
    //foreach (var item in p.Law.NormativePart)
    //{
    //    item.GetAll(LawContentType.Capitulo);
    //}

    //var list = p.Law.NormativePart[1].GetAll(LawContentType.Capitulo);

    //foreach (var item in list)
    //{
    //    Console.WriteLine(item.Text);
    //}

    List<LawText> lawTexts = new List<LawText>();

    foreach(var item in parser.Law.NormativePart)
    {
        var list = item.GetAll(LawContentType.Capitulo);

        if(list.Any())
            lawTexts.AddRange(list);
    }
}
