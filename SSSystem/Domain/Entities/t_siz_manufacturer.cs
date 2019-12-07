namespace Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class t_siz_manufacturer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_siz_manufacturer()
        {
            t_siz_goods = new HashSet<t_siz_goods>();
        }

        public long id { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Наименование")]
        public string name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_siz_goods> t_siz_goods { get; set; }
    }
}
