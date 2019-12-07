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
    class AddEditAgent_ViewModel: ViewModelBase<IDialogView>
    {
        ObservableCollection<Model.Agent> _units;
        Model.Agent _etalon;
        public ObservableCollection<Model.Agent> units 
        { 
            get { return _units; } 
            set { _units = value; RaisePropertyChanged("units"); } 
        }
        Model.Agent _agent;
        bool is_redacting;
        public Model.Agent agent
        {
            get
            { return _agent; }
            set
            { 
                _agent = value;
                if (agent != null)
                {
                    _etalon = new Model.Agent(agent);
                    if (!agent.is_equal(null))
                        is_redacting = true;
                }
                RaisePropertyChanged("agent"); 
            }
        }
        public ObservableCollection<Model.Person> person { get; set; } 
        public AddEditAgent_ViewModel()
            :base(new View.AddEditAgent_View())
        {
            is_redacting = false;
            person = ((App)App.Current).all_data.get_person();
            Refresh(null);
        }
        void Refresh(object param)
        {
            agent = new Model.Agent();
            _units = ((App)App.Current).all_data.get_units();
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
            if (is_redacting)
            {
                ((App)App.Current).all_data.stat_it(agent.Id, ((App)App.Current).the_person.Id, "editing agent");
                ((App)App.Current).all_data.edit_unit(agent);
            }
            else
            {
                ((App)App.Current).all_data.stat_it(0, ((App)App.Current).the_person.Id, "adding agent");
                ((App)App.Current).all_data.add_unit(agent);
                units.Add(agent);
            }
        }
        bool oncreateCMD(object param)
        {
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
            is_redacting = false;
            agent = new Model.Agent();
        }
        bool onnew_card(object param)
        {
            return true;
        }
        public ICommand info
        {
            get
            {
                this._command = new Command(this._info, this.oninfo);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void _info(object param)
        {
            ((App)App.Current).all_data.stat_it(agent.Boss, ((App)App.Current).the_person.Id, "editing person from agent");
            AddEditPersons_ViewModel aepvm = new AddEditPersons_ViewModel();
            foreach (var p in person)
                if (p.Id == agent.Boss)
                    aepvm.person = p as Model.Person;
            aepvm.ShowDialog();
        }
        bool oninfo(object param)
        {
            if(agent!=null)
                return true;
            return false;
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

            ((App)App.Current).all_data.stat_it(agent.Id, ((App)App.Current).the_person.Id, "deleting agent");
            ((App)App.Current).all_data.delete_unit(agent);
            units.Remove(agent);
            agent = new Model.Agent();
        }
        bool ondel_card(object param)
        {
            if(agent!=null)
                return true;
            return false;
        }
        #endregion
    }
}
