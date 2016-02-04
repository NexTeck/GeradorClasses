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

        /// <summary>
        /// Faz com que o objeto atual possa acessar os valores dos campos do ObjetoBD como um vetor
        /// com string
        /// </summary>
        public CampoBD this[string nomeCampo]
        {
            get
            {
                for (int i = 0; i < campos.Length; i++)
                {
                    if (campos[i].NomeCampo == nomeCampo)
                        return campos[i];
                }
                return null;
            }
        }

        /// <summary>
        /// Faz com que o objeto atual possa acessar os valores dos campos do ObjetoBD como um vetor
        /// </summary>
        public CampoBD this[int numeroCampo]
        {
            get { return campos[numeroCampo]; }
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
}
