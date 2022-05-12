using LawLibrary;
string filePath = @"C:\Users\Danilo\Desktop\leis\8112-90.txt";
//XmlTextParser textParser = new XmlTextParser(filePath);
//textParser.Read();

ObjectParser parser = new ObjectParser(filePath);
await parser.ParseAsync();
await parser.SaveAsync(ObjectParserSaveType.Xml, @"D:\file.xml", true);
await parser.SaveAsync(ObjectParserSaveType.Text, @"D:\file.txt", true);
