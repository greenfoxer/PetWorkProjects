﻿using System;
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
using IS_Notification.Support;
using IS_Notification.ViewModel;

namespace IS_Notification.View
{
    /// <summary>
    /// Логика взаимодействия для AddEdit_Window.xaml
    /// </summary>
    public partial class AddEdit_View : Window, IDialogView
    {
        public AddEdit_View()
        {
            InitializeComponent();
        }
    }
}
