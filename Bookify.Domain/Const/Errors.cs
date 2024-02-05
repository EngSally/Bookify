namespace Bookify.Domain.Const
{
	public static class Errors
	{
		public  const  string  Required="Required";
		public  const  string  MaxLength=" {PropertyName} Can not be  more than {MaxLength} charctor";
		public  const  string  Duplicated="Another  record with the same {0} is aleardy exists";
		public  const  string  DuplicatedMail="Another  User with the same Email is aleardy exists";
		public  const  string  DuplicatedBook="Book With Same Title And Author  aleardy exists";
		public  const  string  AllowedImageExtension="Only .jpg .jpeg .png  are allowed";
		public  const  string  AllowedImageSize="Only 2M size are allowed";
		public  const  string  DateNotAtFuture="Publishing Date Not Allowed Future Date";
		public  const  string  RangNotBetween="{PropertyName} Must be between {From} and {To}";
		public  const  string  InvalidStartDate=" Invalid Start Date";
		public  const  string  InvalidEndDate=" Invalid End Date";
		public  const  string  InvalidUsername="Username only  contain letter or digit ";
		public  const  string  MaxMinLength="The {PropertyName} must be at least {MinLength} and at max {MaxLength} characters long.";
		public  const  string  ConfirmPasswordNotMatch="The password and confirmation password do not match.";
		public  const  string WeakPassword="Passwords contain an uppercase character, lowercase character, a digit, and a non-alphanumeric character.";
		public  const  string OnlyEnglishLetters = "Only English letters are allowed.";
		public  const  string OnlyArabicLetters = "Only Arabic letters are allowed.";
		public  const  string OnlyNumbersAndLetters = "Only Arabic/English letters or digits are allowed.";
		public  const  string DenySpecialCharacters = "Special characters are not allowed.";
		public  const  string InvalidMobileNumber = "Invalid mobile number.";
		public  const  string InvalidNatialId = "Invalid National Id.";
		public  const  string EmptyImage = "Please select an image.";
		public  const  string InvalidSerialNumber = "Invalid BookCopy SerailNumber.";
		public  const  string NotAvailableForRental = "This Copy Not Available For Rental";
		public  const  string BlackListedSubscriber = "This subscriber is blacklisted.";
		public  const  string InactiveSubscriber = "This subscriber is inactive.";
		public  const  string MaxCopiesReached = "This subscriber has reached the max number for rentals.";
		public  const  string CopyIsInRental = "This copy is already rentaled.";
		public  const  string ExtendNotAllowForBlockList= "Rental Not allow Extend For BlackList Subscriber.";
		public  const  string ExtendNotAllowForInActive= "Rental Not allow Extend For  this Subscriber before renewal.";
		public  const  string ExtendNotAllow= "Rental Not allow Extend ";
		public  const  string PenaltyShouldBePaid= "Penalty Should Be Paid ";

	}
}
