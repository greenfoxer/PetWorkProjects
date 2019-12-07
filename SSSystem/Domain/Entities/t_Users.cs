namespace Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class t_Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_Users()
        {
            t_siz_users = new HashSet<t_siz_users>();
        }

        public long id { get; set; }

        [Required]
        [Display(Name = "Логин")]
        public string login { get; set; }

        public string system { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_siz_users> t_siz_users { get; set; }
    }
}
