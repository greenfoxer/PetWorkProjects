using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IS_Notification.Support
{
    public interface IView
    {
        void Show();
        object DataContext { get; set; }
    }
}