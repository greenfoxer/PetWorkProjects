namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class t_siz_goods
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_siz_goods()
        {
            t_siz_orders = new HashSet<t_siz_orders>();
            t_siz_goodsize = new HashSet<t_siz_goodsize>();
        }

        public long id { get; set; }

        [StringLength(50)]
        [Display(Name="Артикул")]
        public string code { get; set; }

        [StringLength(200)]
        [Display(Name = "ГОСТ")]
        public string standard { get; set; }

        [StringLength(50)]
        [Display(Name = "Ед.изм.")]
        public string unit { get; set; }

        [Required]
        [Display(Name = "Наименование")]
        [DataType(DataType.MultilineText)]
        public string name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? date { get; set; }

        [StringLength(150)]
        public string author { get; set; }

        public int deleted { get; set; }
        [Display(Name = "Производитель")]
        public long manufacturer { get; set; }
        [Display(Name = "Категория размера")]
        public long? sizetype { get; set; }
        public virtual t_siz_manufacturer t_siz_manufacturer { get; set; }
        public virtual t_siz_size_type t_siz_size_type { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_siz_orders> t_siz_orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_siz_goodsize> t_siz_goodsize { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_siz_depgood> t_siz_depgood { get; set; }
    }
}
