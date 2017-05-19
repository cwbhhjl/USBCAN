using System;
using System.Runtime.InteropServices;

namespace BtFlash.Device.Neo
{
    public enum eHardwareTypes : uint
    {
        NEODEVICE_Unknown = 0,
        NEODEVICE_BLUE = 1,
        NEODEVICE_DW_VCAN = 4,
        NEODEVICE_FIRE = 8,
        NEODEVICE_VCAN3 = 16,
        NEODEVICE_YELLOW = 32,
        NEODEVICE_RED = 64,
        NEODEVICE_ECU = 128,
        NEODEVICE_IEVB = 256,
        NEODEVICE_PENDANT = 512,
        NEODEVICE_PLASMA_1_11 = 0x1000,
        NEODEVICE_FIRE_VNET = 0x2000,
        NEODEVICE_PLASMA_1_12 = 0x10000,
        NEODEVICE_PLASMA_1_13 = 0x20000,
        NEODEVICE_ION_2 = 0x40000,
        NEODEVICE_RADSTAR = 0x80000,
        NEODEVICE_ION_3 = 0x100000,
        NEODEVICE_VCANFD = 0x200000,
        NEODEVICE_ECU15 = 0x400000,
        NEODEVICE_ECU25 = 0x800000,
        NEODEVICE_EEVB = 0x1000000,
        NEODEVICE_ANY_PLASMA = (NEODEVICE_PLASMA_1_11 & NEODEVICE_FIRE_VNET & NEODEVICE_PLASMA_1_12 & NEODEVICE_PLASMA_1_13),
        NEODEVICE_ANY_ION = (NEODEVICE_ION_2 & NEODEVICE_ION_3),
        NEODEVICE_ALL = 0xFFFFFFFF,
    }

    public enum eDATA_STATUS_BITFIELD_1//: long 
    {
        SPY_STATUS_GLOBAL_ERR = 0x01,
        SPY_STATUS_TX_MSG = 0x02,
        SPY_STATUS_XTD_FRAME = 0x04,
        SPY_STATUS_REMOTE_FRAME = 0x08,

        SPY_STATUS_CRC_ERROR = 0x10,
        SPY_STATUS_CAN_ERROR_PASSIVE = 0x20,
        SPY_STATUS_INCOMPLETE_FRAME = 0x40,
        SPY_STATUS_LOST_ARBITRATION = 0x80,

        SPY_STATUS_UNDEFINED_ERROR = 0x100,
        SPY_STATUS_CAN_BUS_OFF = 0x200,
        SPY_STATUS_CAN_ERROR_WARNING = 0x400,
        SPY_STATUS_BUS_SHORTED_PLUS = 0x800,

        SPY_STATUS_BUS_SHORTED_GND = 0x1000,
        SPY_STATUS_CHECKSUM_ERROR = 0x2000,
        SPY_STATUS_BAD_MESSAGE_BIT_TIME_ERROR = 0x4000,
        SPY_STATUS_IFR_DATA = 0x8000,

        SPY_STATUS_HARDWARE_COMM_ERROR = 0x10000,
        SPY_STATUS_EXPECTED_LEN_ERROR = 0x20000,
        SPY_STATUS_INCOMING_NO_MATCH = 0x40000,
        SPY_STATUS_BREAK = 0x80000,

        SPY_STATUS_AVSI_REC_OVERFLOW = 0x100000,
        SPY_STATUS_TEST_TRIGGER = 0x200000,
        SPY_STATUS_AUDIO_COMMENT = 0x400000,
        SPY_STATUS_GPS_DATA = 0x800000,

        SPY_STATUS_ANALOG_DIGITAL_INPUT = 0x1000000,
        SPY_STATUS_TEXT_COMMENT = 0x2000000,
        SPY_STATUS_NETWORK_MESSAGE_TYPE = 0x4000000,
        SPY_STATUS_VSI_TX_UNDERRUN = 0x8000000,

        SPY_STATUS_VSI_IFR_CRC_Bit = 0x10000000,
        SPY_STATUS_INIT_MESSAGE = 0x20000000,
        SPY_STATUS_HIGH_SPEED_MESSAGE = 0x40000000,
    }

    public enum eDATA_STATUS_BITFIELD_2
    {
        SPY_STATUS2_HAS_VALUE = 0,
        SPY_STATUS2_VALUE_IS_BOOLEAN = 2,
        SPY_STATUS2_HIGH_VOLTAGE = 4,
        SPY_STATUS2_LONG_MESSAGE = 8,
    }

    public enum icsConfigSetup : short
    {
        NEO_CFG_MPIC_HS_CAN_CNF1 = 512 + 10,
        NEO_CFG_MPIC_HS_CAN_CNF2 = 512 + 9,
        NEO_CFG_MPIC_HS_CAN_CNF3 = 512 + 8,
        NEO_CFG_MPIC_HS_CAN_MODE = 512 + 54,

        // med speed CAN
        NEO_CFG_MPIC_MS_CAN_CNF1 = 512 + 22,
        NEO_CFG_MPIC_MS_CAN_CNF2 = 512 + 21,
        NEO_CFG_MPIC_MS_CAN_CNF3 = 512 + 20,

        NEO_CFG_MPIC_SW_CAN_CNF1 = 512 + 34,
        NEO_CFG_MPIC_SW_CAN_CNF2 = 512 + 33,
        NEO_CFG_MPIC_SW_CAN_CNF3 = 512 + 32,

        NEO_CFG_MPIC_LSFT_CAN_CNF1 = 512 + 46,
        NEO_CFG_MPIC_LSFT_CAN_CNF2 = 512 + 45,
        NEO_CFG_MPIC_LSFT_CAN_CNF3 = 512 + 44,
    }

    // Network ID
    public enum eNETWORK_ID : int
    {
        NETID_DEVICE = 0,
        NETID_HSCAN = 1,
        NETID_MSCAN = 2,
        NETID_SWCAN = 3,
        NETID_LSFTCAN = 4,
        NETID_FORDSCP = 5,
        NETID_J1708 = 6,
        NETID_AUX = 7,
        NETID_JVPW = 8,
        NETID_ISO = 9,
        NETID_ISO2 = 14,
        NETID_ISO14230 = 15,
        NETID_LIN = 16,
        NETID_ISO3 = 41,
        NETID_HSCAN2 = 42,
        NETID_HSCAN3 = 44,
        NETID_ISO4 = 47,
        NETID_LIN2 = 48,
        NETID_LIN3 = 49,
        NETID_LIN4 = 50,
        NETID_LIN5 = 84,
        NETID_MOST = 51,
        NETID_CGI = 53,
        NETID_HSCAN4 = 61,
        NETID_HSCAN5 = 62,
        NETID_RS232 = 63,
        NETID_UART = 64,
        NETID_UART2 = 65,
        NETID_UART3 = 66,
        NETID_UART4 = 67,
        NETID_ANALOG = 68,
        NETID_ETHERNETDAQ = 69,
        NETID_FLEXRAY1A = 80,
        NETID_FLEXRAY1B = 81,
        NETID_FLEXRAY2A = 82,
        NETID_FLEXRAY2B = 83,
        NETID_MOST25 = 90,
        NETID_MOST50 = 91,
        NETID_MOST150 = 92,
        NETID_ETHERNET = 93,
    }

    //CoreMini Status
    public enum eScriptStatus : int
    {
        SCRIPT_STATUS_STOPPED = 0,
        SCRIPT_STATUS_RUNNING = 1,
    }

    //CoreMini Location
    public enum eScriptLocation : int
    {
        SCRIPT_LOCATION_FLASH_MEM = 0,   //(Valid only on a neoVI Fire or neoVI Red)
        SCRIPT_LOCATION_SDCARD = 1,  //(Valid only on a neoVI Fire or neoVI Red)
        SCRIPT_LOCATION_VCAN3_MEM = 4,   //(Valid only on a ValueCAN 3 device)
    }

    // ePROTOCOL
    public enum ePROTOCOL : int
    {
        SPY_PROTOCOL_CUSTOM = 0,
        SPY_PROTOCOL_CAN = 1,
        SPY_PROTOCOL_GMLAN = 2,
        SPY_PROTOCOL_J1850VPW = 3,
        SPY_PROTOCOL_J1850PWM = 4,
        SPY_PROTOCOL_ISO9141 = 5,
        SPY_PROTOCOL_Keyword2000 = 6,
        SPY_PROTOCOL_GM_ALDL_UART = 7,
        SPY_PROTOCOL_CHRYSLER_CCD = 8,
        SPY_PROTOCOL_CHRYSLER_SCI = 9,
        SPY_PROTOCOL_FORD_UBP = 10,
        SPY_PROTOCOL_BEAN = 11,
        SPY_PROTOCOL_LIN = 12,
        SPY_PROTOCOL_J1708 = 13,
        SPY_PROTOCOL_CHRYSLER_JVPW = 14,
        SPY_PROTOCOL_J1939 = 15,
    }

    // Driver Type Constants
    public enum eDRIVER_TYPE : short
    {
        INTREPIDCS_DRIVER_STANDARD = 0,
        INTREPIDCS_DRIVER_TEST = 1,
    }

    // Port Type Constants
    public enum ePORT_TYPE : short
    {
        NEOVI_COMMTYPE_RS232 = 0,
        NEOVI_COMMTYPE_USB_BULK = 1,
        NEOVI_COMMTYPE_USB_ISO_Do_Not_Use = 2,
        NEOVI_COMMTYPE_USB_BULK_SN = 3,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct icsSpyMessage   //reff
    {
        public Int32 StatusBitField; //4
        public Int32 StatusBitField2; //new '4
        public Int32 TimeHardware; // 4
        public Int32 TimeHardware2; //new ' 4
        public Int32 TimeSystem; // 4
        public Int32 TimeSystem2;
        public byte TimeStampHardwareID; //new ' 1
        public byte TimeStampSystemID;
        public byte NetworkID; //new ' 1
        public byte NodeID;
        public byte Protocol;
        public byte MessagePieceID; // 1
        public byte ColorID; //1
        public byte NumberBytesHeader; // 1
        public byte NumberBytesData; // 1
        public Int16 DescriptionID; // 2
        public Int32 ArbIDOrHeader; // Holds (up to 3 byte 1850 header or 29 bit CAN header) '4
        //public byte[] Data = new byte[8]; //(1 To 8); //8
        public byte Data1;
        public byte Data2;
        public byte Data3;
        public byte Data4;
        public byte Data5;
        public byte Data6;
        public byte Data7;
        public byte Data8;
        public byte AckBytes1;
        public byte AckBytes2;
        public byte AckBytes3;
        public byte AckBytes4;
        public byte AckBytes5;
        public byte AckBytes6;
        public byte AckBytes7;
        public byte AckBytes8;
        //public byte[] AckBytes = new byte[8]; //(1 To 8); //new '8
        public Single Value; // As Single ' 4
        public byte MiscData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stAPIFirmwareInfo
    {
        public Int32 iType;  // 1,2,3 for Generation of HW.

        // Date and Time type 1 and 2
        public Int32 iMainFirmDateDay;
        public Int32 iMainFirmDateMonth;
        public Int32 iMainFirmDateYear;
        public Int32 iMainFirmDateHour;
        public Int32 iMainFirmDateMin;
        public Int32 iMainFirmDateSecond;
        public Int32 iMainFirmChkSum;

        // Version data (only valid for type 3)
        public byte iAppMajor;
        public byte iAppMinor;
        public byte iManufactureDay;
        public byte iManufactureMonth;
        public UInt16 iManufactureYear;
        public byte iBoardRevMajor;
        public byte iBoardRevMinor;
        public byte iBootLoaderVersionMajor;
        public byte iBootLoaderVersionMinor;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct icsSpyMessageLong
    {
        public Int32 StatusBitField; // 4
        public Int32 StatusBitField2; //new '4
        public Int32 TimeHardware;
        public Int32 TimeHardware2; //new ' 4
        public Int32 TimeSystem; //4
        public Int32 TimeSystem2;
        public byte TimeStampHardwareID; //new ' 1
        public byte TimeStampSystemID;
        public byte NetworkID; //new ' 1
        public byte NodeID;
        public byte Protocol;
        public byte MessagePieceID; // 1
        public byte ColorID; // 1
        public byte NumberBytesHeader; //
        public byte NumberBytesData; //2
        public Int16 DescriptionID; //2
        public Int32 ArbIDOrHeader;// Holds (up to 3 byte 1850 header or 29 bit CAN header)
        public Int32 DataMsb;
        public Int32 DataLsb;
        public byte AckBytes1;
        public byte AckBytes2;
        public byte AckBytes3;
        public byte AckBytes4;
        public byte AckBytes5;
        public byte AckBytes6;
        public byte AckBytes7;
        public byte AckBytes8;
        public Single Value; // As Single
        public byte MiscData;

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct icsSpyMessageJ1850
    {
        public Int32 StatusBitField; //4
        public Int32 StatusBitField2; //new '4
        public Int32 TimeHardware; //4
        public Int32 TimeHardware2; //new ' 4
        public Int32 TimeSystem; //4
        public Int32 TimeSystem2;
        public byte TimeStampHardwareID; //new ' 1
        public byte TimeStampSystemID;
        public byte NetworkID; //new ' 1
        public byte NodeID;
        public byte Protocol;
        public byte MessagePieceID; // 1 new
        public byte ColorID; // 1
        public byte NumberBytesHeader; //1
        public byte NumberBytesData; //1
        public Int16 DescriptionID; //2
        public byte Header1;  //Holds (up to 3 byte 1850 header or 29 bit CAN header)
        public byte Header2;
        public byte Header3;
        public byte Header4;
        public byte Data1;
        public byte Data2;
        public byte Data3;
        public byte Data4;
        public byte Data5;
        public byte Data6;
        public byte Data7;
        public byte Data8;
        public byte AckBytes1;
        public byte AckBytes2;
        public byte AckBytes3;
        public byte AckBytes4;
        public byte AckBytes5;
        public byte AckBytes6;
        public byte AckBytes7;
        public byte AckBytes8;
        public Single Value; // As Single '4
        public byte MiscData;
    }

    //Structure for neoVI device types
    [StructLayout(LayoutKind.Sequential)]
    public struct NeoDevice
    {
        public Int32 DeviceType;
        public Int32 Handle;
        public Int32 NumberOfClients;
        public Int32 SerialNumber;
        public Int32 MaxAllowedClients;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct CAN_SETTINGS
    {
        public byte Mode;
        public byte SetBaudrate;
        public byte Baudrate;
        public byte Transceiver_Mode;
        public byte TqSeg1;
        public byte TqSeg2;
        public byte TqProp;
        public byte TqSync;
        public UInt16 BRP;
        public byte auto_baud;
        public byte innerFrameDelay25us;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct SWCAN_SETTINGS
    {
        public byte Mode;
        public byte SetBaudrate;
        public byte Baudrate;
        public byte NetworkType;
        public byte TqSeg1;
        public byte TqSeg2;
        public byte TqProp;
        public byte TqSync;
        public UInt16 BRP;
        public UInt16 high_speed_auto_switch;
        public byte auto_baud;
        public byte Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SVCAN3Settings
    {
        public CAN_SETTINGS Can1;
        public CAN_SETTINGS Can2;
        public UInt16 Network_enables;
        public UInt16 Network_enabled_on_boot;
        public UInt16 Iso15765_separation_time_offset;
        public UInt16 Perf_en;
        public UInt16 Misc_io_initial_ddr;
        public UInt16 Misc_io_initial_latch;
        public UInt16 Misc_io_report_period;
        public UInt16 Misc_io_on_report_events;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct LIN_SETTINGS
    {
        public UInt32 Baudrate;
        public UInt16 spbrg;
        public byte brgh;
        public byte NumBitsDelay;
        public byte MasterResistor;
        public byte Mode;
    }

    // --- UART Settings
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct UART_SETTINGS
    {
        public UInt16 Baudrate;
        public UInt16 spbrg;
        public UInt16 brgh;
        public UInt16 parity;
        public UInt16 stop_bits;
        public byte flow_control; // 0- off, 1 - Simple CTS RTS,
        public byte reserved_1;
        public UInt32 bOptions; //AND to combind these values  invert_tx = 1 invert_rx = 2  half_duplex = 4
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct J1708_SETTINGS
    {
        public UInt16 enable_convert_mode;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct sYellowSettings
    {
        public CAN_SETTINGS can1;
        public CAN_SETTINGS can2;

        public Int16 Reserved0;
        public Int16 Reserved1;
        public Int16 Reserved2;
        public UInt16 hs2_j1708_enable;     // Use hs2 + J1708
        public UInt16 iso_j1850_enable;     // Select ISO + J1850
        public UInt16 j1708_j1850_enable;   // Use J1708 + J1850

        public UInt16 network_enables;
        public UInt16 network_enables_2;
        public UInt16 network_enabled_on_boot;

        public UInt32 pwm_man_timeout;
        public UInt16 pwr_man_enable; // 0 - off, 1 - sleep enabled, 2- idle enabled (fast wakeup)

        public UInt16 misc_io_initial_ddr;
        public UInt16 misc_io_initial_latch;
        public UInt16 misc_io_analog_enable;
        public UInt16 misc_io_report_period;
        public UInt16 misc_io_on_report_events;
        public UInt16 ain_sample_period;
        public UInt16 ain_threshold;

        //ISO 15765 Transport Layer
        public Int16 iso15765_separation_time_offset;

        //ISO9141 - KEYWORD 2000 1
        public UInt16 iso9141_kwp_enable_reserved;
        public ISO9141_KEYWORD2000_SETTINGS iso9141_kwp_settings;
        public LIN_SETTINGS lin1;

        //ISO9141 - KEYWORD 2000 2 //Not USED
        public Int16 iso9141_kwp_enable_reserved_2;  // Not USED
        public ISO9141_KEYWORD2000_SETTINGS iso9141_kwp_settings_2;  // Not USED
        public LIN_SETTINGS lin2;

        //J1708
        public J1708_SETTINGS j1708;

        //Performance Test
        public UInt32 perf_en;

        //ISO9141 - Parity
        public UInt16 iso_parity; // 0 - no parity, 1 - event, 2 - odd
        public UInt16 iso_msg_termination;  // 0 - use inner frame time, 1 - GME CIM-SCL
        public UInt16 UInt16iso_tester_pullup_enable;

        //remove later
        public UInt16 iso_parity_2;                   // 0 - no parity, 1 - event, 2 - odd
        public UInt16 iso_msg_termination_2;    // 0 - use inner frame time, 1 - GME CIM-SCL


        public UInt16 fast_init_network_enables_1;
        public UInt16 fast_init_network_enables_2;

        public UART_SETTINGS uart;
        public UART_SETTINGS uart2;

        public STextAPISettings text_api;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct STextAPISettings
    {
        public UInt32 can1_tx_id;
        public UInt32 can1_rx_id;
        public UInt32 can1_options; // Set to 1 for Extended, 0 for standard

        public UInt32 can2_tx_id;
        public UInt32 can2_rx_id;
        public UInt32 can2_options; // Set to 1 for Extended, 0 for standard

        public UInt32 network_enables;

        public UInt32 can3_tx_id3;
        public UInt32 can3_rx_id3;
        public UInt32 can3_options; // Set to 1 for Extended, 0 for standard

        public UInt32 can4_tx_id4;
        public UInt32 can4_rx_id4;
        public UInt32 can4_options; // Set to 1 for Extended, 0 for standard

        public Int32 Reserved0;
        public Int32 Reserved1;
        public Int32 Reserved2;
        public Int32 Reserved3;
        public Int32 Reserved4;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct ISO9141_KEYWORD2000_SETTINGS
    {
        public UInt32 Baudrate;
        public UInt16 spbrg;
        public UInt16 brgh;
        public ISO9141_KEYWORD2000__INIT_STEP Init_Step_0;
        public ISO9141_KEYWORD2000__INIT_STEP Init_Step_1;
        public ISO9141_KEYWORD2000__INIT_STEP Init_Step_2;
        public ISO9141_KEYWORD2000__INIT_STEP Init_Step_3;
        public ISO9141_KEYWORD2000__INIT_STEP Init_Step_4;
        public ISO9141_KEYWORD2000__INIT_STEP Init_Step_5;
        public ISO9141_KEYWORD2000__INIT_STEP Init_Step_6;
        public ISO9141_KEYWORD2000__INIT_STEP Init_Step_7;
        public ISO9141_KEYWORD2000__INIT_STEP Init_Step_8;
        public ISO9141_KEYWORD2000__INIT_STEP Init_Step_9;
        public ISO9141_KEYWORD2000__INIT_STEP Init_Step_10;
        public ISO9141_KEYWORD2000__INIT_STEP Init_Step_11;
        public ISO9141_KEYWORD2000__INIT_STEP Init_Step_12;
        public ISO9141_KEYWORD2000__INIT_STEP Init_Step_13;
        public ISO9141_KEYWORD2000__INIT_STEP Init_Step_14;
        public ISO9141_KEYWORD2000__INIT_STEP Init_Step_15;
        public byte init_step_count;
        public UInt16 p2_500us;
        public UInt16 p3_500us;
        public UInt16 p4_500us;
        public UInt16 chksum_enabled;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct ISO9141_KEYWORD2000__INIT_STEP
    {
        public UInt16 time_500us;
        public UInt16 k;
        public UInt16 l;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct SFireSettings
    {
        public CAN_SETTINGS can1;
        public CAN_SETTINGS can2;
        public CAN_SETTINGS can3;
        public CAN_SETTINGS can4;
        public SWCAN_SETTINGS swcan;
        public CAN_SETTINGS lsftcan;
        public LIN_SETTINGS lin1;
        public LIN_SETTINGS lin2;
        public LIN_SETTINGS lin3;
        public LIN_SETTINGS lin4;

        public UInt16 cgi_enable;
        public UInt16 cgi_baud;
        public UInt16 cgi_tx_ifs_bit_times;
        public UInt16 cgi_rx_ifs_bit_times;
        public UInt16 cgi_chksum_enable;

        public UInt16 network_enables;
        public UInt16 network_enabled_on_boot;

        public UInt32 pwm_man_timeout;
        public UInt16 pwr_man_enable;

        public UInt16 misc_io_initial_ddr;
        public UInt16 misc_io_initial_latch;
        public UInt16 misc_io_analog_enable;
        public UInt16 misc_io_report_period;
        public UInt16 misc_io_on_report_events;
        public UInt16 ain_sample_period;
        public UInt16 ain_threshold;

        //ISO 15765-2 Transport Layer
        public UInt16 iso15765_separation_time_offset;
        //ISO9141 - KEYWORD 2000
        public UInt16 iso9141_kwp_enable_reserved;
        public ISO9141_KEYWORD2000_SETTINGS iso9141_kwp_settings;
        //Performance Test
        public UInt16 perf_en;
        //ISO9141 - Parity
        public UInt16 iso_parity;  // 0 - no parity, 1 - event, 2 - odd
        public UInt16 iso_msg_termination;  // 0 - use inner frame time, 1 - GME CIM-SCL
        public UInt16 iso_tester_pullup_enable;
        //Additional network enables
        public UInt16 network_enables_2;
        public ISO9141_KEYWORD2000_SETTINGS iso9141_kwp_settings_2;
        public UInt16 iso_parity_2;        // 0 - no parity, 1 - event, 2 - odd
        public UInt16 iso_msg_termination_2;     // 0 - use inner frame time, 1 - GME CIM-SCL
        public ISO9141_KEYWORD2000_SETTINGS iso9141_kwp_settings_3;
        public UInt16 iso_parity_3;        // 0 - no parity, 1 - event, 2 - odd
        public UInt16 iso_msg_termination_3;     // 0 - use inner frame time, 1 - GME CIM-SCL
        public ISO9141_KEYWORD2000_SETTINGS iso9141_kwp_settings_4;
        public UInt16 iso_parity_4;        // 0 - no parity, 1 - event, 2 - odd
        public UInt16 iso_msg_termination_4;     // 0 - use inner frame time, 1 - GME CIM-SCL    
        public UInt16 fast_init_network_enables_1;
        public UInt16 fast_init_network_enables_2;
        public UART_SETTINGS uart;
        public UART_SETTINGS uart2;
        public STextAPISettings text_api;
    }

    //_stChipVersions
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct fire_versions
    {
        public Byte mpic_maj;
        public Byte mpic_min;
        public Byte upic_maj;
        public Byte upic_min;
        public Byte lpic_maj;
        public Byte lpic_min;
        public Byte jpic_maj;
        public Byte jpic_min;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct plasma_fire_vnet
    {
        public Byte mpic_maj;
        public Byte mpic_min;
        public Byte core_maj;
        public Byte core_min;
        public Byte lpic_maj;
        public Byte lpic_min;
        public Byte hid_maj;
        public Byte hid_min;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct vcan3_versions
    {
        public Byte mpic_maj;
        public Byte mpic_min;
        public UInt32 Reserve;
        public UInt16 Reserve2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct yellow_versions
    {
        public Byte mpic_maj;
        public Byte mpic_min;
        public Byte upic_maj;
        public Byte upic_min;
        public UInt32 Reserve;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stCM_ISO157652_TxMessage
    {
        //transmit message
        public UInt16 vs_netid; //< The netid of the message (determines which network to transmit on),  not supported
        public byte padding; //< The padding byte to use to fill the unused portion of
        //  transmitted CAN frames (single frame, first frame, consecutive frame).
        public byte reserved2;
        public UInt32 id;  //< arbId of transmitted frames (CAN id to transmit to).
        public UInt32 fc_id;  //< flow control arb id filter value (response id from receiver).
        public UInt32 fc_id_mask; //< The flow control arb filter mask (response id from receiver).
        public byte stMin;//< Overrides the stMin that the receiver reports, see overrideSTmin. Set to J2534's STMIN_TX if <= 0xFF.
        public byte blockSize; //< Overrides the block size that the receiver reports, see overrideBlockSize.   Set to J2534's BS_TX if <= 0xFF.
        public byte flowControlExtendedAddress;//< Expected Extended Address byte of response from receiver.  see fc_ext_address_enable, not supported.
        public byte extendedAddress;//< Extended Address byte of transmitter. see ext_address_enable, not supported.

        //flow control timeouts
        public UInt16 fs_timeout;    //< max timeout (ms) for waiting on flow control respons. Set this to N_BS_MAX's value if J2534.
        public UInt16 fs_wait; //< max timeout (ms) for waiting on flow control response after receiving flow control
        ///with flow status set to WAIT.   Set this to N_BS_MAX's value if J2534.
        //******************************************************************************************************************
        public byte[] data; //Before you use this structure
        // call: stCM_ISO157652_TxMessage.data = new byte(4096)
        //******************************************************************************************************************
        public UInt32 num_bytes; //< Number of data bytes
        //option bits
        public UInt16 flags;
        //To set the flags, AND the parameter you want from the stCM_ISO157652_TxMessage_Flags Enum

    }

    public enum stCM_ISO157652_TxMessage_Flags : short
    {
        id_29_bit_enable = 1, //< Enables 29 bit arbId for transmitted frames.  Set to 1 so transmitted frames use 29 bit ids, not supported.
        fc_id_29_bit_enable = 2, //< Enables 29 bit arbId for Flow Control filter.  Set to 1 if receiver response uses 29 bit ids, not supported.
        ext_address_enable = 4, //< Enables Extended Addressing, Set to 1 if transmitted frames should have extended addres byte, not supported.
        fc_ext_address_enable = 8, //< Enables Extended Addressing for Flow Control filter.  Set to 1 if receiver responds with extended address byte, not supported.
        overrideSTmin = 16, //< Uses member stMin and not receiver's flow control's stMin.
        overrideBlockSize = 32, //< Uses member BlockSize and not receiver's flow control's BlockSize.
        paddingEnable = 64, //< Enable's padding
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct stCM_ISO157652_RxMessage
    {
        //transmit message
        public UInt16 vs_netid; //< The netid of the message (determines which network to decode receives),  not supported
        public byte padding;//< The padding byte to use to fill the unused portion of
        ///  transmitted CAN frames (flow control), see paddingEnable.
        public UInt32 id; //< ArbId filter value for frames from transmitter (from ECU to neoVI).
        public UInt32 id_mask; //< ArbId filter mask for frames from transmitter (from ECU to neoVI).
        public UInt32 fc_id;  //< flow control arbId to transmit in flow control (from neoVI to ECU).
        public byte flowControlExtendedAddress; //< Extended Address byte used in flow control (from neoVI to ECU). see fc_ext_address_enable.
        public byte extendedAddress; //< Expected Extended Address byte of frames sent by transmitter (from ECU to neoVI).  see ext_address_enable.
        public byte blockSize; //< Block Size to report in flow control response.
        public byte stMin; //< Minimum seperation time (between consecutive frames) to report in flow control response.
        //flow control timeouts
        public UInt16 cf_timeout;    //< max timeout (ms) for waiting on consecutive frame.  Set this to N_CR_MAX's value in J2534.
        //option bits
        public UInt32 flags;
        //To set the flags, AND the parameter you want from the stCM_ISO157652_RxMessage_Flags Enum
        public UInt32 reserved0;
        public UInt32 reserved1;
        public UInt32 reserved2;
        public UInt32 reserved3;
    }

    public enum stCM_ISO157652_RxMessage_Flags : short
    {
        id_29_bit_enable = 1, //< Enables 29 bit arbId filter for frames (from ECU to neoVI).
        fc_id_29_bit_enable = 2, //< Enables 29 bit arbId for Flow Control (from neoVI to ECU).
        ext_address_enable = 4, //< Enables Extended Addressing (from ECU to neoVI).
        fc_ext_address_enable = 8, //< Enables Extended Addressing (from neoVI to ECU).
        enableFlowControlTransmission = 16, //< Enables Flow Control frame transmission (from neoVI to ECU).
        paddingEnable = 32, //< Enable's padding
    }
}
