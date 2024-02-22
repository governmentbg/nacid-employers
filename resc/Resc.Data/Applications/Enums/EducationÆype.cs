using System.ComponentModel;

namespace Resc.Data.Applications.Enums
{
    public enum EducationType
    {
		[Description("Държавна поръчка")]
		Standard = 1,

		[Description("Платено обучение")]
		Payment = 2,
	}
}
