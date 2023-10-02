namespace WebApplication1.Commands
{
    public class LoginCommand : ICommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
