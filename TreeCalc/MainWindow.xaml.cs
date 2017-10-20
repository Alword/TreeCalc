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

namespace TreeCalc
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //string calcData;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Label_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void opBtn_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label)
                AddOperation((sender as Label).DataContext.ToString());
        }

        private void AddOperation(string operation)
        {
            calcData.Content += operation;
        }

        private void wPanel_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Label x in (sender as WrapPanel).Children)
            {
                if (x.DataContext != null)
                    x.MouseUp += opBtn_Click;
            }
            void warmingUp()
            {
                Operation.tryCalc("0");
            }
            new System.Threading.Thread(warmingUp).Start();
        }

        private void ChangeZnak(Label x)
        {
            //x.Content = ;
        }

        private void Label_MouseUp_2(object sender, MouseButtonEventArgs e)
        {
            resultData.Content = Operation.tryCalc(calcData.Content.ToString());
        }

        private void backspace_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label)
            {
                var hwdl = calcData;
                hwdl.Content = Model.backspace(hwdl.Content.ToString());
            }
        }
    }

    public static class Model
    {
        public static string backspace(string calcData)
        {
            var strLen = calcData.Length;
            if (strLen > 0)
            {
                calcData = calcData.ToString().Substring(0, strLen - 1);
            }
            return calcData;
        }
    }
}
