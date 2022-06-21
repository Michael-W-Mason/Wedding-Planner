#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace Wedding_Planner.Models;

public class Wedding
{
    [Key]
    public int WeddingId {get; set;}
    [Required]
    public string WedderOne {get; set;}
    [Required]
    public string WedderTwo {get; set;}
    [Required]
    public DateTime Date {get; set;}
    [Required]
    public string Address {get;set;}
    public int? Creator {get;set;}
    public List<GuestLog> Guests {get;set;} = new List<GuestLog>();

}