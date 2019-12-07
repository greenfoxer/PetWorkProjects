using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Data;
using System.Reflection;
using System.Collections.ObjectModel;

using IS_Notification.Support;

namespace IS_Notification.ViewModel
{
    public class AddEdit_ViewModel: ViewModelBase<IDialogView>
    {
        #region variables'n'refs
        public ObservableCollection<Model.Person> managers { get; set; }
        public ObservableCollection<Model.Person> top_managers { get; set; }
        public enum type_document
        {
            КД, ПД
        }
        string _catalog="c:\\_notification_tmp\\";
        public string catalog
        {
            get { return _catalog; }
        }
        public List<string> td { get; set; } 
        public Model.Card that_card { get; set; }
        bool mode;
        public bool worked;
        ObservableCollection<Model.Agent> _units;
        public ObservableCollection<Model.Agent> units { get { return _units; } set { _units = value; RaisePropertyChanged("units"); } }
        #endregion
        public Role role { get { return ((App)App.Current).role; } }
        public AddEdit_ViewModel(Model.Card card)
            : base(new View.AddEdit_View())
        {
            td = new List<string>();
            td.Add("КД");
            td.Add("ПД");
            td.Add("Акт");
            ad = new List<Model.Card.File>();
            del = new List<Model.Card.File>();
            managers = ((App)App.Current).all_data.get_person("worker");
            foreach (var item in ((App)App.Current).all_data.get_person("manager"))
                managers.Add(item);
            top_managers = ((App)App.Current).all_data.get_person("t_manager");
            top_managers.Add(new Model.Person());
            units = ((App)App.Current).all_data.get_units();

            if (card != null)
            {
                that_card = card;
                mode = true;
            }
            else
            {
                that_card = new Model.Card();
                mode = false;
            }
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
        public ICommand create_card
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
        public List<Model.Card.File> ad, del;
        bool is_valid()
        {
            if (that_card.Card_type != null && that_card.N_protocol!= null && that_card.N_protocol != String.Empty && that_card.Responsible_person != 0 && that_card.Unit != 0)
                return true;
            return false;
        }
        void createCMD(object param)
        {
            if (is_valid())
            {
                foreach(var t in units)
                    if(t.Id==that_card.Unit)
                    {
                        that_card.Unit_manager = t.Boss;
                        break;
                    }
                if (!mode)
                {
                    ((App)App.Current).all_data.stat_it(that_card.Id, ((App)App.Current).the_person.Id, "adding card");
                    ((App)App.Current).all_data.add_card(that_card, ((App)App.Current).the_person.Id, ad);
                }
                else
                {
                    ((App)App.Current).all_data.stat_it(that_card.Id, ((App)App.Current).the_person.Id, "editing card");
                    ((App)App.Current).all_data.edit_card(that_card, del, ad);
                }
                worked = true;
                this.Close();
            }
            else
                MessageBox.Show("Есть незаполненные поля!");
        }
        private bool oncreateCMD(object param)
        {
            return true;
        }
        public ICommand add_files
        {
            get
            {
                this._command = new Command(this._add_files, this.onadd_files);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void _add_files(object param)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                foreach (String file in dlg.FileNames)
                {
                    if (File_list == null)
                        File_list = new ObservableCollection<Model.Card.File>();
                    Model.Card.File a = new Model.Card.File(file.Split('\\')[file.Split('\\').Length - 1], file);
                    File_list.Add(a);
                    ad.Add(a);
                }
            }
        }
        bool onadd_files(object param)
        {
            return true;
        }
        public ICommand del_files
        {
            get
            {
                this._command = new Command(this._del_files, this.ondel_files);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void _del_files(object param)
        {
            File_list.Remove(selected_file);
            del.Add(selected_file);
            //((App)App.Current).all_data.delete_file(that_card.Id,selected_file);
        }
        bool ondel_files(object param)
        {
            if (string.IsNullOrEmpty(selected_file.Fname)&&string.IsNullOrEmpty(selected_file.Sname))
                return false;
            return true;
        }
        
        public ICommand download_file
        {
            get
            {
                this._command = new Command(this._down_file, this.ondown_file);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void _down_file(object param)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = selected_file.Sname;
            dlg.Filter = selected_file.Sname.Split('.')[selected_file.Sname.Split('.').Length - 1] + " files (*." + selected_file.Sname.Split('.')[selected_file.Sname.Split('.').Length - 1] + ")|*." + selected_file.Sname.Split('.')[selected_file.Sname.Split('.').Length - 1] + "|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                ((App)App.Current).all_data.stat_it(that_card.Id, ((App)App.Current).the_person.Id, "downloading file");
                ((App)App.Current).all_data.download_file(selected_file.Fname, that_card.Id,dlg.FileName);
            }
            
        }
        bool ondown_file(object param)
        {
            if (string.IsNullOrEmpty(selected_file.Fname) && string.IsNullOrEmpty(selected_file.Sname))
                return false;
            return true;
        }
        public ICommand open_file
        {
            get
            {
                this._command = new Command(this._open_file, this.ondown_file);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void _open_file(object param)
        {
            System.IO.Directory.CreateDirectory(catalog);
            ((App)App.Current).all_data.stat_it(that_card.Id, ((App)App.Current).the_person.Id, "opening file");
            ((App)App.Current).all_data.download_file(selected_file.Fname, that_card.Id, catalog + selected_file.Sname,true);
        }
        public ICommand quit
        {
            get
            {
                this._command = new Command(this._quit, this.onquit);
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        void _quit(object param)
        {
            this.Close();
        }
        bool onquit(object param)
        {
            return true;
        }
        #endregion
        public Model.Card.File selected_file { get; set; }
        public ObservableCollection<Model.Card.File> File_list
        {
            get { return that_card.File_list; }
            private set
            {
                that_card.File_list = value;
                RaisePropertyChanged("File_list");
            }
        }
    }
}
