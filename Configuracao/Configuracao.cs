﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BDController
{
    /// <summary>
    /// Objeto que armazena as configurações do programa
    /// </summary>
    public static class ControllerConfig
    {
        /// <summary>
        /// Salva as configurações de um objeto de configuração
        /// </summary>
        public static void Salvar(ObjetoConfig objetoConfig)
        {
            FileStream stream = new FileStream(objetoConfig.GetType().Name + ".bin", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, objetoConfig);
            stream.Close();
        }

        /// <summary>
        /// Carrega um objeto de configuração de um diretório específico
        /// </summary>
        /// <param name="tipo">O tipo da configuração que será carregada</param>
        public static ObjetoConfig Carregar(Type tipo)
        {
            FileStream inStr = new FileStream(tipo.Name + ".bin", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            ObjetoConfig objetoConfig = bf.Deserialize(inStr) as ObjetoConfig;
            inStr.Close();
            return objetoConfig;
        }

        /// <summary>
        /// Carrega um objeto de configuração de um diretório específico, se ele não existe então é criado
        /// </summary>
        /// <param name="dir">O diretorio a ser carregado</param>
        public static ObjetoConfig CarregarOuCriar(Type tipo)
        {
            ObjetoConfig objetoConfig = null;
            //Se existir as configurações tenta carregar
            if (File.Exists(tipo.Name + ".bin"))
            {
                try
                {
                    objetoConfig = Carregar(tipo);
                }
                //Se não conseguir define padrão e salva
                catch (Exception ex1)
                {
                    System.Windows.Forms.MessageBox.Show(ex1.Message);
                    try
                    {
                        objetoConfig = (ObjetoConfig)Activator.CreateInstance(tipo);
                        objetoConfig.DefinirPadrao();
                        Salvar(objetoConfig);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                try
                {
                    objetoConfig = (ObjetoConfig)Activator.CreateInstance(tipo);
                    objetoConfig.DefinirPadrao();
                    Salvar(objetoConfig);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
            return objetoConfig;
        }
    }
}
