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
        private decimal ms = 0;
        public decimal MS
        {
            get => ms;
            set
            {
                ms = value;
                memBlock.Text = ms.ToString();
            }
        }
        MainWindow jManager = null;
        public Memory(MainWindow jmanager)
        {
            InitializeComponent();
            jManager = jmanager;
        }

        private void OpBtn_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label)
            {
                var hwnd = sender as Label;
                if (hwnd.Content.ToString() == "MC")
                {
                    MS = 0;
                }
                else if (hwnd.Content.ToString() == "M+")
                {
                    MS += Convert.ToDecimal(jManager.Result);
                }
                else if (hwnd.Content.ToString() == "M-")
                {
                    MS -= Convert.ToDecimal(jManager.Result);
                }
            }
        }
    }
}
