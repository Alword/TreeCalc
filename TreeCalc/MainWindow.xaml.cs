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
            void warmingUp()
            {
                Operation.tryCalc("0");
            }
            new System.Threading.Thread(warmingUp).Start();
            InitializeComponent();

            jurnalBTN.SetHead = "Журнал";
            jurnalBTN.IsActivated = true;

            meoryBTN.SetHead = "Память";
            meoryBTN.IsActivated = false;

        }

        private void AddOperation(string operation)
        {
            calcData.Content += operation;
        }

        private void ChangeZnak(Label x)
        {
            //x.Content = ;
        }

        /// <summary>
        /// Перемещение панели мышкой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// Cвернуть окно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hide_MouseUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Закрыть окно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void opBtn_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label)
                AddOperation((sender as Label).DataContext.ToString());
        }

        private void wPanel_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Label x in (sender as WrapPanel).Children)
            {
                if (x.DataContext != null)
                    x.MouseUp += opBtn_Click;
            }
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
            if (calcData.Length > 0)
            {
                calcData = calcData.ToString().Substring(0, calcData.Length - 1);

                while (calcData.Length > 0 && !Char.IsDigit(calcData[calcData.Length - 1]) && Char.IsLetter(calcData[calcData.Length - 1]))
                {
                    calcData = calcData.ToString().Substring(0, calcData.Length - 1);
                }

            }
            return calcData;
        }
    }
}
