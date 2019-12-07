using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BarcodeOrder.View
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Переход на следующую строку при нажатии Enter, 
        /// тк без этого новая запись не добавится в коллекцию
        /// до нажатия кнопки Сохранить
        /// </summary>
        /// <param name="sender"></param> DataGrid
        /// <param name="e"></param> base + special Key.Enter
        private void DataGrid_KeyUp(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e);

            DataGrid dg = (DataGrid)sender;

            //var uiElement = e.OriginalSource as UIElement;
            if (e.Key == Key.Enter)// && uiElement!=null)
            {
                e.Handled = true;
                dg.CommitEdit(DataGridEditingUnit.Row,true);
                //uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }
        /// <summary>
        /// Переход на следующую строку при покидании курсора DataGrid'а
        /// тк без этого новая запись не добавится в коллекцию
        /// до нажатия кнопки Сохранить
        /// </summary>
        /// <param name="sender"></param> DataGrid
        /// <param name="e"></param> 
        private void DataGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            e.Handled = true;
            dg.CommitEdit(DataGridEditingUnit.Row, true);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(@"1. Редактировать можно весь список сразу. 
2. Для сохранения изменений необходимо нажать <Сохранить все>. 
3. Отработанные заказы - помечать, но не удалять. 
4. Удалять записи по кнопке следует только в случае ошибки в номере заказа.");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ViewModel.MainViewModel mvm = (ViewModel.MainViewModel)DataContext;
            bool result = mvm.CanClosed;
            if (result)
            {
                MessageBoxResult dr = MessageBox.Show("Есть несохраненные элементы. Все равно закрыть?", "Внимание!", MessageBoxButton.YesNo);
                if (dr == MessageBoxResult.Yes)
                    e.Cancel = false;
                else
                    e.Cancel = true;
            }
        }
        
        //private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    Window a = (Window)sender;

        //    e.Cancel = true;
        //}

 
    }
}
