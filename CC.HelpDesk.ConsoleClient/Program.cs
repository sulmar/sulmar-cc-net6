using CC.HelpDesk.Domain;

Console.WriteLine("Hello, .NET!");

var user = new User 
{ 
    Id = 1, 
    FirstName = "John", 
    LastName = "Smith", 
    Email = "john.smith@domain.com",
};

string x = "10";

x = "Hello World!";

// Typ anonimowy
var person = new { user.FirstName, user.LastName };

decimal price = RecalculatePrice(100);

Console.WriteLine(price);


Console.WriteLine($"Hello {user.Id} {user.FirstName} {user.LastName}!");

var calculator = (decimal amount) => amount + 10;

var price2 = calculator(100);

Console.WriteLine(price2);

// Zapis funkcyjny
// f(amount) = amount + 10  amount nalezy do decimal

// amount -> amount + 10


static decimal RecalculatePrice(decimal amount)
{
    return amount + 10;
}