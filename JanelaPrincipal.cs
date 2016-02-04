using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gerador_Classes_BD
{
    public partial class JanelaPrincipal : Form
    {
        Carregamentos carregamentos;

        public JanelaPrincipal()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            carregamentos = new Carregamentos();
            txtDiretorioGerado.Text = ClasseControladora.ConfiguracoesPrincipais.DiretorioGerar;
            txtDiretorioBD.Text = ClasseControladora.ConfiguracoesPrincipais.DiretorioBD;

            txtDiretorioGerado.LostFocus += SalvarConfig;
            txtDiretorioBD.LostFocus += SalvarConfig;
        }

        /// <summary>
        /// Salva as configurações atuais
        /// </summary>
        void SalvarConfig(object sender, EventArgs e)
        {
            if (txtDiretorioBD.Text != ClasseControladora.ConfiguracoesPrincipais.DiretorioBD
                || txtDiretorioGerado.Text != ClasseControladora.ConfiguracoesPrincipais.DiretorioGerar)
            {
                ClasseControladora.ConfiguracoesPrincipais.DiretorioBD = txtDiretorioBD.Text;
                ClasseControladora.ConfiguracoesPrincipais.DiretorioGerar = txtDiretorioGerado.Text;
                ClasseControladora.ConfiguracoesPrincipais.Salvar();
            }
        }

        /// <summary>
        /// Define o texto do diretório gerado como o desktop
        /// </summary>
        private void btnDesktop_Click(object sender, EventArgs e)
        {
            txtDiretorioGerado.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            SalvarConfig(this, null);
        }

        /// <summary>
        /// Busca um diretório para o Diretorio do banco de dados
        /// </summary>
        private void btnBuscarDirBD_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Banco dados sql server|*.mdf";
            DialogResult result = ofd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtDiretorioBD.Text = ofd.FileName;
                SalvarConfig(this, null);
            }
        }

        /// <summary>
        /// Busca um diretório para gerar os arquivos
        /// </summary>
        private void btnBuscarDirGerar_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtDiretorioGerado.Text = fbd.SelectedPath;
                SalvarConfig(this, null);
            }
        }

        /// <summary>
        /// Gera os arquivos do Banco de dados
        /// </summary>
        private void btnGerar_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtDiretorioGerado.Text))
            {
                MessageBox.Show("O diretorio que será gerado os arquivos está inválido");
                txtDiretorioGerado.Focus();
                return;
            }
            if (!File.Exists(txtDiretorioBD.Text))
            {
                MessageBox.Show("O diretorio do banco de dados está inválido");
                txtDiretorioBD.Focus();
                return;
            }
            ControladorBD.StringConexao = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + txtDiretorioBD.Text +
                ";Integrated Security=True;Connect Timeout=30";

            var bw = ClasseControladora.TestarConexao();
            lblStatus.ForeColor = Color.Black;
            carregamentos.FicarAtualizandoLabel("Testando conexão", lblStatus, bw);
            bw.RunWorkerCompleted += ConexaoAceita;
            CancelarBotoes();
        }

        /// <summary>
        /// Quando a conexão é aceita
        /// </summary>
        void ConexaoAceita(object sender, RunWorkerCompletedEventArgs e)
        {
            if (ClasseControladora.EstadoTesteCon == ClasseControladora.TesteConexao.TestouRetornouErro)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Não foi possível conectar com esse banco de dados";
                HabilitarBotoes();
            }
            else if (ClasseControladora.EstadoTesteCon == ClasseControladora.TesteConexao.Testou)
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += GerandoArquivos;
                bw.RunWorkerCompleted += ArquivosGerados;
                bw.RunWorkerAsync();
                lblStatus.ForeColor = Color.Black;
                carregamentos.FicarAtualizandoLabel("Gerando Arquivos", lblStatus, bw);
            }
            else
            {
                MessageBox.Show("Ocorreu um erro desconhecido e o programa precisa ser fechado");
                Application.Exit();
            }
        }

        /// <summary>
        /// Quando está gerando os arquivos
        /// </summary>
        void GerandoArquivos(object sender, DoWorkEventArgs e)
        {
            try
            {
                GeradorObjetosBD gerador = new GeradorObjetosBD();
                gerador.GerarArquivos(txtDiretorioGerado.Text, carregamentos);
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        void ArquivosGerados(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result == null)
            {
                lblStatus.ForeColor = Color.Blue;
                lblStatus.Text = "Arquivos gerados com sucesso";
            }
            else
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Ocorreu um erro ao tentar gerar arquivos";
            }
            HabilitarBotoes();
        }

        public void CancelarBotoes()
        {
            txtDiretorioGerado.Enabled = false;
            txtDiretorioBD.Enabled = false;
            btnBuscarDirBD.Enabled = false;
            btnBuscarDirGerar.Enabled = false;
            btnDesktop.Enabled = false;
            btnGerar.Enabled = false;
        }

        public void HabilitarBotoes()
        {
            txtDiretorioGerado.Enabled = true;
            txtDiretorioBD.Enabled = true;
            btnBuscarDirBD.Enabled = true;
            btnBuscarDirGerar.Enabled = true;
            btnDesktop.Enabled = true;
            btnGerar.Enabled = true;
        }
    }
}
