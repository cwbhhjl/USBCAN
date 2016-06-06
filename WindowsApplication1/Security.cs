using System.Collections;

namespace USBCAN
{
    class Security
    {
        private const uint MASK_N330_BLACKBOX = 0x7FEAC5CBU;
        private const uint MASK_DEFAULT = 0xA5CEFDB6U;
        private IDictionary carSelected = null;

        public Security(IDictionary carSelected)
        {
            this.carSelected = carSelected;
        }
        public uint seedToKey(uint seed)
        {
            if (carSelected["SeedRequest"].ToString().Substring(6,2)=="09")
            {
                return securityAlgorithm_090A(seed);
            }
            if (carSelected["SeedRequest"].ToString().Substring(6, 2) == "03")
            {
                return securityAlgorithm_0304(seed);
            }
            return 0;
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
    }
}
