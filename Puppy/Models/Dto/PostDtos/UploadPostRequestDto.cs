using System.ComponentModel.DataAnnotations;

namespace Puppy.Models.Dto;

public class UploadPostRequestDto
{
    [Microsoft.Build.Framework.Required]
    [StringLength(100)]
    public string Title { get; set; }
    [StringLength(10000)]
    public string Description { get; set; }
    public List<IFormFile> Imgs { get; set; }
}