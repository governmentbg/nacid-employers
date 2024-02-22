using System.ComponentModel;

namespace Resc.Data.Applications.Enums
{
    public enum ContactType
    {
        [Description("Лице за контакт от висше училище")]
        Institution = 1,

        [Description("Лице за контакт на работодателя")]
        Employer = 2,
    }
}
