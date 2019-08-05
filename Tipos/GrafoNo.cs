using System;
using System.Collections.Generic;
using System.Text;

namespace Tipos
{
    public class GrafoNo<T>: Node<T>
    {
        private List<(int, GrafoNo<T>)> arestas;

        public GrafoNo() : base() { }
        public GrafoNo(T valor) : base(valor) { }
        public GrafoNo(T value, NodeList<T> vizinhos) : base(value, vizinhos) { }
    }

    new public NodeList<T> Vizinhos
    {
        get
        {
            if (base.Vizinhos == null)
                base.Vizinhos = new NodeList<T>();

            return base.Vizinhos;
        }
    }

    public List<(int, )> Arestas
    {
        get
        {
            if (arestas == null)
                arestas = new List<(int, GrafoNo<T>)>();

            return arestas;
        }
    }
}
