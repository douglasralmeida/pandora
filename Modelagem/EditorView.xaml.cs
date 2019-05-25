﻿using Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interação lógica para EditorView.xam
    /// </summary>
    public partial class EditorView : UserControl, INotifyPropertyChanged
    {
        private Editor _editor;

        private Objeto _objetoativo;

        public event PropertyChangedEventHandler PropertyChanged;

        public Objeto ObjetoAtivo
        {
            get
            {
                return _objetoativo;
            }

            set
            {
                if (_objetoativo != value)
                {
                    _objetoativo = value;
                    OnPropertyChanged("ObjetoAtivo");
                }
            }
        }

        public EditorView(Editor editor)
        {
            InitializeComponent();
            _editor = editor;
            _editor.TarefaAdded += Editor_TarefaAdded;
            _editor.PropertyChanged += Editor_PropertyChanged;
            _objetoativo = null;
            DataContext = _editor;
        }

        public void ArvoreTarefas_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (sender != null)
            {
                var treeView = sender as TreeView;
                if (treeView != null)
                {
                    var tarefa = treeView.SelectedItem as Tarefa;
                    if (tarefa != null)
                    {
                        exibirTarefa(tarefa);
                    }
                    else
                    {
                        if (treeView.SelectedItem != null && treeView.SelectedItem == treeView.Items.GetItemAt(0))
                            exibirTodasTarefas();
                    }
                }
            }
        }

        private void BtoInserirTarefa_Click(object sender, RoutedEventArgs e)
        {
            _editor.inserirTarefa();
        }

        private void BtoExcluirTarefa_Click(object sender, RoutedEventArgs e)
        {
            var tarefa = ArvoreTarefas.SelectedItem as Tarefa;
            if (tarefa != null)
            {
                exibirTodasTarefas();
                excluirTarefa(tarefa);
            }
        }

        public void Editor_TarefaAdded(object sender, Tarefa tarefa)
        {
            var item = ArvoreTarefas.Items.GetItemAt(0) as TreeViewItem;

            if (item != null)
            {
                var filho = item.ItemContainerGenerator.ContainerFromItem(tarefa) as TreeViewItem;
                if (filho != null)
                {
                    filho.IsSelected = true;
                }
            }
        }

        private void exibirProcesso(Tarefa processo)
        {
            //Paginas.Content = new ProcessoView(processo);
        }

        public void exibirTarefa(Tarefa tarefa)
        {
            Paginas.Content = new TarefaView(tarefa);
            ObjetoAtivo = tarefa;
        }

        public void exibirTodasTarefas()
        {
            Paginas.Content = new TodasTarefasView();
        }

        public void excluirTarefa(Tarefa tarefa)
        {
            _editor.excluirTarefa(tarefa);
        }

        private void Editor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //OnPropertyChanged(e.PropertyName);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}