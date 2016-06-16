namespace USBCAN
{
    /// <summary>
    /// Negative response codes
    /// </summary>
    public struct NRC
    {
        /// <summary>
        /// requiredTimeDelayNotExpired
        /// </summary>
        public const byte RTDNE = 0x37;
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
    }

}
