using Execucao;
using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaPadrao
{
    public class Prisma : Modulo
    {
        public Prisma(): base("Prisma")
        {

        }

        public override void adicionarComandos()
        {
            base.adicionarComandos();
        }

        public override void adicionarConstNecessarias()
        {
            base.adicionarConstNecessarias();

            ConstantesNecessarias.Add("PRISMA_MAQUINA", new ConstanteInfo("Nome da máquina Prisma para acesso", true, false, true));
            ConstantesNecessarias.Add("PRISMA_OL", new ConstanteInfo("OL de acesso ao Prisma", true, false, true));
            ConstantesNecessarias.Add("PRISMA_SENHA", new ConstanteInfo("Senha de acesso ao Prisma", true, true, true));

            ConstantesNecessarias.Add("PRISMA_EXE", new ConstanteInfo("Nome do arquivo executável do Accuterm", false, true, true));
            ConstantesNecessarias.Add("PRISMA_LOCAL", new ConstanteInfo("Diretório dos arquivos de execução do Accuterm", false, true, true));
        }
    }
}
