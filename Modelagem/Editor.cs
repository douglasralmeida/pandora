﻿using Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Modelagem
{
    public class Editor : INotifyPropertyChanged
    {
        private const string ARQUIVO_SEMNOME = "sem nome.pandorapac";

        private string _nomeArquivo;

        private bool _modificado;

        private Pacote _pacote;

        public event PropertyChangedEventHandler PropertyChanged;

        public event TarefaAddedEventHandler TarefaAdded;

        public bool Modificado
        {
            get
            {
                return _modificado;
            }
            set
            {
                if (_modificado != value)
                {
                    _modificado = value;
                    OnPropertyChanged("Modificado");
                }
            }
        }

        public string NomeArquivo
        {
            get
            {
                return _nomeArquivo;
            }
            set
            {
                _nomeArquivo = value;
                OnPropertyChanged("NomeArquivo");
            }
        }

        public ObservableCollection<Processo> Processos
        {
            get
            {
                return _pacote.Processos;
            }
        }

        public ObservableCollection<Tarefa> Tarefas
        {
            get
            {
                return _pacote.Tarefas;
            }
        }

        public Editor()
        {
            _modificado = false;
        }

        public bool abrir(string nomearquivo)
        {
            XElement xml;

            xml = XElement.Load(nomearquivo);
            if (processarXML(xml))
            {
                _nomeArquivo = nomearquivo;
                _modificado = false;
                OnPropertyChanged(null);
                OnPropertyChanged("NomeArquivo");
                OnPropertyChanged("Modificado");
                return true;
            }

            return false;
        }

        public void excluirTarefa(Tarefa tarefa)
        {
            _pacote.excluirTarefa(tarefa);
        }

        private XElement gerarXML()
        {
            XElement xml;

            xml = _pacote.gerarXml();

            return xml;
        }

        public TreeViewItem getArvoreProcessos()
        {
            TreeViewItem item;

            item = new TreeViewItem();
            item.Header = "Todas os processos";

            return item;
        }

        public TreeViewItem getArvoreTarefas()
        {
            TreeViewItem item;

            item = new TreeViewItem();
            item.Header = "Todas as tarefas";
            foreach (Tarefa nome in _pacote.Tarefas)
            {
                item.Items.Add(nome);
            }
            return item;
        }

        public void inserirProcesso()
        {
            _pacote.inserirProcesso();
        }

        public void inserirTarefa()
        {
            _pacote.inserirTarefa();
        }

        public void novo(string usuarioGerador)
        {
            _pacote = new Pacote(usuarioGerador);
            _pacote.PropertyChanged += Pacote_PropertyChanged;
            _pacote.TarefaAdded += Pacote_TarefaAdded;
            _nomeArquivo = ARQUIVO_SEMNOME;
            _modificado = true;
            OnPropertyChanged(null);
            OnPropertyChanged("NomeArquivo");
            OnPropertyChanged("Modificado");
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnTarefaAdded(object sender, Base.Tarefa tarefa)
        {
            TarefaAdded?.Invoke(this, tarefa);
        }

        private void Pacote_TarefaAdded(object sender, Base.Tarefa tarefa)
        {
            OnTarefaAdded(sender, tarefa);
        }

        private void Pacote_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Tarefas")
                OnPropertyChanged(e.PropertyName);
            if (e.PropertyName == "Processos")
                OnPropertyChanged(e.PropertyName);
            Modificado = true;
        }

        private bool processarXML(XElement xml)
        {
            _pacote = new Pacote(xml);
            _pacote.PropertyChanged += Pacote_PropertyChanged;
            _pacote.TarefaAdded += Pacote_TarefaAdded;

            return true;
        }

        public void salvar(string nomeArquivo)
        {
            XElement xml;

            xml = gerarXML();
            xml.Save(nomeArquivo);
            NomeArquivo = nomeArquivo;
            Modificado = false;
        }
    }
}
