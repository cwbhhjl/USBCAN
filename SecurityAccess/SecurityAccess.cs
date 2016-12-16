using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityAccess
{
    public class SecurityAccess
    {
        public static byte[] seedToKey(uint mask, byte[] seed)
        {
            return securityAlgorithm_0(mask, seed);
        }

        public static byte[] seedToKey(byte[] seed)
        {
            return securityAlgorithm_2(seed);
        }

        private static byte[] securityAlgorithm_0(uint MASK, byte[] seed)
        {
            uint seedInt = (uint)((seed[0] << 24) + (seed[1] << 16) + (seed[2] << 8) + seed[3]);
            uint keyInt = (seedInt ^ MASK) + MASK;

            byte[] key = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                key[i] = (byte)(keyInt >> (8 * (3 - i)));
            }
            return key;
        }

        private static uint securityAlgorithm_1(uint MASK, uint seed)
        {
            for (int i = 0; i < 35; i++)
            {
                if ((seed & 0x80000000) == 0x80000000)
                {
                    seed <<= 1;
                    seed ^= MASK;
                }
                else
                {
                    seed <<= 1;
                }
            }
            return seed;
        }

        private static byte[] securityAlgorithm_1(uint MASK, byte[] seed)
        {
            uint seedInt = (uint)((seed[0] << 24) + (seed[1] << 16) + (seed[2] << 8) + seed[3]);
            uint keyInt = securityAlgorithm_1(MASK, seedInt);
            byte[] key = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                key[i] = (byte)(keyInt >> (8 * (3 - i)));
            }
            return key;
        }

        private static byte[] securityAlgorithm_2(byte[] seed)
        {
            byte[] Xor = new byte[4]
            {
                0x14,0x28,0xEC,0x6D
            };
            byte[] Cal = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                Cal[i] = (byte)(seed[i] ^ Xor[i]);
            }
            byte[] key = new byte[4];
            key[0] = (byte)(((Cal[3] & 0x0F) << 4) | (Cal[2] & 0xF0));
            key[1] = (byte)(((Cal[2] & 0x3F) << 2) | ((Cal[1] & 0xFC) >> 2));
            key[2] = (byte)(((Cal[1] & 0xFC) >> 2) | (Cal[0] & 0xC0));
            key[3] = (byte)(((Cal[0] & 0x0F) << 4) | (Cal[3] & 0x0F));
            return key;
        }
    }
}
