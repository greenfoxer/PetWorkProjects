using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IS_Notification.Support;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace IS_Notification.ViewModel
{
    class AddEditRole_ViewModel: ViewModelBase<IDialogView>
    {
        ObservableCollection<Role> _roles;
        public Role selected_role { get; set; }
        string _new_name;
        public string new_name { get { return _new_name; } set { _new_name = value; RaisePropertyChanged(new_name); } }
        public ObservableCollection<Role> roles { get { return _roles; } set { _roles = value; RaisePropertyChanged("roles"); } }
        public AddEditRole_ViewModel()
            : base(new View.AddEditRole_View())
        {
            _roles = ((App)App.Current).all_data.get_roles();
        }
        public bool? ShowDialog()
        {
            return this.View.ShowDialog();
        }
        public void Close()
        {
            this.View.Close();
        }
        #region cmds
        private ICommand _command;
        public ICommand add_role
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
            Role rl = new Role();
            rl.Role_name = new_name;
            roles.Add(rl);
            new_name = "";
        }
        private bool oncreateCMD(object param)
        {
            if (string.IsNullOrEmpty(new_name))
                return false;
            return true;
        }
        public ICommand del_role
        {
            get
            {
                this._command = new Command(this.deleteCMD, this.ondeleteCMD);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void deleteCMD(object param)
        {
            roles.Remove(selected_role);
        }
        private bool ondeleteCMD(object param)
        {
            if (selected_role!=null)
                return true;
            return false;
        }
        public ICommand apply_list
        {
            get
            {
                this._command = new Command(this._apply, this._can_apply);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void _apply(object param)
        {
            ((App)App.Current).all_data.apply_roles(roles);
            this.Close();
        }
        bool _can_apply(object param)
        {
            return true;
        }
        #endregion

    }
}
