namespace DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Vaccine")]
    public partial class Vaccine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vaccine()
        {
            VaccinationSchedules = new HashSet<VaccinationSchedule>();
        }

        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(50)]
        public string NameVaccine { get; set; }

        [StringLength(50)]
        public string Munafacturer { get; set; }

        [StringLength(150)]
        public string Path { get; set; }
        public DateTime? ProductionDate { get; set; }

        public DateTime? ExpirationData { get; set; }

        public int? QuantityStock { get; set; }

        [StringLength(150)]
        public string Description { get; set; }
        [StringLength(150)]
        public string Note { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VaccinationSchedule> VaccinationSchedules { get; set; }
    }
}
