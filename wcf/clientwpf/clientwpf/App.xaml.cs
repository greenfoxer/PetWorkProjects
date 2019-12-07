using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security;
using System.Windows;

namespace clientwpf
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        MainWindowMV mv; 

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            mv = new MainWindowMV();
            mv.Show();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            string me = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            mv.end(me);
        }
    }
}
