using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Modelagem
{
    /// <summary>
    /// Interação lógica para TarefaControl.xam
    /// </summary>
    public partial class TarefaView : UserControl
    {
        public static readonly DependencyProperty TarefaProperty = DependencyProperty.Register("Tarefa", typeof(Base.Tarefa), typeof(TarefaView));

        public Base.Tarefa Tarefa
        {
            get { return (Base.Tarefa)GetValue(TarefaProperty); }
            set { SetValue(TarefaProperty, value); }
        }
        public TarefaView()
        {
            InitializeComponent();
            this.DataContext = this;
        }


    }
}
