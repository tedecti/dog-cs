using System.ComponentModel.DataAnnotations;

namespace Puppy.Models.Dto;

public class AddCommentaryRequestDto
{
    [Microsoft.Build.Framework.Required]
    [StringLength(100)]
    public string Text { get; set; }
}