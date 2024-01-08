using System.ComponentModel;

namespace BookTest.Core.ViewModels.Categories
{
	public class CategoriesFormViewModel
	{
		public int Id { get; set; }
		[MaxLength(100, ErrorMessage = Errors.MaxLength)]
		[Remote("AllowItem", null, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated)]
		[DisplayName("Category"),
			 RegularExpression(RegexPatterns.CharactersOnly_Eng, ErrorMessage = Errors.OnlyEnglishLetters)]
		public string Name { get; set; } = null!;

	}
}
