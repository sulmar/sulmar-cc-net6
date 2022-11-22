using CC.HelpDesk.Domain;

Console.WriteLine("Hello, .NET!");

decimal amount = 10;

RecalculatePrice(amount);

Console.WriteLine(amount);

var test = new decimal();

// Typy wartościowe (value types)
// int, decimal, DateTime, struct

// Typy referencyjne (reference types)
// class

var user = new User(1, "John", "Smith") 
{ 
    Email = "john.smith@domain.com",
    Salary = 1000  
};

// var newUser = user;

var newUser = new User(user.Id, user.FirstName, user.LastName) {

    Email = user.Email,
    Salary = user.Salary
};

newUser.Salary = 2000;

Console.WriteLine(user.Salary);
Console.WriteLine(newUser.Salary);



RecalculateSalary(user);

Console.WriteLine(user.Salary);


#region Testy





// Typ anonimowy
var person = new { user.FirstName, user.LastName };

// decimal price = RecalculatePrice(100);

// Console.WriteLine(price);


Console.WriteLine($"Hello {user.Id} {user.FirstName} {user.LastName}!");

var calculator = (decimal amount) => amount + 10;

var price2 = calculator(100);

Console.WriteLine(price2);

// Zapis funkcyjny
// f(amount) = amount + 10  amount nalezy do decimal

// amount -> amount + 10


#endregion

static void RecalculatePrice(decimal amount)
{
    amount = amount + 10;

    Console.WriteLine(amount);
}

static void RecalculateSalary(User user)
{
    user.Salary = user.Salary + 10;

    Console.WriteLine(user.Salary);
}

