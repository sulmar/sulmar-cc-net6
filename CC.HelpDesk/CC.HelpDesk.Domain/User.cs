using CC.Core.Domain;

namespace CC.HelpDesk.Domain;



public class User : BaseEntity
{

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string HashedPassword { get; set; }
    public string Email { get; set; }
    public decimal Salary { get; set; }

    // Konstruktor bezparametryczny (parameterless constructor)
    private User()
    {
        CreatedOn = DateTime.Now;
    }

    // Konstruktor parametryczny
    public User(string firstName, string lastName)
        : this()
    {        
        FirstName = firstName;
        LastName = lastName;
    }

}
