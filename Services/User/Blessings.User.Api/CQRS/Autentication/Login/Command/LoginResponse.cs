namespace Blessings.User.Api.CQRS.Autentication;

public class LoginResponse
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string AccessToken { get; set; }
}