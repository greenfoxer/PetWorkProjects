namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class t_siz_orders
    {
        public long id { get; set; }
        [Display(Name = "Наименование СиЗ")]
        public long goods { get; set; }
        [Display(Name = "Размер")]
        public long size { get; set; }

        public long? department { get; set; }

        public long user { get; set; }
        [Display(Name = "Количество за первый квартал")]
        public int num1 { get; set; }
        [Display(Name = "Количество за второй квартал")]
        public int num2 { get; set; }
        [Display(Name = "Количество за третий квартал")]
        public int num3 { get; set; }
        [Display(Name = "Количество за четвертый квартал")]
        public int num4 { get; set; }
        public int num { get { return num1 + num2 + num3 + num4; } }

        public int month { get; set; }

        [Required]
        [StringLength(50)]
        public string quarter { get; set; }

        public int? year { get; set; }

        public int deleted { get; set; }

        [Column(TypeName = "date")]
        public DateTime date { get; set; }

        public virtual t_siz_department t_siz_department { get; set; }

        public virtual t_siz_goods t_siz_goods { get; set; }

        public virtual t_siz_goods_size t_siz_goods_size { get; set; }

        public virtual t_siz_users t_siz_users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t_siz_orders_matrix> t_siz_orders_matrix { get; set; }
    }
}
