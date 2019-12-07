namespace OrderBarcodeData.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tOrderBarcode")]
    public partial class tOrderBarcode
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(6)]
        [Display(Name = "Заказ")]
        public string Code { get; set; }

        [Column(Order = 1)]
        [StringLength(50)]
        [Display(Name = "Штрихкод")]
        public string Barcode { get; set; }

        [Column(Order = 2)]
        [Display(Name = "В работе")]
        public bool IsActive { get; set; }
    }
}
