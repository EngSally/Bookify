namespace BookTest.Core.ViewModels
{
    public class CategoriesFormViewModel
    {
        public int Id { get; set; }

        [MaxLength(100,ErrorMessage ="Max Lenght is 100 char")]
        public string Name { get; set; } = null!;
    }
}
