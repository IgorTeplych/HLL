using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLL
{
    public static class MurMurHash3
    {
        const uint seed = 0;

        public static int Hash(byte[] bytes)
        {
            unchecked
            {
                const uint c1 = 0xcc9e2d51;
                const uint c2 = 0x1b873593;

                uint h1 = seed;
                uint k1 = 0;

                for (int i = 0; i < bytes.Length; i = i + 4)
                {
                    var chunkLength = bytes.Length - i;
                    switch (chunkLength)
                    {
                        case 4:
                            k1 = (uint)(bytes[i] | bytes[i + 1] << 8 | bytes[i + 2] << 16 | bytes[i + 3] << 24);

                            k1 *= c1;
                            k1 = rotl32(k1, 15);
                            k1 *= c2;

                            h1 ^= k1;
                            h1 = rotl32(h1, 13);
                            h1 = h1 * 5 + 0xe6546b64;
                            break;
                        case 3:
                            k1 = (uint)(bytes[i] | bytes[i + 1] << 8 | bytes[i + 2] << 16);
                            k1 *= c1;
                            k1 = rotl32(k1, 15);
                            k1 *= c2;
                            h1 ^= k1;
                            break;
                        case 2:
                            k1 = (uint)(bytes[i] | bytes[i + 1] << 8);
                            k1 *= c1;
                            k1 = rotl32(k1, 15);
                            k1 *= c2;
                            h1 ^= k1;
                            break;
                        case 1:
                            k1 = (uint)(bytes[i]);
                            k1 *= c1;
                            k1 = rotl32(k1, 15);
                            k1 *= c2;
                            h1 ^= k1;
                            break;
                    }
                }

                h1 ^= (uint)bytes.Length;
                h1 = finalizer32(h1);

                return (int)h1;
            }
        }

        private static uint rotl32(uint x, byte r)
        {
            return (x << r) | (x >> (32 - r));
        }

        private static uint finalizer32(uint h)
        {
            unchecked
            {
                h ^= h >> 16;
                h *= 0x85ebca6b;
                h ^= h >> 13;
                h *= 0xc2b2ae35;
                h ^= h >> 16;
                return h;
            }
        }
    }
}
