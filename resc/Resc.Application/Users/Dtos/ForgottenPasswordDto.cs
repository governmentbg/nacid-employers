namespace Resc.Application.Users.Dtos
{
    public class ForgottenPasswordDto
    {
        public string Token { get; set; }

        public string NewPassword { get; set; }

        public string NewPasswordAgain { get; set; }
    }
}
