using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using System.DirectoryServices;

namespace SSSWebUI.Controllers
{
    public class SSSController : Controller
    {
        private ISSSystem repository;
        private List<t_Users> currentuser;
        private List<string> systems;
        public SSSController(ISSSystem repo)
        {
            repository = repo;
            //string cur = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToLower().Split('\\')[1];
            ////HttpContext.User.Identity.Name.ToString();
            //currentuser = new List<t_Users>( repository.SSSUsers.Where(p => p.login.ToLower() == cur));
            //systems = new List<string>();
            //foreach (var a in currentuser)
            //    systems.Add(a.system);
        }

        public ViewResult Print()
        {
            string cur = User.Identity.Name.Split('\\').Last<String>().ToLower();
            currentuser = new List<t_Users>(repository.SSSUsers.Where(p => p.login.ToLower() == cur));
            systems = new List<string>();
            foreach (var a in currentuser)
                systems.Add(a.system);
            ViewBag.Role = "User.css";
            return View(repository.SSSList.Where(s=>systems.Contains(s.code)));
        }
        public ActionResult CreateUser(string user)//LDAP://gz.local
        {
            DirectorySearcher dssearch = new DirectorySearcher("LDAP://gz.local");
            dssearch.Filter = "(mail=" + user + ")";
            SearchResult sresult = dssearch.FindOne();
            DirectoryEntry dsresult = sresult.GetDirectoryEntry();
            String email = dsresult.Properties["mail"][0].ToString();
            return RedirectToAction("UserList");
        }
    }
}
