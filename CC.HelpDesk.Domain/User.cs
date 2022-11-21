namespace CC.HelpDesk.Domain;



public class User : BaseEntity
{
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string HashedPassword { get; set; }
    public string Email { get; set; }

}
