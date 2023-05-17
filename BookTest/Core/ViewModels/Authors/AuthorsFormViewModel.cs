namespace BookTest.Core.ViewModels.Authors
{

    public class AuthorsFormViewModel
    {

        public int Id { get; set; }
        [Remote("Allow", null, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated)]
        [MaxLength(100, ErrorMessage = Errors.MaxLength), Display(Name = "Author")]
        public string Name { get; set; } = null!;
    }
}
