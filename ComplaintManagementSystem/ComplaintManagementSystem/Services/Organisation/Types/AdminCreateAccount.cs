using System.ComponentModel.DataAnnotations;

public class AdminCreateAccountRequest
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string emailAddress { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string password { get; set; }
    [Required]
    public Guid businessReference { get; set; }
    [Required]
    public string firstName { get; set; }
    [Required]
    public string lastName { get; set; }
    [Required]
    public RolesEnum role { get; set; }
}
