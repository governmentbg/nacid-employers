using Resc.Application.Applications;
using Resc.Data.Common.Models;

namespace Resc.Data.Applications.Register
{
	public class StudentPart : Part<Student>
	{
		public StudentPart()
			: base()
		{

		}

		public StudentPart(StudentPart part)
			: base(part)
		{

		}
	}
}
