namespace Web_2.Models;

public class InformationUser
{
    public int Idname { get; set; }
    public int User_id { get; set; }
    public string Username { get; set; }
    public int Phone { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    
    public virtual User User { get; set; }
}