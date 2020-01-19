namespace realworldapp.Error
{
    public class ErrorMessages
    {
        public const string NotFound = "not found";
        public const string AlreadyExist = "already exist";
        public const string NoPrivileges = "No privileges";
        public const string DeleteByNotAuthor = "Only author of an article or author of a comment can delete it.";
        public const string EditArticleByUnauthorizedUser = "Only author of an article can upodate it.";
        public const string YouMustLoginFirst = "You must login first";
    }
}