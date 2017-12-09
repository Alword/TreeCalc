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
        Gadgets.Jurnal jurnal = null;
        Gadgets.Memory memory = null;

        public MainWindow()
        {
            new System.Threading.Thread(() => Operation.TryCalc("0")).Start();

            jurnal = new Gadgets.Jurnal(JurnalLeftBtn, JurnalRightBtn);

            memory = new Gadgets.Memory(this);

            InitializeComponent();

            BTNDockPanel.Children.Add(new JurnalBtn("Журнал", OnJurnalSelected));

            BTNDockPanel.Children.Add(new JurnalBtn("Память", OnMemSelected));

            (BTNDockPanel.Children[0] as JurnalBtn).IsActivated = true;

        }

        public string Query
        {
            get => calcData.Text;
            set
            {
                calcData.Text = value.Replace(",", ".");
                FastDetermine();
            }
        }

        public string Result
        {
            get => resultData.Content.ToString();
            set
            {
                resultData.Content = value;
            }
        }

        private bool FastDetermine()
        {
            string res = resultData.Content.ToString();
            bool didDetermine = Model.TryDetermine(Query, ref res);
            resultData.Content = didDetermine ? res : resultData.Content;
            return didDetermine;
        }

        private void OnJurnalSelected()
        {
            GadgetGrid.Children.Clear();
            GadgetGrid.Children.Add(jurnal);
        }

        private void OnMemSelected()
        {
            GadgetGrid.Children.Clear();
            GadgetGrid.Children.Add(memory);
        }

        private void JurnalRightBtn(string result)
        {
            if (Model.ItCanBe(Query, result[0]) && Model.IsOneDot(Query, result))
                Query += result;
        }

        private void JurnalLeftBtn(string query)
        {
            Query = query;
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

        /// <summary>
        /// Нажата другая кнопка или цифра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpBtn_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label)
            {
                var hwnd = sender as Label;
                if (hwnd.DataContext != null)
                {
                    if (Model.ItCanBe(Query, hwnd.DataContext.ToString()[0]))
                        Query += hwnd.DataContext.ToString();
                }//(hwnd.DataContext != null)
            }//(sender is Label
        }

        /// <summary>
        /// Нажата кнопка равно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EqualsMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (FastDetermine())
            {
                jurnal.AddNote(Query, Result);
                Query = Result;
            }
        }

        /// <summary>
        /// Нажата кнопка C
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Backspace_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Query = Model.Backspace(Query);
        }

        /// <summary>
        /// Нажата кнопка шифт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            i = 5; j = 3;
            do
            {
                (GridField.Children[i] as Label).Content = shiftContent[j];
                (GridField.Children[i] as Label).DataContext = shiftContext[j];
                j++;
                i++;
            } while (j < 6);
        }

        /// <summary>
        /// Нажата кнопка скобки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Breaketing_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string source = Query;

            int dif = Model.BraketsDiff(source);

            if (source.Length > 0 && (char.IsNumber(source[source.Length - 1]) || source[source.Length - 1] == ')'))
            {
                if (dif > 0)
                {
                    Query += ")";
                }
            }
            else
            {
                Query += "(";
            }
        }

        /// <summary>
        /// Нажата кнопка очистки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Query = "";
            Result = "";
        }

        /// <summary>
        /// Нажата кнопка смены знака
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            if (decimal.TryParse(Result, out decimal result))
            {
                Result = (result * -1).ToString();
            }
        }

        /// <summary>
        /// Нажата кнопка точка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseUp_2(object sender, MouseButtonEventArgs e)
        {
            if (Model.IsDotCanBeAdded(Query))
                Query += ".";
        }

        private void MemoryHandle(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label)
            {
                var hwnd = sender as Label;
                {
                    try
                    {
                        if (hwnd.Content.ToString() == "MC")
                        {
                            memory.MS = 0;
                        }
                        else if (hwnd.Content.ToString() == "MR")
                        {
                            if (Model.ItCanBe(Query, memory.MS.ToString()[0]) && Model.IsOneDot(Query, memory.MS.ToString()))
                                Query += memory.MS.ToString();
                        }
                        else if (hwnd.Content.ToString() == "M+")
                        {
                            memory.MS += Convert.ToDecimal(Result);
                        }
                        else if (hwnd.Content.ToString() == "M-")
                        {
                            memory.MS -= Convert.ToDecimal(Result);
                        }
                        else if (hwnd.Content.ToString() == "MS")
                        {
                            memory.MS = Convert.ToDecimal(Result);
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
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

        public static bool TryDetermine(string Query, ref string result)
        {
            bool canDetermine = (BraketsDiff(Query) == 0 && (char.IsNumber(Query.Last()) || Query.Last() == ')'));
            if (canDetermine)
            {
                result = Model.GetAnswer(Query);
            }
            return canDetermine;
        }

        public static bool ItCanBe(string source, char context)
        {
            bool itPossible = false;
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
            return itPossible;
        }

        public static bool IsOneDot(string source, string result)
        {
            bool isPosible = true;
            if (char.IsNumber(source.Last()) || '.' == source.Last())
            {
                isPosible = (IsDotCanBeAdded(source) || result.IndexOf(',') < 0);
            }
            return isPosible;
        }

        public static bool IsDotCanBeAdded(string query)
        {
            int numLen = 0;
            int i = query.Length - 1;
            while (i > -1 && char.IsNumber(query[i]))
            {
                numLen++;
                i--;
            }
            return (numLen > 0 && ((i > -1 && query[i] != '.') || (i < 0)));
        }

        private static string ChangeZnak(string calcData)
        {
            return calcData;
        }

        public static int BraketsDiff(string source)
        {
            return new Regex(@"\(").Matches(source).Count - new Regex(@"\)").Matches(source).Count;
        }

        private static string GetAnswer(string data)
        {
            return Operation.TryCalc(data);
        }

    }
}
public static class Extensions
{
    public static char Last(this string str)
    {
        return (str.Length == 0) ? '\n' : str[str.Length - 1];
    }
}
