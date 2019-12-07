using System.Linq;
using OrderBarcodeData.Abstract;
using OrderBarcodeData.Concrete;
using OrderBarcodeData.Entities;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace BarcodeOrder.ViewModel
{
    class MainViewModel: ViewModelBase
    {
        ObservableCollection<tOrderBarcode> _lo;
        public tOrderBarcode SelectedItem { get; set; }
        bool _canClosed;
        public bool CanClosed
        {
            get
            {
                IOrderBarcode repository = new OrderBarcodeRepository();
                _canClosed = repository.IsListChanged(lo.ToList());
                return _canClosed;
            }
        }
        public ObservableCollection<tOrderBarcode> lo 
        { 
            get{return _lo;} 
            set 
            { 
                _lo = value; 
                this.RaisePropertyChanged("lo"); 
            }
        }
        bool _isActual;
        public IDialogService DialogService;
        public bool IsActual { get { return _isActual; } set { _isActual = value; RaisePropertyChanged("IsActual"); } }
        public MainViewModel()
        {
            IsActual = true;
            DialogService = new DefaultDialogService();
            RefreshList();
        }
        void RefreshList()
        {
            IOrderBarcode repository = new OrderBarcodeRepository();
            if (IsActual)
                lo = new ObservableCollection<tOrderBarcode>(repository.BarcodeActualList);
            else
                lo = new ObservableCollection<tOrderBarcode>(repository.BarcodeFullList);
            SelectedItem = null;
        }
        //COMMANDS//
        #region Commands
        private ICommand _command;
        public ICommand Switch
        {
            get
            {
                this._command = new Command(
                    (object param) =>
                    {
                        RefreshList();
                    }, (object param) => { return true; }
                    );
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        public ICommand SaveAll
        {
            get
            {
                this._command = new Command((object param) =>
                {
                    if (DialogService.YesNoDialog("Сохранить", "Сохранить внесенные изменения?"))
                    {
                        IOrderBarcode repository = new OrderBarcodeRepository();
                        repository.SaveCollection(lo.ToList());
                        RefreshList();
                    }
                },
                (object param) => { return true; });
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        public ICommand DeleteElement
        {
            get
            {
                this._command = new Command(
                    (object param) =>
                    {
                        if (SelectedItem != null)
                        {
                            if (DialogService.YesNoDialog("Удалить", "Удалить заказ и штрихкод?"))
                            {
                                IOrderBarcode repository = new OrderBarcodeRepository();
                                repository.DeleteOrder(SelectedItem);
                                RefreshList();
                            }
                        }
                    }, (object param) => { return true; }
                    );
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        
        public ICommand CheckClose
        {
            get
            {
                this._command = new Command(
                    (object param) =>
                    {
                        IOrderBarcode repository = new OrderBarcodeRepository();
                       
                        
                    }, (object param) => { return true; }
                    );
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }
        #endregion
    }
}
