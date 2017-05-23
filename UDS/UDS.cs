namespace BtFlash.UDS
{
    /// <summary>
    /// Negative response codes
    /// </summary>
    public struct NRC
    {
        /// <summary>
        /// serviceNotSupported
        /// </summary>
        public const byte SNS = 0x11;
        /// <summary>
        /// subFunctionNotSupported
        /// </summary>
        public const byte SFNS = 0x12;
        /// <summary>
        /// incorrectMessageLengthOrInvalidFormat
        /// </summary>
        public const byte IMLOIF = 0x13;
        /// <summary>
        /// responseTooLong
        /// </summary>
        public const byte RTL = 0x14;
        /// <summary>
        /// busyRepeatRequest
        /// </summary>
        public const byte BRR = 0x21;
        /// <summary>
        /// conditionsNotCorrect
        /// </summary>
        public const byte CNC = 0x22;
        /// <summary>
        /// requestSequenceError
        /// </summary>
        public const byte RSE = 0x24;
        /// <summary>
        /// invalidKey
        /// </summary>
        public const byte IK = 0x35;
        /// <summary>
        /// requiredTimeDelayNotExpired
        /// </summary>
        public const byte RTDNE = 0x37;
        /// <summary>
        /// generalProgrammingFailure
        /// </summary>
        public const byte GPF = 0x72;
        /// <summary>
        /// wrongBlockSequenceCounter
        /// </summary>
        public const byte WBSC = 0x73;
        /// <summary>
        /// requestCorrectlyReceived-ResponsePending
        /// </summary>
        public const byte RCRRP = 0x78;
    }

    /// <summary>
    /// Service identifier
    /// </summary>
    public struct SI
    {
        /// <summary>
        /// Negative response service identifier
        /// </summary>
        public const byte NRSI = 0x7F;
        /// <summary>
        /// ECU reset service identifier
        /// </summary>
        public const byte ERSI = 0x11;
        /// <summary>
        /// Read data by identifier service identifier
        /// </summary>
        public const byte RDBISI = 0x22;
        /// <summary>
        /// Security access service identifier
        /// </summary>
        public const byte SASI = 0x27;
        /// <summary>
        /// Communication control service identifier
        /// </summary>
        public const byte CCSI = 0x28;
        /// <summary>
        /// Request download service identifier
        /// </summary>
        public const byte RDSI = 0x34;
        /// <summary>
        /// Transfer data service identifier
        /// </summary>
        public const byte TDSI = 0x36;
        /// <summary>
        /// Request transfer exit service identifier
        /// </summary>
        public const byte RTESI = 0x37;
        /// <summary>
        /// Tester present request service identifier 
        /// </summary>
        public const byte TPSI = 0x3E;
    }

    public struct N_PCI
    {
        public struct SF
        {
            public const byte N_PCItype = 0;
        }

        public struct FF
        {
            public const byte N_PCItype = 1;
        }

        public struct CF
        {
            public const byte N_PCItype = 2;
        }

        public struct FC
        {
            public const byte N_PCItype = 3;

            public struct FS
            {
                public const byte CTS = 0x0;
                public const byte WT = 0x1;
                public const byte OVFLW = 0x02;
            }
        }

    }
}
