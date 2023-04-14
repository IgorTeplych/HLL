using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLL
{
    internal class MurMurHash2
    {
        public static int GetHash(char[] chars)
        {
            int m = 0x5bd1e995;
            int seed = 0;
            int r = 24;
            int len = chars.Length;

            int h = seed ^ len;
            int k = 0;

            int count = 0;
            while (len >= 4)
            {
                k = chars[count];
                k |= chars[count++] << 8;
                k |= chars[count++] << 16;
                k |= chars[count++] << 24;

                k *= m;
                k ^= k >> r;
                k *= m;

                h *= m;
                h ^= k;
                len -= 4;
            }

            switch (len)
            {
                case 3:
                    {
                        h ^= chars[2] << 16;
                        h ^= chars[1] << 8;
                        h ^= chars[0];
                        h *= m;
                        break;
                    }
                case 2:
                    {
                        h ^= chars[1] << 8;
                        h ^= chars[0];
                        h *= m;
                        break;
                    }
                case 1:
                    {
                        h ^= chars[0];
                        h *= m;
                        break;
                    }
            };

            h ^= h >> 13;
            h *= m;
            h ^= h >> 15;

            return h;
        }
        public static int GetHash(byte[] bytes)
        {
            int m = 0x5bd1e995;
            int seed = 0;
            int r = 24;
            int len = bytes.Length;

            int h = seed ^ len;
            int k = 0;

            int count = 0;
            while (len >= 4)
            {
                k = bytes[count];
                k |= bytes[count++] << 8;
                k |= bytes[count++] << 16;
                k |= bytes[count++] << 24;

                k *= m;
                k ^= k >> r;
                k *= m;

                h *= m;
                h ^= k;
                len -= 4;
            }

            switch (len)
            {
                case 3:
                    {
                        h ^= bytes[2] << 16;
                        h ^= bytes[1] << 8;
                        h ^= bytes[0];
                        h *= m;
                        break;
                    }
                case 2:
                    {
                        h ^= bytes[1] << 8;
                        h ^= bytes[0];
                        h *= m;
                        break;
                    }
                case 1:
                    {
                        h ^= bytes[0];
                        h *= m;
                        break;
                    }
            };

            h ^= h >> 13;
            h *= m;
            h ^= h >> 15;

            return h;
        }
    }
}
