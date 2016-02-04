using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClassesBD
{
    /* Informações da classe:
     * Pode acessar ou definir os valores das classes de quatro formas:
     * cliente.ClienteID = 1
     * cliente[0] = 1
     * cliente["ClienteID"]
     * cliente.PegarValor(0) -- Mais rápida
     * 
     * Pode-se também acessar as configurações dos campos do objeto de três formas:
     * cliente.PegarCampo(0)
     * cliente.PegarCampo("ClienteID")
     * tbCliente.CamposBD[0] -- Mais rápida
     * 
     * Pelo campoBD pode acessar as informações do campos como: Nome, Permitenulos, TipoBD, TipoC#, TamanhoTexto
     * 
     * Sempre que um objeto é criado ele vem com os parametros de nova classe
     * Pode definir como inserida no banco de dados, assim a classe passa a não poder 
     * modificar os atributos. Uma classe que já foi inserida pode dar BeginEdit para iniciar a
     * edição dos valores dos campos e dar RollBack ou Commit a qualquer momento
     * 
     * Pode-se também ativar Binding na classe fazendo com que ela avise que os campos
     * foram mudados.
     * 
     * 
     * */
    /// <summary>
    /// Classe usada para determinar os objetos usados no banco de dados
    /// Andrei 29/10/2015
    /// </summary>

    [System.Diagnostics.DebuggerDisplay("Tabela = {this.GetType().Name}")]
    public abstract class ObjetoBD
    {
        #region Campos, Propriedades e Eventos
        /// <summary>
        /// Campo que serve para identificar qual é o indice da tabela do ObjetoBD atual
        /// </summary>
        public abstract int IndexTabela { get; }

        /// <summary>
        /// Os valores das variáveis de todos atributos
        /// </summary>
        private object[] valoresAtributos;

        /// <summary>
        /// Os objetos pai que são acessados por PegarObjPai("NomePai") e definidos DefinirObjPai(ObjetoBD)
        /// </summary>
        private ObjetoBD[] objetosPai;

        /// <summary>
        /// As listas dos objetos filhos que são acessados por PegarObjFilho("NomeFilho") e definidos DefinirObjFilho(List<ObjetoBD>)
        /// Pode definir mais um por AddObjFilho(ObjetoBD)
        /// </summary>
        private List<ObjetoBD>[] objetosFilhos;

        /// Objeto que possui todas as características da edição caso seja ativada
        private EdicaoObjetoBD edicao;

        /// <summary>
        /// Indica se é um novo objeto ou se é um enviado do banco de dados
        /// </summary>
        public TipoObjetoBD TipoObjeto { get; protected set; }


        /// <summary>
        /// Os esquemas das tabelas
        /// </summary>
        private EsquemaTabela esquema;
        /// <summary>
        /// Os esquemas das tabelas
        /// </summary>
        private CampoBD[] campos;
        /// <summary>
        /// Os esquemas das tabelas
        /// </summary>
        public EsquemaTabela EsquemaTabela
        {
            get { return esquema; }
        }

        /// <summary>
        /// Retorna todos os indices dos itens editados
        /// </summary>
        /// <returns></returns>
        public int[] ItensEditados
        {
            get
            {
                if (edicao != null)
                    return edicao.PegarVetorValoresEditados();
                else
                    return null;
            }
        }

        /// <summary>
        /// Retorna se teve alguma modificação
        /// </summary>
        public bool FoiModificado
        {
            get 
            {
                if (edicao != null)
                    return edicao.FoiModificado;
                else
                    return false;
            }
        }

        /// <summary>
        /// Evento de quando qualquer propriedade muda
        /// </summary>
        public event PropriedadeMudouHandler PropriedadeMudou;
        public delegate void PropriedadeMudouHandler(int coluna, object valor);
        #endregion

        /// <summary>
        /// Construtor do objeto
        /// </summary>
        public ObjetoBD()
        {
            esquema = EsquemasObjBD.EsquemasTabelas[IndexTabela];
            campos = esquema.Campos;
            DefinirNovo();
        }

        /// <summary>
        /// Define o objeto como um novo objeto
        /// </summary>
        public void DefinirNovo()
        {
            TipoObjeto = TipoObjetoBD.Novo;
            valoresAtributos = new object[campos.Length];
            objetosPai = new ObjetoBD[esquema.RelacoesPai.Length];
            objetosFilhos = new List<ObjetoBD>[esquema.RelacoesFilha.Length];
            //Define os valores padrão
            for (int i = 0; i < campos.Length; i++)
            {
                valoresAtributos[i] = campos[i].RetornarValorPadrao();
            }
            edicao = new EdicaoObjetoBD(valoresAtributos);
        }

        #region Funções get, set
        /// <summary>
        /// Faz com que o objeto atual possa acessar os valores dos campos do ObjetoBD como um vetor
        /// com string
        /// </summary>
        public object this[string nomeCampo]
        {
            get
            {
                for (int i = 0; i < esquema.Campos.Length; i++)
                {
                    if (campos[i].NomeCampo == nomeCampo)
                        return PegarValor(i);
                }
                return null;
            }

            set
            {
                for (int i = 0; i < campos.Length; i++)
                {
                    if (campos[i].NomeCampo == nomeCampo)
                        DefinirValor(i, value);
                }
            }
        }

        /// <summary>
        /// Faz com que o objeto atual possa acessar os valores dos campos do ObjetoBD como um vetor
        /// </summary>
        public object this[int numeroCampo]
        {
            get { return PegarValor(numeroCampo); }
            set { DefinirValor(numeroCampo, value); }
        }

        /// <summary>
        /// Pega o valor de uma coluna específicado pelo número
        /// </summary>
        public object PegarValor(int coluna)
        {
            if (TipoObjeto != TipoObjetoBD.PertenceBD)
                return edicao.ValoresEditados[coluna];
            else
                return valoresAtributos[coluna];
        }

        /// <summary>
        /// Define o valor de um campo qualquer,
        /// esse método é sempre chamado para qualquer campo modificado
        /// </summary>
        /// <param name="coluna">a coluna do vetor valoresAtributos</param>
        /// <param name="valor">o valor que será modificado</param>
        public void DefinirValor(int coluna, object valor)
        {
            if (TipoObjeto == TipoObjetoBD.PertenceBD)
                throw new Exception("Não se pode definir um valor a um objeto a não ser que ele seja uma novo ou esteja habilitado a edição");
            // Se o valor é igual ao anterior, então retorna para não definir no binding
            if (!edicao.DefinirValor(coluna, valor))
                return;
            //Chama os eventos que estiverem ligados ao campo atual
            if (PropriedadeMudou != null)
                PropriedadeMudou(coluna, valor);
        }

        /// <summary>
        /// Pega o valor de uma coluna específicado pelo número
        /// </summary>
        protected ObjetoBD PegarObjPai(int numeroPai)
        {
            return objetosPai[numeroPai];
        }

        /// <summary>
        /// Define o valor de um campo qualquer,
        /// esse método é sempre chamado para qualquer campo modificado
        /// </summary>
        /// <param name="coluna">a coluna do vetor valoresAtributos</param>
        /// <param name="valor">o valor que será modificado</param>
        protected void DefinirObjPai(int numeroPai, ObjetoBD valor)
        {
            objetosPai[numeroPai] = valor;
        }

        /// <summary>
        /// Pega o valor de uma coluna específicado pelo número
        /// </summary>
        protected List<ObjetoBD> PegarObjetosFilho(int numeroFilhos)
        {
            return objetosFilhos[numeroFilhos];
        }

        /// <summary>
        /// Define o valor de um campo qualquer,
        /// esse método é sempre chamado para qualquer campo modificado
        /// </summary>
        /// <param name="coluna">a coluna do vetor valoresAtributos</param>
        /// <param name="valor">o valor que será modificado</param>
        protected void DefinirObjetosFilho(int numeroFilhos, List<ObjetoBD> valor)
        {
            objetosFilhos[numeroFilhos] = valor;
        }

        /// <summary>
        /// Define o valor de um campo qualquer,
        /// esse método é sempre chamado para qualquer campo modificado
        /// </summary>
        /// <param name="filho">O filho que será adicionado</param>
        public void AddFilho(ObjetoBD filho)
        {
            int index = -1;
            for (int i = 0; i < esquema.RelacoesFilha.Length; i++)
            {
                if (esquema.RelacoesFilha[i].IndexTabelaFilha == filho.IndexTabela)
                    index = i;
            }
            if (objetosFilhos[index] == null)
                objetosFilhos[index] = new List<ObjetoBD>();
            objetosFilhos[index].Add(filho);
        }
        #endregion

        #region Edição
        /// <summary>
        /// Define que o objeto poderá ser editado
        /// </summary>
        public void BeginEdit()
        {
            if (TipoObjeto == TipoObjetoBD.SendoEditado || TipoObjeto == TipoObjetoBD.Novo)
                return;
            edicao = new EdicaoObjetoBD(valoresAtributos);
            TipoObjeto = TipoObjetoBD.SendoEditado;
        }

        /// <summary>
        /// Desfaz as modificações.
        /// Só pode ser desfazer as modificações se estiver sendo editado
        /// </summary>
        public void RollBack()
        {
            if (TipoObjeto != TipoObjetoBD.SendoEditado)
                throw new Exception("Só pode ser desfazer as modificações se estiver sendo editado");
            TipoObjeto = TipoObjetoBD.PertenceBD;
            int[] colunasAtt = edicao.PegarVetorValoresEditados();
            for (int i = 0; i < colunasAtt.Length; i++)
            {
                ChamarBinding(colunasAtt[i]);
            }
            edicao = null;
        }

        /// <summary>
        /// Define as modificações como definição atual.
        /// Não deve ser definido se não foi modificado no banco de dados
        /// </summary>
        public void CommitBD()
        {
            if (TipoObjeto == TipoObjetoBD.PertenceBD)
                throw new Exception("Só pode ser aceitar as modificações quando o objeto é novo ou está sendo editado");
            edicao.Commit();
            edicao = null;
            TipoObjeto = TipoObjetoBD.PertenceBD;
        }

        /// <summary>
        /// Classe que serve para auxiliar na edição do ObjetoBD
        /// </summary>
        internal class EdicaoObjetoBD
        {
            object[] valoresOriginais;
            object[] valoresEditados;
            /// <summary>
            /// Os valores das variáveis de todos atributos
            /// </summary>
            public object[] ValoresEditados
            {
                get { return valoresEditados; }
            }

            /// <summary>
            /// Lista sem repetição que possui os itens que já foram editados, 
            /// se a lista não possui elementos, então nenhum foi editado
            /// </summary>
            SortedSet<int> itensEditados;

            /// <summary>
            /// Outra lista de itens editados, mas essa é para auxiliar quais itens foram editados
            /// </summary>
            bool[] itensEditados2;

            /// <summary>
            /// Indica se foi modificado qualquer item
            /// </summary>
            public bool FoiModificado
            {
                get
                {
                    if (itensEditados.Count > 0)
                        return true;
                    else
                        return false;
                }
            }

            /// <summary>
            /// Construtor do objeto de edição
            /// </summary>
            /// <param name="valoresAtributos"></param>
            public EdicaoObjetoBD(object[] valoresAtributos)
            {
                valoresOriginais = valoresAtributos;
                itensEditados = new SortedSet<int>();
                itensEditados2 = new bool[valoresAtributos.Length];
                valoresEditados = new object[valoresAtributos.Length];
                for (int i = 0; i < valoresAtributos.Length; i++)
                {
                    valoresEditados[i] = valoresAtributos[i];
                }
            }

            /// <summary>
            /// Define o valor de uma coluna na edição
            /// </summary>
            public bool DefinirValor(int coluna, object valor)
            {
                if (valor.Equals(valoresEditados[coluna]))
                    return false;
                valoresEditados[coluna] = valor;
                //Se os dois valores estão iguais
                if (valoresEditados[coluna].Equals(valoresOriginais[coluna]))
                {
                    //Se tinha sido modificado
                    if (itensEditados2[coluna] == true)
                    {
                        itensEditados2[coluna] = false;
                        itensEditados.Remove(coluna);
                    }
                }
                else
                {
                    //Se eram iguais
                    if (itensEditados2[coluna] == false)
                    {
                        itensEditados2[coluna] = true;
                        itensEditados.Add(coluna);
                    }
                }
                return true;
            }

            /// <summary>
            /// Torna os valores editados como atuais
            /// </summary>
            public void Commit()
            {
                if (!FoiModificado)
                    return;
                for (int i = 0; i < itensEditados.Count; i++)
                {
                    int a = itensEditados.ElementAt(i) + 1;
                    valoresOriginais[itensEditados.ElementAt(i)] = valoresEditados[itensEditados.ElementAt(i)];
                }
            }

            /// <summary>
            /// Pega um vetor com todos os indices dos valores editados
            /// </summary>
            public int[] PegarVetorValoresEditados()
            {
                return itensEditados.ToArray();
            }
        }
        #endregion

        #region Bindings
        /// <summary>
        /// Chama as funções vinculadas
        /// </summary>
        /// <param name="coluna">A coluna que será chamada os bindings</param>
        public void ChamarBinding(int coluna)
        {
            if (PropriedadeMudou != null)
            {
                PropriedadeMudou(coluna, PegarValor(coluna));
            }
        }
        #endregion

        #region FazerCopia, VerificarCampoFoiModificado
        /// <summary>
        /// Faz uma copia de um objetoBD para ser editada
        /// essa classe só pode ser usada quando o objeto foi gerado automáticamente por
        /// ter a propriedade estática CampoBD[]
        /// </summary>
        /// <param name="objetoBD">O objeto que será copiado</param>
        /// <returns>A copia do objeto</returns>
        public static ObjetoBD FazerCopia(ObjetoBD objetoBD)
        {
            //Cria o novo objeto que será a copia
            ObjetoBD novoObjeto = (ObjetoBD)Activator.CreateInstance(objetoBD.GetType());

            //Copia cada valor para o novo objeto
            for (int i = 0; i < objetoBD.campos.Length; i++)
            {
                novoObjeto.valoresAtributos[i] = objetoBD.valoresAtributos[i];
            }
            return novoObjeto;
        }

        /// <summary>
        /// Verifica se um campo específico teve modificações
        /// os dois parametros devem ser iguais e um deve ser a copia do outro
        /// </summary>
        /// <returns>se é igual ou n</returns>
        public static bool VerificarCampoFoiModificado(string NomeCampo, ObjetoBD Original, ObjetoBD Copia)
        {
            if (!Original[NomeCampo].Equals(Copia[NomeCampo]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Verifica se um campo específico teve modificações
        /// os dois parametros devem ser iguais e um deve ser a copia do outro
        /// </summary>
        /// <returns>se é igual ou n</returns>
        public static bool VerificarCampoFoiModificado(int NumeroCampo, ObjetoBD Original, ObjetoBD Copia)
        {
            if (!Original[NumeroCampo].Equals(Copia[NumeroCampo]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// Comparador de ObjetoBD de ordem crescente
        /// depois é só usar
        /// List<ObjetoBD> list;
        /// list.Sort(new ObjetoBDComparer_Asc())
        /// </summary>
        public class ObjetoBDComparer_Asc : IComparer<ObjetoBD>
        {
            public int Compare(ObjetoBD x, ObjetoBD y)
            {
                if (x.IndexTabela > y.IndexTabela) return 1;
                else if (x.IndexTabela < y.IndexTabela) return -1;
                else return 0;
            }
        }
    }

    /// <summary>
    /// Indica o tipo do objeto no banco de dados
    /// </summary>
    public enum TipoObjetoBD
    {
        /// <summary> Novo objeto que poderá ser cadastrado no banco de dados pela camada lógica </summary>
        Novo = 0,
        /// <summary> Indica se é um objeto que teve seus atributos definidos igual ao BD </summary>
        PertenceBD,
        /// <summary> Indica que está sendo editado </summary>
        SendoEditado
    }
}
