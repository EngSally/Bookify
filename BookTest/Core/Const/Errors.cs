namespace BookTest.Core.Const
{
    public static class Errors
    {
        public  const  string  Required="Required";
        public  const  string  MaxLength=" Length Can not be  more than {1} charctor";
        public  const  string  Duplicated="Another  record with the same {0} is aleardy exists";
        public  const  string  DuplicatedBook="Book With Same Title And Author  aleardy exists";
        public  const  string  AllowedImageExtension="Only .jpg .jpeg .png  are allowed";
        public  const  string  AllowedImageSize="Only 2M size are allowed";
        public  const  string  DateNotAtFuture="Publishing Date Not Allowed Future Date";
        public  const  string  RangNotBetween="{0} Must be between {1} and {2} ";
        public  const  string  InvalidUsername="Username only  contain letter or digit ";
        public  const  string  MaxMinLength="The {0} must be at least {2} and at max {1} characters long.";
        public  const  string  ConfirmPasswordNotMatch="The password and confirmation password do not match.";
        public  const  string WeakPassword="Passwords contain an uppercase character, lowercase character, a digit, and a non-alphanumeric character.";
        public  const  string OnlyEnglishLetters = "Only English letters are allowed.";
        public  const  string OnlyArabicLetters = "Only Arabic letters are allowed.";
        public  const  string OnlyNumbersAndLetters = "Only Arabic/English letters or digits are allowed.";
        public  const  string DenySpecialCharacters = "Special characters are not allowed.";

    }
}
