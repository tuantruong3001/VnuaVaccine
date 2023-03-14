namespace DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MedicalStaff")]
    public partial class MedicalStaff
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MedicalStaff()
        {
            VaccinationManagements = new HashSet<VaccinationManagement>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int? Age { get; set; }

        [StringLength(10)]
        public string Sex { get; set; }

        [StringLength(150)]
        public string Address { get; set; }

        public int? PhoneNumber { get; set; }

        public int? IdUserName { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VaccinationManagement> VaccinationManagements { get; set; }
    }
}
