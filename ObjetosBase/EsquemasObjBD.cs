using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesBD
{
    /// <summary>
    /// Classe que é enviada junta com os ObjetosBD, ela possui as informações
    /// dos esquemas dos Objetos, com a ordem deles também,
    /// se não configurar essa classe, todos ObjetoBD usados darão erro
    /// </summary>
    public static class EsquemasObjBD
    {
        /// <summary>
        /// Os esquemas das tabelas usadas
        /// </summary>
        public static EsquemaTabela[] EsquemasTabelas { get; private set; }

        /// <summary>
        /// Os esquemas das tabelas usadas
        /// </summary>
        public static RelacaoBD[] Relacoes { get; private set; }

        /// <summary>
        /// Configura todos os esquemas das tabelas
        /// </summary>
        public static void ConfigurarEsquemas(EsquemaTabela[] esquemasTabelas, RelacaoBD[] relacoes)
        {
            EsquemasTabelas = esquemasTabelas;
            Relacoes = relacoes;
        }
    }
}
