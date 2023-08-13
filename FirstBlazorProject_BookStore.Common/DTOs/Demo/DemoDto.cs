using System.ComponentModel.DataAnnotations;

namespace FirstBlazorProject_BookStore.Common.DTOs.Demo;

public class DemoDto
{
    [Required]
    public string Name { get; set; }
}