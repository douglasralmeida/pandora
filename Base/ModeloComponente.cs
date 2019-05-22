using System;
using System.Collections.Generic;
using System.Text;

namespace Base
{
    //
    // Resumo:
    //     Representa o método que manipulará o evento Base.Pacote.TarefaAdded
    //     gerado quando uma tarefa é adicionada no pacote.
    //
    // Parâmetros:
    //   sender:
    //     A fonte do evento.
    //
    //   tarefa:
    //     A tarefa adicionada que gerou o evento.
    public delegate void TarefaAddedEventHandler(object sender, Tarefa tarefa);
}
