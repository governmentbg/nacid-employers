using System.ComponentModel;

namespace Resc.Data.Applications.Enums
{
	[Description("Студентски статус")]
	public enum StudentStatusType
	{
		[Description("Действащ")]
		Current = 1,

		[Description("Прекъснал")]
		Interrupted = 2,

		[Description("Завършил")]
		Graduated = 3
	}
}