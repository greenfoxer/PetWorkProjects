using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace IS_Notification.Support
{
    public class Role
    {
        bool redacting, top_manager, looking, admin;
        long id_role;

        public bool Redact { get { return redacting; } set { redacting = value; } }
        public bool Top_manager { get { return top_manager; } set { top_manager = value; } }
        public bool Looking { get { return looking; } set { looking = value; } }
        public bool Admin { get { return admin; } set { admin = value; } }
        public long Id { get { return id_role; } set { id_role = value; } }
        //public bool Date { get { return date; } set { date = value; } }
        //public bool Date_execution { get { return date_execution; } set { date_execution = value; } }
        //public bool Date_control { get { return date_control; } set { date_control = value; } }
        //public bool Card_type { get { return card_type; } set { card_type = value; } }
        //public bool N_protocol { get { return n_protocol; } set { n_protocol = value; } }
        //public bool Unit { get { return unit; } set { unit = value; } }
        //public bool Comment { get { return comment; } set { comment = value; } }
        //public bool Is_tomanager { get { return is_tomanager; } set { is_tomanager = value; } }
        //public bool Is_executed { get { return is_executed; } set { is_executed = value; } }
        //public bool Is_controlled { get { return is_controlled; } set { is_controlled = value; } }
        //public bool Phone { get { return phone; } set { phone = value; } }
        //public bool Mail { get { return mail; } set { mail = value; } }
        //public bool Department { get { return department; } set { department = value; } }
        //public bool Roleman { get { return role; } set { role = value; } }
        //public bool Domen_name { get { return domen_name; } set { domen_name = value; } }
        //public bool Fname { get { return fname; } set { fname = value; } }
        //public bool Sname { get { return sname; } set { sname = value; } }
        //public bool Mname { get { return mname; } set { mname = value; } }
        //public bool Buttons { get { return buttons; } set { buttons = value; } }
        public string Role_name { get; set; }
        public long Current_department { get; set; }
        public Role()
        { }
        public Role(object st)
        {
            if (st != null)
            {

                if (((Model.Person)st).Role == "t_manager")
                {
                    //pesponsible_person = unit_manager = top_manager = date = date_execution = date_control =
                    //card_type = n_protocol = unit = comment = is_tomanager = is_executed = is_controlled =
                    //phone = mail = department = role = domen_name = fname = sname = mname = buttons = true;
                    looking = redacting = top_manager = true;
                    Role_name = "t_manager";
                }
                if (((Model.Person)st).Role == "manager")
                {
                    //pesponsible_person = unit_manager = date = date_execution = date_control =
                    //card_type = n_protocol = unit = comment = is_tomanager = is_executed = is_controlled = buttons = true;
                    //top_manager = phone = mail = department = role = domen_name = fname = sname = mname = false;
                    looking = redacting = true; top_manager = false;
                    Role_name = "manager";
                }
                Current_department = ((Model.Person)st).Department;
            }
            else
            {
                //pesponsible_person = unit_manager = top_manager = date = date_execution = date_control =
                //card_type = n_protocol = unit = comment = is_tomanager = is_executed = is_controlled =
                //phone = mail = department = role = domen_name = fname = sname = mname = buttons = false;
                looking = true;
                redacting = top_manager = false;
                Role_name = "guest";
                Current_department = 0;
            }

        }
        public Role(SqlDataReader rd)
        {
            id_role = (long)rd["id_role"];
            top_manager = (0!=(int)rd["top_manager"]);
            redacting = (0 != (int)rd["redact"]);
            looking = (0 != (int)rd["looking"]);
            admin = (0 != (int)rd["admin"]);
            //date = (0 != (int)rd["date"]);
            //date_execution = (0 != (int)rd["date_execution"]);
            //date_control = (0 != (int)rd["date_control"]);
            //card_type = (0 != (int)rd["card_type"]);
            //n_protocol = (0 != (int)rd["n_protocol"]);
            //unit = (0 != (int)rd["unit"]);
            //comment = (0 != (int)rd["comment"]);
            //is_tomanager = (0 != (int)rd["is_tomanager"]);
            //is_executed = (0 != (int)rd["is_executed"]);
            //is_controlled = (0 != (int)rd["is_controlled"]);
            //phone = (0 != (int)rd["phone"]);
            //mail = (0 != (int)rd["mail"]);
            //department = (0 != (int)rd["department"]);
            //role = (0 != (int)rd["role"]);
            //domen_name = (0 != (int)rd["domen_name"]);
            //fname = (0 != (int)rd["fname"]);
            //sname = (0 != (int)rd["sname"]);
            //mname = (0 != (int)rd["mname"]);
            //buttons = (0 != (int)rd["buttons"]);
            Role_name = (string)rd["Role_name"];
        }

    }
}
