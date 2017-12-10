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

        /// <summary>
        /// Конструктор интерфейса пользователя
        /// </summary>
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

        /// <summary>
        /// Очередь для расчёта
        /// </summary>
        public string Query
        {
            get => calcData.Text;
            set
            {
                calcData.Text = value.Replace(",", ".");
                FastDetermine();
            }
        }

        /// <summary>
        /// Результат вычислений
        /// </summary>
        public string Result
        {
            get => resultData.Content.ToString();
            set
            {
                resultData.Content = value.Replace(",", ".");
            }
        }

        /// <summary>
        /// Быстрые вычисления
        /// </summary>
        /// <returns>True - выражение возможно вычислить</returns>
        private bool FastDetermine()
        {
            string res = Result;
            bool didDetermine = Model.TryDetermine(Query, ref res);
            Result = res;
            return didDetermine;
        }

        /// <summary>
        /// Кнопка "Журнал"
        /// </summary>
        private void OnJurnalSelected()
        {
            GadgetGrid.Children.Clear();
            GadgetGrid.Children.Add(jurnal);
        }

        /// <summary>
        /// Кнопка "Память"
        /// </summary>
        private void OnMemSelected()
        {
            GadgetGrid.Children.Clear();
            GadgetGrid.Children.Add(memory);
        }

        /// <summary>
        /// Нажата правая кнопка мыши на элемент журнала
        /// </summary>
        /// <param name="result">Добавляет резултат в очередь вычислений</param>
        private void JurnalRightBtn(string result)
        {
            if (Model.IsAdditable(Query, result))
            {
                Query += result;
            }
        }

        /// <summary>
        /// Нажата левая кнопка мыши на элемент журнала
        /// </summary>
        /// <param name="query">Возвращает вычисления записанные в журнал</param>
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
                    if (Model.IsAdditable(Query, hwnd.DataContext.ToString()))
                    {
                        Query += hwnd.DataContext.ToString();
                    }
                    else
                    {
                        //TODO Notice
                    }
                }
            }
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
            var hwnd = ShiftBtn;
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

            if (source.Length > 0 && (char.IsNumber(source.Last()) || source.Last() == '∞' || source.Last() == ')'))
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
        private void ChangeZnackMouseUp(object sender, MouseButtonEventArgs e)
        {
            Result = Model.ChangeZnak(Result);
        }

        private void MemoryHandle(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label)
            {
                var hwnd = sender as Label;
                {
                    try
                    {
                        if (hwnd.Content.ToString() == "MC")//Очистка памяти
                        {
                            memory.MS = Operation.TryCalc($"0");
                        }
                        else if (hwnd.Content.ToString() == "MR")//Память в очередь вычислений
                        {
                            if (Model.IsAdditable(Query, memory.MS))
                            {
                                Query = memory.MS;
                            }
                        }
                        else if (hwnd.Content.ToString() == "M+")//Добавление в память
                        {
                            memory.MS = Operation.TryCalc($"({memory.MS})+({Result})");
                        }
                        else if (hwnd.Content.ToString() == "M-")//Удаление из памяти
                        {
                            memory.MS = Operation.TryCalc($"({memory.MS})-({Result})");
                        }
                        else if (hwnd.Content.ToString() == "MS")//Установить результат в память
                        {
                            memory.MS = Operation.TryCalc($"{Result}");
                        }
                    }
                    catch (Exception)
                    {
                        //TODO Log
                    }
                }
            }
        }

        /// <summary>
        /// Нажата левая кнопка мыши на результат
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResultData_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(Result);
        }

        /// <summary>
        /// Нажата левая кнопка мыши на выражение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalcData_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(Query);
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            string BtnContext = "";

            switch (e.Key)
            {

                case Key.NumPad0: BtnContext = "0"; break;
                case Key.NumPad1: BtnContext = "1"; break;
                case Key.NumPad2: BtnContext = "2"; break;
                case Key.NumPad3: BtnContext = "3"; break;
                case Key.NumPad4: BtnContext = "4"; break;
                case Key.NumPad5: BtnContext = "5"; break;
                case Key.NumPad6: BtnContext = "6"; break;
                case Key.NumPad7: BtnContext = "7"; break;
                case Key.NumPad8: BtnContext = "8"; break;
                case Key.NumPad9: BtnContext = "9"; break;

                case Key.D0: BtnContext = "0"; break;
                case Key.D1: BtnContext = "1"; break;
                case Key.D2: BtnContext = "2"; break;
                case Key.D3: BtnContext = "3"; break;
                case Key.D4: BtnContext = "4"; break;
                case Key.D5: BtnContext = "5"; break;
                case Key.D6: BtnContext = "6"; break;
                case Key.D7: BtnContext = "7"; break;
                case Key.D8: BtnContext = "8"; break;
                case Key.D9: BtnContext = "9"; break;

                case Key.Add: BtnContext = "+"; break;
                case Key.Multiply: BtnContext = "*"; break;
                case Key.Divide: BtnContext = "/"; break;
                case Key.Subtract: BtnContext = "-"; break;
                case Key.Decimal: BtnContext = "."; break;
                case Key.F: BtnContext = "fact("; break;

                case Key.OemPlus: BtnContext = "+"; break;
                case Key.OemMinus: BtnContext = "-"; break;
                case Key.OemQuestion: BtnContext = "/"; break;

                case Key.Return: EqualsMouseUp(sender, null); break;
                case Key.LeftShift: ShiftMouseUp(sender, null); break;
                case Key.RightShift: ShiftMouseUp(sender, null); break;
                case Key.Space: Breaketing_MouseUp(sender, null); break;
                case Key.Back: Backspace_MouseUp(sender, null); break;
                case Key.Escape: Label_MouseUp(sender, null); break;
            }

            if (Model.IsAdditable(Query, BtnContext))
            {
                Query += BtnContext;
            }
        }
    }

    public static class Model
    {
        //Список дополнительных функций
        public static List<string> shiftContent = new List<string>()
        {
                "sinh",
                "cosh",
                "tanh",
                "asinh",
                "acosh",
                "atanh"
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

        /// <summary>
        /// Проверка возможности добавить буфер к выражению
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static bool IsAdditable(string expression, string buffer)
        {
            bool result = false;
            if (buffer.Length > 0)
            {
                if (buffer == ".")
                {
                    result = IsDotCanBeAdded(expression);
                }
                else
                {
                    result = (ItCanBe(expression, buffer[0]) && IsOneDot(expression, buffer));
                }
            }
            return result;
        }

        /// <summary>
        /// Попытка вычислений
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryDetermine(string expression, ref string result)
        {
            bool canDetermine = (BraketsDiff(expression) == 0 && (char.IsNumber(expression.Last()) || expression.Last() == ')'));
            //Можно считать если //последня часть или чисто или )
            if (canDetermine)
            {
                result = GetAnswerForce(expression);
            }
            return canDetermine;
        }

        /// <summary>
        /// Удаление [C]
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string Backspace(string expression)
        {
            int lenght = expression.Length;
            if (lenght > 0)
            {
                do
                {
                    lenght--;

                } while (lenght > 0 && !Char.IsDigit(expression[lenght - 1]) && Char.IsLetter(expression[lenght - 1]));

            }
            return expression.Substring(0, lenght);
        }

        /// <summary>
        /// Замена знака результата
        /// </summary>
        /// <param name="calcData"></param>
        /// <returns></returns>
        public static string ChangeZnak(string result)
        {
            return GetAnswerForce($"{result}*(-1)");
        }

        /// <summary>
        /// Разница между открытыми и закрытыми скобками
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int BraketsDiff(string expression)
        {
            return new Regex(@"\(").Matches(expression).Count - new Regex(@"\)").Matches(expression).Count;
        }

        /// <summary>
        /// Получение ответа без проверок
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static string GetAnswerForce(string expression)
        {
            return Operation.TryCalc(expression);
        }

        /// <summary>
        /// Совместимость выражения и буфера
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static bool ItCanBe(string expression, char right)
        {
            bool itPossible = false;

            ///Если выражения ещё нет, то добавить можно -,функцию,бесконечность,скобку
            if (expression.Length == 0)
            {
                if ((right == '-' || char.IsLetterOrDigit(right)))
                {
                    itPossible = true;
                }
            }
            else
            {
                char left = expression.Last();

                //Если последний - ( => {-} или {a} или {num}
                //Если последний - {*x+-/} => {a} или num
                //Если последний - {num} => ({num} или {*x+-/}) не буква
                if (char.IsNumber(left) && !char.IsLetter(right))
                {
                    itPossible = true;
                }
                else if (!char.IsLetterOrDigit(left))
                {
                    if (left == '(' && (right == '-' || char.IsLetterOrDigit(right)))
                    {
                        itPossible = true;
                    }
                    else if (left == ')' || left == '∞')
                    {
                        if (!char.IsLetterOrDigit(right) && right != '∞')
                        {
                            itPossible = true;
                        }
                    }
                    else if (char.IsLetterOrDigit(right) || right == '∞')
                    {
                        itPossible = true;
                    }
                }
                else if (char.IsDigit(left) && !char.IsLetter(right) && right != '∞')
                {
                    itPossible = true;
                }

            }
            return itPossible;
        }

        /// <summary>
        /// Проверка возможности добавления числа с точкой
        /// </summary>
        /// <param name="source"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static bool IsOneDot(string source, string result)
        {
            bool isPosible = true;
            if (char.IsNumber(source.Last()) || '.' == source.Last())
            {
                isPosible = (IsDotCanBeAdded(source) || result.IndexOf('.') < 0);
            }
            return isPosible;
        }

        /// <summary>
        /// Проверка возможности поставить точку
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private static bool IsDotCanBeAdded(string query)
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
    }
}
public static class Extensions
{
    /// <summary>
    /// Последний символ строки
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static char Last(this string str)
    {
        return (str.Length == 0) ? '\n' : str[str.Length - 1];
    }

    /// <summary>
    /// Количество вхождений подстроки в строку
    /// </summary>
    /// <param name="s"></param>
    /// <param name="s0"></param>
    /// <returns></returns>
    public static int CountWords(this string s, string s0)
    {
        int count = (s.Length - s.Replace(s0, "").Length) / s0.Length;
        return count;
    }

}
