using System;
using System.Collections;

namespace USBCAN
{
    class Security
    {
        private uint MASK;
        private const uint MASK_DEFAULT = 0xA5CEFDB6U;
        private byte securityAlgorithmType;

        public Security()
        {
            securityAlgorithmType = 0;
            MASK = MASK_DEFAULT;
        }

        public Security(byte securityAlgorithmType)
        {
            this.securityAlgorithmType = securityAlgorithmType;
            MASK = MASK_DEFAULT;
        }

        public Security(byte securityAlgorithmType, uint mask)
        {
            this.securityAlgorithmType = securityAlgorithmType;
            MASK = mask;
        }

        public uint seedToKey(uint seed)
        {
            switch (securityAlgorithmType)
            {
                case 0x00:
                    return securityAlgorithm_0(seed);
                case 0x01:
                    return securityAlgorithm_1(seed);
                default:
                    return 0;
            }
        }

        public byte[] seedToKey(byte[] seed)
        {
            switch (securityAlgorithmType)
            {
                case 0x00:
                    return securityAlgorithm_0(seed);
                case 0x01:
                    return securityAlgorithm_1(seed);
                case 0x02:
                    return securityAlgorithm_2(seed);
                default:
                    return null;
            }
        }

        private uint securityAlgorithm_1(uint seed)
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

        private byte[] securityAlgorithm_1(byte[] seed)
        {
            uint seedInt = (uint)((seed[0] << 24) + (seed[1] << 16) + (seed[2] << 8) + seed[3]);
            uint keyInt = securityAlgorithm_1(seedInt);
            byte[] key = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                key[i] = (byte)(keyInt >> (8 * (3 - i)));
            }
            return key;
        }

        private uint securityAlgorithm_0(uint seed)
        {
            return (seed ^ MASK) + MASK;
        }

        private byte[] securityAlgorithm_0(byte[] seed)
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

        private byte[] securityAlgorithm_2(byte[] seed)
        {
            byte[] Xor = new byte[4]
            {
                0x14,0x28,0xEC,0x6D
            };
            byte[] Cal = new byte[4];
            for(int i = 0; i < 4; i++)
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
