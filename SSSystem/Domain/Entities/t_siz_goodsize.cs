namespace Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class t_siz_goodsize
    {
        [Key]
        [Column(Order = 0)]
        public long id { get; set; }


        public long good { get; set; }


        public long size { get; set; }


        public int deleted { get; set; }

        public int sign { get; set; }

        public virtual t_siz_goods t_siz_goods { get; set; }

        public virtual t_siz_goods_size t_siz_goods_size { get; set; }
    }
}
