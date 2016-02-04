using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace Gerador_Classes_BD
{
    /// <summary>
    /// ESTA CLASSE ESTÁ EM CONSTRUÇÃO!
    /// Ultima att 30/10/2015
    /// </summary>

    public class ControladorBD
    {
        /// <summary>
        /// Essa é a string de conexão que está como um objeto estático, a string de conexão é 
        /// determinada ao iniciar o programa
        /// </summary>
        private static string stringConexao;
        /// <summary>
        /// Propriedade unicamente para definir o valor da string conexão
        /// </summary>
        public static string StringConexao
        {
            get { return stringConexao; }
            set { stringConexao = value; }
        }

        /// <summary>
        /// Esse é o objeto de conexão usado pela classe
        /// </summary>
        private SqlConnection con;
        
        #region públicos: Ler, LerEAlterar, alterar, contar
        /// <summary>
        /// Faz a leitura de um dado Sql,
        /// Abre a conexão e fecha,
        /// Não interage com SqlTransaction
        /// </summary>
        public DataTable LerT(string stringSQL)
        {
            DataTable result = new DataTable();
            SqlCommand cmd = new SqlCommand(stringSQL, con);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            result.Load(reader);
            con.Close();
            return result;
        }

        /// <summary>
        /// Faz a leitura de um dado Sql,
        /// Abre a conexão e fecha,
        /// Não interage com SqlTransaction
        /// </summary>
        public DataTable LerT(string stringSQL, SqlParameter[] parametros)
        {
            DataTable result = new DataTable();
            SqlCommand cmd = new SqlCommand(stringSQL, con);
            cmd.Parameters.AddRange(parametros);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            result.Load(reader);
            con.Close();
            return result;
        }

        /// <summary>
        /// Faz a leitura de um dado Sql,
        /// Abre a conexão, mas não fecha
        /// Não interage com SqlTransaction
        /// </summary>
        public SqlDataReader Ler(string stringSQL)
        {
            SqlCommand cmd = new SqlCommand(stringSQL, con);

            con.Open();
            return cmd.ExecuteReader();
        }

        /// <summary>
        /// Faz a leitura de um dado Sql usando parametros,
        /// Abre a conexão, mas não fecha
        /// Não interage com SqlTransaction
        /// </summary>
        public SqlDataReader Ler(string stringSQL, SqlParameter[] parametros)
        {
            SqlCommand cmd = new SqlCommand(stringSQL, con);
            cmd.Parameters.AddRange(parametros);

            con.Open();
            return cmd.ExecuteReader();
        }

        /// <summary>
        /// Faz a leitura de um dado Sql,
        /// Abre a conexão e fecha,
        /// Faz BeginTransaction e commit
        /// </summary>
        public DataTable LerTEAlterar(string stringSQL)
        {
            DataTable result = new DataTable();

            con.Open();
            SqlTransaction transa = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(stringSQL, con, transa);
            SqlDataReader reader = cmd.ExecuteReader();
            result.Load(reader);
            transa.Commit();
            con.Close();
            return result;
        }

        /// <summary>
        /// Faz a leitura de um dado Sql,
        /// Abre a conexão e fecha,
        /// Faz BeginTransaction e commit
        /// </summary>
        public DataTable LerTEAlterar(string stringSQL, SqlParameter[] parametros)
        {
            DataTable result = new DataTable();

            con.Open();
            SqlTransaction transa = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(stringSQL, con, transa);
            cmd.Parameters.AddRange(parametros);
            SqlDataReader reader = cmd.ExecuteReader();
            result.Load(reader);
            transa.Commit();
            con.Close();
            return result;
        }

        /// <summary>
        /// Faz a leitura de um dado Sql,
        /// Abre a conexão, mas não fecha
        /// Faz BeginTransaction e commit
        /// </summary>
        public SqlDataReader LerEAlterar(string stringSQL)
        {
            con.Open();
            SqlTransaction transa = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(stringSQL, con, transa);
            transa.Commit();
            return cmd.ExecuteReader();
        }

        /// <summary>
        /// Faz a leitura de um dado Sql usando parametros,
        /// Abre a conexão, mas não fecha
        /// Faz BeginTransaction e commit
        /// </summary>
        public SqlDataReader LerEAlterar(string stringSQL, SqlParameter[] parametros)
        {
            con.Open();
            SqlTransaction transa = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(stringSQL, con, transa);
            cmd.Parameters.AddRange(parametros);
            transa.Commit();
            return cmd.ExecuteReader();
        }


        public int alterar(string stringSQL)
        {
            con.Open();
            SqlTransaction transa = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(stringSQL, con, transa);
            return nonQuery(cmd, transa);
        }

        public int alterar(string stringSQL, SqlParameter[] parametros)
        {
            con.Open();
            SqlTransaction transa = con.BeginTransaction();
            SqlCommand cmd = new SqlCommand(stringSQL, con, transa);
            cmd.Parameters.AddRange(parametros);
            return nonQuery(cmd, transa);
        }

        public T contar<T>(string stringSQL)
        {
            con.Open();
            T resultado;
            SqlCommand cmd = new SqlCommand(stringSQL, con);
            //object obj = cmd.ExecuteScalar();
            //MessageBox.Show(obj.GetType().ToString());
            resultado = (T)cmd.ExecuteScalar();
            con.Close();
            return resultado;
        }

        public T contar<T>(string stringSQL, SqlParameter[] parametros)
        {
            con.Open();
            T resultado;
            SqlCommand cmd = new SqlCommand(stringSQL, con);
            cmd.Parameters.AddRange(parametros);
            resultado = (T)cmd.ExecuteScalar();
            con.Close();
            return resultado;
        }
        #endregion

        #region privados: read, nonQuery, Scalar
        private SqlDataReader read(SqlCommand cmd)
        {
            con.Open();
            return cmd.ExecuteReader();
        }

        private int nonQuery(SqlCommand cmd, SqlTransaction transa)
        {
            int rows;
            rows = cmd.ExecuteNonQuery();
            transa.Commit();
            return rows;
        }
        #endregion

        /// <summary>
        /// Declara o controlador de banco de dados para fazer transações no bd
        /// pode definir se já é declarado fazendo a conexão
        /// </summary>
        public ControladorBD()
        {
            con = new SqlConnection(stringConexao);
        }

        /// <summary>
        /// Testa a conexão com o banco de dados,
        /// uso no início do programa
        /// </summary>
        /// <returns>
        /// Retorna um erro caso não seja possível conectar e
        /// retorna nulo caso consiga
        /// </returns>
        public Exception TestarConexao()
        {
            try
            {
                con.Open();
                con.Close();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        #region Funções para pegar os esquemas do bd
        /// <summary>
        /// Pega o esquema das colunas de uma tabela
        /// </summary>
        /// <param name="tabela">O nome da tabela que será pego o esquema das colunas</param>
        /// <returns>A tabela com o esquema das colunas</returns>
        public DataTable PegarEsquemaColunas(string tabela)
        {//Foreign Keys
            con.Open();
            string[] restrictions = new string[4];
            restrictions[2] = tabela;
            DataTable table = con.GetSchema("Columns", restrictions);
            con.Close();
            DataView view = table.DefaultView;
            view.Sort = "ORDINAL_POSITION ASC";
            return view.ToTable();
        }

        /// <summary>
        /// Pega o esquema das tabelas
        /// OBS: Somente aonde estão registradas as tabelas e as tabelas que possui,
        /// Não pega as colunas ou coisa do tipo
        /// </summary>
        /// <param name="tabela">O nome da tabela que será pego o esquema das colunas</param>
        /// <returns>A tabela com o esquema das tabelas</returns>
        public DataTable PegarEsquemaTabelas()
        {
            con.Open();
            DataTable table = con.GetSchema("Tables");
            con.Close();
            return table;
        }

        /// <summary>
        /// Exibe com mensagebox todos os campos de uma tabela e os seus valores
        /// </summary>
        /// <param name="tabela"></param>
        public void ExibirMsgBoxTabela(DataTable table, int max = 5)
        {
            string msg;
            int count = table.Rows.Count;
            if (max > 0 && count > max)
            {
                count = max;
                MessageBox.Show("Há "+table.Rows.Count+" elementos, mas serão exibidos "+max);
            }
            for (int i = 0; i < count; i++)
            {
                msg = "";
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    msg += j + ": " + table.Columns[j].ColumnName + ":   " + table.Rows[i][j] + Environment.NewLine;
                }
                MessageBox.Show(msg);
            }
        }
        #endregion
    }
}