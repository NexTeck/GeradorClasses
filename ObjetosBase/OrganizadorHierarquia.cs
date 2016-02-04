using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador_Classes_BD
{
    /* Essa classe foi feita para conseguir determinar a hierarquia das tabelas para o método UPDATE
     * O método UPDATE similar do tableAdapter vai seguir algumas partes
     * terá um objeto auxiliar que salvará as querys e chaves primárias e estrangeiras de um objeto antes de 
     * executar o código
     * 
     * terá as chaves primárias e estrangeiras porque se o código der erro no banco de dados
     * é executado o rollback que voltará as chaves primárias dos objetos
     * 
     * Esse modelo só serve para relações de primarykey com foreingkey
     * */

    /// <summary>
    /// Interface que serve para definir uma hierarquia de valores
    /// </summary>
    public interface IObjetoHierarquico
    {
        /// <summary>
        /// Verifica se um objeto hierarquico é menor do que outro
        /// </summary>
        /// <param name="objTestado">O objeto testado</param>
        /// <returns>Se é menor ou não</returns>
        bool VerificarSeSerMaior(IObjetoHierarquico objTestado);
    }

    /// <summary>
    /// Classe que serve para ordenar a hierarquia de objetos IObjetoHierarquico
    /// </summary>
    public static class OrganizadorDeHierarquia
    {
        /// <summary>
        /// Cria um novo vetor organizado hierarquicamente a partir de um desorganizado
        /// </summary>
        public static IObjetoHierarquico[] OrganizarHierarquia(IObjetoHierarquico[] objetosForaOrdem)
        {
            IObjetoHierarquico[] vetorOrdenado = new IObjetoHierarquico[objetosForaOrdem.Length];
            for (int i = 0; i < vetorOrdenado.Length; i++)
            {
                vetorOrdenado[i] = objetosForaOrdem[i];
            }
            //Verifica um por um
            for (int i = 0; i < vetorOrdenado.Length - 1; i++)
            {
                for (int j = i + 1; j < vetorOrdenado.Length; j++)
                {
                    // Ser filho equivale a ser menor do que o outro
                    if (vetorOrdenado[i].VerificarSeSerMaior(vetorOrdenado[j]))
                    {
                        //Faz uma inversão
                        IObjetoHierarquico copia = vetorOrdenado[i];
                        vetorOrdenado[i] = vetorOrdenado[j];
                        vetorOrdenado[j] = copia;
                    }
                }
            }
            return vetorOrdenado;
        }
    }
}
