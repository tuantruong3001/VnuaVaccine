namespace DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VaccinationManagement")]
    public partial class VaccinationManagement
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? IdStaff { get; set; }
      
        public string Description { get; set; }

        public DateTime? Time { get; set; }

        public int? Quantity { get; set; }

        public virtual MedicalStaff MedicalStaff { get; set; }

        public virtual VaccinationSchedule VaccinationSchedule { get; set; }
    }
}
