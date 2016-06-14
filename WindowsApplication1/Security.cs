using System.Collections;

namespace USBCAN
{
    class Security
    {
        private const uint MASK_N330_BLACKBOX = 0x7FEAC5CBU;
        private const uint MASK_DEFAULT = 0xA5CEFDB6U;
        private byte securityAccessType;

        public Security(byte securityAccessType)
        {
            this.securityAccessType = securityAccessType;
        }
        public uint seedToKey(uint seed)
        {
            if (securityAccessType == 0x09)
            {
                return securityAlgorithm_090A(seed);
            }
            if (securityAccessType == 0x03)
            {
                return securityAlgorithm_0304(seed);
            }
            return 0;
        }

        public byte[] seedToKey(byte[] seed)
        {
            switch (securityAccessType)
            {
                case 0x03:
                    return securityAlgorithm_0304(seed);
            }
            return null;
        }

        private uint securityAlgorithm_090A(uint seed)
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

        private uint securityAlgorithm_0304(uint seed)
        {
            return (seed ^ MASK_DEFAULT) + MASK_DEFAULT;
        }

        private byte[] securityAlgorithm_0304(byte[] seed)
        {
            uint seedInt = (uint)((seed[0] << 24) + (seed[1] << 16) + (seed[2] << 8) + seed[3]);
            uint keyInt = securityAlgorithm_0304(seedInt);
            byte[] keyRe = new byte[4];
            for(int i = 0; i < 4; i++)
            {
                keyRe[i] = (byte)(keyInt >> (8 * (3 - i)));
            }
            return keyRe;
        }
    }
}
