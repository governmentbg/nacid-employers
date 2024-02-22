namespace Resc.Application.Common.Interfaces
{
    public interface IUserContext
	{
		int UserId { get; }
		string Username { get; }
		string InstitutionName { get; }
		string Role { get; }
	}
}
