using System.Collections.Generic;
using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace MrCoto.ChileanRut
{
    public class Rut : IComparable<Rut>
    {
        private static readonly string RGX_NUMBER = @"^([1-9]\d?(\.?\d{3}){0,2}|[1-9]\d{0,2}(\.?\d{3})?)$";
        private static readonly string RGX_DV = @"^([0-9]|k|K)$";

        private const int ELEVEN = 11;
        private const int TEN = 10;
        private const string ZERO = "0";
        private const string K = "K";

        private const int MIN_RANGE = 4_000_000;
        private const int MAX_RANGE = 80_000_000;

        private int _number;
        private string _dv;

        public Rut(string number, string dv)
        {   
            Contract.Requires(Regex.Match(number, RGX_NUMBER).Success, "Formato Inválido");
            Contract.Requires(Regex.Match(dv, RGX_DV).Success, "Digito Verificador Inválido");
            _number = int.Parse(number.Replace(".", ""));
            _dv = dv.ToLower();
        }

        public static Rut Parse(string rut)
        {
            if (string.IsNullOrWhiteSpace(rut) || rut == ZERO) return new Rut("", "");
            var dv = !string.IsNullOrWhiteSpace(rut) ? rut[^1].ToString() : "";
            var sub = rut.Substring(0, rut.Length - 1);
            var number = sub[^1] == '-' ? sub.Substring(0, sub.Length - 1) : sub;
            return new Rut(number, dv);
        }

        public static Rut Parse(int number) => new Rut(number.ToString(), CalcDv(number));

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

        public static Rut Random(int min = MIN_RANGE, int max = MAX_RANGE, int? seed = null)
        {
            var random = seed == null ? new Random() : new Random((int) seed);
            return Rut.Parse(random.Next(min, max));
        }

        public static List<Rut> Randoms(int n = 1, int min = MIN_RANGE, int max = MAX_RANGE, int? seed = null)
        {
            var random = seed == null ? new Random() : new Random((int) seed);
            var rutList = new List<Rut>();
            for(int i = 0; i < n; i++) rutList.Add(Rut.Parse(random.Next(min, max)));
            return rutList;
        }

        public static List<Rut> Uniques(int n = 1, int min = MIN_RANGE, int max = MAX_RANGE, int? seed = null)
        {
            var random = seed == null ? new Random() : new Random((int) seed);
            var rutUniqueList = new List<Rut>();
            while(rutUniqueList.Count < n) rutUniqueList.Add(Rut.Parse(random.Next(min, max)));
            return rutUniqueList;
        }

        public bool IsValid() => CalcDv(_number) == _dv;
        
        public string Format(RutFormat format = RutFormat.FULL) => format switch
        {
            RutFormat.FULL => $"{_number:n0}-{_dv}".Replace(",", "."),
            RutFormat.ONLY_DASH => $"{_number}-{_dv}",
            RutFormat.ESCAPED => $"{_number}{_dv}",
            _ => this.Format() 
        };

        public int CompareTo(Rut other) => _number.CompareTo(other._number);

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

        public override int GetHashCode() => 31 * _number + _dv.GetHashCode();

        public override string ToString() => $"Rut({_number}, {_dv})";

        public void Deconstruct(out int number, out string dv)
        {
            number = _number;
            dv = _dv;
        }

        public static bool operator <(Rut x, Rut y) => x.CompareTo(y)  < 0;
        public static bool operator <=(Rut x, Rut y) => x.CompareTo(y)  <= 0;
        public static bool operator >(Rut x, Rut y) => x.CompareTo(y)  > 0;
        public static bool operator >=(Rut x, Rut y) => x.CompareTo(y)  >= 0;
        public static bool operator ==(Rut x, Rut y) => x.Equals(y);
        public static bool operator !=(Rut x, Rut y) => !x.Equals(y);

    }
}