using Resc.Data.Emails;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resc.Application.Emails.Interfaces
{
    public interface IEmailService
	{
		IEnumerable<Email> GetPendingEmails(int limit);

		Task<Email> ComposeEmailAsync(string alias, object templateData, params string[] recipients);

		bool SendEmail(Email email, IEmailConfiguration emailConfiguration);
	}
}
