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
        public string SetHead
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
            }
        }

        public Action onActivation;

        public JurnalBtn()
        {
            InitializeComponent();
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
            if(onActivation != null)
                onActivation.Invoke();
        }
    }
}
