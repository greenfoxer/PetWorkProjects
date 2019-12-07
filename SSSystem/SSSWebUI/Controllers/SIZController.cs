using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.DirectoryServices;
using Domain.Abstract;
using Domain.Entities;

namespace SSSWebUI.Controllers
{
    public class SIZController : Controller
    {
        private ISIZSystem repository;
        private t_siz_users currentuser;
        t_siz_parameters paramet;
        private string style;
        public SIZController(ISIZSystem repo)
        {
            repository = repo;
            quarters = new List<string>()   { "1 и 2 квартал", "3 и 4 квартал"};
            //string cur = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToLower().Split('\\')[1];
            //currentuser = repository.SIZUsersList.Where(p => p.login.ToLower() == cur).FirstOrDefault();
            //paramet = repository.SIZParameters;
            //switch(currentuser.role)
            //{
            //    case "Администратор": style = "Admin.css"; break;
            //    case "Руководитель": style = "Manager.css"; break;
            //    case "Пользователь": style = "User.css"; break;
            //    default: style="User.css"; break;
            //}
            //if (paramet.admin.ToLower() == currentuser.login.ToLower() || paramet.admin2.ToLower() == currentuser.login.ToLower())
            //    style = "Admin.css";
        }
        public t_siz_users GetUserAu()
        {
            string cur = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToLower().Split('\\')[1];
            //cur = "GRADSKIY_R_I".ToLower();
            t_siz_users cuser = repository.SIZUsersList.Where(p => p.login.ToLower() == cur).FirstOrDefault();
            paramet = repository.SIZParameters;
            switch (cuser.role)
            {
                case "Администратор": style = "Admin.css"; break;
                case "Руководитель": style = "Manager.css"; break;
                case "Пользователь": style = "User.css"; break;
                default: style = "User.css"; break;
            }
            if (paramet.admin.ToLower() == cuser.login.ToLower() || paramet.admin2.ToLower() == cuser.login.ToLower())
                style = "Admin.css";
            return cuser;
        }
        /// //////////////////////////////////////////////////////////////////////////////////////
        #region Заявки
        public ViewResult Orders()
        {
            currentuser = GetUserAu();
            ViewBag.POLZ = currentuser.login;
            ViewBag.Role = style;
            int selectIndex = 1;
            SelectList Goods = new SelectList(repository.SIZGoodsList, "id", "name", selectIndex);
            ViewBag.Goods = Goods;
            SelectList Sizes = new SelectList(repository.SIZSizeList, "id", "size", selectIndex);
            ViewBag.Sizes = Sizes;
            ViewBag.User = currentuser.name;
            if (style == "User.css")
                return View(repository.SIZOrdersList.Where(t => t.deleted != 1).Where(p => p.department == currentuser.department));
            else
                return View(repository.SIZOrdersList.Where(t => t.deleted != 1));
        }
        public ActionResult CreateOrder()
        {
            currentuser = GetUserAu();

            if (currentuser.department == null)
                return PartialView("Wrong");
            int selectIndex = 0;
            List<t_siz_goods> goods = new List<t_siz_goods>(repository.SIZGoodsList.Where(p=>p.deleted!=1));
            //goods.Add(new t_siz_goods() { id = 0, name = string.Empty });
            SelectList Goods = new SelectList(goods, "id", "name");
            ViewBag.Goods = Goods;
            List<long> lgs = repository.GoodSize.Where(s => s.good == selectIndex).Select(p => p.size).ToList();
            List<t_siz_goods_size> size = new List<t_siz_goods_size>(repository.SIZSizeList.Where(p => lgs.Contains(p.id)));
            //size.Add(new t_siz_goods_size() { id = 0, size = string.Empty });
            SelectList Sizes = new SelectList(size, "id", "size");
            ViewBag.Sizes = Sizes;
            return PartialView("CreateOrder");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder(t_siz_orders order)
        {
            paramet = repository.SIZParameters;
            order.date = DateTime.Now;
            order.year = paramet.year;
            order.quarter = paramet.quarter;
            order.month = DateTime.Now.Month;
            order.department = currentuser.department;
            order.user = currentuser.id;
            //УКАЗАТЬ ОТДЕЛ   
            repository.AddOrder(order);
            return RedirectToAction("Orders");
        }
        public ActionResult DeleteOrder(long id)
        {
            return PartialView("DeleteOrder",id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        [ActionName("DeleteOrder")]
        public ActionResult DeleteCurrentOrder(long id)
        {

            t_siz_orders order = repository.SIZOrdersList.Where(t => t.id == id).First();
            order.deleted = 1;
            repository.AddOrder(order);
            return RedirectToAction("Orders");
        }
        public ActionResult EditOrder(long id)
        {
            currentuser = GetUserAu();

            t_siz_orders current = repository.GetById(id, typeof(t_siz_orders).ToString()) as t_siz_orders;
            if (current != null&&currentuser.department!=null)
            {
                int selectIndex = 1;
                SelectList Goods = new SelectList(repository.SIZGoodsList.Where(p=>p.deleted!=1), "id", "name", current.goods);
                ViewBag.Goodss = Goods;
                List<long> lgs = repository.GoodSize.Where(s => s.good == current.goods).Select(p => p.size).ToList();
                List<t_siz_goods_size> sizes = new List<t_siz_goods_size>(repository.SIZSizeList.Where(p => lgs.Contains(p.id)));
                //sizes.Add(new t_siz_goods_size() { id = 0, size = string.Empty });
                SelectList Sizes = new SelectList(sizes, "id", "size");
                ViewBag.Sizes = Sizes;
                return PartialView("EditOrder", current);
            }
            return PartialView("Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOrder(t_siz_orders order)
        {
            paramet = repository.SIZParameters;
            order.date = DateTime.Now;
            order.year = paramet.year;
            order.quarter = paramet.quarter;
            order.month = DateTime.Now.Month;
            order.department = currentuser.department;
            order.user = currentuser.id;
            repository.AddOrder(order);
            return RedirectToAction("Orders");
        }
        #endregion
        /// //////////////////////////////////////////////////////////////////////////////////////
  
        /// //////////////////////////////////////////////////////////////////////////////////////
        #region Работа с СИЗ
        public ViewResult Print()
        {
            return View(repository.SIZGoodsList.Where(p=>p.deleted!=1));
        }
        public ActionResult CreateGood()
        {
            int selectIndex = 1;
            SelectList Goods = new SelectList(repository.SIZGoodsList, "id", "name", selectIndex);
            ViewBag.Goods = Goods;
            SelectList Mans = new SelectList(repository.SIZManList, "id", "name", selectIndex);
            ViewBag.Mans = Mans;
            return PartialView("CreateGood");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGood(t_siz_goods good)
        {
            good.author = currentuser.login;
            good.date = DateTime.Now;
            //УКАЗАТЬ ОТДЕЛ   
            repository.AddGood(good);
            return RedirectToAction("Print");
        }
        public ActionResult EditGood(long id)
        {
            t_siz_goods current = repository.GetById(id, typeof(t_siz_goods).ToString()) as t_siz_goods;
            if (current != null)
            {
                int selectIndex = 1;
                SelectList Goods = new SelectList(repository.SIZGoodsList, "id", "name", selectIndex);
                ViewBag.Goods = Goods;
                SelectList Mans = new SelectList(repository.SIZManList, "id", "name", selectIndex);
                ViewBag.Mans = Mans;
                return PartialView("EditGood", current);
            }
            else
                return (PartialView("Error"));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGood(t_siz_goods good)
        {
            good.author = currentuser.login;
            good.date = DateTime.Now; 
            repository.AddGood(good);
            return RedirectToAction("Print");
        }
        public ActionResult DeleteGood(long id)
        {
            return (PartialView("DeleteGood", id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteGood")]
        public ActionResult DeleteCurrentGood(long id)
        {
            t_siz_goods current = repository.GetById(id, typeof(t_siz_goods).ToString()) as t_siz_goods;
            if (current != null)
            {
                current.author = currentuser.login;
                current.date = DateTime.Now;
                current.deleted = 1;
                //УКАЗАТЬ ОТДЕЛ   
                repository.AddGood(current);
                return RedirectToAction("Print");
            }
            else
            {
                return (PartialView("Error"));
            }
        }
        public ActionResult ListMan()
        {
            return (PartialView("ListMan", repository.SIZManList));
        }
        public ActionResult CreateMan()
        {
            return PartialView("CreateGood");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMan(t_siz_manufacturer man)
        {
            repository.AddMan(man);
            return RedirectToAction("Print");
        }
        #endregion
        /// //////////////////////////////////////////////////////////////////////////////////////

        /// //////////////////////////////////////////////////////////////////////////////////////
        #region Таблица параметров
        public List<string> quarters;
        public ActionResult EditParam()
        {
            t_siz_parameters current = repository.GetById(1, typeof(t_siz_parameters).ToString()) as t_siz_parameters;
            if (current != null)
            {
                int selectIndex = 1;
                SelectList quart = new SelectList(quarters,selectIndex);
                ViewBag.Quart = quart;
                return PartialView("EditParam", current);
            }
            else
                return (PartialView("Error"));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditParam(t_siz_parameters param)
        {
            repository.AddParam(param);
            return RedirectToAction("Orders");
        }
#endregion
        /// //////////////////////////////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////////////////////////////////
        #region Размеры
        public ViewResult SizeList()
        {
            return View(repository.SIZSizeList);
        }
        public ActionResult AdaptiveSize(long id)
        {
            List<long> lgs = repository.GoodSize.Where(s=>s.good==id).Select(p=>p.size).ToList();
            List<t_siz_goods_size> sizes = new List<t_siz_goods_size>(repository.SIZSizeList.Where(p => lgs.Contains(p.id)));
            //sizes.Add(new t_siz_goods_size() { id = 0, size = string.Empty });
            SelectList Sizes = new SelectList(sizes , "id", "size");
            ViewBag.Sizes = Sizes;
            return View(sizes);
        }
        public ActionResult EditSize(long id)
        {
            t_siz_goods_size current = repository.GetById(id, typeof(t_siz_goods_size).ToString()) as t_siz_goods_size;
            if (current != null)
            {
                return PartialView("EditSize", current);
            }
            else
                return (PartialView("Error"));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSize(t_siz_goods_size good)
        {
            repository.AddSize(good);
            return RedirectToAction("SizeList");
        }
        public ActionResult DeleteSize(long id)
        {
            return (PartialView("DeleteSize", id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteSize")]
        public ActionResult DeleteCurrentSize(long id)
        {
            t_siz_goods_size current = repository.GetById(id, typeof(t_siz_goods_size).ToString()) as t_siz_goods_size;
            if (current != null)
            {
                repository.AddSize(current);
                return RedirectToAction("SizeList");
            }
            else
            {
                return (PartialView("Error"));
            }
        }
        public ActionResult CreateSize()
        {
            return PartialView("CreateSize");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSize(t_siz_goods_size size)
        {
            repository.AddSize(size);
            return RedirectToAction("SizeList");
        }
        public ActionResult NewSize()
        {
            return PartialView("NewSize");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewSize(t_siz_goods_size goodsize)
        {
            if (goodsize != null)
            {
                repository.AddSize(goodsize);
                return RedirectToAction("SizeList");
            }
            else
                return PartialView("Error");

        }
        public ActionResult AddSizeTable(long id)
        {
            var res = repository.SIZSizeList;
            var res2 = repository.GoodSize.Where(t => t.good == id).Select(p=>p.size).ToList();
            ViewBag.Res = res;
            ViewBag.Res2 = res2;
            t_siz_goods res3 = repository.GetById(id, typeof(t_siz_goods).ToString()) as t_siz_goods;
            return PartialView("AddSizeTable",res3);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSizeTable(t_siz_goods good, long[] selectedSizes)
        {
            if (selectedSizes != null)
            {
                List<t_siz_goodsize> sizes = new List<t_siz_goodsize>();
                foreach (var p in selectedSizes)
                    sizes.Add(new t_siz_goodsize() { good = good.id, size = p, sign=1 });
                repository.AddGoodSize(good.id, sizes);

            }
            return RedirectToAction("Print");
        

        }
        #endregion
        /// //////////////////////////////////////////////////////////////////////////////////////

        /// //////////////////////////////////////////////////////////////////////////////////////
        #region Пользователи
        public ViewResult UserList()
        { 
            return View(repository.SIZUsersList);
        }
        public ActionResult EditUser(long id)
        {
            currentuser = GetUserAu();
            t_siz_users current = repository.GetById(id, typeof(t_siz_users).ToString()) as t_siz_users;
            if (current != null)
            {
                List<t_siz_department> dept = repository.SIZDepartmentList.ToList();
                dept.Add(new t_siz_department(){id=0,name=String.Empty});
                ViewBag.Dept = new SelectList(dept, "id", "name", current.department!=null?current.department:0);
                List<string> roles;
                if(style=="Admin.css")
                    roles = new List<string>() { "Администратор", "Руководитель", "Пользователь", string.Empty };
                else
                    roles = new List<string>() { "Руководитель", "Пользователь", string.Empty };
                ViewBag.Roles = new SelectList(roles, current.role != null ? current.role : string.Empty);
                return PartialView("EditUser", current);
            }
            else
                return (PartialView("Error"));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(t_siz_users good)
        {
            if (good.department == 0)
                good.department = null;
            repository.AddUser(good);
            return RedirectToAction("UserList");
        }
        public ActionResult AddUser()
        {
                ViewBag.Dept = new SelectList(repository.SIZDepartmentList, "id", "name", 1);
                List<string> roles;
                if (style == "Admin.css")
                    roles = new List<string>() { "Администратор", "Руководитель", "Пользователь", string.Empty };
                else
                    roles = new List<string>() { "Руководитель", "Пользователь", string.Empty };
                ViewBag.Roles = new SelectList(roles);
                return PartialView("AddUser");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(string user)
        {
            DirectorySearcher dssearch = new DirectorySearcher("LDAP://gz.local");
            dssearch.Filter = "(mail=" + user + ")";
            SearchResult sresult = dssearch.FindOne();
            if (sresult != null)
            {
                DirectoryEntry dsresult = sresult.GetDirectoryEntry();
                String email = dsresult.Properties["mail"][0].ToString().Split('@')[0];
                t_siz_users usera = new t_siz_users() { login = email, system = "SIZ", name = dsresult.Properties["displayName"][0].ToString() };
                repository.AddUser(usera);
            }
            else
                return PartialView("Error");
            return RedirectToAction("UserList");
        }
        #endregion
        /// //////////////////////////////////////////////////////////////////////////////////////
    }
}
