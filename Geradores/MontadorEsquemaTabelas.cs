using ClassesBD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador_Classes_BD
{
    /// <summary>
    /// Essa classe pega os esquemas das tabelas do banco de dados
    /// </summary>
    public class MontadorEsquemaTabelas
    {
        /// <summary>
        /// Pega os esquemas das tabelas no banco de dados
        /// Gera os objetos do banco de dados
        /// Essa função demora muito por conta do banco de dados
        /// </summary>
        public static EsquemaTabela[] PegarEsquemaTabelas(Carregamentos exibicaoCar)
        {
            exibicaoCar.TextoAtualizado = "Definindo tabelas e colunas";
            EsquemaTabela[] esquemas = PegarEsquemaTabelasColunas();

            exibicaoCar.TextoAtualizado = "Definindo relações";
            RelacaoBD[] relacoes = PegarRelacoes(esquemas);

            //Configura a classe EsquemasObjBD
            EsquemasObjBD.ConfigurarEsquemas(esquemas, relacoes);

            exibicaoCar.TextoAtualizado = "Definindo chave primárias e unique";
            PegarColunasPkEUnique(esquemas);

            exibicaoCar.TextoAtualizado = "Organizando hierarquia";
            esquemas = DefinirHierarquia(esquemas, relacoes);
            return esquemas;
        }

        public static EsquemaTabela[] PegarEsquemaTabelasColunas()
        {
            EsquemaTabela[] esquemaTabelas;
            System.Data.DataTable tabEsquemaColunas;
            CampoBD[] camposTabela;
            ControladorBD cBD = new ControladorBD();
            //Pega o esquema das tabelas no BD
            System.Data.DataTable tabEsquemaTabelas = cBD.PegarEsquemaTabelas();

            esquemaTabelas = new EsquemaTabela[tabEsquemaTabelas.Rows.Count];
            //Laço para pegar o esquema de cada tabela
            for (int i = 0; i < tabEsquemaTabelas.Rows.Count; i++)
            {
                //Define a tabela atual
                esquemaTabelas[i] = new EsquemaTabela(tabEsquemaTabelas.Rows[i][2].ToString());
                //Pega as colunas
                tabEsquemaColunas = cBD.PegarEsquemaColunas(esquemaTabelas[i].NomeTabela);
                camposTabela = new CampoBD[tabEsquemaColunas.Rows.Count];
                //Laço para definir cada coluna
                for (int j = 0; j < tabEsquemaColunas.Rows.Count; j++)
                {
                    string nome = tabEsquemaColunas.Rows[j][3].ToString();
                    bool permiteNulo;
                    if (tabEsquemaColunas.Rows[j][6].ToString() == "NO")
                        permiteNulo = false;
                    else
                    {
                        permiteNulo = true;
                    }
                    //Define os tipos
                    System.Data.SqlDbType tipoBD = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType),
                        tabEsquemaColunas.Rows[j][7].ToString(), true);
                    Type tipo = PegarTipoColuna(tipoBD);
                    //Define a quantidade de linhas
                    int maxLenght = 0;
                    if (tabEsquemaColunas.Rows[j][8].GetType() == typeof(int))
                        maxLenght = (int)tabEsquemaColunas.Rows[j][8];

                    camposTabela[j] = new CampoBD(
                         i,
                         nome,
                         permiteNulo,
                         tipoBD,
                         tipo,
                         maxLenght,
                         TipoCampo.Comum
                        );
                }
                esquemaTabelas[i].Campos = camposTabela;
            }
            return esquemaTabelas;
        }

        /// <summary>
        /// Pega o tipo da coluna
        /// </summary>
        /// <param name="tipo">O tipo em texto</param>
        /// <returns>O tipo da coluna já com os dados</returns>
        public static Type PegarTipoColuna(System.Data.SqlDbType tipo)
        {
            //Eita switch grande eim
            //O maior que eu já fiz. Andrei
            switch (tipo)
            {
                case System.Data.SqlDbType.VarChar:
                case System.Data.SqlDbType.NVarChar:
                case System.Data.SqlDbType.NChar:
                case System.Data.SqlDbType.Char:
                    return typeof(string);
                case System.Data.SqlDbType.Money:
                case System.Data.SqlDbType.Decimal:
                case System.Data.SqlDbType.SmallMoney:
                    return typeof(decimal);
                case System.Data.SqlDbType.Int:
                case System.Data.SqlDbType.SmallInt:
                case System.Data.SqlDbType.TinyInt:
                    return typeof(int);
                case System.Data.SqlDbType.Real:
                case System.Data.SqlDbType.Float:
                    return typeof(double);
                case System.Data.SqlDbType.BigInt:
                    return typeof(long);
                case System.Data.SqlDbType.Bit:
                    return typeof(bool);
                case System.Data.SqlDbType.Date:
                case System.Data.SqlDbType.DateTime:
                case System.Data.SqlDbType.DateTime2:
                case System.Data.SqlDbType.DateTimeOffset:
                case System.Data.SqlDbType.SmallDateTime:
                    return typeof(DateTime);
                default:
                    return typeof(object);
            }
        }

        /// <summary>
        /// Pega as relações de uma tabela e já as define em um esquema de tabela
        /// </summary>
        /// <param name="esquema">O esquema da tabela</param>
        public static RelacaoBD[] PegarRelacoes(EsquemaTabela[] esquemas)
        {
            ControladorBD cBD = new ControladorBD();
            System.Data.DataTable tbRelacoes = cBD.LerT(@"SELECT
    K_Table = FK.TABLE_NAME,
    FK_Column = CU.COLUMN_NAME,
    PK_Table = PK.TABLE_NAME,
    PK_Column = PT.COLUMN_NAME,
    Constraint_Name = C.CONSTRAINT_NAME
FROM
    INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C
INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK
    ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME
INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK
    ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME
INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU
    ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME
INNER JOIN (
            SELECT
                i1.TABLE_NAME,
                i2.COLUMN_NAME
            FROM
                INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1
            INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2
                ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME
            WHERE
                i1.CONSTRAINT_TYPE = 'PRIMARY KEY'
           ) PT
    ON PT.TABLE_NAME = PK.TABLE_NAME");

            RelacaoBD[] relacoes = new RelacaoBD[tbRelacoes.Rows.Count];
            for (int i = 0; i < relacoes.Length; i++)
            {
                relacoes[i] = new RelacaoBD(
                            (string)tbRelacoes.Rows[i][0],
                            (string)tbRelacoes.Rows[i][1],
                            (string)tbRelacoes.Rows[i][2],
                            (string)tbRelacoes.Rows[i][3],
                            (string)tbRelacoes.Rows[i][4]);
            }
            return relacoes;
        }

        /// <summary>
        /// Pega as relações de uma tabela e já as define em um esquema de tabela
        /// </summary>
        /// <param name="esquema">O esquema da tabela</param>
        public static void PegarColunasPkEUnique(EsquemaTabela[] esquemas)
        {
            ControladorBD cBD = new ControladorBD();
            System.Data.DataTable tbPkEUnique = cBD.LerT(
                @"SELECT
                i1.TABLE_NAME,
                i2.COLUMN_NAME,
                i1.CONSTRAINT_TYPE
            FROM
                INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1
            INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2
                ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME");
            DataView view = tbPkEUnique.DefaultView;

            for (int i = 0; i < esquemas.Length; i++)
            {
                view.RowFilter = "TABLE_NAME = '" + esquemas[i].NomeTabela + "'";
                ConfigurarPkEUnique(view, esquemas[i]);
            }
        }

        /// <summary>
        /// Continuação da função pegar primarykey e unique
        /// </summary>
        static void ConfigurarPkEUnique(DataView view, EsquemaTabela esquema)
        {
            //Verifica constraint por constraint
            for (int i = 0; i < view.Count; i++)
            {
                //Verifica coluna por coluna do esquema
                for (int j = 0; j < esquema.Campos.Length; j++)
                {
                    string nomeColuna = (string)view[i][1];
                    string tipo = (string)view[i][2];
                    //Verifica se a coluna possui o mesmo nome
                    if (esquema.Campos[j].NomeCampo == nomeColuna)
                    {
                        if (tipo == "FOREIGN KEY")
                        {
                            if (esquema.Campos[j].TipoCampo == TipoCampo.Comum)
                                esquema.Campos[j].TipoCampo = TipoCampo.ForeignKey;
                            else if (esquema.Campos[j].TipoCampo == TipoCampo.PrimaryKey)
                                esquema.Campos[j].TipoCampo = TipoCampo.ForeignKey_PrimaryKey;
                            else
                                esquema.Campos[j].TipoCampo = TipoCampo.ForeignKey_Unique;
                        }
                        else if (tipo == "PRIMARY KEY")
                        {
                            if (esquema.Campos[j].TipoCampo == TipoCampo.Comum)
                                esquema.Campos[j].TipoCampo = TipoCampo.PrimaryKey;
                            else if (esquema.Campos[j].TipoCampo == TipoCampo.ForeignKey)
                                esquema.Campos[j].TipoCampo = TipoCampo.ForeignKey_PrimaryKey;
                            esquema.IndexColunaPK = j;
                        }
                        else if (tipo == "UNIQUE")
                        {
                            if (esquema.Campos[j].TipoCampo == TipoCampo.Comum)
                                esquema.Campos[j].TipoCampo = TipoCampo.Unique;
                            else if (esquema.Campos[j].TipoCampo == TipoCampo.ForeignKey)
                                esquema.Campos[j].TipoCampo = TipoCampo.ForeignKey_Unique;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determina as relações nos objetos
        /// Deve ter determinado os indices das tabelas nas tabelas e nas relações
        /// </summary>
        public static void DefinirRelacoesTabelas(EsquemaTabela[] esquemas, RelacaoBD[] relacoes)
        {
            List<RelacaoBD> relacoesPai;
            List<RelacaoBD> relacoesFilho;
            foreach (var es in esquemas)
            {
                relacoesPai = new List<RelacaoBD>();
                relacoesFilho = new List<RelacaoBD>();
                for (int i = 0; i < relacoes.Length; i++)
                {
                    if (relacoes[i].IndexTabelaPai == es.IndiceTabela)
                    {
                        relacoesPai.Add(relacoes[i]);
                    }
                    if (relacoes[i].IndexTabelaFilha == es.IndiceTabela)
                    {
                        relacoesFilho.Add(relacoes[i]);
                    }
                }
                if (relacoesPai.Count > 0)
                    es.RelacoesFilha = relacoesPai.ToArray();
                if (relacoesFilho.Count > 0)
                    es.RelacoesPai = relacoesFilho.ToArray();
            }
        }

        /// <summary>
        /// Determina os indices das tabelas nelas
        /// </summary>
        public static void DeterminarIndicesTabelas(EsquemaTabela[] esquemas)
        {
            for (int i = 0; i < esquemas.Length; i++)
            {
                esquemas[i].IndiceTabela = i;
            }
        }

        #region Definição da hierarquia das tabelas
        /// <summary>
        /// Define a hierarquia das tabelas e retorna um vetor reorganizado
        /// A classe EsquemasObjBD deve estar configurada
        /// </summary>
        public static EsquemaTabela[] DefinirHierarquia(EsquemaTabela[] esquemas, RelacaoBD[] relacoes)
        {
            //Determina os indices das tabelas nas relações
            foreach (var r in relacoes)
            {
                r.PegarIndicesTabelas();
            }

            //Determina os indices das tabelas nelas mesmas
            DeterminarIndicesTabelas(esquemas);
            //Define as relações nas tabelas
            DefinirRelacoesTabelas(esquemas, relacoes);

            EsquemaTabela[] novoEsquemas = new EsquemaTabela[esquemas.Length];
            for (int i = 0; i < esquemas.Length; i++)
            {
                novoEsquemas[i] = esquemas[i];
            }

            VerificarEsquemasMaiores(novoEsquemas);

            // Determina os indices das tabelas
            for (int i = 0; i < novoEsquemas.Length; i++)
            {
                EsquemasObjBD.EsquemasTabelas[i] = novoEsquemas[i];
            }

            // Redefine indices das tabelas nas relações
            foreach (var r in relacoes)
            {
                r.PegarIndicesTabelas();
                r.PegarIndicesColunas();
            }
            return EsquemasObjBD.EsquemasTabelas;
        }

        /// <summary>
        /// Faz toda a verificação para determinar a hierarquia das tabelas
        /// </summary>
        /// <param name="novoesquema"></param>
        static void VerificarEsquemasMaiores(EsquemaTabela[] novoesquema)
        {
            for (int i = novoesquema.Length-1; i >= 0; i--)
            {
                VerificarMenoresEsquema(novoesquema[i], novoesquema);
            }
        }

        /// <summary>
        /// Verifica o esquema com todos os esquemas anteriores. Sempre que o esquema for
        /// pai de um esquema anterior, o esquema troca de lugar com o anterior.
        /// </summary>
        static bool VerificarMenoresEsquema(EsquemaTabela esquema, EsquemaTabela[] novoesquema)
        {
            bool inverteu = false;
            int indiceVerificado = esquema.IndiceTabela - 1;
            while (indiceVerificado >= 0)
            {
                if (VerificarSeEhPai(esquema, novoesquema[indiceVerificado]))
                {
                    InverterIndices(esquema, novoesquema[indiceVerificado], novoesquema);
                    inverteu = true;
                }
                indiceVerificado--;
            }
            return inverteu;
        }

        /// <summary>
        /// Verifica se o esquema pai é pai do filho por meio das relações
        /// </summary>
        static bool VerificarSeEhPai(EsquemaTabela pai, EsquemaTabela filho)
        {
            if (filho.RelacoesPai == null)
                return false;
            for (int i = 0; i < filho.RelacoesPai.Length; i++)
            {
                if (filho.RelacoesPai[i].EsquemaPai == pai)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Inverte os indices de dois esquemas
        /// </summary>
        static void InverterIndices(EsquemaTabela esquema1, EsquemaTabela esquema2, EsquemaTabela[] novoesquema)
        {
            int novoIndice1 = esquema2.IndiceTabela;
            int novoIndice2 = esquema1.IndiceTabela;

            esquema1.IndiceTabela = novoIndice1;
            esquema2.IndiceTabela = novoIndice2;

            novoesquema[novoIndice1] = esquema1;
            novoesquema[novoIndice2] = esquema2;
        }
        #endregion
    }
}
