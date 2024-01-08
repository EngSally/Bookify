namespace BookTest.Filters
{
	public class GreaterThanAttribute : ValidationAttribute
	{
		public  int greteValue;
		protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
		{
			if (Convert.ToInt32(value) > greteValue)
			{ return ValidationResult.Success; }
			else
			{
				return new ValidationResult("not greater ");
			}

		}
	}
}
