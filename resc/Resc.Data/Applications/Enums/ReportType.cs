using System.ComponentModel;

namespace Resc.Data.Applications.Enums
{
	public enum ReportType
	{
		[Description("Обща бройка обучаеми")]
		DefaultReport = 1,

		[Description("По висше училище")]
		ReportByInstitution = 2,

		[Description("По професионално направление")]
		ReportByResearchArea = 3,

		[Description("По специалност")]
		ReportBySpecialty = 4,

		[Description("По професионално направление и специалност")]
		ReportByResearchAreaAndSpecialty = 5,

		[Description("По професионално направление, специалност и ВУ")]
		ReportByResearchAreaAndSpecialityAndInstitution = 6,
	}
}
