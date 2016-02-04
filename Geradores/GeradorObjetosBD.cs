using ClassesBD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador_Classes_BD
{
    /// <summary>
    /// Essa classe gera objetos para serem usados pelo banco de dados automaticamente
    /// Os objetos gerados são salvos em uma pasta e em arquivos .cs
    /// </summary>
    public class GeradorObjetosBD
    {

        /// <summary>
        /// Faz todo trabalho de pegar o esquema das tabelas, de gerar o código e salva-lo no diretorio especificado
        /// </summary>
        public EsquemaTabela[] GerarArquivos(string diretorio, Carregamentos exibicaoCar)
        {
            EsquemaTabela[] es = MontadorEsquemaTabelas.PegarEsquemaTabelas(exibicaoCar);
            exibicaoCar.TextoAtualizado = "Gerando Arquivos";
            string[] codigos = new string[es.Length];
            string[] nomesCodigos = new string[es.Length];
            for (int i = 0; i < es.Length; i++)
            {
                codigos[i] = GerarCodigoTabela(es[i]);
                nomesCodigos[i] = es[i].NomeTabela;
            }
            SalvarCodigos(nomesCodigos, codigos, diretorio);
            SalvarCodigo("ObjetosBD", GerarEsquemasObjBD(), diretorio);
            return es;
        }

        /// <summary>
        /// Salva os códigos C# dentro de uma pasta no diretório especificado
        /// </summary>
        public void SalvarCodigos(string[] nomesCodigos, string[] codigos, string diretorio)
        {
            string diretorioArquivo;
            //Cria a pasta
            string diretorioCompleto = Path.Combine(diretorio, "ClassesBD");
            Directory.CreateDirectory(diretorioCompleto);
            for (int i = 0; i < codigos.Length; i++)
            {
                diretorioArquivo = Path.Combine(diretorioCompleto, nomesCodigos[i] + ".cs");
                File.WriteAllText(diretorioArquivo, codigos[i]);
            }
        }

        /// <summary>
        /// Salva um código C# dentro de uma pasta no diretório especificado
        /// </summary>
        public void SalvarCodigo(string nomeCodigo, string codigo, string diretorio)
        {
            string diretorioArquivo;
            //Cria a pasta
            string diretorioCompleto = Path.Combine(diretorio, "ClassesBD");
            Directory.CreateDirectory(diretorioCompleto);
            diretorioArquivo = Path.Combine(diretorioCompleto, nomeCodigo + ".cs");
            File.WriteAllText(diretorioArquivo, codigo);
        }

        #region Geração do código

        #region Geração de ObjetoBD
        /// <summary>
        /// Gera o código de uma única tabela
        /// </summary>
        /// <param name="esquema">O esquema da tabela</param>
        /// <returns>O código da tabela gerado</returns>
        public string GerarCodigoTabela(EsquemaTabela esquema)
        {
            //Adiciona a primeira parte do código
            string codigo =
@"using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ClassesBD
{
    /// <summary>
    /// Objeto que representa a tabela " + esquema.NomeTabela + @" no banco de dados. 
    /// Essa classe foi gerada automaticamente pelo gerador de ObjetosBD no dia " + DateTime.Now + @"
    /// </summary>
    public class ";
            codigo += esquema.NomeTabela + @" : ObjetoBD
    {
        // Colunas: ";
            codigo += PegarNomesColunas(esquema) + @"
"
            + GerarAtributosObjeto(esquema) +
@"
"
                + GerarObjetosPai(esquema) + GerarObjetosFilho(esquema);
            codigo += @"
        #region Configuracoes
        // O indice desse ObjetoBD no EsquemasObjBD
        public override int IndexTabela { get { return " + esquema.IndiceTabela + @"; } }

        /// <summary>
        /// Define o esquema de tabela usado por esse ObjetoBD
        /// </summary>
        internal static EsquemaTabela DefinirTabela()
        {
            EsquemaTabela esquema = new EsquemaTabela(" + "\"" + esquema.NomeTabela + "\"" + @");
            // Os campos desse
            esquema.Campos = new CampoBD[]
            {
";
            codigo += GerarCamposObjeto(esquema) +
@"            };
            esquema.IndexColunaPK = " + esquema.IndexColunaPK + @";
            esquema.IndiceTabela = " + esquema.IndiceTabela + @";

            return esquema;
        }
        #endregion
    }
}";
            return codigo;
        }

        /// <summary>
        /// Pega os nomes das colunas da tabela e as ordena
        /// Exemplo: ContatoID, Nome, Telefone
        /// </summary>
        public string PegarNomesColunas(EsquemaTabela esquema)
        {
            string colunas = "";
            for (int i = 0; i < esquema.Campos.Length; i++)
            {
                colunas += esquema.Campos[i].NomeCampo;
                if (i + 1 < esquema.Campos.Length)
                {
                    colunas += ", ";
                }
            }
            return colunas;
        }

        /// <summary>
        /// Gera os atributos da tabela atual
        /// Exemplo: 
        /// new CampoBD("ClienteID", false, System.Data.SqlDbType.Int, typeof(int), 0)
        /// </summary>
        public string GerarCamposObjeto(EsquemaTabela esquema)
        {
            string[,] cod = new string[esquema.Campos.Length, 7];

            for (int i = 0; i < esquema.Campos.Length; i++)
            {
                cod[i, 0] = "                new CampoBD(" + i + ",";
                cod[i, 1] = "\"" + esquema.Campos[i].NomeCampo + "\",";
                cod[i, 2] = esquema.Campos[i].PermiteNull.ToString().ToLower() + ",";
                cod[i, 3] = "SqlDbType." + esquema.Campos[i].TipoVariavelBD.ToString() + ",";
                cod[i, 4] = "typeof(" + PegarNomeTipo(esquema.Campos[i].TipoVariavel) + "),";
                cod[i, 5] = esquema.Campos[i].MaxLenght.ToString() + ",";
                cod[i, 6] = "TipoCampo." + esquema.Campos[i].TipoCampo.ToString() + ")";
                if (i + 1 < esquema.Campos.Length)
                {
                    cod[i, 6] += ",";
                }
            }
            return IdentarCodigoBlocos(cod);
        }


        /// <summary>
        /// Gera os atributos da tabela atual
        /// Exemplo:
        ///         public int?      CNPJ         { get { return (int?)PegarValor(2); } set { DefinirValor(8, value); } }
        /// </summary>
        public string GerarAtributosObjeto(EsquemaTabela esquema)
        {
            string[,] cod = new string[esquema.Campos.Length, 4];

            for (int i = 0; i < esquema.Campos.Length; i++)
            {
                Type tipo = esquema.Campos[i].TipoVariavel;
                string t = PegarNomeTipo(tipo);
                if (esquema.Campos[i].PermiteNull
                    && (tipo == typeof(int)
                    || tipo == typeof(decimal)
                    || tipo == typeof(double)
                    || tipo == typeof(DateTime)
                    || tipo == typeof(long)
                    || tipo == typeof(bool)))
                    t += "?";

                cod[i, 0] = "        public " + t;
                cod[i, 1] = esquema.Campos[i].NomeCampo;
                cod[i, 2] = "{ get { return (" + t + ") PegarValor(" + i + "); }";
                cod[i, 3] = "set { DefinirValor(" + i + ", value); } }";
            }
            return IdentarCodigoBlocos(cod);
        }

        #region Relações
        /// <summary>
        /// Gera os objetos pai desse
        /// Exemplo: 
        /// tbEndereco tbEnderecoR { get; set; }
        /// </summary>
        public string GerarObjetosPai(EsquemaTabela esquema)
        {
            if (esquema.RelacoesPai == null)
                return "";

            string[,] cod = new string[esquema.RelacoesPai.Length, 4];

            for (int i = 0; i < esquema.RelacoesPai.Length; i++)
            {
                cod[i, 0] = "        public " + esquema.RelacoesPai[i].TabelaPai;
                cod[i, 1] = esquema.RelacoesPai[i].TabelaPai + "R";
                cod[i, 2] = "{ get { return (" + esquema.RelacoesPai[i].TabelaPai + ")PegarObjPai(" + i + "); }";
                cod[i, 3] = "set { DefinirObjPai(" + i + ", value); } }";
            }
            return @"
        //Relações com tabelas pai
" + IdentarCodigoBlocos(cod) + @"
";
        }

        /// <summary>
        /// Gera os objetos filho desse 
        /// Gera como lista
        /// Exemplo: 
        /// List<tbCliente> tbClienteR { get; set; }
        /// </summary>
        public string GerarObjetosFilho(EsquemaTabela esquema)
        {
            if (esquema.RelacoesFilha == null)
                return "";

            string[,] cod = new string[esquema.RelacoesFilha.Length, 3];

            for (int i = 0; i < esquema.RelacoesFilha.Length; i++)
            {
                cod[i, 0] = "        public List<ObjetoBD> " + esquema.RelacoesFilha[i].TabelaFilha + "R";
                cod[i, 1] = "{ get { return PegarObjetosFilho(" + i + "); }";
                cod[i, 2] = "set { DefinirObjetosFilho(" + i + ", value); } }";
            }
            return @"        //Relações com tabelas filhas
" + IdentarCodigoBlocos(cod) + @"
";
        }
        #endregion
        #endregion Geração de ObjetoBD

        #region Geração de EsquemasObjBD
        /// <summary>
        /// Gera o código da classe EsquemasObjBD
        /// </summary>
        public string GerarEsquemasObjBD()
        {
            //Adiciona a primeira parte do código
            string codigo =
@"using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ClassesBD
{
    /// <summary>
    /// Classe que é enviada junta com os outros ObjetoBD, ela possui as informações
    /// dos esquemas dos Objetos, com a ordem deles também,
    /// se não configurar essa classe, todos ObjetoBD usados darão erro
    /// Essa classe foi gerada automaticamente pelo gerador de ObjetosBD no dia " + DateTime.Now + @"
    /// </summary>
    public class ObjetosBD : EsquemasObjBD
    {
        /// <summary>
        /// Configura todos os esquemas das tabelas
        /// </summary>
        public static void ConfigurarEsquemas()
        {
            // Os esquemas das tabelas são inseridos na ordem de atualização hierarquica
            EsquemasTabelas = new EsquemaTabela[]
            {
"
                + GerarTabelasObjBD() + @"
            };

            Relacoes = new RelacaoBD[]
            {
";
            codigo += GerarRelacoesObjBD();
            codigo += @"            };

            DefinirRelacoesTabelas();
        }
    }
}";
            return codigo;
        }

        /// <summary>
        /// Gera as tabelas atuais
        /// Exemplo: 
        /// tbTipoContato.DefinirTabela()
        /// </summary>
        public string GerarTabelasObjBD()
        {
            string cod = "";
            for (int i = 0; i < EsquemasObjBD.EsquemasTabelas.Length; i++)
            {
                cod += "                " + EsquemasObjBD.EsquemasTabelas[i].NomeTabela + ".DefinirTabela()";
                if (i + 1 < EsquemasObjBD.EsquemasTabelas.Length)
                {
                    cod += ", //" + i + Environment.NewLine;
                }
                else
                    cod += " //" + i;
            }

            return cod;
        }

        /// <summary>
        /// Gera as relações das tabelas atuais
        /// Exemplo: 
        /// new RelacaoBD("tF", "cF", "tP", "cP", "FK_AlgumaCoisa")
        /// </summary>
        public string GerarRelacoesObjBD()
        {
            string[,] cod = new string[EsquemasObjBD.Relacoes.Length, 5];

            for (int i = 0; i < EsquemasObjBD.Relacoes.Length; i++)
            {
                cod[i, 0] = "                new RelacaoBD(\"" + EsquemasObjBD.Relacoes[i].ConstraintName + "\",";
                cod[i, 1] = EsquemasObjBD.Relacoes[i].IndexColunaFilha + ",";
                cod[i, 2] = EsquemasObjBD.Relacoes[i].IndexColunaPai + ",";
                cod[i, 3] = "EsquemasTabelas[" + EsquemasObjBD.Relacoes[i].IndexTabelaFilha + "],";
                cod[i, 4] = "EsquemasTabelas[" + EsquemasObjBD.Relacoes[i].IndexTabelaPai + "])";
                if (i + 1 < EsquemasObjBD.Relacoes.Length)
                {
                    cod[i, 4] += ",";
                }
            }
            return IdentarCodigoBlocos(cod);
        }
        #endregion

        #region Outros
        /// <summary>
        /// Pega o nome do tipo de uma variável para usa-la com typeof ou na declaração
        /// Exemplo de saida: string, int, decimal
        /// </summary>
        public string PegarNomeTipo(Type tipo)
        {
            if (tipo == typeof(int))
                return "int";
            else if (tipo == typeof(decimal))
                return "decimal";
            else if (tipo == typeof(double))
                return "double";
            else if (tipo == typeof(DateTime))
                return "DateTime";
            else if (tipo == typeof(long))
                return "long";
            else if (tipo == typeof(string))
                return "string";
            else if (tipo == typeof(bool))
                return "bool";
            else
                return "object";
        }

        /// <summary>
        /// Pega diversos códigos e os monta deixando identado
        /// </summary>
        public string IdentarCodigoBlocos(string[,] blocos)
        {
            //Para cada coluna da matriz faz a verificação do maior e adiciona os espaços faltantes ao resto
            for (int i = 0; i < blocos.GetLength(1); i++)
            {
                //Pega o tamanho da maior coluna
                int tamanhoMaior = blocos[0, i].Length;
                for (int j = 0; j < blocos.GetLength(0); j++)
                {
                    if (blocos[j, i].Length > tamanhoMaior)
                        tamanhoMaior = blocos[j, i].Length;
                }

                //Adiciona os espaços faltantes
                int faltantes;
                for (int j = 0; j < blocos.GetLength(0); j++)
                {
                    faltantes = tamanhoMaior - blocos[j, i].Length + 1;
                    blocos[j, i] += String.Concat(Enumerable.Repeat(" ", faltantes));
                }
            }

            string txt = "";
            for (int i = 0; i < blocos.GetLength(0); i++)
            {
                for (int j = 0; j < blocos.GetLength(1); j++)
                {
                    txt += blocos[i, j];
                    if (j + 1 >= blocos.GetLength(1))
                        txt += Environment.NewLine;
                }
            }
            return txt;
        }
        #endregion Outros

        #endregion Geração do código
    }
}
