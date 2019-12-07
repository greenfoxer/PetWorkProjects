namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_SSS
    {
        public long id { get; set; }

        public string Name { get; set; }

        [StringLength(15)]
        public string code { get; set; }

        [StringLength(255)]
        public string link { get; set; }
    }
}
