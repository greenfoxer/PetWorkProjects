using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IS_Notification.Support;
using System.Data;
using System.Windows;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Windows.Input;

namespace IS_Notification.ViewModel
{
    class Statistic_ViewModel:ViewModelBase<IDialogView>
    {
        public DataView statistic{get;set;}
        public ObservableCollection<Model.Card> del_cards { get; set; }
        public ObservableCollection<Model.Person> del_persons { get; set; }
        public ObservableCollection<Model.Agent> del_agents { get; set; }
        public DataView del_files { get; set; }
        public Model.Card selected_card {get;set;}
        public Model.Person selected_person{get;set;}
        public Model.Agent selected_agent { get; set; }
        public Statistic_ViewModel()
            : base(new View.Statistic_View())
        {
            refresh();
        }
        public bool? ShowDialog()
        {
            return this.View.ShowDialog();
        }
        public void Close()
        {
            this.View.Close();
        }
        void refresh()
        {
            del_cards = ((App)App.Current).all_data.get_deleted_cards();
            del_persons = ((App)App.Current).all_data.get_deleted_person();
            del_agents = ((App)App.Current).all_data.get_deleted_units();
            del_files = ((App)App.Current).all_data.get_deleted_files().AsDataView();
            statistic = ((App)App.Current).all_data.get_statistic().AsDataView();
        }
        private ICommand _command;
        public ICommand restore_card
        {
            get
            {
                this._command = new Command(this.restorecard, this.onrestore_card);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void restorecard(object param)
        {
            ((App)App.Current).all_data.restore_card(selected_card.Id);
            ((App)App.Current).all_data.stat_it(selected_card.Id, ((App)App.Current).the_person.Id, "restoring card");
            del_cards.Remove(selected_card);
        }
        private bool onrestore_card(object param)
        {
            if(selected_card!=null)
            return true;
            return false;
        }
        public ICommand restore_person
        {
            get
            {
                this._command = new Command(this.restoreperson, this.onrestoreperson);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void restoreperson(object param)
        {
            ((App)App.Current).all_data.restore_person(selected_person.Id);
            ((App)App.Current).all_data.stat_it(selected_person.Id, ((App)App.Current).the_person.Id, "restoring person");
            del_persons.Remove(selected_person);
        }
        private bool onrestoreperson(object param)
        {
            if(selected_person!=null)
                return true;
            return false;
        }
        public ICommand restore_agent
        {
            get
            {
                this._command = new Command(this.restoreagent, this.onrestoreagent);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void restoreagent(object param)
        {
            ((App)App.Current).all_data.restore_agent(selected_agent.Id);
            ((App)App.Current).all_data.stat_it(selected_agent.Id, ((App)App.Current).the_person.Id, "restoring agent");
            del_agents.Remove(selected_agent);
        }
        private bool onrestoreagent(object param)
        {
            if(selected_agent!=null)
            return true;
            return false;
        }
    }
}
