namespace WindowsApplication1
{
    class Security
    {
        private const uint MASK_N330_BLACKBOX = 0x7FEAC5CBU;
        private const uint MASK_DEFAULT = 0xA5CEFDB6U;
        private string car;

        public Security(string car)
        {
            this.car = car;
        }
        public uint seedToKey(uint seed)
        {
            if (car.Equals("N330_BlackBox"))
            {
                return securityAlgorithm_090A(seed);
            }
            return 0;
        }

        uint securityAlgorithm_090A(uint seed)
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

        uint securityAlgorithm_0304(uint seed)
        {
            return (seed ^ MASK_DEFAULT) + MASK_DEFAULT;
        }
    }
}
