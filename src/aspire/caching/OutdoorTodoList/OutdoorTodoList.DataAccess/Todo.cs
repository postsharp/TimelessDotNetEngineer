using Metalama.Patterns.Caching.Aspects;
using System.ComponentModel.DataAnnotations;

public class Todo
{
    [CacheKey]
    public int Id { get; set; }

    public bool IsCompleted { get; set; }

    [Required]
    public string Title { get; set; }
}
