using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace MrCoto.ChileanRut
{
    /// <summary>
    /// Rut Chileno.
    /// </summary>
    public class Rut : IComparable<Rut>
    {
        private static readonly string RGX_NUMBER = @"^([1-9]\d?(\.?\d{3}){0,2}|[1-9]\d{0,2}(\.?\d{3})?)$";
        private static readonly string RGX_DV = @"^([0-9]|k|K)$";

        private const int ELEVEN = 11;
        private const int TEN = 10;
        private const string ZERO = "0";
        private const string K = "k";

        private const int MIN_RANGE = 4_000_000;
        private const int MAX_RANGE = 80_000_000;

        private int _number;
        private string _dv;

        /// <summary>
        /// Constructor principal.
        /// </summary>
        /// <param name="number">Parte númerica de un rut (de 123-k, la parte númerica es "123").</param>
        /// <param name="dv">Dígito verificador de un rut.</param>
        public Rut(string number, string dv)
        {   
            if (!Regex.Match(number, RGX_NUMBER).Success) throw new ArgumentException("Formato Inválido");
            if (!Regex.Match(dv, RGX_DV).Success) throw new ArgumentException("Digito Verificador Inválido");
            _number = int.Parse(number.Replace(".", ""));
            _dv = dv.ToLower();
        }

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
            if (string.IsNullOrWhiteSpace(rut) || rut == ZERO) return new Rut("", "");
            var dv = !string.IsNullOrWhiteSpace(rut) ? rut[^1].ToString() : "";
            var sub = rut.Substring(0, rut.Length - 1);
            var number = sub[^1] == '-' ? sub.Substring(0, sub.Length - 1) : sub;
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

        /// <summary>
        /// Valida un Rut.
        /// </summary>
        /// <returns>Verdadero si el RUT es válido.</returns>
        public bool IsValid() => CalcDv(_number) == _dv;
        
        /// <summary>
        /// Formatea el RUT.
        /// 
        /// - [RUTFORMAT.FULL] -> RUT con puntos y guión (12.345.678-k)
        /// - [RUTFORMAT.ONLY_DASH] -> RUT sin puntos y con guión (12345678-k)
        /// - [RUTFORMAT.ESCAPED] -> RUT sin puntos y guión (12345678k)
        /// </summary>
        /// <param name="format">Formato del RUT</param>
        /// <returns>String del Rut formateado.</returns>
        public string Format(RutFormat format = RutFormat.FULL) => format switch
        {
            RutFormat.FULL => $"{_number:n0}-{_dv}".Replace(",", "."),
            RutFormat.ONLY_DASH => $"{_number}-{_dv}",
            RutFormat.ESCAPED => $"{_number}{_dv}",
            _ => this.Format() 
        };

        /// <summary>
        /// Compara esta instancia de Rut con otra,
        /// </summary>
        /// <param name="other">La otra instancia de Rut.</param>
        /// <returns>Valor de comparación.</returns>
        public int CompareTo(Rut other) => _number.CompareTo(other._number);

        /// <summary>
        /// Igualdad estructural.
        /// </summary>
        /// <param name="obj">Otra Instancia (o null).</param>
        /// <returns>Verdadero si son un Rut es igual a otro (estructuralmente).</returns>
        #nullable enable
        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Rut))
            {
                return false;
            }
            var other = (Rut) obj;
            return other._number == _number && other._dv == _dv;
        }
        #nullable disable

        /// <summary>
        /// Hash Code.
        /// </summary>
        /// <returns>Valor hash.</returns>
        public override int GetHashCode() => 31 * _number + _dv.GetHashCode();

        /// <summary>
        /// Representación de un Rut.
        /// </summary>
        /// <returns>Representación en string de un Rut.</returns>
        public override string ToString() => $"Rut({_number}, {_dv})";

        /// <summary>
        /// Destructuración de un Rut.
        /// </summary>
        /// <param name="number">Parte numérica del Rut.</param>
        /// <param name="dv">Dígito verificador del Rut.</param>
        public void Deconstruct(out int number, out string dv)
        {
            number = _number;
            dv = _dv;
        }

        /// <summary>
        /// Definición de 'Menor que'.
        /// </summary>
        /// <param name="x">Operando Izquierdo.</param>
        /// <param name="y">Operando Derecho.</param>
        /// <returns>Verdadero Si x es menor que y.</returns>
        public static bool operator <(Rut x, Rut y) => x.CompareTo(y)  < 0;
        /// <summary>
        /// Definición de 'Menor o igual que'.
        /// </summary>
        /// <param name="x">Operando Izquierdo.</param>
        /// <param name="y">Operando Derecho.</param>
        /// <returns>Verdadero Si x es menor o igual que y.</returns>
        public static bool operator <=(Rut x, Rut y) => x.CompareTo(y)  <= 0;
        /// <summary>
        /// Definición de 'Mayor que'.
        /// </summary>
        /// <param name="x">Operando Izquierdo.</param>
        /// <param name="y">Operando Derecho.</param>
        /// <returns>Verdadero Si x es mayor que y.</returns>
        public static bool operator >(Rut x, Rut y) => x.CompareTo(y)  > 0;
        /// <summary>
        /// Definición de 'Mayor o igual que'.
        /// </summary>
        /// <param name="x">Operando Izquierdo.</param>
        /// <param name="y">Operando Derecho.</param>
        /// <returns>Verdadero Si x es mayor o igual que y.</returns>
        public static bool operator >=(Rut x, Rut y) => x.CompareTo(y)  >= 0;
        /// <summary>
        /// Definición de 'Igual que'.
        /// </summary>
        /// <param name="x">Operando Izquierdo.</param>
        /// <param name="y">Operando Derecho.</param>
        /// <returns>Verdadero Si x es igual que y.</returns>
        public static bool operator ==(Rut x, Rut y) => x.Equals(y);
        /// <summary>
        /// Definición de 'Distinto de'.
        /// </summary>
        /// <param name="x">Operando Izquierdo.</param>
        /// <param name="y">Operando Derecho.</param>
        /// <returns>Verdadero Si x es distinto de y.</returns>
        public static bool operator !=(Rut x, Rut y) => !x.Equals(y);

    }
}