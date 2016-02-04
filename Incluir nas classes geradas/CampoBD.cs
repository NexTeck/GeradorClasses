using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesBD
{

    /// <summary>
    /// Classe que possui os atributos básicos de um campo no banco de dados
    /// </summary>
    public class CampoBD
    {
        TipoVariavelPadraoCampo tipoPadrao;
        /// <summary>
        /// A ordem desse campo no ObjetoBD
        /// </summary>
        public int OrdemCampo { get; private set; }
        /// <summary>
        /// O nome do campo no banco de dados
        /// </summary>
        public string NomeCampo { get; private set; }
        /// <summary>
        /// Indica se permite nulos
        /// </summary>
        public bool PermiteNull { get; private set; }
        /// <summary>
        /// O tipo do campo no banco de dados
        /// </summary>
        public System.Data.SqlDbType TipoVariavelBD { get; private set; }
        /// <summary>
        /// O tipo do campo na variável
        /// </summary>
        public Type TipoVariavel { get; private set; }
        /// <summary>
        /// Tamanho quando é do tipo varchar ou outro
        /// </summary>
        public int MaxLenght { get; private set; }
        /// <summary>
        /// Tamanho quando é do tipo varchar ou outro
        /// </summary>
        public TipoCampo TipoCampo { get; set; }

        /// <summary>
        /// Declara o objeto e define seus campos
        /// </summary>
        public CampoBD(int ordemCampo, string nomeCampo, bool permiteNull, System.Data.SqlDbType tipoBD, Type tipo, int maxLenght, TipoCampo tipoCampo)
        {
            OrdemCampo = ordemCampo;
            NomeCampo = nomeCampo;
            PermiteNull = permiteNull;
            TipoVariavelBD = tipoBD;
            TipoVariavel = tipo;
            MaxLenght = maxLenght;
            TipoCampo = tipoCampo;

            if (PermiteNull || tipo == typeof(string))
                tipoPadrao = TipoVariavelPadraoCampo.Nulo;
            else if (tipo == typeof(DateTime))
                tipoPadrao = TipoVariavelPadraoCampo.Data;
            else if (tipo == typeof(int) || tipo == typeof(long) || tipo == typeof(double) || tipo == typeof(decimal))
            {
                if (TipoCampo == ClassesBD.TipoCampo.Comum || TipoCampo == ClassesBD.TipoCampo.Unique)
                    tipoPadrao = TipoVariavelPadraoCampo.Numero;
                else
                    tipoPadrao = TipoVariavelPadraoCampo.ChaveNumerica;
            }
        }

        /// <summary>
        /// Retorna o valor padrão do campo
        /// </summary>
        public object RetornarValorPadrao()
        {
            if (PermiteNull)
                return null;
            else if (tipoPadrao == TipoVariavelPadraoCampo.Numero)
                return 0;
            else if (tipoPadrao == TipoVariavelPadraoCampo.Data)
                return DateTime.Now;
            else
                return -1;
        }

        /// <summary>
        /// Indica o tipo padrao de variavel
        /// </summary>
        private enum TipoVariavelPadraoCampo
        {
            /// <summary> O tipo padrão será um número </summary>
            Numero = 0,
            /// <summary> O tipo padrão será uma chave númerica ou seja -1 </summary>
            ChaveNumerica,
            /// <summary> O tipo padrão será uma data </summary>
            Data,
            /// <summary> O tipo padrão será nulo </summary>
            Nulo
        }
    }

    /// <summary>
    /// Indica o tipo de um campo
    /// </summary>
    public enum TipoCampo
    {
        /// <summary> É um campo comum </summary>
        Comum = 0,
        /// <summary> É uma chave primária </summary>
        PrimaryKey,
        /// <summary> É uma chave unica </summary>
        Unique,
        /// <summary> É uma chave estrangeira </summary>
        ForeignKey,
        /// <summary> É uma chave primária e também uma chave estrangeira </summary>
        ForeignKey_PrimaryKey,
        /// <summary> É uma chave estrangeira e também unica </summary>
        ForeignKey_Unique
    }
}
