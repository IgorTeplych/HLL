using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLL
{
    //https://dimchansky.github.io/posts/2014/10/26/hyperloglog-csharp-implementation/
    internal class HyperLogLog
    {
        const double TwoPowOf32 = 0x100000000;

        private readonly int _kComplement;
        private readonly int _m;
        private readonly double _alphaM2;
        private readonly byte[] _registerM;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="k">Число байтовых регистров, которое будет выделено в оперативной памяти для приблизительного подсчета числа уникальных элементов</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public HyperLogLog(int k)
        {
            if (k < 4 || k > 16)
            {
                throw new ArgumentOutOfRangeException("k", k, "k must be between 4 and 16 inclusive");
            }

            _kComplement = 32 - k;
            _m = 1 << k;
            var alphaM = _m == 16
                ? 0.673
                : _m == 32
                    ? 0.697
                    : _m == 64
                        ? 0.709
                        : 0.7213 / (1.0 + 1.079 / _m);
            _alphaM2 = alphaM * _m * _m;
            _registerM = new byte[_m];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hash">32-битный хеш элемента из набора, в котором нужно оценить число уникальных элементов</param>
        public void Add(int hash)
        {
            var idx = (uint)hash >> _kComplement;
            var rank = Math.Min(_kComplement, CountTrailingZeroBits(hash)) + 1;
            _registerM[idx] = (byte)Math.Max(_registerM[idx], rank);
        }

        public double EstimateCount()
        {
            double c = 0.0;
            for (int i = 0; i < _m; i++)
            {
                c += 1.0 / Math.Pow(2, _registerM[i]);
            }

            double estimate = _alphaM2 / c;

            if (estimate <= (2.5 * _m))
            {
                int v = 0;
                for (int i = 0; i < _m; i++)
                {
                    if (_registerM[i] == 0) { v++; }
                }

                if (v > 0)
                {
                    estimate = _m * Math.Log((double)_m / v);
                }
            }
            else if (estimate > (TwoPowOf32 / 30.0))
            {
                estimate = -TwoPowOf32 * Math.Log(1.0 - (estimate / TwoPowOf32));
            }

            return estimate;
        }

        public void UnionWith(HyperLogLog other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            if (other._m != _m)
            {
                throw new InvalidOperationException("The number of counters must be the same.");
            }

            if (other == this)
            {
                return;
            }

            for (int i = 0; i < _m; i++)
            {
                _registerM[i] = Math.Max(_registerM[i], other._registerM[i]);
            }
        }

        private static int CountTrailingZeroBits(int v)
        {
            if ((v & 0x1) == 1) { return 0; }

            int c = 1;
            if ((v & 0xffff) == 0) { v >>= 16; c += 16; }
            if ((v & 0xff) == 0) { v >>= 8; c += 8; }
            if ((v & 0xf) == 0) { v >>= 4; c += 4; }
            if ((v & 0x3) == 0) { v >>= 2; c += 2; }
            c -= v & 0x1;
            return c;
        }
    }
}
