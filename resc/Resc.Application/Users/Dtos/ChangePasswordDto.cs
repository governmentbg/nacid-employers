namespace Resc.Application.Users.Dtos
{
	public class ChangePasswordDto
	{
		public string OldPassword { get; set; }

		public string NewPassword { get; set; }

		public string NewPasswordAgain { get; set; }
	}
}
