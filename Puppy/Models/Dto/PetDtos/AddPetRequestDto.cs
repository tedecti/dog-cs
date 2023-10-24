using System.ComponentModel.DataAnnotations;
using Curs.Models;

namespace Puppy.Models.Dto
{
	public class AddPetRequestDto
	{
		[Microsoft.Build.Framework.Required]
		[StringLength(100)]
		public string Name { get; set; }
		[Microsoft.Build.Framework.Required]
		[StringLength(100)]
		public string PassportNumber { get; set; }
	}
}
