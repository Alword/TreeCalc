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

namespace TreeCalc.Gadgets
{
    /// <summary>
    /// Логика взаимодействия для Memory.xaml
    /// </summary>
    public partial class Memory : UserControl
    {

        public string MS
        {
            get => memBlock.Text;
            set
            {
                memBlock.Text = value;
            }
        }

        MainWindow jManager = null;

        public Memory(MainWindow jmanager)
        {
            InitializeComponent();
            jManager = jmanager;
        }

        /// <summary>
        /// Нажата одна из кнопок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpBtn_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label)
            {
                var hwnd = sender as Label;
                if (hwnd.Content.ToString() == "MC")
                {
                    MS = Operation.TryCalc($"0");
                }
                else if (hwnd.Content.ToString() == "M+")
                {
                    MS = Operation.TryCalc($"({MS})+({jManager.Result})");
                }
                else if (hwnd.Content.ToString() == "M-")
                {
                    MS = Operation.TryCalc($"({MS})-({jManager.Result})");
                }
            }
        }
    }
}
