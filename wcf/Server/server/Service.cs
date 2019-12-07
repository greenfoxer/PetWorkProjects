using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Service: IContract
    {
        Engine eg = new Engine();

        public List<Message> get_my_message(ChatUser user)
        {
            return eg.gmm(user);
        }

        public string Say(Message input)
        {
            return eg.send_message(input);
        }
        public void Me(ChatUser input)
        {
            eg.send_about_me(input);
        }
        public void UnMe(ChatUser input)
        {
            eg.disconnect_me(input);
        }
        public ObservableCollection<ChatUser> GetUs()
        {
            return eg.get_us();
        }
    }
}
