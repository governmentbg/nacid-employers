using Resc.Data.Nomenclatures.Models;
using System;

namespace Resc.Data.Nomenclatures
{
    public class SchoolYear : Nomenclature
    {
        public int PrimaryYear { get; set; }
        public bool IsCurrent { get; set; }

        private SchoolYear()
        {

        }

        public SchoolYear(int previousYear)
        {
            this.PrimaryYear = previousYear + 1;
            this.Name = $"{this.PrimaryYear}/{this.PrimaryYear + 1}";
            this.IsActive = true;
            this.IsCurrent = true;
            this.ViewOrder = 1;
        }
    }
}
