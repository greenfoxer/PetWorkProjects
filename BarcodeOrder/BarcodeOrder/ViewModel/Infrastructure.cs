namespace BarcodeOrder.ViewModel
{
    using System;
    using Microsoft.Win32;
    using System.Windows;
    using System.Windows.Input;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    //********************************************************************************************
    //********************************************************************************************
    //***********************************DIALOG SERVICE*******************************************
    //********************************************************************************************
    //********************************************************************************************
    #region DialogService
    public interface IDialogService
    {
        void ShowMessage(string message);   // показ сообщения
        string FilePath { get; set; }   // путь к выбранному файлу
        bool OpenFileDialog();  // открытие файла
        bool SaveFileDialog();  // сохранение файла
        bool YesNoDialog(string header, string question);
    }
    public class DefaultDialogService : IDialogService
    {
        public string FilePath { get; set; }

        public bool OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }

        public bool SaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                return true;
            }
            return false;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public bool YesNoDialog(string header, string question)
        {
         MessageBoxResult dr = MessageBox.Show(question, header, MessageBoxButton.YesNo);
         if (dr == MessageBoxResult.Yes)
             return true;
         return false;
        }
    }
    #endregion
    //********************************************************************************************
    //********************************************************************************************
    //*********************************COMMANDS DEFINITION****************************************
    //********************************************************************************************
    //********************************************************************************************
    #region Commands
    internal class Command : ICommand
    {
        #region Fields

        private readonly CommandOnCanExecute _canExecute;

        private readonly CommandOnExecute _execute;

        #endregion

        #region Constructors and Destructors

        public Command(CommandOnExecute onExecuteMethod, CommandOnCanExecute onCanExecuteMethod)
        {
            _execute = onExecuteMethod;
            _canExecute = onCanExecuteMethod;
        }

        #endregion

        #region Delegates

        public delegate bool CommandOnCanExecute(object parameter);

        public delegate void CommandOnExecute(object parameter);

        #endregion

        #region Public Events

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        #endregion

        #region Public Methods and Operators

        public bool CanExecute(object parameter)
        {
            return _canExecute.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            _execute.Invoke(parameter);
        }

        #endregion
    }
    #endregion
    //********************************************************************************************
    //********************************************************************************************
    //*****************************VIEWMODEL BASE PROPERTY CHANGED********************************
    //********************************************************************************************
    //********************************************************************************************
    #region ViewModelBase
    class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //protected bool SetProperty(ref T storage, T value, [CallerMemberName] string propertyName = null)
        //{
        //    if (object.Equals(storage, value)) 
        //        return false;
        //    storage = value;
        //    this.RaisePropertyChanged(propertyName);
        //    return true;
        //}
        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (this.PropertyChanged != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
            //if (this.PropertyChanged != null)
            //{
            //    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            //}
        }
    }
    #endregion
    //********************************************************************************************
    //********************************************************************************************
    //****************************************END*************************************************
    //********************************************************************************************
    //********************************************************************************************
}
