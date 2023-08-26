using System.ComponentModel.DataAnnotations;

namespace FirstBlazorProject_BookStore.Model.DTOs.Demo;

public class DemoDto
{
    [Required]
    public string Name { get; set; } = default!;
}