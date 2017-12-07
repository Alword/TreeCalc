using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        //string calcData.Content;
        public MainWindow()
        {
            void warmingUp()
            {
                Operation.TryCalc("0");
            }
            new System.Threading.Thread(warmingUp).Start();
            InitializeComponent();

            jurnalBTN.SetHead = "Журнал";
            jurnalBTN.IsActivated = true;

            meoryBTN.SetHead = "Память";
            meoryBTN.IsActivated = false;
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

        private void OpBtn_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label)
            {
                var hwnd = sender as Label;
                bool itPossible = false;

                if (hwnd.DataContext != null)
                {
                    string source = calcData.Content.ToString();
                    char context = hwnd.DataContext.ToString()[0];

                    if (source.Length == 0)
                    {
                        if ((context == '-' || char.IsLetterOrDigit(context)))
                        {
                            itPossible = true;
                        }
                    }
                    else
                    {
                        char last = source[source.Length - 1];

                        //Если последний - ( => {-} или {a} или {num}
                        //Если последний - {*x+-/} => {a} или num
                        //Если последний - {num} => ({num} или {*x+-/}) не буква
                        if (char.IsNumber(last) && !char.IsLetter(context))
                        {
                            itPossible = true;
                        }
                        else if (!char.IsLetterOrDigit(last))
                        {
                            if (last == '(' && (context == '-' || char.IsLetterOrDigit(context)))
                            {
                                itPossible = true;
                            }
                            else
                            if (last == ')' && (!char.IsLetterOrDigit(context)))
                            {
                                itPossible = true;
                            }
                            else
                            if (last != ')' && last != '(' && char.IsLetterOrDigit(context))
                            {
                                itPossible = true;
                            }
                        }
                        else if (char.IsDigit(last) && !char.IsLetter(context))
                        {
                            itPossible = true;
                        }

                    }
                    if (itPossible)
                        AddOperation(hwnd.DataContext.ToString());

                }//(hwnd.DataContext != null)
            }//(sender is Label)

            void AddOperation(string operation)
            {
                calcData.Content += operation;
            }
        }//OpBtn_Click(

        private void EqualsMouseUp(object sender, MouseButtonEventArgs e)
        {
            string source = calcData.Content.ToString();
            if (Model.BraketsDiff(source) == 0)
            {
                resultData.Content = Model.Equals(source);
            }
        }

        private void Backspace_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label)
            {
                var hwdl = calcData;
                hwdl.Content = Model.Backspace(hwdl.Content.ToString());
            }
        }

        private void ShiftMouseUp(object sender, MouseButtonEventArgs e)
        {
            var hwnd = (sender as Label);
            List<string> shiftContent = Model.shiftContent;
            List<string> shiftContext = Model.shiftContext;


            int i = 0;
            int j = 0;
            if ((string)hwnd.Content == "↓")
            {
                shiftContent = Model.beforContent;
                shiftContext = Model.beforContext;
                hwnd.Content = "↑";
            }
            else
            {
                hwnd.Content = "↓";
            }
            do
            {
                (GridField.Children[i] as Label).Content = shiftContent[i];
                (GridField.Children[i] as Label).DataContext = shiftContext[i];
                i++;
            } while (i < 3);

            i = 5;
            do
            {
                (GridField.Children[i] as Label).Content = shiftContent[j];
                (GridField.Children[i] as Label).DataContext = shiftContext[j];
                j++;
                i++;
            } while (j < 3);
        }

        private void Breaketing_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string source = (string)calcData.Content.ToString();

            int dif = Model.BraketsDiff(source);

            if (source.Length > 0 && (char.IsNumber(source[source.Length - 1]) || source[source.Length - 1] == ')'))
            {
                if (dif > 0)
                {
                    calcData.Content += ")";
                }
            }
            else
            {
                calcData.Content += "(";
            }
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            calcData.Content = "";
        }
    }

    public static class Model
    {
        public static List<string> shiftContent = new List<string>()
        {
                "Sinh",
                "Cosh",
                "Tanh",
                "Asinh",
                "Acosh",
                "Atanh"
        };
        public static List<string> shiftContext = new List<string>()
            {
                "sinh(",
                "cosh(",
                "tanh(",
                "asinh(",
                "acosh(",
                "atanh("
            };
        public static List<string> beforContent = new List<string>()
            {
                "sin",
                "cos",
                "tan",
                "asin",
                "acos",
                "atan"
            };
        public static List<string> beforContext = new List<string>()
            {
                "sin(",
                "cos(",
                "tan(",
                "asin(",
                "acos(",
                "atan("
            };


        public static string Backspace(string calcData)
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

        public static string Equals(string data)
        {
            return Operation.TryCalc(data);
        }

        private static string ChangeZnak(string calcData)
        {
            return calcData;
        }

        public static int BraketsDiff(string source)
        {
            return new Regex(@"\(").Matches(source).Count - new Regex(@"\)").Matches(source).Count;
        }
    }
}
