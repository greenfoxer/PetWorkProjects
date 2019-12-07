namespace Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("t_siz_users")]
    public partial class t_siz_users: t_Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_siz_users()
        {
            t_siz_orders = new HashSet<t_siz_orders>();
        }
        
        [StringLength(500)]
        [Display(Name = "Имя пользователя")]
        public string name { get; set; }
        [Display(Name = "Подразделение")]
        public long? department { get; set; }

        public int deleted { get; set; }

        [StringLength(50)]
        [Display(Name = "Роль")]
        public string role { get; set; }

        public long? global { get; set; }

        public virtual t_siz_department t_siz_department { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_siz_orders> t_siz_orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_siz_orders_mas> t_siz_orders_mas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_siz_orders_mas> t_siz_orders_mas1 { get; set; }

        public virtual t_Users t_Users { get; set; }
    }
}
