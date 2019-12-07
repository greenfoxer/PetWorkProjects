using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IS_Notification.Support
{
    public interface IDialogView : IView
    {
        bool? ShowDialog();
        bool? DialogResult { get; set; }
        void Close();
        System.Windows.Window Owner { get; set; }
    }
}