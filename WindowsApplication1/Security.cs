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
                case 0x03:
                    return securityAlgorithm_3(seed);
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

        private byte[] securityAlgorithm_3(byte[] seed)
        {
            uint wSubSeed;
            uint wMiddle;
            uint wLastBit;
            uint wLeft31Bits;
            uint keyInt;
            byte counter, i, DB1, DB2, DB3;
            int middle;

            uint seedInt =((uint)(seed[0] << 24) + (uint)(seed[1] << 16) + (uint)(seed[2] << 8) + seed[3]);

            wSubSeed = seedInt;
            middle = (int)(((MASK & 0x00001000) >> 11) | ((MASK & 0x00400000) >> 22));

            switch (middle)
            {
                case 0:
                    wMiddle = (byte)(seedInt & 0x000000FF);
                    break;
                case 1:
                    wMiddle = (byte)((seedInt & 0x0000FF00) >> 8);
                    break;
                case 2:
                    wMiddle = (byte)((seedInt & 0x00FF0000) >> 16);
                    break;
                case 3:
                    wMiddle = (byte)((seedInt & 0xFF000000) >> 24);
                    break;
                default:
                    wMiddle = 0;
                    break;
            }

            DB1 = (byte)((MASK & 0x000007F8) >> 3);
            DB2 = (byte)(((MASK & 0x7F800000) >> 23) ^ 0xA5);
            DB3 = (byte)(((MASK & 0x003FC000) >> 14) ^ 0x5A);

            counter = (byte)(((wMiddle ^ DB1) & DB2) + DB3);

            for(i = 0; i < counter; i++)
            {
                wMiddle = ((wSubSeed & 0x20000000) / 0x20000000) ^ ((wSubSeed & 0x01000000) / 0x01000000) ^ 
                    ((wSubSeed & 0x2000) / 0x2000) ^ ((wSubSeed & 0x08) / 0x08);
                wLastBit = (wMiddle & 0x00000001);
                wSubSeed = wSubSeed << 1;
                wLeft31Bits = wSubSeed & 0xFFFFFFFE;
                wSubSeed = wLeft31Bits | wLastBit;
            }

            if ((MASK & 0x00000002) != 0)
            {
                wLeft31Bits = ((wSubSeed & 0x00FF0000) >> 8) | ((wSubSeed & 0xFF000000) >> 24) | 
                    ((wSubSeed & 0x000000FF) << 16) | ((wSubSeed & 0x0000FF00) << 16);
            }
            else
            {
                wLeft31Bits = wSubSeed;
            }

            keyInt = wLeft31Bits ^ MASK;

            byte[] key = new byte[4];
            for (int j = 0; j < 4; j++)
            {
                key[j] = (byte)(keyInt >> (8 * (3 - j)));
            }

            return key;
        }


    }
}
