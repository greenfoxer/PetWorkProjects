using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace clientwpf
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    interface IContract
    {
        [OperationContract]
        string Say(Message input);

        [OperationContract]
        void Me(ChatUser input);

        [OperationContract]
        void UnMe(ChatUser input);

        [OperationContract]
        ObservableCollection<ChatUser> GetUs();

        [OperationContract]
        List<Message> get_my_message(ChatUser user);
    }

    [DataContract(Namespace="dc")]
    public class Message
    {
        private ChatUser sender;
        [DataMember]
        public ChatUser Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        private ChatUser recepient;
        [DataMember]
        public ChatUser Recepient
        {
            get { return recepient; }
            set { recepient = value; }
        }

        private string body;
        [DataMember]
        public string Body
        { get { return body; } set { body = value; } }

        private DateTime time;
        [DataMember]
        public DateTime Time
        { get { return time; } set { time = value; } }

        byte[] file;
        [DataMember]
        public byte[] File
        { get { return file; } set { file = value; } }

        public override string ToString()
        {
            return Body;
        }
    }

    [DataContract(Namespace = "dc")]
    public class ChatUser
    {
        private string userName, ipAddress, hostName;
        [DataMember]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        [DataMember]
        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }
        [DataMember]
        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        public override string ToString()
        {
            return this.UserName;
        }
    }
}
