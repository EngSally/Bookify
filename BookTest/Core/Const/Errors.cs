namespace BookTest.Core.Const
{
    public static class Errors
    {
        public  const  string  MaxLength=" Length Can not be  more than {1} charctor";
        public  const  string  Duplicated="{0} with the  same name is  aleardy exists";
        public  const  string  DuplicatedBook="Book With Same Title And Author  aleardy exists";
        public  const  string  AllowedImageExtension="Only .jpg .jpeg .png  are allowed";
        public  const  string  AllowedImageSize="Only 2M size are allowed";
        public  const  string  DateNotAtFuture="Publishing Date Not Allowed Future Date";
    }
}
