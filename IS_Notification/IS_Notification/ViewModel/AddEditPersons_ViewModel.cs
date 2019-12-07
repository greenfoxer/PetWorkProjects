using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using IS_Notification.Support;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace IS_Notification.ViewModel
{
    class AddEditPersons_ViewModel: ViewModelBase<IDialogView>
    {
        ObservableCollection<Model.Person> _managers;
        Model.Person _etalon;
        bool is_exist;
        bool o;
        ObservableCollection<Model.Agent> _units;
        public ObservableCollection<Model.Agent> units { get { return _units; } set { _units = value; RaisePropertyChanged("units"); } }
        
        public Role role { get { return ((App)App.Current).role; } }
        public ObservableCollection<Model.Person> managers
        {
            get { return _managers; }
            private set
            {
                this._managers = value;
                this.RaisePropertyChanged("managers");
            }
        }
        Model.Person _person;
        public Model.Person person
        {
            get { return _person; }
            set
            {
                this._person = value;
                if (person != null)
                {
                    _etalon = new Model.Person(person);
                    if(!person.is_equal(null))
                        is_exist = true;
                }
                this.RaisePropertyChanged("person");
            }
        }
        public ObservableCollection<Role> td { get; set; } 
        public AddEditPersons_ViewModel()
            :base(new View.AddEditPerson_View())
        {
            _new_card(null);
            Refresh();
            is_exist = false;
            //td = new List<string>();
            //td.Add("manager");
            //td.Add("t_manager");
        }
        public bool? ShowDialog()
        {
            return this.View.ShowDialog();
        }
        public void Close()
        {
            this.View.Close();
        }
        #region commands

        private ICommand _command;
        public ICommand create
        {
            get
            {
                this._command = new Command(this.createCMD, this.oncreateCMD);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void createCMD(object param)
        {
            bool can_add = true;
            if (is_exist)
            {
                ((App)App.Current).all_data.stat_it(person.Id, ((App)App.Current).the_person.Id, "editing person");
                ((App)App.Current).all_data.edit_person(person);
            }
            else
            {
                foreach (var t in managers)
                    if (t.Domen_name == person.Domen_name)
                    {
                        MessageBox.Show("Пользователь с таким доменным именем уже существует!");
                        ((App)App.Current).all_data.stat_it(t.Id, ((App)App.Current).the_person.Id, "fail adding person exist");
                        can_add = false;
                        break;
                    }
                if (can_add)
                {
                    ((App)App.Current).all_data.stat_it(0, ((App)App.Current).the_person.Id, "adding person");
                    ((App)App.Current).all_data.add_person(person);
                }
            }
            is_exist = false;
            Refresh();
        }
        bool oncreateCMD(object param)
        {
            if (person == null||person.is_equal(null))
                return false;
            if (person.is_equal(_etalon)&&is_exist)
                return false;
            if (String.IsNullOrEmpty(person.Domen_name) || String.IsNullOrEmpty(person.Fname) ||
                String.IsNullOrEmpty(person.Mail) || String.IsNullOrEmpty(person.Mname) || String.IsNullOrEmpty(person.Phone) ||
                String.IsNullOrEmpty(person.Role) || String.IsNullOrEmpty(person.Sname))
                return false;
            else
                return true;
        }
        public ICommand new_card
        {
            get
            {
                this._command = new Command(this._new_card, this.onnew_card);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void _new_card(object param)
        {
            is_exist = false;
            person = new Model.Person();
        }
        bool onnew_card(object param)
        {
            return true;
        }
        public ICommand del_card
        {
            get
            {
                this._command = new Command(this._del_card, this.ondel_card);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void _del_card(object param)
        {
            ((App)App.Current).all_data.stat_it(person.Id, ((App)App.Current).the_person.Id, "deleting person");
            ((App)App.Current).all_data.delete_person(person);
            is_exist = false;
            Refresh();
        }
        bool ondel_card(object param)
        {
            if(person!=null&&is_exist)
                if (person.is_equal(_etalon))
                    return true;
            return false;
        }
        #endregion
        void Refresh()
        {
            managers = ((App)App.Current).all_data.get_person();
            units = ((App)App.Current).all_data.get_units();
            td = ((App)App.Current).all_data.get_roles();
            if (((App)App.Current).the_person.Role != "admin")
            {
                foreach (var t in td)
                    if (t.Role_name == "admin")
                    {
                        td.Remove(t);
                        break;
                    }
            }
        }

    }
}
