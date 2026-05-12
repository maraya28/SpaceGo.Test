
Console.ForegroundColor = ConsoleColor.Green;

Console.WriteLine("------------");
Console.WriteLine("JWT Token:");
Console.WriteLine("------------");
Console.WriteLine(string.Empty);

Console.ForegroundColor = ConsoleColor.White;
var jwt = TokenGenerator.TokenGenerator.Create();
Console.WriteLine(jwt);
Console.ReadLine();


