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
using System.Windows.Navigation;
using System.Windows.Shapes;
using OrderBarcodeData.Abstract;
using OrderBarcodeData.Concrete;
using OrderBarcodeData.Entities;


namespace BarcodeOrder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IOrderBarcode repository = new OrderBarcodeRepository();
            List<tOrderBarcode> lo = repository.BarcodeFullList.ToList();
            //tOrderBarcode to = new tOrderBarcode(){Code = "182010", Barcode = "4601009050421", IsActive = true};
            //repository.AddBarcode(to);

        }
    }
}
