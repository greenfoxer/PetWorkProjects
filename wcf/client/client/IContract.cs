using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace client
{
    [ServiceContract]
    interface IContract
    {
        [OperationContract]
        string Say(string input);
    }
}
