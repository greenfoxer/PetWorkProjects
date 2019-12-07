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
using System.Drawing;
using System.Windows.Shapes;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using System.IO.Ports;
using System.Windows.Interactivity;
using System.Globalization;
using System.ComponentModel;
using System.Threading;

namespace QR_code_generator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        bool is_txt;
        public MainWindow()
        {
            DataContext = this;
            dpi = "300";
            hw = "170";
            nm = "новыйQR";
            card = new vCard();
            is_txt = true;
            InitializeComponent();
        }
        public string txt { get; set; }
        public string nm { get; set; }
        public string dpi { get; set; }
        public string hw { get; set; }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int a = tbctrl.SelectedIndex;
            if (a == 0)
            {
                if (txt != null & txt != "")
                    try
                    {
                        var bw = new ZXing.BarcodeWriter();
                        var encOptions = new ZXing.Common.EncodingOptions() { Width = Convert.ToInt32(hw), Height = Convert.ToInt32(hw), Margin = 0 };
                        bw.Options = encOptions;
                        bw.Format = ZXing.BarcodeFormat.QR_CODE;
                        //string text = "BEGIN:VCARD VERSION:3.0 N:Derlysh;Ilya;V TITLE:Chief of information security TEL;TYPE=work:+4956835753 TEL;TYPE=cell:+79039705658 EMAIL;TYPE=INTERNET:derlysh_i_v@goznak.ru END:VCARD";
                        var result = new Bitmap(bw.Write(txt));
                        System.Windows.Controls.Image i = new System.Windows.Controls.Image();
                        result.SetResolution(Convert.ToInt32(dpi), Convert.ToInt32(dpi));
                        result.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + nm + ".jpg");
                        MessageBox.Show("Выполнено!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                else
                    MessageBox.Show("Нет данных!");
            }
            else
            {
                try
                {
                    var bw = new ZXing.BarcodeWriter();
                    var encOptions = new ZXing.Common.EncodingOptions() { Width = Convert.ToInt32(hw), Height = Convert.ToInt32(hw), Margin = 0 };
                    bw.Options = encOptions;
                    bw.Format = ZXing.BarcodeFormat.QR_CODE;
                    //string text = "BEGIN:VCARD VERSION:3.0 N:Derlysh;Ilya;V TITLE:Chief of information security TEL;TYPE=work:+4956835753 TEL;TYPE=cell:+79039705658 EMAIL;TYPE=INTERNET:derlysh_i_v@goznak.ru END:VCARD";
                    bw.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
                    bw.Options.Hints.Add(EncodeHintType.DISABLE_ECI,true);
                    var result = new Bitmap(bw.Write(card.generate()));
                    System.Windows.Controls.Image i = new System.Windows.Controls.Image();
                    result.SetResolution(Convert.ToInt32(dpi), Convert.ToInt32(dpi));
                    result.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + nm + ".jpg");
                    MessageBox.Show("Выполнено!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

        }
        public class vCard
        {
            string fname, lname, mname, title, telw, telm, email;
            public string Fname { get { return fname; } set { fname = value; } }
            public string Lname { get { return lname; } set { lname = value; } }
            public string Mname { get { return mname; } set { mname = value; } }
            public string Title { get { return title; } set { title = value; } }
            public string Telw { get { return telw; } set { telw = value; } }
            public string Telm { get { return telm; } set { telm = value; } }
            public string Email { get { return email; } set { email = value; } }
            public vCard()
            {

            }
            public string generate()
            {
                return "BEGIN:VCARD\nVERSION:3.0\nN:"+Lname+";"+Fname+";"+Mname+"\nTITLE:"+Title+"\nTEL;TYPE=work:"+Telw+"\nTEL;TYPE=cell:"+Telm+"\nEMAIL;TYPE=INTERNET:"+Email+"\nEND:VCARD";
            }
                
            //string text = "BEGIN:VCARD VERSION:3.0 
            //N:Derlysh;Ilya;V 
            //TITLE:Chief of information security 
            //TEL;TYPE=work:+4956835753 
            //TEL;TYPE=cell:+79039705658 
            //EMAIL;TYPE=INTERNET:derlysh_i_v@goznak.ru 
            //END:VCARD";
                    
        }
        public vCard card{get;set;}

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                Process(dlg.FileName);
            }
        }
        string _decodedData;
        public string decodedData 
        { 
            get { return _decodedData; } 
            set { 
                _decodedData = value;
                RaisePropertyChanged("decodedData"); 
            } 
        }
        void Process(string path)
        {
            //Bitmap bitmap = new Bitmap(@"C:\Users\Zavodkin_R_S\Desktop\newQR.jpg");
            Bitmap bitmap = new Bitmap(path);
            try
            {
                BarcodeReader reader = new BarcodeReader { AutoRotate = true, TryHarder = true };
                Result result = reader.Decode(bitmap);
                decodedData = result.Text;
            }
            catch
            {
                throw new Exception("Cannot decode the QR code");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        ///////////////////////////////////////////////////////
      
        /*  public class ComDevice
        {
            public ComDevice(string portName, string description)
            {
                _currSerialPort = new SerialPort();
                _currSerialPort.DataReceived += ComDevice_DataReceived;
                _currSerialPort.StopBits = StopBits.One;
                _currSerialPort.DataBits = 8;
                _currSerialPort.BaudRate = 9600;
                //_currSerialPort.PortName = PortName;
                //_currSerialPort.ReadTimeout = AppMain.Settings.ComDeviceTimeOut;
                //_currSerialPort.WriteTimeout = AppMain.Settings.ComDeviceTimeOut;

                try
                {
                    Open();
                }
                catch
                {
                    //Status = DeviceStatus.St1NotFound;
                }
            }

            private readonly SerialPort _currSerialPort;

            private void Open()
            {
                if (!_currSerialPort.IsOpen)
                {
                    _currSerialPort.Open();
                }
            }

            private void Close()
            {
                if (_currSerialPort.IsOpen)
                {
                    try
                    {
                        _currSerialPort.Close();
                    }
                    catch (Exception exception)
                    {
                        //LoggerHelper.Logger.Error(string.Format("{0}. Ошибка при закрытии порта", this), exception);
                    }
                }
            }

            public void ComDevice_DataReceived(object sender, SerialDataReceivedEventArgs e)
            {
                //Status = DeviceStatus.St4Found;

                //ждем
                Thread.Sleep(500);
                try
                {
                    //считываем штрих код
                    var codeMass = new char[_currSerialPort.BytesToRead];
                    _currSerialPort.Read(codeMass, 0, codeMass.Length);
                    var receivedCode = codeMass.Aggregate("",
                        (current, currSym) => current + currSym.ToString(CultureInfo.InvariantCulture));
                    OnDataReceived(receivedCode);
                }
                catch (Exception exception)
                {
                    //LoggerHelper.Logger.Error("Ошибка при получении данных с COM порта!", exception);
                }
            }

            public delegate void DataReceivedEventHandler(object sender, string mess);
            public event DataReceivedEventHandler DataReceived;
            public void OnDataReceived(string data)
            {
                if (DataReceived != null) { DataReceived(this, data); }
            }

            public void SendMessage(string message)
            {
                if (string.IsNullOrEmpty(message))
                {
                    //LoggerHelper.Logger.Error("message is empty");
                    return;
                }

                try
                {
                    Open();
                }
                catch (Exception e)
                {
                    //LoggerHelper.Logger.Error("Невозможно открыть Com-порт", e);
                    //Status = DeviceStatus.St1NotFound;
                    return;
                }

                if (message.Length > 0)
                {
                    try
                    {
                        _currSerialPort.WriteLine(message);
                        //LoggerHelper.Logger.Info(this + ". Отправлено сообщение :\r\n" + message);
                        //Status = DeviceStatus.St4Found;
                    }
                    catch (Exception exception)
                    {
                        var errorMessage = this + ". Ошибка при отправке сообщения";

                        var t = exception as TimeoutException;
                        if (t != null)
                        {
                            errorMessage += " Возможная причина: отсутствие по указанному COM-порту адресата.";
                        }

                        //LoggerHelper.Logger.Error(errorMessage, exception);
                        //Status = DeviceStatus.St1NotFound;
                    }
                }
            }

            //public override void Dispose()
            //{
            //    Close();
            //}

            public override string ToString()
            {
                return base.ToString() + ", порт: ";// +PortName;
            }
        }*/

    }
}
