namespace DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VaccinationSchedule")]
    public partial class VaccinationSchedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VaccinationSchedule()
        {
            Appointments = new HashSet<Appointment>();
            VaccinationManagements = new HashSet<VaccinationManagement>();
        }

        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public DateTime? Time { get; set; }
        public DateTime? CreateAt { get; set; }

        public int? Quantity { get; set; }

        public int? IdPatient { get; set; }

        public int? Status { get; set; }

        public int? IdVaccine { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Appointment> Appointments { get; set; }

        public virtual Patient Patient { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VaccinationManagement> VaccinationManagements { get; set; }

        public virtual Vaccine Vaccine { get; set; }      
    }
}
