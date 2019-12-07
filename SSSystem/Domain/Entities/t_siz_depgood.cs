namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_siz_depgood
    {
        public long id { get; set; }

        public long department { get; set; }

        public long good { get; set; }

        public int deleted { get; set; }

        public virtual t_siz_department t_siz_department { get; set; }

        public virtual t_siz_goods t_siz_goods { get; set; }
    }
}