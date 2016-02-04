using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesBD
{
    /// <summary>
    /// Um esquema de tabela com seus atributos do banco de dados.
    /// </summary>
    public class EsquemaTabela
    {
        /// <summary>
        /// Indica se o esquema não pode mais ser alterado
        /// </summary>
        private bool definido = false;
        int indiceTabela = -1;
        private string nomeTabela;
        private CampoBD[] campos;
        private int indexColunaPK;
        RelacaoBD[] relacoesPai;
        RelacaoBD[] relacoesFilha;

        #region Propriedades
        /// <summary>
        /// O nome da tabela do esquema atual
        /// </summary>
        public string NomeTabela
        {
            get { return nomeTabela; }
            set
            {
                if (!definido)
                    nomeTabela = value;
                else
                    throw new Exception("Não pode alterar o esquema da tabela uma vez definido");
            }
        }

        /// <summary>
        /// O indice do esquema atual
        /// </summary>
        public int IndiceTabela
        {
            get { return indiceTabela; }
            set
            {
                if (!definido)
                    indiceTabela = value;
                else
                    throw new Exception("Não pode alterar o esquema da tabela uma vez definido");
            }
        }

        /// <summary>
        /// Os campos dessa tabela atual
        /// </summary>
        public CampoBD[] Campos
        {
            get { return campos; }
            set 
            {
                if (!definido)
                    campos = value;
                else
                    throw new Exception("Não pode alterar o esquema da tabela uma vez definido");
            }
        }

        /// <summary>
        /// O número da coluna da PrimaryKey
        /// </summary>
        public int IndexColunaPK
        {
            get { return indexColunaPK; }
            set
            {
                if (!definido)
                    indexColunaPK = value;
                else
                    throw new Exception("Não pode alterar o esquema da tabela uma vez definido");
            }
        }

        /// <summary>
        /// As relações pai dessa
        /// </summary>
        public RelacaoBD[] RelacoesPai
        {
            get { return relacoesPai; }
            set
            {
                if (!definido)
                    relacoesPai = value;
                else
                    throw new Exception("Não pode alterar o esquema da tabela uma vez definido");
            }
        }

        /// <summary>
        /// As relações filha dessa
        /// </summary>
        public RelacaoBD[] RelacoesFilha
        {
            get { return relacoesFilha; }
            set
            {
                if (!definido)
                    relacoesFilha = value;
                else
                    throw new Exception("Não pode alterar o esquema da tabela uma vez definido");
            }
        }
        #endregion

        /// <summary>
        /// O campo da Primary Key
        /// </summary>
        public CampoBD CampoPK
        {
            get { return Campos[indexColunaPK]; }
        }

        public EsquemaTabela(string nomeTabela_)
        {
            nomeTabela = nomeTabela_;
        }

        public void Finalizar()
        {
            definido = true;
        }
    }

    /// <summary>
    /// Classe que possui os atributos básicos de um campo no banco de dados
    /// </summary>
    public class CampoBD
    {
        /// <summary>
        /// A ordem desse campo no ObjetoBD
        /// </summary>
        public int OrdemCampo { get; private set; }
        /// <summary>
        /// O nome do campo no banco de dados
        /// </summary>
        public string NomeCampo { get; private set; }
        /// <summary>
        /// Indica se permite nulos
        /// </summary>
        public bool PermiteNull { get; private set; }
        /// <summary>
        /// O tipo do campo no banco de dados
        /// </summary>
        public System.Data.SqlDbType TipoVariavelBD { get; private set; }
        /// <summary>
        /// O tipo do campo na variável
        /// </summary>
        public Type TipoVariavel { get; private set; }
        /// <summary>
        /// Tamanho quando é do tipo varchar ou outro
        /// </summary>
        public int MaxLenght { get; private set; }
        /// <summary>
        /// Tamanho quando é do tipo varchar ou outro
        /// </summary>
        public TipoCampo TipoCampo { get; set; }

        /// <summary>
        /// Declara o objeto e define seus campos
        /// </summary>
        public CampoBD(int ordemCampo, string nomeCampo, bool permiteNull, System.Data.SqlDbType tipoBD, Type tipo, int maxLenght, TipoCampo tipoCampo)
        {
            OrdemCampo = ordemCampo;
            NomeCampo = nomeCampo;
            PermiteNull = permiteNull;
            TipoVariavelBD = tipoBD;
            TipoVariavel = tipo;
            MaxLenght = maxLenght;
            TipoCampo = tipoCampo;
        }
    }

    /// <summary>
    /// Classe que possui os atributos básicos de um campo no banco de dados
    /// </summary>
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

    /// <summary>
    /// Indica o tipo de um campo
    /// </summary>
    public enum TipoCampo
    {
        /// <summary> É um campo comum </summary>
        Comum = 0,
        /// <summary> É uma chave primária </summary>
        PrimaryKey,
        /// <summary> É uma chave unica </summary>
        Unique,
        /// <summary> É uma chave estrangeira </summary>
        ForeignKey,
        /// <summary> É uma chave primária e também uma chave estrangeira </summary>
        ForeignKey_PrimaryKey,
        /// <summary> É uma chave estrangeira e também unica </summary>
        ForeignKey_Unique
    }
}
