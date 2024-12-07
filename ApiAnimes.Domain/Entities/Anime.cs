using System.ComponentModel.DataAnnotations;


namespace ApiAnimes.Domain.Entities;
public class Anime
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Author { get; set; }
}
