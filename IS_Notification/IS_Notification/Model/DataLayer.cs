using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.IO;

namespace IS_Notification.Model
{
    public class DataLayer
    {
        public ObservableCollection<Card> get_cards()
        {
            ObservableCollection<Card> cards = new ObservableCollection<Card>();
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                if (((App)App.Current).the_person.Role!="worker")
                    cmd.CommandText = "[dbo].[LoadCards]";
                else
                {
                    cmd.CommandText = "[dbo].[LoadCardsForWorker]";
                    cmd.Parameters.Add(new SqlParameter("@department", ((App)App.Current).the_person.Department) { Direction = ParameterDirection.Input });
                }
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cards.Add(new Card(reader));
                }
                connection.Close();
             }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            return cards;
        }
        public ObservableCollection<Card> get_deleted_cards()
        {
            ObservableCollection<Card> cards = new ObservableCollection<Card>();
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[LoadDeletedCards]";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cards.Add(new Card(reader));
                }
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            return cards;
        }
        public void restore_card(long id)
        {
            ObservableCollection<Card> cards = new ObservableCollection<Card>();
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[RestoreCard]";
                cmd.Parameters.Add(new SqlParameter("@id", id) { Direction = ParameterDirection.Input });
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
        }
        public ObservableCollection<Agent> get_units()
        {
            ObservableCollection<Agent> cards = new ObservableCollection<Agent>();
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[LoadUnits]";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cards.Add(new Agent(reader));
                }
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            return cards;
        }
        public ObservableCollection<Agent> get_deleted_units()
        {
            ObservableCollection<Agent> cards = new ObservableCollection<Agent>();
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[LoadDeletedUnits]";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cards.Add(new Agent(reader));
                }
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            return cards;
        }
        public void restore_agent(long id)
        {
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[RestoreUnit]";
                cmd.Parameters.Add(new SqlParameter("@id", id) { Direction = ParameterDirection.Input });
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
        }
        public ObservableCollection<IS_Notification.Support.Role> get_roles()
        {
            ObservableCollection<IS_Notification.Support.Role> cards = new ObservableCollection<IS_Notification.Support.Role>();
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[LoadRoles]";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cards.Add(new IS_Notification.Support.Role(reader));
                }
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            return cards;
        }
        public DataTable get_statistic()
        {
            DataTable cards = new DataTable();
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[LoadStatistic]";
                SqlDataReader reader = cmd.ExecuteReader();
                cards.Load(reader);
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            return cards;
        }
        public IS_Notification.Support.Role get_my_role(Person st)
        {
            IS_Notification.Support.Role rl = new Support.Role();
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[LoadMyRole]";
                cmd.Parameters.Add(new SqlParameter("@role", st.Role) { Direction = ParameterDirection.Input });
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rl = new IS_Notification.Support.Role(reader);
                }
                rl.Current_department = ((Model.Person)st).Department;
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            return rl;
        }
        public void apply_roles(ObservableCollection<Support.Role> roles)
        { 
        
        }
        public ObservableCollection<Model.Card.File> get_files(long id)
        {
            ObservableCollection<Model.Card.File> cards = new ObservableCollection<Model.Card.File>();
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[LoadPaths]";
                cmd.Parameters.Add(new SqlParameter("@card_id", id) { Direction = ParameterDirection.Input });
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cards.Add(new Model.Card.File(reader));
                }
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            return cards;
        }
        public DataTable get_deleted_files()
        {
            DataTable cards = new DataTable();
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[LoadDeletedPaths]";
                SqlDataReader reader = cmd.ExecuteReader();
                cards.Load(reader);
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            return cards;
        }
        public void restore_person(long id)
        {
            ObservableCollection<Card> cards = new ObservableCollection<Card>();
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[RestorePerson]";
                cmd.Parameters.Add(new SqlParameter("@id", id) { Direction = ParameterDirection.Input });
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
        }
        public void delete_files(long id,string f, string s)
        {
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[DeletePaths]";

                cmd.Parameters.Add(new SqlParameter("@id", id) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@fname", f) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@sname", s) { Direction = ParameterDirection.Input });
                //cmd.Parameters.Add(new SqlParameter("@status_code",100) { Direction=ParameterDirection.Input});
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
        }
        public void delete_file(long id, string n)
        {
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[DeletePath]";

                cmd.Parameters.Add(new SqlParameter("@id", id) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@path", n) { Direction = ParameterDirection.Input });
                //cmd.Parameters.Add(new SqlParameter("@status_code",100) { Direction=ParameterDirection.Input});
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
        }
        public ObservableCollection<Person> get_person()
        {
            ObservableCollection<Person> persons = new ObservableCollection<Person>();
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[LoadManagers]";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    persons.Add(new Person(reader));
                }
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            return persons;
        }
        public ObservableCollection<Person> get_deleted_person()
        {
            ObservableCollection<Person> persons = new ObservableCollection<Person>();
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[LoadDeletedManagers]";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    persons.Add(new Person(reader));
                }
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            return persons;
        }
        public Person validate(string domen_name)
        {
            ObservableCollection<Person> p = get_person();
            foreach (var i in p)
                if (i.Domen_name.ToUpper() == domen_name.ToUpper())
                    return i;
            return null;
        }
        public ObservableCollection<Person> get_person(string str)
        {
            ObservableCollection<Person> persons = new ObservableCollection<Person>();
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[LoadTopManagers]";
                cmd.Parameters.Add(new SqlParameter("@str", str) { Direction = ParameterDirection.Input });
                
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    persons.Add(new Person(reader));
                }
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            return persons;
        }
        void receive_mail(long id)
        {
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[ReceivingStartMail]";
                cmd.Parameters.Add(new SqlParameter("@id_card", id) { Direction = ParameterDirection.Input });
                cmd.ExecuteNonQuery();
                connection.Close();
                ((App)App.Current).all_data.stat_it(0, ((App)App.Current).the_person.Id, "success receiving_start_mail"); 
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); ((App)App.Current).all_data.stat_it(0, ((App)App.Current).the_person.Id, "success receiving_start_mail"); }
        }
        public void add_card(Model.Card card, long creator, List<Model.Card.File> ad)
        {
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[CreateNewCard]";
                if (card.Top_manager != 0 && card.notificate_top != card.Top_manager)
                    card.Is_totopmanager = 1;
                cmd.Parameters.Add(new SqlParameter("@card_type",card.Card_type) { Direction=ParameterDirection.Input});
                cmd.Parameters.Add(new SqlParameter("@n_protocol",card.N_protocol) { Direction=ParameterDirection.Input});
                cmd.Parameters.Add(new SqlParameter("@date",card.Date) { Direction=ParameterDirection.Input});
                cmd.Parameters.Add(new SqlParameter("@unit",card.Unit) { Direction=ParameterDirection.Input});
                cmd.Parameters.Add(new SqlParameter("@responsible_person",card.Responsible_person) { Direction=ParameterDirection.Input});
                cmd.Parameters.Add(new SqlParameter("@is_tomanager",card.Is_tomanager) { Direction=ParameterDirection.Input});
                cmd.Parameters.Add(new SqlParameter("@unit_manager",card.Unit_manager) { Direction=ParameterDirection.Input});
                cmd.Parameters.Add(new SqlParameter("@top_manager",card.Top_manager) { Direction=ParameterDirection.Input});
	            cmd.Parameters.Add(new SqlParameter("@date_execution",card.Date_execution) { Direction=ParameterDirection.Input});
                cmd.Parameters.Add(new SqlParameter("@is_executed",card.Is_executed) { Direction=ParameterDirection.Input});
                cmd.Parameters.Add(new SqlParameter("@date_control",card.Date_control) { Direction=ParameterDirection.Input});
                cmd.Parameters.Add(new SqlParameter("@is_controlled",card.Is_controlled) { Direction=ParameterDirection.Input});
                cmd.Parameters.Add(new SqlParameter("@is_totopmanager", card.Is_totopmanager) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@comment",card.Comment) { Direction=ParameterDirection.Input});
                cmd.Parameters.Add(new SqlParameter("@creator",creator) { Direction=ParameterDirection.Input});
                cmd.Parameters.Add(new SqlParameter("@id",card.Id) { Direction = ParameterDirection.Output });
                cmd.Parameters.Add(new SqlParameter("@is_startrecieve", '0') { Direction = ParameterDirection.Input});
                //cmd.Parameters.Add(new SqlParameter("@status_code",100) { Direction=ParameterDirection.Input});

                cmd.ExecuteNonQuery();
                card.Id = Convert.ToInt64(cmd.Parameters["@id"].Value);
                if(ad.Count!=0)
                    load_files(card.Id, card.File_list, ad);
                ((App)App.Current).all_data.stat_it(card.Id, ((App)App.Current).the_person.Id, "success add_card");
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); ((App)App.Current).all_data.stat_it(card.Id, ((App)App.Current).the_person.Id, "fail add_card"); }
           // receive_mail(card.Id);
        }
        public ObservableCollection<string> load_files(long id, ObservableCollection<Model.Card.File> paths, List<Model.Card.File> ad)
        {
            ObservableCollection<string> cards = new ObservableCollection<string>();
            FileStream fStream;
            byte[] contents;
            if (paths.Count!=0)
            try
            {
       
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[SavePaths]";
                cmd.Parameters.Add(new SqlParameter("@id", id) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter());
                cmd.Parameters.Add(new SqlParameter());
                cmd.Parameters.Add(new SqlParameter());
                foreach (var p in ad)
                {
                    cmd.Parameters[1]= new SqlParameter("@path", p.Fname) { Direction = ParameterDirection.Input};
                    cmd.Parameters[2] = new SqlParameter("@short", p.Sname) { Direction = ParameterDirection.Input };
                    fStream = File.OpenRead(p.Fname);
                    contents = new byte[fStream.Length];
                    fStream.Read(contents, 0, (int)fStream.Length);
                    fStream.Close();
                    cmd.Parameters[3] =new SqlParameter("@file", contents);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            return cards;
        }
        public void edit_card(Model.Card card, List<Model.Card.File> delet, List<Model.Card.File> ad)
        {
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[EditCard]";
                if (card.Top_manager != 0 && card.notificate_top != card.Top_manager)
                    card.Is_totopmanager = 1;
                cmd.Parameters.Add(new SqlParameter("@card_type", card.Card_type) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@n_protocol", card.N_protocol) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@date", card.Date) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@unit", card.Unit) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@responsible_person", card.Responsible_person) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@is_tomanager", card.Is_tomanager) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@unit_manager", card.Unit_manager) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@top_manager", card.Top_manager) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@date_execution", card.Date_execution) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@is_executed", card.Is_executed) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@date_control", card.Date_control) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@is_controlled", card.Is_controlled) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@is_totopmanager", card.Is_totopmanager) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@comment", card.Comment) { Direction = ParameterDirection.Input });
                //cmd.Parameters.Add(new SqlParameter("@creator", "0") { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@id", card.Id) { Direction = ParameterDirection.Input });
                //cmd.Parameters.Add(new SqlParameter("@status_code",100) { Direction=ParameterDirection.Input});

                cmd.ExecuteNonQuery();
                foreach(var f in delet)
                    delete_files(card.Id,f.Fname,f.Sname);
                load_files(card.Id, card.File_list, ad);
                
                ((App)App.Current).all_data.stat_it(card.Id, ((App)App.Current).the_person.Id, "success edit_card");
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); ((App)App.Current).all_data.stat_it(card.Id, ((App)App.Current).the_person.Id, "fail edit_card"); }
        }
        void receive_mail_to_top(long id)
        {
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[ReceivingMailTop]";
                //cmd.Parameters.Add(new SqlParameter("@id_card", id) { Direction = ParameterDirection.Input });
                cmd.ExecuteNonQuery();
                ((App)App.Current).all_data.stat_it(id, ((App)App.Current).the_person.Id, "success receive_mail_top");
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); ((App)App.Current).all_data.stat_it(id, ((App)App.Current).the_person.Id, "fsil receive_mail_top"); }
        }
        public void delete_card(Model.Card card)
        {
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[DeleteCard]";

                cmd.Parameters.Add(new SqlParameter("@id", card.Id) { Direction = ParameterDirection.Input });
                //cmd.Parameters.Add(new SqlParameter("@status_code",100) { Direction=ParameterDirection.Input});

                cmd.ExecuteNonQuery();
                ((App)App.Current).all_data.stat_it(card.Id, ((App)App.Current).the_person.Id, "success delete_person");
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); ((App)App.Current).all_data.stat_it(card.Id, ((App)App.Current).the_person.Id, "fail delete_person"); }
        }
        public void download_file(string path, long id,string savepath, bool o=false)
        {
            string conn = Properties.Settings.Default.con_str;
            using (SqlConnection cn
               = new SqlConnection(Properties.Settings.Default.con_str))
            {
                cn.Open();
                using (SqlCommand cmd
                    = new SqlCommand(/*"select [file] from dbo.[control_data_links] where uid_card=" + id + " and path='"+path+"'", cn*/))
                {
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.Parameters.Add(new SqlParameter("@path", path));
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[dbo].[GetFile]";
                    using (SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.Default))
                    {
                        if (dr.Read())
                        {
                            byte[] fileData = (byte[])dr.GetValue(0);
                            try
                            {
                                string path1 = Path.GetDirectoryName(savepath);
                                string file1 = Path.GetFileName(savepath);

                                dll_file_saver.file_saver.save_file(path1, file1, fileData);

                                //using (System.IO.FileStream fs = new System.IO.FileStream(savepath, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite))
                                //{
                                //    using (System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs))
                                //    {
                                //        bw.Write(fileData);
                                //        bw.Close();
                                //    }
                                //}
                            }
                            catch { }
                        }
                        dr.Close();
                        if (o)
                            try
                            { System.Diagnostics.Process.Start(savepath); }
                            catch
                            { MessageBox.Show("Ой! Кажется файла нет в базе данных. Проверьте, что вы сохранили свои последние изменения."); }
                    }
                }
            }
        }
        public void add_person(Model.Person card)
        {
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[CreateNewPerson]";

                cmd.Parameters.Add(new SqlParameter("@department", card.Department) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@domen_name", card.Domen_name) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@fname", card.Fname) { Direction = ParameterDirection.Input });
                //cmd.Parameters.Add(new SqlParameter("@unit", "XXXX") { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@mail", card.Mail.Split('@')[0]) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@mname", card.Mname) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@phone", card.Phone) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@role", card.Role) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@sname", card.Sname) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@id", card.Id) { Direction = ParameterDirection.Input });
                //cmd.Parameters.Add(new SqlParameter("@status_code",100) { Direction=ParameterDirection.Input};
                cmd.ExecuteNonQuery();
                ((App)App.Current).all_data.stat_it(0, ((App)App.Current).the_person.Id, "success add_person");
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); ((App)App.Current).all_data.stat_it(0, ((App)App.Current).the_person.Id, "fail add_person"); }
        }
        public void edit_person(Model.Person card)
        {
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[EditPerson]";

                cmd.Parameters.Add(new SqlParameter("@department", card.Department) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@domen_name", card.Domen_name) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@fname", card.Fname) { Direction = ParameterDirection.Input });
                //cmd.Parameters.Add(new SqlParameter("@unit", card.Department) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@mail", card.Mail.Split('@')[0]) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@mname", card.Mname) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@phone", card.Phone) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@role", card.Role) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@sname", card.Sname) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@id", card.Id) { Direction = ParameterDirection.Input });
                //cmd.Parameters.Add(new SqlParameter("@status_code",100) { Direction=ParameterDirection.Input};
                cmd.ExecuteNonQuery();
                ((App)App.Current).all_data.stat_it(card.Id, ((App)App.Current).the_person.Id, "success edit_person");
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); ((App)App.Current).all_data.stat_it(card.Id, ((App)App.Current).the_person.Id, "fail edit_person"); }
        }
        public void delete_person(Model.Person card)
        {
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[DeletePerson]";

                cmd.Parameters.Add(new SqlParameter("@id", card.Id) { Direction = ParameterDirection.Input });
                //cmd.Parameters.Add(new SqlParameter("@status_code",100) { Direction=ParameterDirection.Input});

                cmd.ExecuteNonQuery();
                ((App)App.Current).all_data.stat_it(card.Id, ((App)App.Current).the_person.Id, "success delete_person");
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); ((App)App.Current).all_data.stat_it(card.Id, ((App)App.Current).the_person.Id, "fail delete_person"); }
        }
        public ObservableCollection<Agent> get_unit()
        {
            ObservableCollection<Agent> persons = new ObservableCollection<Agent>();
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[LoadUnits]";
               
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    persons.Add(new Agent(reader));
                }
                persons.Add(new Agent());
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            return persons;
        }
        public void delete_unit(Model.Agent card)
        {
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[DeleteUnit]";

                cmd.Parameters.Add(new SqlParameter("@id", card.Id) { Direction = ParameterDirection.Input });
                //cmd.Parameters.Add(new SqlParameter("@status_code",100) { Direction=ParameterDirection.Input});

                cmd.ExecuteNonQuery();
                ((App)App.Current).all_data.stat_it(card.Id, ((App)App.Current).the_person.Id, "success delete_unit");
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); ((App)App.Current).all_data.stat_it(card.Id, ((App)App.Current).the_person.Id, "fail delete_unit"); }
        }
        public void edit_unit(Model.Agent card)
        {
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[EditUnit]";

                cmd.Parameters.Add(new SqlParameter("@boss", card.Boss) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@name", card.Name) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@id", card.Id) { Direction = ParameterDirection.Input });
                //cmd.Parameters.Add(new SqlParameter("@status_code",100) { Direction=ParameterDirection.Input};
                cmd.ExecuteNonQuery();
                ((App)App.Current).all_data.stat_it(card.Id, ((App)App.Current).the_person.Id, "success edit_unit");
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); ((App)App.Current).all_data.stat_it(card.Id, ((App)App.Current).the_person.Id, "fail edit_unit"); }
        }
        public void add_unit(Model.Agent card)
        {
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[CreateNewUnit]";

                cmd.Parameters.Add(new SqlParameter("@boss", card.Boss) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@name", card.Name) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@id", card.Id) { Direction = ParameterDirection.Output });
                //cmd.Parameters.Add(new SqlParameter("@status_code",100) { Direction=ParameterDirection.Input};
                cmd.ExecuteNonQuery();
                ((App)App.Current).all_data.stat_it(0, ((App)App.Current).the_person.Id, "success add_unit");
                card.Id = Convert.ToInt64(cmd.Parameters["@id"].Value);
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); ((App)App.Current).all_data.stat_it(0, ((App)App.Current).the_person.Id, "fail add_unit"); }
        }
        public void spam()
        {
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandText = "[dbo].[ReceivingDailyMail]";
                cmd.CommandText = "[dbo].[spAM]";
                cmd.ExecuteNonQuery();
                ((App)App.Current).all_data.stat_it(0, ((App)App.Current).the_person.Id, "receiving dailymail");
                connection.Close();
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); ((App)App.Current).all_data.stat_it(0, ((App)App.Current).the_person.Id, "fail receiving dailymail"); }
        }
        public void stat_it(long card, long who, string what)
        {
            try
            {
                string con_str = Properties.Settings.Default.con_str;
                SqlConnection connection = new SqlConnection(con_str);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[dbo].[CreateStatistic]";
                cmd.Parameters.Add(new SqlParameter("@card", card) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@who", who) { Direction = ParameterDirection.Input });
                cmd.Parameters.Add(new SqlParameter("@what", what) { Direction = ParameterDirection.Input });
                //cmd.Parameters.Add(new SqlParameter("@status_code",100) { Direction=ParameterDirection.Input};
                cmd.ExecuteNonQuery();
                connection.Close();

            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
        }
    }
}
