namespace BookTest.Core.ViewModels.Authors
{

    public class AuthorsFormViewModel
    {

        public int Id { get; set; }
        [Remote("Allow", null, AdditionalFields = "Id", ErrorMessage = Error.Duplicate)]
        [MaxLength(100, ErrorMessage = Error.MaxLength), Display(Name = "Author")]
        public string Name { get; set; } = null!;
    }
}
