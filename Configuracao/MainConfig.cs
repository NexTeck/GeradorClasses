using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador_Classes_BD
{
    /// <summary>
    /// Classe que armazena as principais configurações do programa
    /// Andrei 27/10/2015
    /// </summary>
    [Serializable]
    public class MainConfig : BDController.ObjetoConfig
    {
        /// <summary>
        /// O diretorio do banco de dados que será copiado
        /// </summary>
        public string DiretorioBD { get; set; }
        /// <summary>
        /// O diretorio que será gerado os arquivos de .cs
        /// </summary>
        public string DiretorioGerar { get; set; }

        /// <summary>
        /// Define as configurações padrão do programa
        /// </summary>
        public override void DefinirPadrao()
        {
            DiretorioBD = "";
            DiretorioGerar = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }
    }
}
