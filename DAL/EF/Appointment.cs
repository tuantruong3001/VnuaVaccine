namespace DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Appointment")]
    public partial class Appointment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? IdPatient { get; set; }

        public int? IdSchedule { get; set; }

        public DateTime? DateSchedule { get; set; }

        public int? StatusSchedule { get; set; }

        public virtual Patient Patient { get; set; }

        public virtual VaccinationSchedule VaccinationSchedule { get; set; }
    }
}
