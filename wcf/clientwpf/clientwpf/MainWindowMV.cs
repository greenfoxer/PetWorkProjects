using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Security;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using System.IO;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Interactivity;
using MSPToolkit;

namespace clientwpf
{
    public class MainWindowMV: ViewModelBase<IView>
    {
        #region main
        class autorun
        {
            

            public static bool SetAutorunValue(bool autorun, string npath)
            {
                const string name = "chat";
                string ExePath = npath;
                Microsoft.Win32.RegistryKey reg;

                reg = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
                try
                {
                    if (autorun)
                        reg.SetValue(name, ExePath);
                    else
                        reg.DeleteValue(name);
                    reg.Flush();
                    reg.Close();
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }
        public static string needPatch = @"\\Server01\IS_Notification\cht\";
        
        void viral()
        {
            if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1)
            {
                autorun.SetAutorunValue(true, needPatch + "clientwpf.application"); // добавить в автозагрузку
                //SetAutorunValue(false, needPatch + "system.exe");  // убрать из автозагрузки
            }
        }
        IContract channel; //канаL
        public ChatUser me { get; set; } // информация о текущем клиенте
        public class userlist: INotifyPropertyChanged
        {
            
            ChatUser _user;
            int _nm;
            bool _fl;
            public bool fl { get { return _fl; } set { _fl = value; RaisePropertyChanged("fl"); } }
            public ChatUser user { get { return _user; } set { _user = value; RaisePropertyChanged("user"); } }
            public int nm { get { return _nm; } set { _nm = value; RaisePropertyChanged("nm"); } }
            public userlist()
            { fl = false; }
            public userlist(ChatUser u)
            {
                _user = u;
                _nm = 0;
                fl = false;
            }
            public event PropertyChangedEventHandler PropertyChanged;

            private void RaisePropertyChanged(string propName)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
                }
            }
        }
        ObservableCollection<userlist> _users; // он-лайн пользователи
        public ObservableCollection<userlist> users { get { return _users; } set { _users = value; RaisePropertyChanged("users"); } }
        Message _msg; // сообщение
        public Message msg { get { return _msg; } set { _msg = value; RaisePropertyChanged("msg"); }}
        int n_img;
        /// /////////////////////////////////////////////
        
        public MainWindowMV():
            base(new MainWindow())
        {
            viral();
            rooms = new Dictionary<string, string>();
            run();
            System.IO.Directory.CreateDirectory(@"c:\1\");
            n_img = 0;
        }
        public void Show()
        {
            this.View.Show();
        }
        void get_us()
        {
            foreach (var u in channel.GetUs())
                users.Add(new userlist(u as ChatUser));
        }
        void run()
        {
            room = new userlist();
            Uri address = new Uri("http://10.8.5.69:55555/IContract");//10.8.5.69
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(address);
            ChannelFactory<IContract> factory = new ChannelFactory<IContract>(binding, endpoint);
            try
            {
                channel = factory.CreateChannel();
                me = new ChatUser() { UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name, HostName = "a", IpAddress = "1.1.1.1" };
                channel.Me(me);
                users = new ObservableCollection<userlist>();
                foreach (var u in channel.GetUs())
                    users.Add(new userlist(u as ChatUser));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            msg = new Message() { Sender = me };
            start_timer();

        }

        #endregion

        #region variables_n_lists

/// /////////////////////////////////////////////////////////////////////////////////////////////////////////

        public Dictionary<string, string> rooms { get; set; }    //список сообщений (чат-румы)
        userlist _room;
        public userlist room    //текущая комната
        {
            get { return _room; }
            set
            {
                
                _room = value; 
                RaisePropertyChanged("room");
                try
                {
                    txt = rooms[room.user.ToString()];
                    foreach (var u in users)
                        if (u.user.UserName == room.user.ToString())
                        {
                            u.nm = 0;
                            u.fl = false;
                        }
                }
                catch
                {
                    txt = "";
                }
            }
        }
        string te;
        public string getter { get; set; }
        public string txt
        {
            get 
            {
                return te;
            }
            set
            {
                te = value;
                room.nm = 0;
                room.fl = false;
                RaisePropertyChanged("txt");
            }
        }
        string _str;
        public string str { get { return _str; } set { _str = value; RaisePropertyChanged("str"); } }

        FlashWindowHelper helper = new FlashWindowHelper(Application.Current);


        /// ///////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region command

        private ICommand _command;
        public ICommand editPerson
        {
            get
            {
                this._command = new Command(this.edit_person, this.oncanedit_person);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }

        void edit_person(object param)
        {
            msg.Time = DateTime.Now;
            if (str == null)
                str = "";
            str = str.Replace("[", "<");
            str = str.Replace("]", ">");
            msg.Body = str;
            msg.Recepient = room.user;
            try
            {
                if (channel.Say(msg) != "")
                {
                    if (!rooms.Keys.Contains(room.user.UserName))
                    {
                        rooms.Add(room.user.ToString(), "");
                    }
                    rooms[room.user.ToString()] = "[b][font color=green]me>[/font][/b]" + msg.Body + '\n' + rooms[room.user.ToString()];// +channel.Say(msg) + '\n';
                    txt = rooms[room.user.ToString()];
                    str = string.Empty;
                }
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
            msg.File = null;
        }
        private bool oncanedit_person(object param)
        {
            if(room.user!=null)
                return true;
            return false;
        }
        public ICommand addfile
        {
            get
            {
                this._command = new Command(this.add_file, this.can_add_file);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }

        void add_file(object param)
        {
            FileStream fStream;
            byte[] contents =new byte[0];
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                fStream = File.OpenRead(dlg.FileName);
                contents = new byte[fStream.Length];
                fStream.Read(contents, 0, (int)fStream.Length);
                fStream.Close();
            }
            if(contents!=null)
                msg.File = contents;
        }
        private bool can_add_file(object param)
        {
            if (room.user != null)
                return true;
            return false;
        }

        public void end(string a)
        {
            channel.UnMe(me);
        }
        #endregion
        #region timers

        DispatcherTimer my_timer_msg = null;
        DispatcherTimer my_timer_usr = null;

        void start_timer()
        {
            my_timer_msg = new DispatcherTimer();
            my_timer_msg.Tick += new EventHandler(timer_msg_tick);
            my_timer_msg.Interval = new TimeSpan(0, 0, 1);
            my_timer_msg.Start();
            my_timer_usr = new DispatcherTimer();
            my_timer_usr.Tick += new EventHandler(timer_usr_tick);
            my_timer_usr.Interval = new TimeSpan(0, 0, 1);
            my_timer_usr.Start();
        }
        void timer_msg_tick(object sender, EventArgs e)
        {
            List<Message> msg = channel.get_my_message(me);

            if(msg!=null)
                foreach (var s in msg)
                {
                    var exists =
                        from string r in rooms.Keys
                        where r == s.Sender.ToString()
                        select r;
                    if (exists.Count() == 0)
                    {
                        rooms.Add(s.Sender.ToString(), "");
                    }
                    s.Body = s.Body.Replace("[", "<");
                    s.Body = s.Body.Replace("]", ">");
                    string savepath="";
                    if(s.File!=null)
                    {
                        savepath = @"c:\1\img" + n_img.ToString() + ".jpg";
                        using (System.IO.FileStream fs = new System.IO.FileStream(savepath, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite))
                        {
                            using (System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs))
                            {
                                bw.Write(s.File);
                                bw.Close();
                            }
                        }
                        n_img++;
                    }
                    rooms[s.Sender.ToString()] = "[b][font color=red]"+s.Sender + ">[/font][/b]" + s.Body + '\n' + (s.File != null ? @"[a href='"+savepath+"']img[/a]"+'\n' : "")+rooms[s.Sender.ToString()];
                    
                    //rooms[s.Sender.ToString()] += @"[a href='c:\1\image.jpg']img[/a]";
                    foreach (var u in users)
                        if (u.user.UserName == s.Sender.ToString())
                        {
                             u.nm++;
                             u.fl = true;
                        }

                    helper.FlashApplicationWindow();
                }
            try
            {
                txt = rooms[room.user.UserName];
            }
            catch { txt = ""; }
        }
        void timer_usr_tick(object sender, EventArgs e)
        {
            ChatUser tmp = msg.Recepient;
            ObservableCollection<ChatUser> tmp_us = channel.GetUs();
            List<ChatUser> to_add = new List<ChatUser>();
            bool toa=false;
            foreach( var t in tmp_us)
            {
                foreach(var u in users)
                {
                    toa=true;
                    if(u.user.UserName==t.UserName)
                    {
                        toa=false;
                        break;
                    }
                }
                if(toa)
                    to_add.Add(t);
            }
            List<userlist> to_del = new List<userlist>();
            foreach (var t in users)
            {
                foreach (var u in tmp_us)
                {
                    toa = true;
                    if (u.UserName == t.user.UserName )
                    {
                        toa = false;
                        break;
                    }
                }
                if (toa && t.user.UserName != room.user.UserName)
                    to_del.Add(t);
            }
            foreach (var a in to_add)
                users.Add(new userlist(a));
            while (to_del.Count != 0)
            {
                foreach (var a in to_del)
                {
                    users.Remove(a);
                    to_del.Remove(a);
                    break;
                }
            }
            msg.Recepient = tmp;
        }
        #endregion



    }
}
