using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace TreeCalc.Gadgets
{
    /// <summary>
    /// Логика взаимодействия для Jurnal.xaml
    /// </summary>
    public partial class Jurnal : UserControl
    {
        /// <summary>
        /// Коллекция журнала
        /// </summary>
        public ObservableCollection<JurnalElement> JurnalElements { get; set; }

        Action<string> OnItemLeftClick = null;//Действие при нажатии левой кнопки мыши

        Action<string> OnItemRightClick = null;//Действие при нажатии правой кнопки мыши

        /// <summary>
        /// Элемент коллекции
        /// </summary>
        public class JurnalElement
        {
            public string Expression { get; set; }//Выражение
            public string Result { get; set; }//Результат

            /// <summary>
            /// Конструктор коллекции
            /// </summary>
            /// <param name="expression">Выражение</param>
            /// <param name="result">Результат</param>
            public JurnalElement(string expression, string result)
            {
                Expression = expression;
                Result = result;
            }
        }

        public Jurnal(Action<string> leftBtn, Action<string> rigthBtn)
        {
            InitializeComponent();
            OnItemLeftClick = leftBtn;
            OnItemRightClick = rigthBtn;
            JurnalElements = new ObservableCollection<JurnalElement>();
            jurnalList.ItemsSource = JurnalElements;
        }

        /// <summary>
        /// Добавление записи в начало коллекции
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="result"></param>
        public void AddNote(string expression, string result)
        {
            JurnalElements.Insert(0, new JurnalElement(expression, result));
        } 

        /// <summary>
        /// Обработчик события "Нажата правая кнопка мыши"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JurnalList_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            JurnalElement p = (JurnalElement)jurnalList.SelectedItem;
            OnItemRightClick.Invoke(p.Result);
        }

        /// <summary>
        /// Обработчик события "Нажата правая леая мыши"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JurnalList_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            JurnalElement p = (JurnalElement)jurnalList.SelectedItem;
            OnItemLeftClick.Invoke(p.Expression);
        }

        /// <summary>
        /// Нажата кнопка очистки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            JurnalElements.Clear();
        }
    }
}
