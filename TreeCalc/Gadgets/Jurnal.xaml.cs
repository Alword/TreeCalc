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
        public ObservableCollection<JurnalElement> JurnalElements { get; set; }
        Action<string> OnItemLeftClick = null;
        Action<string> OnItemRightClick = null;

        public class JurnalElement
        {
            public string Expression { get; set; }
            public string Result { get; set; }
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

        public void AddNote(string expression, string result)
        {
            JurnalElements.Insert(0, new JurnalElement(expression, result));
        } 

        private void JurnalList_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            JurnalElement p = (JurnalElement)jurnalList.SelectedItem;
            OnItemRightClick.Invoke(p.Result);
        }

        private void JurnalList_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            JurnalElement p = (JurnalElement)jurnalList.SelectedItem;
            OnItemLeftClick.Invoke(p.Expression);
        }
    }
}
