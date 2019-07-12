using System;
using System.Collections.Generic;
using System.Text;

namespace Base
{
    public enum AtividadeFase
    {
        FaseNormal,
        FasePre,
        FasePos
    }

    public class Atividade
    {
        public Atividade(Objeto objeto)
        {
            Fase = AtividadeFase.FaseNormal;

            ObjetoRelacionado = objeto;
        }

        public AtividadeFase Fase { get; set; }

        public string Nome { get => ObjetoRelacionado.Nome; }

        public Objeto ObjetoRelacionado { get; private set; }
    }
}
