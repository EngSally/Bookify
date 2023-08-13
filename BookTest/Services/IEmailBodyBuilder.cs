namespace BookTest.Services
{
    public interface IEmailBodyBuilder
    {
        string GetEmailBody(string imageUrl, string body, string url, string header, string linkTitle);
    }
}
