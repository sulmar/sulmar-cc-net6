namespace CC.HelpDesk.Domain;



public class User : BaseEntity
{
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string HashedPassword { get; set; }
    public string Email { get; set; }


    // Konstruktor bezparametryczny (parameterless constructor)
    private User()
    {
        CreatedOn = DateTime.Now;
    }

    // Konstruktor parametryczny
    public User(int id, string firstName, string lastName)
        : this()
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

}
