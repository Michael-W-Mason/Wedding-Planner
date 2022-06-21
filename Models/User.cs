#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Wedding_Planner.Models;

public class User
{
    [Key]
    public int UserId {get; set;}
    [Required]
    public string FirstName {get; set;}
    [Required]
    public string LastName {get; set;}
    [Required]
    [EmailAddress]
    public string Email {get; set;}
    [Required]
    [DataType(DataType.Password)]
    public string Password {get; set;}
    [Required]
    [NotMapped]
    [Compare("Password")]
    public string ConfirmPassword {get; set;}

    public List<GuestLog> Weddings {get;set;} = new List<GuestLog>();
}