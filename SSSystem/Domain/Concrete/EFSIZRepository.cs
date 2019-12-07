using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Abstract;
using Messenger;

namespace Domain.Concrete
{
    public class EFSIZRepository: ISIZSystem
    {
        Model context = new Model();
        public IEnumerable<t_siz_goods> SIZGoodsList
        {
            get { return context.t_siz_goods; }
        }
        public IEnumerable<t_Users> SSSUsers
        {
            get { return context.t_Users; }
        }
        public t_siz_parameters SIZParameters
        {
            get { return context.t_siz_parameters.Where(p=>p.id==1).First(); }
        }
        public IEnumerable<t_siz_manufacturer> SIZManList
        {
            get { return context.t_siz_manufacturer; }
        }
        public IEnumerable<t_siz_orders> SIZOrdersList
        {
            get { return context.t_siz_orders; }
        }
        public IEnumerable<t_siz_department> SIZDepartmentList
        {
            get { return context.t_siz_department; }
        }
        public IEnumerable<t_siz_goods_size> SIZSizeList
        {
            get { return context.t_siz_goods_size; }
        }
        public IEnumerable<t_siz_users> SIZUsersList
        {
            get { return context.t_siz_users; }
        }
        public IEnumerable<t_siz_goodsize> GoodSize
        {
            get { return context.t_siz_goodsize; }
        }
        public IEnumerable<t_siz_depgood> DepGood
        {
            get { return context.t_siz_depgood; }
        }
       public IEnumerable<t_siz_orders_mas> SIZOrdersMatrix
        {
            get { return context.t_siz_orders_mas; }
        }
       public IEnumerable<t_siz_orders_matrix> OrdersItems
       {
           get { return context.t_siz_orders_matrix; }
       }
       public IEnumerable<t_siz_size_type> SIZSizeType 
       {
           get
           { return context.t_siz_size_type; }
       }
        public void AddOrder(t_siz_orders order)
        {
            if (order.id == 0)
                context.t_siz_orders.Add(order);
            else
            {
                t_siz_orders dbEbtry = context.t_siz_orders.Find(order.id);
                if (dbEbtry != null)
                {
                    dbEbtry.deleted = 1;
                    if(order.deleted!=1)
                        context.t_siz_orders.Add(order);
                }
            }
            context.SaveChanges();
        }
        public void AddParam(t_siz_parameters param)
        {
            t_siz_parameters dbentry = context.t_siz_parameters.Find(param.id);
            if (dbentry != null)
            {
                dbentry.year = param.year;
                dbentry.quarter = param.quarter;
            }
            context.SaveChanges();
        }

        public void AddGood(t_siz_goods good)
        {
            if (good.id == 0)
                context.t_siz_goods.Add(good);
            else
            {
                t_siz_goods dbEbtry = context.t_siz_goods.Find(good.id);
                if (dbEbtry != null)
                {
                    dbEbtry.deleted = 1;
                    if (good.deleted != 1)
                        context.t_siz_goods.Add(good);
                    context.SaveChanges();
                    foreach (var dg in context.t_siz_depgood)
                    {
                        if (dg.good == dbEbtry.id)
                        {
                            dg.deleted = 1;
                            context.t_siz_depgood.Add(new t_siz_depgood() { deleted = 0, good = good.id, department = dg.department });
                        }
                    }
                }
            }
            context.SaveChanges();
        }
        public void AddSize(t_siz_goods_size size)
        {
            if (size.id == 0)
                context.t_siz_goods_size.Add(size);
            else
            {
                t_siz_goods_size dbEbtry = context.t_siz_goods_size.Find(size.id);
                if (dbEbtry != null)
                {
                    if (size.goods != 1)
                    {
                        dbEbtry.size = size.size;
                        dbEbtry.type = size.type;
                        dbEbtry.alternative = size.alternative;
                    }
                    else
                        context.t_siz_goods_size.Remove(dbEbtry);
                }
            }
            context.SaveChanges();
        }
        public void AddUser(t_siz_users user)
        {
            if (user.id == 0)
                context.t_siz_users.Add(user);
            else
            {
                t_siz_users dbEbtry = context.t_siz_users.Find(user.id);
                if (dbEbtry != null)
                {
                    dbEbtry.department = user.department;
                    dbEbtry.role = user.role;
                }
            }
            context.SaveChanges();
        }
        public void AddMan(t_siz_manufacturer good)
        {
 
            context.SaveChanges();
        }
        public void AddGoodSize(long id, List<t_siz_goodsize> size)
        {
            foreach (var a in context.t_siz_goodsize.Where(t => t.good == id))
                a.deleted = 1;
            context.SaveChanges();
            foreach (var p in size)
            {
                t_siz_goodsize dbEntry = context.t_siz_goodsize.SingleOrDefault(b => b.size == p.size && b.good == p.good);
                if (dbEntry != null)
                    dbEntry.deleted = 0;
                else 
                    context.t_siz_goodsize.Add(p);
            }
            context.SaveChanges();
        }
        public void AddDepGood(long id, List<t_siz_depgood> dept)
        {
            //foreach (var a in SIZDepartmentList)
            //{
            //    foreach (var b in SIZGoodsList)
            //    {
            //        if (b.deleted != 1 && b.manufacturer==3)
            //        {
            //            t_siz_depgood dg = new t_siz_depgood() { department = a.id, good = b.id, deleted = 0 };
            //            context.t_siz_depgood.Add(dg);
            //        }
            //    }
            //}
            //context.SaveChanges();

            //List<t_siz_goodsize> sizelis = new List<t_siz_goodsize>(context.t_siz_goodsize.Where(t=>t.deleted!=1));
            //foreach (var good in context.t_siz_goods.Where(p => p.date.ToString() == "2016-12-05" && p.deleted != 1))
            //{
            //    foreach(var size in context.t_siz_goods_size.Where(y=>y.type==good.sizetype))
            //    {
            //        t_siz_goodsize n = new t_siz_goodsize(){good=good.id, size=size.id,deleted=0,sign=1};
            //        bool o =false;
            //        foreach(var t in sizelis)
            //            if(t.good==n.good&&t.size==n.size)
            //                o=true;
            //        if (!o)
            //        {
            //            context.t_siz_goodsize.Add(n);
            //        }
            //    }
            //}
            //context.SaveChanges();

            foreach (var a in context.t_siz_depgood.Where(t => t.good == id))
                a.deleted = 1;
            context.SaveChanges();
            foreach (var p in dept)
            {
                t_siz_depgood dbEntry = context.t_siz_depgood.SingleOrDefault(b => b.department == p.department && b.good == p.good);
                if (dbEntry != null)
                    dbEntry.deleted = 0;
                else
                    context.t_siz_depgood.Add(p);
            }
            context.SaveChanges();
        }
        public void AddOrdersItems(long newitm, long order, long olditm=0)
        {
            if (olditm == 0)
            {
                context.t_siz_orders_matrix.Add(new t_siz_orders_matrix() { order = order, order_item = newitm });
            }
            else
            {
                foreach(var dbEntry in context.t_siz_orders_matrix.Where(p => p.order == order && p.order_item == olditm))
                {
                    dbEntry.order_item=newitm;
                }
            }
            context.SaveChanges();
        }
        
        public void MesFromUserToManager(long user, string dept)
        {
            string header = "Уведомление о закрытии заявки на СИЗ";
            string body = "Пользователь " + context.t_siz_users.Where(p => p.id == user).FirstOrDefault().name + " прислал на согласование заяку на СИЗ по подразделению " + dept + ".";
            List<string> emails = new List<string>();
            emails.Add(context.t_siz_parameters.FirstOrDefault().leader + "@goznak.ru");
            messenger mes = new messenger(emails,header,body);
            mes.Send();
        }

        public void MesFromManagerToUserAcceptUnaccept(List<string> users)
        {
            string header = "Возврат на доработку заявки на СИЗ";
            string body = "Ваша заявка на СИЗ за " + SIZParameters.quarter + " квартал " + SIZParameters.year + " года возвращена на доработку.<p/><a href=\"http://Servertest01:8080/SIZ/OrdersMatrix\">Ссылка на заполнение</a>";
            messenger mes = new messenger(users, header, body);
            mes.Send();
        }
        public void MesFromManagerToUserNotification(long dept, t_siz_parameters param)
        {
            List<string> users = new List<string>(context.t_siz_users.Where(p => p.department == dept).Select(t=>t.login+"@goznak.ru"));
            string header = "Заполнение новой заявки на СИЗ";
            string body = "Для заполнения доступна заявка на СИЗ за " + param.quarter + " квартал " + param.year + " года.<p/><a href=\"http://Servertest01:8080/SIZ/OrdersMatrix\">Ссылка на заполнение</a>";
            messenger mes = new messenger(users, header, body);
            mes.Send();
        }
        public void SendToManager(long order, long user)
        {
            t_siz_orders_mas dbEntry = context.t_siz_orders_mas.Where(p => p.id == order && p.deleted!=1).FirstOrDefault();
            dbEntry.accepted = 1;//sended
            context.SaveChanges();
            MesFromUserToManager(user, dbEntry.t_siz_department.name);
        }

        public void CreateOrdersForAll()
        {
            t_siz_parameters parameter = context.t_siz_parameters.Find(1);
            List<long> depts = new List<long>(context.t_siz_department.Select(p => p.id));
            foreach (long dept in depts)
            {
                t_siz_orders_mas newEntry = new t_siz_orders_mas() { date = System.DateTime.Now, quarter = parameter.quarter, year = parameter.year, deleted = 0, department = dept };
                context.t_siz_orders_mas.Add(newEntry);
                MesFromManagerToUserNotification(dept,parameter);
            }
            context.SaveChanges();
        }
        public void EditMatrix(t_siz_orders_mas matrix)
        {
            t_siz_orders_mas dbEntry = context.t_siz_orders_mas.Where(p => p.id == matrix.id).FirstOrDefault();
            dbEntry = matrix;
            context.SaveChanges();
            if (matrix.accepted == 2)
                MesFromManagerToUserAcceptUnaccept(context.t_siz_users.Where(p=>p.department==matrix.department).Select(t=>t.login+"@goznak.ru").ToList());
        }
        public void InitSize(t_siz_goods good, long oldid=0)
        {
            foreach (var s in context.t_siz_goodsize.Where(t => t.good == oldid))
                s.deleted = 1;
            List<long> sizes = new List<long>(context.t_siz_goods_size.Where(p => p.type == good.sizetype).Select(t => t.id));
            foreach (long t in sizes)
            {
                t_siz_goodsize gs = new t_siz_goodsize() { good = good.id, size = t, sign = 1, deleted = 0 };
                context.t_siz_goodsize.Add(gs);
            }
            context.SaveChanges();
        }
        public object GetById(long id, string type)
        {
            object result;
            switch (type)
            { 
                case "Domain.Entities.t_siz_goods":
                    result = context.t_siz_goods.Find(id);
                    break;
                case "Domain.Entities.t_siz_goods_size":
                    result = context.t_siz_goods_size.Find(id);
                    break;
                case "Domain.Entities.t_siz_parameters":
                    result = context.t_siz_parameters.Find(id);
                    break;
                case "Domain.Entities.t_siz_users":
                    result = context.t_siz_users.Find(id);
                    break;
                case "Domain.Entities.t_siz_orders":
                    result = context.t_siz_orders.Find(id);
                    break;
                case "Domain.Entities.t_siz_orders_mas":
                    result = context.t_siz_orders_mas.Find(id);
                    break;
                default:
                    result = null;
                    break;
            }
            return result;
        }
    }
}
