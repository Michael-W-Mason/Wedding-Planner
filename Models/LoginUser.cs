#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Wedding_Planner.Models;

public class LoginUser
{
    [Required]
    public string LoginEmail {get; set;}
    [Required]
    public string LoginPassword {get; set;}
}