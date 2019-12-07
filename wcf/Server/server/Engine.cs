using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace server
{
    class Engine
    {
        ObservableCollection<ChatUser> _users = new ObservableCollection<ChatUser>();
        
        ObservableCollection<ChatUser> users
        {
            get { return _users; }
        }

        Dictionary<string, List<Message>> messages = new Dictionary<string, List<Message>>();

        public string send_message(Message input)
        {
            var exists =
                from string e in messages.Keys
                where e == input.Recepient.ToString()
                select e;
            if (exists.Count() == 0)
            {
                messages.Add(input.Recepient.ToString(), new List<Message>());
            }
            Console.WriteLine("Message from "+input.Sender+" to "+ input.Recepient+" : " + input.ToString());
            messages[input.Recepient.ToString()].Add(input);
            return "srvr> i got it";
        }
        
        public void send_about_me(ChatUser input)
        {
            if(!users.Contains(input))
                users.Add(input);
            //messages.Add(input.UserName, new List<Message>());
            Console.WriteLine("user on-line: " + input.ToString());
        }
        
        public void disconnect_me(ChatUser input)
        {
            try
            {
                var exists =
                from e in users
                where e.UserName == input.UserName
                select e;
                while(exists.ToList().Count!=0)
                {
                    foreach (var s in exists)
                    {
                        users.Remove(s);
                        exists.ToList().Remove(s);
                        break;
                    }
                }

                //foreach (var u in exists)
                //    users.Remove(u);
            }
            catch (Exception e)
            { Console.WriteLine(e); }
            Console.WriteLine("user off-line: " + input.ToString());
        }

        public ObservableCollection<ChatUser> get_us()
        {
            return users;
        }

        public List<Message> gmm(ChatUser usr)
        {
            var exists =
                from string e in messages.Keys
                where e == usr.ToString()
                select e;
            List<Message> myNewMessages;

            if (exists.Count() != 0)
            {
                myNewMessages = messages[usr.ToString()];
                messages[usr.ToString()] = new List<Message>();
                return myNewMessages;
            }
            else
                return null;
        }

    }
}
