namespace Domain.Entities
{

    public partial class t_siz_orders_matrix
    {
        public long id { get; set; }

        public long order { get; set; }

        public long order_item { get; set; }

        public virtual t_siz_orders t_siz_orders { get; set; }

        public virtual t_siz_orders_mas t_siz_orders_mas { get; set; }
    }
}
