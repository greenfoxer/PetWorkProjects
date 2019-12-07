namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class t_siz_orders_mas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t_siz_orders_mas()
        {
            t_siz_orders_matrix = new HashSet<t_siz_orders_matrix>();
        }

        public long id { get; set; }

        public DateTime date { get; set; }

        public long? creator { get; set; }

        public int? accepted { get; set; }

        public long? acceptor { get; set; }

        [StringLength(150)]
        public string quarter { get; set; }

        public int? year { get; set; }
        public int deleted { get; set; }
        public DateTime? dateaccepted { get; set; }

        public long department { get; set; }

        public virtual t_siz_department t_siz_department { get; set; }

        public virtual t_siz_users t_siz_users { get; set; }

        public virtual t_siz_users t_siz_users1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_siz_orders_matrix> t_siz_orders_matrix { get; set; }
    }
}
