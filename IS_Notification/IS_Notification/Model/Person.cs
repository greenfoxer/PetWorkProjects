using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace IS_Notification.Model
{
    public class Person
    {
        #region Parameters
        long id, department;
        string phone, mail, role, domen_name, fname, sname, mname;
        public long Id
        { get { return id; } set { } }
        public string Name
        {
            get { return sname + " " + fname + " " + mname; }
        }
        public string Phone
        { get { return phone; } set { phone = value; } }
        public string Mail
        { get { return mail+"@goznak.ru"; } set { mail = value; } }
        public long Department
        { get { return department; } set { department = value; } }
        public string Role
        { get { return role; } set { role = value; } }
        public string Domen_name
        { get { return domen_name; } set { domen_name = value; } }
        
        public string Fname
        {
            get { return fname; }
            set { fname = value; }
        }
        public string Sname
        {
            get { return sname; }
            set { sname = value; }
        }
        public string Mname
        {
            get { return mname; }
            set { mname = value; }
        }
        #endregion
        public Person(SqlDataReader rd)
        {
            id = (long)rd["id_person"];
            fname = (string)rd["fname"];
            mname = (string)rd["mname"];
            sname = (string)rd["sname"];
            department = (long)rd["department"];
            role = (string)rd["role"];
            domen_name = (string)rd["domen_name"];
            phone = (string)rd["phone"];
            mail = (string)rd["mail"];
        }
        public Person()
        { role = "guest"; }
        public Person(Person p)
        {
            id = p.id;
            fname = p.fname ;
            mname = p.mname;
            sname = p.sname;
            department = p.department;
            role = p.role;
            domen_name = p.domen_name;
            phone = p.phone;
            mail = p.mail;
        }
        public bool is_equal(Person p)
        {
            if (p == null)
            {
                if (id == 0 && fname == null && mname == null && sname == null && department == 0 
                    && domen_name == null && phone == null && mail == null)
                    return true;
            }
            else
            {
                if (id == p.id && fname == p.fname && mname == p.mname && sname == p.sname && department == p.department &&
                role == p.role && domen_name == p.domen_name && phone == p.phone && mail == p.mail)
                    return true;
            }
            return false;
        }
    }
}
