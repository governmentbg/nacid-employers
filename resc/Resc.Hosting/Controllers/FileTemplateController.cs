using Microsoft.AspNetCore.Mvc;
using Resc.Application.Common.Interfaces;
using Resc.Data;
using Resc.Hosting.Controllers.Common;

namespace Resc.Hosting.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FileTemplateController : BaseEntityController<FileTemplate>
	{
		public FileTemplateController(IAppDbContext context)
			: base(context)
		{

		}
	}
}
