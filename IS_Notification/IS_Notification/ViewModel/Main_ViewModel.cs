using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using IS_Notification.Support;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace IS_Notification.ViewModel
{
    public class Main_ViewModel: ViewModelBase<IView>
    {
        //public static Model.DataLayer all_data;
        public Model.Card selectedCard { get; set; }
        ObservableCollection<Model.Agent> _unit_filter;
        string _selected_unit="--";
        ObservableCollection<Model.Agent> _units;
        public ObservableCollection<Model.Agent> units { get { return _units; } set { _units = value; RaisePropertyChanged("units"); } }
        public string selected_unit { get { return _selected_unit; }
            set
            {
                if (value != "--")
                {
                    if (_etalon_cards == null)
                        _etalon_cards = ((App)App.Current).all_data.get_cards();
                    _cards.Clear();
                    foreach (var card in _etalon_cards)
                    {
                        if (card.Unit_name.Contains(value))
                        {
                            _cards.Add(card);
                        }
                    }
                }
                else
                {
                    Refresh(null);
                    _etalon_cards.Clear();
                    _etalon_cards = null;
                }
                _selected_unit = value;
            }
        }
        public ObservableCollection<Model.Agent> unit_filter
        {
            get 
            { 
                return _unit_filter; 
            }
            set
            {
                _unit_filter = value;
                RaisePropertyChanged("unit_filter");
            }
        }
        public Role role { get { return ((App)App.Current).role; } }
        public Model.Person person { get { return ((App)App.Current).the_person; } }
        ObservableCollection<Model.Card> _cards;
        ObservableCollection<Model.Card> _etalon_cards;
        public ObservableCollection<Model.Card> cards
        {
            get { return _cards; }
            private set
            {
                this._cards = value;
                this.RaisePropertyChanged("cards");
            }
        }
        string _view_filter;
        public string view_filter
        {
            get
            {
                return this._view_filter;
            }
            set
            {
                if (string.IsNullOrEmpty(_view_filter)||_etalon_cards==null)
                {
                    _etalon_cards = ((App)App.Current).all_data.get_cards();
                }
                this._view_filter = value.Trim();
                this.RaisePropertyChanged("ContractorsMakerFilter");
                if (!string.IsNullOrEmpty(_view_filter))
                {
                    _cards.Clear();
                    foreach (var card in _etalon_cards)
                    {
                        if (card.N_protocol.ToLower().Contains(_view_filter.ToLower())||card.Card_type.ToLower().Contains(_view_filter.ToLower()))
                        {
                            _cards.Add(card);
                        }
                    }
                }
                else
                {
                    Refresh(null);
                    _etalon_cards.Clear();
                }
            }
        }
        public ObservableCollection<Model.Person> persons { get; set; }
        public Main_ViewModel()
            : base(new View.Main_View())
        {
            //all_data = new Model.DataLayer();
            Refresh(null);
            //_cards = ((App)App.Current).all_data.get_cards();
        }
        
        public void Show()
        {
            this.View.Show();
        }

        #region commands
        private ICommand _command;
        public ICommand new_card
        {
            get
            {
                this._command = new Command(this.execCMD, this.oncanexecCMD);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        public ICommand delete
        {
            get
            {
                this._command = new Command(this._delete, this._can_delete);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void _delete(object param)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Вы уверены, что хотите удалить карточку?", "Удаление карточки", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                ((App)App.Current).all_data.stat_it(selectedCard.Id, ((App)App.Current).the_person.Id, "deleting card");
                ((App)App.Current).all_data.delete_card(selectedCard);
                cards.Remove(selectedCard);
            }
        }
        bool _can_delete(object param)
        {
            if(selectedCard!=null)
                return true;
            return false;
        }
        public ICommand editPerson
        {
            get
            {
                this._command = new Command(this.edit_person, this.oncanedit_person);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void edit_person(object param)
        {
            ((App)App.Current).all_data.stat_it(0, ((App)App.Current).the_person.Id, "button add/edit_person");
            AddEditPersons_ViewModel addedvm = new AddEditPersons_ViewModel();
            addedvm.ShowDialog();
            Refresh(null);
        }
        private bool oncanedit_person(object param)
        {
            return true;
        }
        public ICommand editRole
        {
            get
            {
                this._command = new Command(this.edit_role, this.oncanedit_role);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void edit_role(object param)
        {
            ((App)App.Current).all_data.stat_it(0, ((App)App.Current).the_person.Id, "button edit_role");
            AddEditRole_ViewModel addedvm = new AddEditRole_ViewModel();
            addedvm.ShowDialog();
            Refresh(null);
        }
        private bool oncanedit_role(object param)
        {
            return true;
        }
        public ICommand editAgents
        {
            get
            {
                this._command = new Command(this.edit_agent, this.oncanedit_agent);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void edit_agent(object param)
        {
            ((App)App.Current).all_data.stat_it(0, ((App)App.Current).the_person.Id, "button add/edit_agents"); 
            AddEditAgent_ViewModel addedvm = new AddEditAgent_ViewModel();
            addedvm.ShowDialog();
            Refresh(null);
        }
        private bool oncanedit_agent(object param)
        {
            return true;
        }
        void execCMD(object param)
        {
            Model.Card new_card = null;
            ((App)App.Current).all_data.stat_it(0, ((App)App.Current).the_person.Id, "button new_card");
            AddEdit_ViewModel addedvm=new AddEdit_ViewModel(new_card);
            addedvm.ShowDialog();
            if(addedvm.worked)
                cards.Add(addedvm.that_card);
            try
            {
                System.IO.Directory.Delete(addedvm.catalog, true);
            }
            catch { }
        }
        private bool oncanexecCMD(object param)
        {
            return true;
        }
        public ICommand edit_card
        {
            get
            {
                this._command = new Command(this.editCMD, this.oneditCMD);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        public System.Windows.Data.CollectionView cv { get; set; }
        void Refresh(object param)
        {
            cards = ((App)App.Current).all_data.get_cards();
            //cv = new System.Windows.Data.CollectionView(cards);
            //cv.GroupDescriptions.Add(new System.Windows.Data.PropertyGroupDescription("Type_card"));
            cv = new ListCollectionView(cards);

            cv.GroupDescriptions.Clear();
            cv.GroupDescriptions.Add(new PropertyGroupDescription("Year"));

            RaisePropertyChanged("cv");

            persons = ((App)App.Current).all_data.get_person();
            units = ((App)App.Current).all_data.get_unit();
            _unit_filter = ((App)App.Current).all_data.get_unit();
        }
        bool isrefresh(object param)
        {
            return true;
        }
        public ICommand refresh
        {
            get
            {
                this._command = new Command(this.Refresh, this.isrefresh);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void editCMD(object param)
        {
            ((App)App.Current).all_data.stat_it(selectedCard.Id, ((App)App.Current).the_person.Id, "editing card start");
            Model.Card tmp = new Model.Card(selectedCard);
            AddEdit_ViewModel addedvm = new AddEdit_ViewModel(selectedCard);
            addedvm.ShowDialog();
            if (!addedvm.worked)
                selectedCard.copy(tmp);
            try
            {
                System.IO.Directory.Delete(addedvm.catalog, true);
            }
            catch { }
        }
        private bool oneditCMD(object param)
        {
            return true;
        }
        public ICommand receive
        {
            get
            {
                this._command = new Command(this.Receive, this.onReceive);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void Receive(object param)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Вы уверены, что хотите произвести утреннюю рассылку?", "Рассылка", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                ((App)App.Current).all_data.stat_it(0, ((App)App.Current).the_person.Id, "button mail_receive");
                ((App)App.Current).all_data.spam();
            }
            else if (dialogResult == MessageBoxResult.No)
            {
                //do something else
            }
            //((App)App.Current).all_data.stat_it(0, ((App)App.Current).the_person.Id, "button mail_receive");
            //((App)App.Current).all_data.spam();
        }
        private bool onReceive(object param)
        {
            return true;
        }
        public ICommand edit_history
        {
            get
            {
                this._command = new Command(this.editHistory, this.oneditHistory);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void editHistory(object param)
        {
            ((App)App.Current).all_data.stat_it(0, ((App)App.Current).the_person.Id, "button statistic");
            Statistic_ViewModel nst=new Statistic_ViewModel();
            nst.ShowDialog();
        }
        private bool oneditHistory(object param)
        {
            return true;
        }
        #endregion
    }
}
