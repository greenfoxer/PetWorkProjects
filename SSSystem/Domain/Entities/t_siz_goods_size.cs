namespace Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class t_siz_goods_size
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_siz_goods_size()
        {
            t_siz_goodsize = new HashSet<t_siz_goodsize>();
            t_siz_orders = new HashSet<t_siz_orders>();
        }

        public long? goods { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Размер")]
        public string size { get; set; }

        public long id { get; set; }
        public long? type { get; set; }

        [StringLength(100)]
        public string alternative { get; set; }

        public virtual t_siz_size_type t_siz_size_type { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_siz_orders> t_siz_orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_siz_goodsize> t_siz_goodsize { get; set; }
    }
}
