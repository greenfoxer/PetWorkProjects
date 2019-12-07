using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace IS_Notification.Model
{
    public class Agent
    {
        long id, boss;
        string name;
        public long Id { get { return id; } set { id = value; } }
        public long Boss { get { return boss; } set { boss = value; } }
        public string Name { get { return name; } set { name = value; } }
        public Agent()
        {
            name = "--";
        }
        public bool is_equal(Agent a)
        {
            if (a != null)
            {
                if (this.id != a.id || this.name != a.name || this.boss != a.boss)
                    return false;
            }
            else
            {
                if (this.id != 0 || this.name != "--" || this.boss != 0)
                    return false;
            }
            return true;
        }
        public Agent(SqlDataReader rd)
        {
            id = (long)rd["id_unit"];
            boss = (long)rd["boss"];
            name = (string)rd["name"];
        }
        public Agent(Agent a)
        {
            this.id = a.id;
            this.name = a.name;
            this.boss = a.boss;
        }
    }
}
