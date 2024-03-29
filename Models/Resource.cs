using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace learnify.Models;
public class Resource{
    [Key]
    public Guid Id { get; set; }
    [Required]
    [MinLength(3,ErrorMessage ="Name must be at least 3 letters long")]
    public string? Name { get; set;}
    [Required]
    [Url(ErrorMessage ="Link must be a valid URL.")]
    public string? Link { get; set; }
    [Required]
    [MinLength(10, ErrorMessage ="Please explain a little bit about the course.")]
    public string? Description {get; set; }
    public string? Field { get; set; }
    public string Tags { get; set; }

    public Guid UserId {get; set;}

//relations
    public User? User {get; set;}
}