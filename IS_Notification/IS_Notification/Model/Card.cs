using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace IS_Notification.Model
{
    public class Card
    {
        long id, pesponsible_person, unit, unit_manager, top_manager;
        DateTime date, date_execution, date_control;
        string card_type, n_protocol,  comment, unit_name;
        int is_tomanager, is_executed, is_controlled; int? is_startrecieve;
        public ObservableCollection<File> file_list;
        public int Year { get { return date.Year; } private set { } }
        public long notificate_top{get;set;}
        #region properties
        public string Is_wrong
        {
            get 
            {
                DateTime controldate = DateTime.Today;
                if (((date_execution < controldate) && (!Convert.ToBoolean(is_executed))) || ((date_control < controldate) && (!Convert.ToBoolean(is_controlled))))
                    return "Просрочен";
                return "Не просрочен";
            }
        }
        public struct Stat
        {
            string exec;
            string cntrl;
            public string Exec { get { return exec; } set { exec = value; } }
            public string Cntrl { get { return cntrl; } set { cntrl = value; } }
            public Stat(string e, string c)
            {
                exec = e;
                cntrl = c;
            }
        }
        public Stat stat
        { 
            get 
            {
                if (card_type != "Акт")
                    return new Stat("Выполнение " + card_type, "Оценка результативности " + card_type);
                else
                    return new Stat("1-й срок контроля", "2-й срок контроля");
            }
        }
        public int Is_totopmanager { get; set; }
        public string Comment
        { get { return comment; } set { comment = value; } }
        public struct File
        {
            string sname;
            string fname;
            public string Sname { get { return sname; } set { sname = value; } }
            public string Fname { get { return fname; } set { fname = value; } }
            public File(string s, string f)
            {
                sname = s;
                fname = f;
            }
            public File(SqlDataReader rd)
            { 
                sname=(string)(rd["short"]);
                fname = (string)(rd["path"]);
            }
        }
        public ObservableCollection<File> File_list
        { get { return file_list; } set { file_list = value;} }
        public string Unit_name
        { get { return unit_name; } set { this.unit_name = value; } }
        public string  Card_type
        { get { return card_type; } set { this.card_type = value; } }
        public string N_protocol
        { get { return n_protocol; } set { this.n_protocol = value; } }
        public long Unit
        { get { return unit; } set { this.unit = value; } }
        public long Responsible_person
        { get { return pesponsible_person; } set { this.pesponsible_person = value; } }
        public long Unit_manager
        { get { return unit_manager; } set { this.unit_manager = value; } }
        public long Top_manager
        { get { return top_manager; } set { this.top_manager = value; } }
        public string Date_executions
        { get { return String.Format("{0:dd/MM/yyyy}", date_execution); }  }
        public string Date_controls
        { get { return String.Format("{0:dd/MM/yyyy}", date_control); }}
        public string Dates
        { get { return String.Format("{0:dd/MM/yyyy}", date); }  }
        public DateTime Date_execution
        { get { return date_execution; } set { this.date_execution = value; } }
        public DateTime Date_control
        { get { return date_control; } set { this.date_control = value; } }
        public DateTime Date
        { get { return date; } set { this.date = value; } }
        public int Is_tomanager
        { get { return is_tomanager; } set { this.is_tomanager = value; } }
        public int Is_executed
        { get { return is_executed; } set { this.is_executed = value; } }
        public int Is_controlled
        { get { return is_controlled; } set{ this.is_controlled = value; } }
        public long Id
        { get { return id; } set { this.id = value; } }
        public bool Is_top
        { get { return top_manager != 0; } set { return; } }
        public string Status_execution
        { get; set; }
        public string Status_control
        { get; set; }
        #endregion
        public Card() 
        {
            date = DateTime.Now;
            date_control = DateTime.Now;
            date_execution = DateTime.Now;
            is_startrecieve = 0;
            is_tomanager = 0;
            comment = "";
            notificate_top = 0;
            Is_totopmanager = 0;
            file_list = new ObservableCollection<File>();
        }
        public Card(
            long pesponsible_person, long unit_manager, long top_manager,
            DateTime date, DateTime date_execution, DateTime date_control,
            string card_type, string n_protocol, long unit, string comment,
            int is_tomanager, int is_executed, int is_controlled, int is_startrecieve)
        {
            this.card_type = card_type;
            this.n_protocol = n_protocol;
            this.unit = unit;
            this.is_tomanager = is_tomanager;
            this.pesponsible_person = pesponsible_person;
            this.unit_manager = unit_manager;
            this.top_manager = top_manager;
            this.date_execution = date_execution;
            this.is_executed = is_executed;
            this.date_control = date_control;
            this.is_controlled = is_controlled;
            this.comment = comment;
            this.date = date;
            this.Is_totopmanager = Is_totopmanager;
            this.is_startrecieve = is_startrecieve;
            notificate_top = top_manager;
            file_list = new ObservableCollection<File>();
        }
        public Card(SqlDataReader rd)
        {
            id = (long)rd["id"];
            card_type = (string)rd["card_type"];
            n_protocol = (string)rd["n_protocol"];
            unit = (long)rd["unit"];
            is_tomanager = (int)rd["is_tomanager"];
            pesponsible_person = (long)rd["responsible_person"];
            unit_manager = (long)rd["unit_manager"];
            top_manager = (long)rd["top_manager"];
            date_execution = (DateTime)rd["date_execution"];
            is_executed = (int)rd["is_executed"];
            date_control = (DateTime)rd["date_control"];
            is_controlled = (int)rd["is_controlled"];
            comment = (string)rd["comment"];
            date = (DateTime)rd["date"];
            unit_name = (string)rd["name"];
            is_startrecieve = (int)rd["is_startrecieve"];
            Is_totopmanager = (int)rd["is_totopmanager"];
            notificate_top = top_manager;
            File_list = ((App)App.Current).all_data.get_files(id);
            DateTime controldate = DateTime.Today;
            if ((date_execution >= controldate))
            {
                if (!Convert.ToBoolean(is_executed))
                    Status_execution = "На контроле";
                else
                    Status_execution = "Выполнено";
            }
            else
            {
                if (!Convert.ToBoolean(is_executed))
                    Status_execution = "Просрочено";
                else
                    Status_execution = "Выполнено";
            }
            if (date_control >= controldate)
            {
                if (!Convert.ToBoolean(is_controlled))
                    Status_control = "На контроле";
                else
                    Status_control = "Выполнено";
            }
            else
            {
                if (!Convert.ToBoolean(is_controlled))
                    Status_control = "Просрочено";
                else
                    Status_control = "Выполнено";
            }
           
        }
        public Card(Card c)
        {
            copy(c);    
        }
        public void copy(Card c)
        {
            id = c.id;
            card_type = c.card_type;
            n_protocol = c.n_protocol;
            unit = c.unit;
            is_tomanager = c.is_tomanager;
            pesponsible_person = c.pesponsible_person;
            unit_manager = c.unit_manager;
            top_manager = c.top_manager;
            date_execution = c.date_execution;
            is_executed = c.is_executed;
            date_control = c.date_control;
            is_controlled = c.is_controlled;
            comment = c.comment;
            date = c.date;
            unit_name = c.unit_name;
            is_startrecieve = c.is_startrecieve;
            Is_totopmanager = c.Is_totopmanager;
            notificate_top = c.notificate_top;
            File_list = c.File_list;
            DateTime controldate = DateTime.Today;
            if ((date_execution >= controldate))
            {
                if (!Convert.ToBoolean(is_executed))
                    Status_execution = "На контроле";
                else
                    Status_execution = "Выполнено";
            }
            else
            {
                if (!Convert.ToBoolean(is_executed))
                    Status_execution = "Просрочено";
                else
                    Status_execution = "Выполнено";
            }
            if (date_control >= controldate)
            {
                if (!Convert.ToBoolean(is_controlled))
                    Status_control = "На контроле";
                else
                    Status_control = "Выполнено";
            }
            else
            {
                if (!Convert.ToBoolean(is_controlled))
                    Status_control = "Просрочено";
                else
                    Status_control = "Выполнено";
            }
        }
        public void add()
        {
            
        }
    }
}
