using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using IS_Notification.Support;
using System.Windows;

namespace IS_Notification
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        private Model.DataLayer _all_data;
        public Model.DataLayer all_data
        {
            get { return this._all_data; }
            private set
            {
                _all_data = value;
            }
        }
        public Role role { get; private set; }
        public Model.Person the_person { get; set; }
        void Authentication()
        {
            the_person = all_data.validate(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            //the_person.Role="guest";
            //the_person.Role = "worker";
            //the_person.Department = 5;
            if (the_person != null)
                MessageBox.Show("Вы вошли в ИС Оповещения как " + the_person.Name);
            else
            {
                MessageBox.Show("Вы вошли в ИС Оповещения как гость");
                the_person = new Model.Person();
            }
            role = all_data.get_my_role(the_person);
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            all_data = new Model.DataLayer();
            Authentication();
            all_data.stat_it(0,the_person.Id,"log-in");
            ViewModel.Main_ViewModel vm = new ViewModel.Main_ViewModel();
            vm.Show();
        }
    }
}
