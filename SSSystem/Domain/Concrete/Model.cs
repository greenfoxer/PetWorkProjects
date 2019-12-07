namespace Domain.Concrete
{
    using System.Data.Entity;
    using Domain.Entities;

    public partial class Model : DbContext
    {
        public Model()
            : base("name=ModelSSS")
        {
        }

        public virtual DbSet<t_siz_department> t_siz_department { get; set; }
        public virtual DbSet<t_siz_goods> t_siz_goods { get; set; }
        public virtual DbSet<t_siz_goods_size> t_siz_goods_size { get; set; }
        public virtual DbSet<t_siz_manufacturer> t_siz_manufacturer { get; set; }
        public virtual DbSet<t_siz_orders> t_siz_orders { get; set; }
        public virtual DbSet<t_siz_users> t_siz_users { get; set; }
        public virtual DbSet<t_SSS> t_SSS { get; set; }
        public virtual DbSet<t_Users> t_Users { get; set; }
        public virtual DbSet<t_siz_orders_mas> t_siz_orders_mas { get; set; }
        public virtual DbSet<t_siz_orders_matrix> t_siz_orders_matrix { get; set; }
        public virtual DbSet<t_siz_parameters> t_siz_parameters { get; set; }
        public virtual DbSet<t_siz_goodsize> t_siz_goodsize { get; set; }
        public virtual DbSet<t_siz_size_type> t_siz_size_type { get; set; }
        public virtual DbSet<t_siz_depgood> t_siz_depgood { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<t_siz_department>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<t_siz_parameters>()
                .Property(e => e.quarter)
                .IsUnicode(false);

            modelBuilder.Entity<t_siz_department>()
                .HasMany(e => e.t_siz_orders)
                .WithRequired(e => e.t_siz_department)
                .HasForeignKey(e => e.department)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<t_siz_department>()
                .HasMany(e => e.t_siz_users)
                .WithRequired(e => e.t_siz_department)
                .HasForeignKey(e => e.department)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<t_siz_goods>()
                .Property(e => e.code)
                .IsUnicode(false);

            modelBuilder.Entity<t_siz_goods>()
                .Property(e => e.standard)
                .IsUnicode(false);

            modelBuilder.Entity<t_siz_goods>()
                .Property(e => e.unit)
                .IsUnicode(false);

            modelBuilder.Entity<t_siz_goods>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<t_siz_goods>()
                .Property(e => e.author)
                .IsUnicode(false);

            modelBuilder.Entity<t_siz_goods>()
                .HasMany(e => e.t_siz_orders)
                .WithRequired(e => e.t_siz_goods)
                .HasForeignKey(e => e.goods)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<t_siz_goods_size>()
                .Property(e => e.size)
                .IsUnicode(false);

            modelBuilder.Entity<t_siz_goods_size>()
                .HasMany(e => e.t_siz_orders)
                .WithRequired(e => e.t_siz_goods_size)
                .HasForeignKey(e => e.size)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<t_siz_manufacturer>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<t_siz_manufacturer>()
                .HasMany(e => e.t_siz_goods)
                .WithRequired(e => e.t_siz_manufacturer)
                .HasForeignKey(e => e.manufacturer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<t_siz_orders>()
                .Property(e => e.quarter)
                .IsUnicode(false);

            modelBuilder.Entity<t_siz_users>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<t_siz_users>()
                .Property(e => e.login)
                .IsUnicode(false);

            modelBuilder.Entity<t_siz_users>()
                .HasMany(e => e.t_siz_orders)
                .WithRequired(e => e.t_siz_users)
                .HasForeignKey(e => e.user)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<t_SSS>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<t_SSS>()
                .Property(e => e.code)
                .IsUnicode(false);

            modelBuilder.Entity<t_SSS>()
                .Property(e => e.link)
                .IsUnicode(false);

            modelBuilder.Entity<t_Users>()
                .HasMany(e => e.t_siz_users)
                .WithRequired(e => e.t_Users)
                .HasForeignKey(e => e.global)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<t_siz_goods>()
                .HasMany(e => e.t_siz_goodsize)
                .WithRequired(e => e.t_siz_goods)
                .HasForeignKey(e => e.good)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<t_siz_goods_size>()
                .HasMany(e => e.t_siz_goodsize)
                .WithRequired(e => e.t_siz_goods_size)
                .HasForeignKey(e => e.size)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<t_siz_orders>()
                .HasMany(e => e.t_siz_orders_matrix)
                .WithRequired(e => e.t_siz_orders)
                .HasForeignKey(e => e.order_item)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<t_siz_orders_mas>()
                .Property(e => e.quarter)
                .IsUnicode(false);

            modelBuilder.Entity<t_siz_orders_mas>()
                .HasMany(e => e.t_siz_orders_matrix)
                .WithRequired(e => e.t_siz_orders_mas)
                .HasForeignKey(e => e.order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<t_siz_department>()
                .HasMany(e => e.t_siz_orders_mas)
                .WithRequired(e => e.t_siz_department)
                .HasForeignKey(e => e.department)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<t_siz_users>()
                .HasMany(e => e.t_siz_orders_mas)
                .WithRequired(e => e.t_siz_users)
                .HasForeignKey(e => e.creator)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<t_siz_users>()
                .HasMany(e => e.t_siz_orders_mas1)
                .WithOptional(e => e.t_siz_users1)
                .HasForeignKey(e => e.acceptor);

            modelBuilder.Entity<t_siz_department>()
                .HasMany(e => e.t_siz_depgood)
                .WithRequired(e => e.t_siz_department)
                .HasForeignKey(e => e.department)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<t_siz_goods>()
                .HasMany(e => e.t_siz_depgood)
                .WithRequired(e => e.t_siz_goods)
                .HasForeignKey(e => e.good)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<t_siz_size_type>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<t_siz_size_type>()
                .Property(e => e.template)
                .IsUnicode(false);

            modelBuilder.Entity<t_siz_size_type>()
                .HasMany(e => e.t_siz_goods_size)
                .WithOptional(e => e.t_siz_size_type)
                .HasForeignKey(e => e.type);

            modelBuilder.Entity<t_siz_size_type>()
                .HasMany(e => e.t_siz_goods)
                .WithOptional(e => e.t_siz_size_type)
                .HasForeignKey(e => e.sizetype);
        }
    }
}
