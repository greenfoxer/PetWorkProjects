namespace Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public partial class t_siz_parameters
    {
        public long id { get; set; }
        [Display(Name = "Год")]
        public int? year { get; set; }
        [Display(Name = "Квартал")]
        public string quarter { get; set; }

        public string admin { get; set; }

        public string admin2 { get; set; }
        [Display(Name = "Количество")]
        public string leader { get; set; }

        public string leader2 { get; set; }
    }
}