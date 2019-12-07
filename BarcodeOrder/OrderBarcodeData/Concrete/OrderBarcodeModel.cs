namespace OrderBarcodeData.Concrete
{
    using System.Data.Entity;
    using OrderBarcodeData.Entities;

    partial class OrderBarcodeModel : DbContext
    {
        public OrderBarcodeModel()
            : base("name=OrderBarcodeModel")
        {
        }

        public virtual DbSet<tOrderBarcode> tOrderBarcode { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tOrderBarcode>()
                .Property(e => e.Code)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tOrderBarcode>()
                .Property(e => e.Barcode)
                .IsUnicode(false);
        }
    }
}
