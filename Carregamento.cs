using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gerador_Classes_BD
{
    /// <summary>
    /// Classe responsável por mostrar carregamentos em andamento
    /// </summary>
    public class Carregamentos
    {
        string textoAtualizado;
        bool textoMudou;
        Timer timer;
        int contador;
        Label label;
        public bool Rodando { get; private set; }

        public string TextoAtualizado
        {
            get { return textoAtualizado; }
            set 
            {
                if (value != null && textoAtualizado != value)
                {
                    textoAtualizado = value;
                    textoMudou = true;
                }
            }
        }

        public void FicarAtualizandoLabel(string textoAtualizado_, Label label_, System.ComponentModel.BackgroundWorker bw)
        {
            if (Rodando)
            {
                CancelarOperacao();
            }
            textoAtualizado = textoAtualizado_;
            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += timer_Tick;
            Rodando = true;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            label = label_;
            timer.Enabled = true;
        }

        void CancelarOperacao()
        {
            if (Rodando)
            {
                textoMudou = false;
                Rodando = false;
                contador = 0;
                timer.Tick -= timer_Tick;
                timer.Enabled = false;
                timer = null;
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (textoMudou)
            {
                textoMudou = false;
                contador = 0;
            }
            contador++;
            int a = contador / 5;
            if (a >= 4)
            {
                contador = 0;
            }
            string pontos = "";
            for (int i = 0; i < a; i++)
            {
                pontos += ".";
            }
            label.Text = textoAtualizado + pontos;
        }

        void bw_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            CancelarOperacao();
        }
    }
}
