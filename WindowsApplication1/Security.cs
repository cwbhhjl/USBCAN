using System.Collections;

namespace USBCAN
{
    class Security
    {
        private const uint MASK_N330_BLACKBOX = 0x7FEAC5CBU;
        private const uint MASK_DEFAULT = 0xA5CEFDB6U;
        private byte securityAlgorithmType;

        public Security()
        {
            securityAlgorithmType = 0;
        }

        public Security(byte securityAlgorithmType)
        {
            this.securityAlgorithmType = securityAlgorithmType;
        }

        public uint seedToKey(uint seed)
        {
            switch (securityAlgorithmType)
            {
                case 0:
                    return securityAlgorithm_0(seed);
                case 1:
                    return securityAlgorithm_1(seed);

            }
            return 0;
        }

        public byte[] seedToKey(byte[] seed)
        {
            switch (securityAlgorithmType)
            {
                case 0x0:
                    return securityAlgorithm_0(seed);
            }
            return null;
        }

        private uint securityAlgorithm_1(uint seed)
        {
            for (int i = 0; i < 35; i++)
            {
                if ((seed & 0x80000000) == 0x80000000)
                {
                    seed <<= 1;
                    seed ^= MASK_N330_BLACKBOX;
                }
                else
                {
                    seed <<= 1;
                }
            }
            return seed;
        }

        private uint securityAlgorithm_0(uint seed)
        {
            return (seed ^ MASK_DEFAULT) + MASK_DEFAULT;
        }

        private byte[] securityAlgorithm_0(byte[] seed)
        {
            uint seedInt = (uint)((seed[0] << 24) + (seed[1] << 16) + (seed[2] << 8) + seed[3]);
            uint keyInt = (seedInt ^ MASK_DEFAULT) + MASK_DEFAULT;

            byte[] key = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                key[i] = (byte)(keyInt >> (8 * (3 - i)));
            }
            return key;
        }
    }
}
