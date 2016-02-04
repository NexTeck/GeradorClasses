using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesBD
{
    /// <summary>
    /// Classe que possui os atributos básicos de um campo no banco de dados
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("ConstraintName = {ConstraintName}")]
    public class RelacaoBD
    {
        /// <summary>
        /// O nome da relação no banco de dados
        /// </summary>
        public string ConstraintName { get; private set; }
        /// <summary>
        /// Indica o nome da coluna filha
        /// </summary>
        public string ColunaFilha { get; private set; }
        /// <summary>
        /// Indica o nome da coluna filha
        /// </summary>
        public string ColunaPai { get; private set; }
        /// <summary>
        /// Indica o nome da coluna filha
        /// </summary>
        public string TabelaFilha { get; private set; }
        /// <summary>
        /// Indica o nome da coluna filha
        /// </summary>
        public string TabelaPai { get; private set; }

        /// <summary>
        /// Indica o nome da coluna filha
        /// </summary>
        public int IndexColunaFilha { get; set; }
        /// <summary>
        /// Indica o nome da coluna filha
        /// </summary>
        public int IndexColunaPai { get; set; }
        /// <summary>
        /// Indica o nome da coluna filha
        /// </summary>
        public int IndexTabelaFilha { get; set; }
        /// <summary>
        /// Indica o nome da coluna filha
        /// </summary>
        public int IndexTabelaPai { get; set; }

        public EsquemaTabela EsquemaPai { get; set; }
        public EsquemaTabela EsquemaFilho { get; set; }

        /// <summary>
        /// Declara o objeto e define seus campos
        /// </summary>
        public RelacaoBD(string tabelaFilha, string colunaFilha, string tabelaPai, string colunaPai, string constraintName)
        {
            TabelaFilha = tabelaFilha;
            ColunaFilha = colunaFilha;
            TabelaPai = tabelaPai;
            ColunaPai = colunaPai;
            ConstraintName = constraintName;
        }

        /// <summary>
        /// Declara o objeto e define seus campos já definindo o esquema de tabela enviado
        /// </summary>
        public RelacaoBD(string constraintName, int indexColunaFilha, int indexColunaPai, EsquemaTabela esquemaFilho, EsquemaTabela esquemaPai)
        {
            ConstraintName = constraintName;
            IndexColunaFilha = indexColunaFilha;
            IndexColunaPai = indexColunaPai;
            EsquemaPai = esquemaPai;
            EsquemaFilho = esquemaFilho;
            TabelaPai = esquemaPai.NomeTabela;
            TabelaFilha = esquemaFilho.NomeTabela;
            ColunaPai = esquemaPai.Campos[indexColunaPai].NomeCampo;
            ColunaFilha = esquemaFilho.Campos[indexColunaFilha].NomeCampo;
            IndexTabelaPai = esquemaPai.IndiceTabela;
            IndexTabelaFilha = esquemaFilho.IndiceTabela;
        }

        /// <summary>
        /// Pega os indices das colunas
        /// Deve ter definido o esquema das tabelas
        /// </summary>
        public void PegarIndicesTabelas()
        {
            EsquemaTabela[] esquemas = EsquemasObjBD.EsquemasTabelas;
            for (int i = 0; i < esquemas.Length; i++)
            {
                if (esquemas[i].NomeTabela == TabelaPai)
                {
                    IndexTabelaPai = i;
                    EsquemaPai = esquemas[i];
                }
                if (esquemas[i].NomeTabela == TabelaFilha)
                {
                    IndexTabelaFilha = i;
                    EsquemaFilho = esquemas[i];
                }
            }
        }

        /// <summary>
        /// Pega os indices das colunas
        /// Deve ter pego os indices das tabelas
        /// </summary>
        public void PegarIndicesColunas()
        {
            CampoBD[] camposPai = EsquemasObjBD.EsquemasTabelas[IndexTabelaPai].Campos;
            CampoBD[] camposFilho = EsquemasObjBD.EsquemasTabelas[IndexTabelaFilha].Campos;
            for (int i = 0; i < camposPai.Length; i++)
            {
                if (camposPai[i].NomeCampo == ColunaPai)
                {
                    IndexColunaPai = i;
                    break;
                }
            }
            for (int i = 0; i < camposFilho.Length; i++)
            {
                if (camposFilho[i].NomeCampo == ColunaFilha)
                {
                    IndexColunaFilha = i;
                    break;
                }
            }
        }
    }
}
