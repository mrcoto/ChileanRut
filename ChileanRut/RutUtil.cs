using System;
using System.Collections.Generic;

namespace MrCoto.ChileanRut
{
    /// <summary>
    /// Métodos static
    /// </summary>
    public partial class Rut
    {
        /// <summary>
        /// Obtiene un Rut dada la parte numérica y su dv.
        /// 
        /// Ejemplo formatos aceptados:
        /// 
        /// - 12345678-k
        /// - 12345678k
        /// - 12.345.678-k
        /// - 12.345.678k
        /// - 12.345.678K.
        /// </summary>
        /// <param name="rut">Rut con su parte númerica y dv.</param>
        /// <returns>Instancia de un Rut</returns>
        public static Rut Parse(string rut)
        {
            if (string.IsNullOrWhiteSpace(rut) || rut.Length <= 1) return new Rut("", "");
            var dv = !string.IsNullOrWhiteSpace(rut) ? rut[^1].ToString() : "";
            var sub = rut[0..^1];
            var number = sub[^1] == '-' ? sub[0..^1] : sub;
            return new Rut(number, dv);
        }

        /// <summary>
        /// Obtiene un Rut dada la parte numérica.
        /// </summary>
        /// <param name="number">Parte numérica del RUT (Ej si es 123-k, sería 123).</param>
        /// <returns>Instancia de un Rut</returns>
        public static Rut Parse(int number) => new Rut(number.ToString(), CalcDv(number));

        /// <summary>
        /// Calcula el digito verificador de un Rut (data una parte numérica).
        /// </summary>
        /// <param name="number">Parte númerica del rut.</param>
        /// <returns>Dígito verificador.</returns>
        public static string CalcDv(int number)
        {
            int[] series = {2, 3, 4, 5, 6, 7};
            var (sum, index) = (0, 0);
            while (number > 0)
            {
                sum += series[index] * (number % TEN);
                number /= TEN;
                index = index < series.Length - 1 ? index + 1 : 0;
            }
            return DvFromResult(ELEVEN - (sum % ELEVEN));
        }

        private static string DvFromResult(int result) => result switch
        {
            ELEVEN => ZERO,
            TEN => K,
            _ => result.ToString()
        };

        /// <summary>
        /// Genera un Rut de forma aleatoria.
        /// </summary>
        /// <param name="min">Valor mínimo que podría tener la parte numérica del Rut.</param>
        /// <param name="max">Valor máximo que podría tener la parte numérica del Rut.</param>
        /// <param name="seed">Indica si el RUT se generará mediante un seed dado. null si no se debe aplicar.</param>
        /// <returns>Rut aleatorio.</returns>
        public static Rut Random(int min = MIN_RANGE, int max = MAX_RANGE, int? seed = null)
        {
            var random = seed == null ? new Random() : new Random((int) seed);
            return Rut.Parse(random.Next(min, max));
        }

        /// <summary>
        /// Genera una lista de Ruts de forma aleatoria
        /// </summary>
        /// <param name="n">Tamaño de la lista.</param>
        /// <param name="min">Valor mínimo que podría tener la parte numérica del Rut.</param>
        /// <param name="max">Valor máximo que podría tener la parte numérica del Rut.</param>
        /// <param name="seed">Indica si el RUT se generará mediante un seed dado. null si no se debe aplicar.</param>
        /// <returns>Lista de Ruts aleatorios.</returns>
        public static List<Rut> Randoms(int n = 1, int min = MIN_RANGE, int max = MAX_RANGE, int? seed = null)
        {
            var random = seed == null ? new Random() : new Random((int) seed);
            var rutList = new List<Rut>();
            for(int i = 0; i < n; i++) rutList.Add(Rut.Parse(random.Next(min, max)));
            return rutList;
        }

        /// <summary>
        /// Genera una lista de Ruts únicos de forma aleatoria
        /// </summary>
        /// <param name="n">Tamaño de la lista.</param>
        /// <param name="min">Valor mínimo que podría tener la parte numérica del Rut.</param>
        /// <param name="max">Valor máximo que podría tener la parte numérica del Rut.</param>
        /// <param name="seed">Indica si el RUT se generará mediante un seed dado. null si no se debe aplicar.</param>
        /// <returns>Lista única de Ruts aleatorios.</returns>
        public static List<Rut> Uniques(int n = 1, int min = MIN_RANGE, int max = MAX_RANGE, int? seed = null)
        {
            var random = seed == null ? new Random() : new Random((int) seed);
            var rutUniqueList = new List<Rut>();
            while(rutUniqueList.Count < n) rutUniqueList.Add(Rut.Parse(random.Next(min, max)));
            return rutUniqueList;
        }

    }
}