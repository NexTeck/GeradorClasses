using System;
using System.ComponentModel;

namespace Gerador_Classes_BD
{
    /// <summary>
    /// Classe que configura e armazena as principais variáveis do programa
    /// ela deve ser chamada no início do programa com o método Configurar(string nomePrograma)
    /// Andrei 27/10/2015
    /// </summary>
    public static class ClasseControladora
    {
        /// <summary>
        /// As configurações principais do programa
        /// </summary>
        public static MainConfig ConfiguracoesPrincipais { get; private set; }

        /// <summary>
        /// Estado atual do teste de conexão
        /// </summary>
        public static TesteConexao EstadoTesteCon { get; private set; }

        /// <summary>
        /// Erros ocorridos no teste de conexão
        /// é nulo se não houve
        /// </summary>
        public static Exception ErroTeste { get; private set; }

        /// <summary>
        /// Configura as principais características do programa
        /// Andrei 27/10/2015
        /// </summary>
        /// <param name="nomePrograma"></param>
        public static void Configurar(string nomePrograma)
        {
            //Carrega ou cria as configurações principais
            ConfiguracoesPrincipais = (MainConfig)BDController.ControllerConfig.CarregarOuCriar(typeof(MainConfig));
        }

        #region Teste Conexão
        /// <summary>
        /// Enum do resultado do teste de conexão
        /// </summary>
        public enum TesteConexao
        {
            NaoTestou = 0,
            Testando = 1,
            Testou = 2,
            TestouRetornouErro = 3
        }

        /// <summary>
        /// Testa a conexão com o banco de dados, deve ter declarado a string de conexão para isso
        /// </summary>
        public static BackgroundWorker TestarConexao()
        {
            ErroTeste = null;
            EstadoTesteCon = TesteConexao.Testando;

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += TestandoConexao;
            bw.RunWorkerAsync();
            return bw;
        }

        static void TestandoConexao(object sender, DoWorkEventArgs e)
        {
            ControladorBD cBD = new ControladorBD();
            ErroTeste = cBD.TestarConexao();
            if (ErroTeste != null)
                EstadoTesteCon = TesteConexao.TestouRetornouErro;
            else
                EstadoTesteCon = TesteConexao.Testou;
        }
        #endregion
    }
}
