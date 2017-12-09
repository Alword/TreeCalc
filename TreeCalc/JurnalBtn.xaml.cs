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
    /// Логика взаимодействия для JurnalBtn.xaml
    /// </summary>
    public partial class JurnalBtn : UserControl
    {
        public string Header
        {
            get
            {
                return headerTextBlock.Text;
            }
            set
            {
                headerTextBlock.Text = value;
            }
        }

        public bool IsActivated
        {
            get
            {
                return lineLablel.Height > 0;
            }
            set
            {
                lineLablel.Height = value ? 3 : 0;
                if (onActivation != null && value)
                    onActivation.Invoke();
            }
        }

        public Action onActivation;

        public JurnalBtn(string header, Action onAct)
        {
            InitializeComponent();
            Header = header;
            IsActivated = false;
            onActivation = onAct;
        }

        private void UserControl_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            IsActivated = true;
            if (Parent is Panel)
            {
                var hwnd = (Parent as Panel).Children;
                foreach (JurnalBtn x in hwnd)
                {
                    if (x != this)
                    {
                        x.IsActivated = false;
                    }
                }
            }
        }
    }
}
