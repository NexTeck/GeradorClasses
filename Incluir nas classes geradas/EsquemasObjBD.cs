using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ClassesBD
{
    /// <summary>
    /// Classe que é enviada junta com os ObjetosBD, ela possui as informações
    /// dos esquemas dos Objetos, com a ordem deles também,
    /// se não configurar essa classe, todos ObjetoBD usados darão erro
    /// Essa classe foi gerada automaticamente pelo gerador de ObjetosBD no dia 01/11/2015 23:03:24
    /// </summary>
    public abstract class EsquemasObjBD
    {
        /// <summary>
        /// Os esquemas das tabelas usadas
        /// </summary>
        public static EsquemaTabela[] EsquemasTabelas { get; protected set; }

        /// <summary>
        /// As relações das tabelas usadas pelos esquemas
        /// </summary>
        public static RelacaoBD[] Relacoes { get; protected set; }

        /// <summary>
        /// Termina de configurar as relações
        /// </summary>

        /// <summary>
        /// Determina as relações nos objetos
        /// </summary>
        protected static void DefinirRelacoesTabelas()
        {
            List<RelacaoBD> relacoesPai;
            List<RelacaoBD> relacoesFilho;
            foreach (var es in EsquemasTabelas)
            {
                relacoesPai = new List<RelacaoBD>();
                relacoesFilho = new List<RelacaoBD>();
                for (int i = 0; i < Relacoes.Length; i++)
                {
                    if (Relacoes[i].IndexTabelaPai == es.IndiceTabela)
                    {
                        relacoesPai.Add(Relacoes[i]);
                    }
                    if (Relacoes[i].IndexTabelaFilha == es.IndiceTabela)
                    {
                        relacoesFilho.Add(Relacoes[i]);
                    }
                }
                if (relacoesPai.Count > 0)
                    es.RelacoesFilha = relacoesPai.ToArray();
                if (relacoesFilho.Count > 0)
                    es.RelacoesPai = relacoesFilho.ToArray();
            }
        }
    }
}