using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Execucao
{
    /* bool -> funcao executou corretamente
     * string -> parametros da funcao
     */
    public delegate (bool, string) Funcao(Dictionary<string, dynamic> constantes, ObservableCollection<Variavel> parametros);

    public class Comando
    {
        private Funcao _funcao;

        public Dictionary<string, dynamic> Constantes { get; private set; }

        public int Espera { get; private set; }

        public string Nome
        {
            get; set;
        }

        public ObservableCollection<Variavel> Parametros { get; private set; }

        public int ParamObrigatoriosCont
        {
            get
            {
                int i = 0;
                foreach (Variavel v in Parametros)
                {
                    if (!v.Opcional)
                        i++;
                }

                return i;
            }
        }

        public string Retorno { get; private set; }

        public Comando(string nome, Funcao funcao)
        {
            Nome = nome;
            Constantes = null;
            _funcao = funcao;
            Espera = 0;
            Parametros = new ObservableCollection<Variavel>();
            Retorno = null;
        }

        public async Task<bool> executarAsync(Dictionary<string, dynamic> constantes, int atraso)
        {
            int espera;
            bool executou;

            if (atraso > 0)
                espera = atraso;
            else
                espera = Espera;

            (executou, Retorno) = _funcao(constantes, Parametros);
            if (espera > 0)
                await Task.Delay(espera * 1000);

            return executou;
        }

        public override string ToString()
        {
            return Nome + string.Join(", ", Parametros);
        }
    }
}
