using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface ISIZSystem
    {
        IEnumerable<t_siz_goods> SIZGoodsList { get; }
        IEnumerable<t_siz_manufacturer> SIZManList { get; }
        IEnumerable<t_siz_department> SIZDepartmentList { get; }
        IEnumerable<t_siz_goods_size> SIZSizeList { get; }
        IEnumerable<t_siz_orders> SIZOrdersList { get; }
        IEnumerable<t_siz_users> SIZUsersList { get; }
        IEnumerable<t_Users> SSSUsers { get; }
        IEnumerable<t_siz_goodsize> GoodSize { get; }
        IEnumerable<t_siz_depgood> DepGood { get; }
        IEnumerable<t_siz_orders_mas> SIZOrdersMatrix { get; }
        IEnumerable<t_siz_size_type> SIZSizeType { get; }
        IEnumerable<t_siz_orders_matrix> OrdersItems { get; }
        t_siz_parameters SIZParameters { get; }
        void AddOrdersItems(long newitm, long order, long olditm=0);
        void AddOrder(t_siz_orders order);
        void AddGood(t_siz_goods good);
        void AddMan(t_siz_manufacturer man);
        void AddParam(t_siz_parameters param);
        void AddSize(t_siz_goods_size size);
        void AddUser(t_siz_users size);
        void AddGoodSize(long id, List<t_siz_goodsize> size);
        void AddDepGood(long id, List<t_siz_depgood> dept);
        void SendToManager(long order, long user);
        void CreateOrdersForAll();
        void EditMatrix(t_siz_orders_mas matrix);
        void InitSize(t_siz_goods good, long oldid=0);
        object GetById(long id, string type);
    }
}
