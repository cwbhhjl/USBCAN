using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BtFlash.Device.Neo
{
    internal static class NativeMethods
    {
        [DllImport("icsneo40.dll")]
        public static extern int icsneoFindNeoDevices(uint DeviceTypes, ref NeoDevice pNeoDevice, ref int pNumDevices);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoOpenNeoDevice(ref NeoDevice pNeoDevice, ref int hObject, ref byte bNetworkIDs, int bConfigRead, int bSyncToPC);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoClosePort(int hObject, ref int pNumberOfErrors);
        [DllImport("icsneo40.dll")]
        public static extern void icsneoFreeObject(int hObject);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoOpenPortEx(int lPortNumber, int lPortType, int lDriverType, int lIPAddressMSB, int lIPAddressLSBOrBaudRate, int bConfigRead, ref byte bNetworkID, ref int hObject);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoGetMessages(int hObject, ref icsSpyMessage pMsg, ref int pNumberOfMessages, ref int pNumberOfErrors);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoTxMessages(int hObject, ref icsSpyMessageJ1850 pMsg, int lNetworkID, int lNumMessages);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoTxMessages(int hObject, ref icsSpyMessage pMsg, int lNetworkID, int lNumMessages);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoWaitForRxMessagesWithTimeOut(int hObject, uint iTimeOut);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoEnableNetworkRXQueue(int hObject, int iEnable);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoGetTimeStampForMsg(int hObject, ref icsSpyMessage pMsg, ref double pTimeStamp);
        [DllImport("icsneo40.dll")]
        public static extern void icsneoGetISO15765Status(int hObject, int lNetwork, int lClearTxStatus, int lClearRxStatus, ref int lTxStatus, ref int lRxStatus);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoGetConfiguration(int hObject, ref byte pData, ref int lNumBytes);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoSendConfiguration(int hObject, ref byte pData, int lNumBytes);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoGetFireSettings(int hObject, ref SFireSettings pSettings, int iNumBytes);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoSetFireSettings(int hObject, ref SFireSettings pSettings, int iNumBytes, int bSaveToEEPROM);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoGetYellowSettings(int hObject, ref sYellowSettings pSettings, int iNumBytes);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoSetYellowSettings(int hObject, ref sYellowSettings pSettings, int iNumBytes, int bSaveToEEPROM);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoGetVCAN3Settings(int hObject, ref SVCAN3Settings pSettings, int iNumBytes);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoSetVCAN3Settings(int hObject, ref SVCAN3Settings pSettings, int iNumBytes, int bSaveToEEPROM);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoSetBitRate(int hObject, int BitRate, int NetworkID);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoGetDeviceParameters(int hObject, ref char pParameter, ref char pValues, short ValuesLength);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoSetDeviceParameters(int hObject, ref char pParmValue, ref int pErrorIndex, int bSaveToEEPROM);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoGetLastAPIError(int hObject, ref uint pErrorNumber);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoGetErrorMessages(int hObject, ref int pErrorMsgs, ref int pNumberOfErrors);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoGetErrorInfo(int iErrorNumber, StringBuilder sErrorDescriptionShort, StringBuilder sErrorDescriptionLong, ref int iMaxLengthShort, ref int iMaxLengthLong, ref int lErrorSeverity, ref int lRestartNeeded);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoValidateHObject(int hObject);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoGetDLLVersion();
        [DllImport("icsneo40.dll")]
        public static extern int icsneoStartSockServer(int hObject, int iPort);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoStopSockServer(int hObject);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoScriptStart(int hObject, int iLocation);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoScriptStop(int hObject);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoScriptLoad(int hObject, ref byte bin, uint len_bytes, int iLocation);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoScriptClear(int hObject, int iLocation);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoScriptStartFBlock(int hObject, uint fb_index);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoScriptGetFBlockStatus(int hObject, uint fb_index, ref int piRunStatus);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoScriptStopFBlock(int hObject, uint fb_index);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoScriptGetScriptStatus(int hObject, ref int piStatus);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoScriptReadAppSignal(int hObject, uint iIndex, ref double dValue);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoScriptWriteAppSignal(int hObject, uint iIndex, double dValue);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoScriptReadRxMessage(int hObject, uint iIndex, ref icsSpyMessage pRxMessageMask, ref icsSpyMessage pRxMessageValue);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoScriptReadTxMessage(int hObject, uint iIndex, ref icsSpyMessage pTxMessage);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoScriptWriteRxMessage(int hObject, uint iIndex, ref icsSpyMessage pRxMessageMask, ref icsSpyMessage pRxMessageValue);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoScriptWriteTxMessage(int hObject, uint iIndex, ref icsSpyMessage pTxMessage);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoOpenPort(int lPortNumber, int lPortType, int lDriverType, ref byte bNetworkID, ref byte bSCPIDs, ref int hObject);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoEnableNetworkCom(int hObject, int Enable);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoFindAllCOMDevices(int lDriverType, int lGetSerialNumbers, int lStopAtFirst, int lUSBCommOnly, ref int p_lDeviceTypes, ref int p_lComPorts, ref int p_lSerialNumbers, ref int lNumDevices);
    }

    public class IcsNeo
    {
        public const double NEOVI_TIMEHARDWARE2_SCALING = 0.1048576;
        public const double NEOVI_TIMEHARDWARE_SCALING = 0.0000016;

        public const double NEOVIPRO_VCAN_TIMEHARDWARE2_SCALING = 0.065536;
        public const double NEOVIPRO_VCAN_TIMEHARDWARE_SCALING = 0.000001;

        // med speed CAN
        public const short NEO_CFG_MPIC_MS_CAN_CNF1 = 512 + 22;
        public const short NEO_CFG_MPIC_MS_CAN_CNF2 = 512 + 21;
        public const short NEO_CFG_MPIC_MS_CAN_CNF3 = 512 + 20;

        public const short NEO_CFG_MPIC_SW_CAN_CNF1 = 512 + 34;
        public const short NEO_CFG_MPIC_SW_CAN_CNF2 = 512 + 33;
        public const short NEO_CFG_MPIC_SW_CAN_CNF3 = 512 + 32;

        public const short NEO_CFG_MPIC_LSFT_CAN_CNF1 = 512 + 46;
        public const short NEO_CFG_MPIC_LSFT_CAN_CNF2 = 512 + 45;
        public const short NEO_CFG_MPIC_LSFT_CAN_CNF3 = 512 + 44;

        // Protocols
        public const short SPY_PROTOCOL_CUSTOM = 0;
        public const short SPY_PROTOCOL_CAN = 1;
        public const short SPY_PROTOCOL_GMLAN = 2;
        public const short SPY_PROTOCOL_J1850VPW = 3;
        public const short SPY_PROTOCOL_J1850PWM = 4;
        public const short SPY_PROTOCOL_ISO9141 = 5;
        public const short SPY_PROTOCOL_Keyword2000 = 6;
        public const short SPY_PROTOCOL_GM_ALDL_UART = 7;
        public const short SPY_PROTOCOL_CHRYSLER_CCD = 8;
        public const short SPY_PROTOCOL_CHRYSLER_SCI = 9;
        public const short SPY_PROTOCOL_FORD_UBP = 10;
        public const short SPY_PROTOCOL_BEAN = 11;
        public const short SPY_PROTOCOL_LIN = 12;

        
        public static double icsneoGetTimeStamp(long TimeHardware, long TimeHardware2)
        {
            return NEOVI_TIMEHARDWARE2_SCALING * TimeHardware2 + NEOVI_TIMEHARDWARE_SCALING * TimeHardware;
        }

        public static void ConvertCANtoJ1850Message(ref icsSpyMessage icsCANStruct, ref icsSpyMessageJ1850 icsJ1850Struct)
        {
            icsJ1850Struct.StatusBitField = icsCANStruct.StatusBitField;
            icsJ1850Struct.StatusBitField2 = icsCANStruct.StatusBitField2;
            icsJ1850Struct.TimeHardware = icsCANStruct.TimeHardware;
            icsJ1850Struct.TimeHardware2 = icsCANStruct.TimeHardware2;
            icsJ1850Struct.TimeSystem = icsCANStruct.TimeSystem;
            icsJ1850Struct.TimeSystem2 = icsCANStruct.TimeSystem2;
            icsJ1850Struct.TimeStampHardwareID = icsCANStruct.TimeStampHardwareID;
            icsJ1850Struct.TimeStampSystemID = icsCANStruct.TimeStampSystemID;
            icsJ1850Struct.NetworkID = icsCANStruct.NetworkID;
            icsJ1850Struct.NodeID = icsCANStruct.NodeID;
            icsJ1850Struct.Protocol = icsCANStruct.Protocol;
            icsJ1850Struct.MessagePieceID = icsCANStruct.MessagePieceID;
            icsJ1850Struct.ColorID = icsCANStruct.ColorID;
            icsJ1850Struct.NumberBytesHeader = icsCANStruct.NumberBytesHeader;
            icsJ1850Struct.NumberBytesData = icsCANStruct.NumberBytesData;
            icsJ1850Struct.DescriptionID = icsCANStruct.DescriptionID;
            icsJ1850Struct.Header1 = Convert.ToByte(icsCANStruct.ArbIDOrHeader & 0xff);
            icsJ1850Struct.Header2 = Convert.ToByte((0xFF00 & icsCANStruct.ArbIDOrHeader) / 256);
            icsJ1850Struct.Header3 = Convert.ToByte((0xFF0000 & icsCANStruct.ArbIDOrHeader) / 65536);
            icsJ1850Struct.Data1 = icsCANStruct.Data1;
            icsJ1850Struct.Data2 = icsCANStruct.Data2;
            icsJ1850Struct.Data3 = icsCANStruct.Data3;
            icsJ1850Struct.Data4 = icsCANStruct.Data4;
            icsJ1850Struct.Data5 = icsCANStruct.Data5;
            icsJ1850Struct.Data6 = icsCANStruct.Data6;
            icsJ1850Struct.Data7 = icsCANStruct.Data7;
            icsJ1850Struct.Data8 = icsCANStruct.Data8;
            icsJ1850Struct.AckBytes1 = icsCANStruct.AckBytes1;
            icsJ1850Struct.AckBytes2 = icsCANStruct.AckBytes2;
            icsJ1850Struct.AckBytes3 = icsCANStruct.AckBytes3;
            icsJ1850Struct.AckBytes4 = icsCANStruct.AckBytes4;
            icsJ1850Struct.AckBytes5 = icsCANStruct.AckBytes5;
            icsJ1850Struct.AckBytes6 = icsCANStruct.AckBytes6;
            icsJ1850Struct.AckBytes7 = icsCANStruct.AckBytes7;
            icsJ1850Struct.AckBytes8 = icsCANStruct.AckBytes8;
            icsJ1850Struct.Value = icsCANStruct.Value;
            icsJ1850Struct.MiscData = icsCANStruct.MiscData;
        }

        public static void ConvertJ1850toCAN(ref icsSpyMessage icsCANStruct, ref icsSpyMessageJ1850 icsJ1850Struct)
        {
            //Becuse memcopy is not available.  
            icsCANStruct.StatusBitField = icsJ1850Struct.StatusBitField;
            icsCANStruct.StatusBitField2 = icsJ1850Struct.StatusBitField2;
            icsCANStruct.TimeHardware = icsJ1850Struct.TimeHardware;
            icsCANStruct.TimeHardware2 = icsJ1850Struct.TimeHardware2;
            icsCANStruct.TimeSystem = icsJ1850Struct.TimeSystem;
            icsCANStruct.TimeSystem2 = icsJ1850Struct.TimeSystem2;
            icsCANStruct.TimeStampHardwareID = icsJ1850Struct.TimeStampHardwareID;
            icsCANStruct.TimeStampSystemID = icsJ1850Struct.TimeStampSystemID;
            icsCANStruct.NetworkID = icsJ1850Struct.NetworkID;
            icsCANStruct.NodeID = icsJ1850Struct.NodeID;
            icsCANStruct.Protocol = icsJ1850Struct.Protocol;
            icsCANStruct.MessagePieceID = icsJ1850Struct.MessagePieceID;
            icsCANStruct.ColorID = icsJ1850Struct.ColorID;
            icsCANStruct.NumberBytesHeader = icsJ1850Struct.NumberBytesHeader;
            icsCANStruct.NumberBytesData = icsJ1850Struct.NumberBytesData;
            icsCANStruct.DescriptionID = icsJ1850Struct.DescriptionID;
            icsCANStruct.ArbIDOrHeader = (icsJ1850Struct.Header3 * 65536) + (icsJ1850Struct.Header2 * 256) + icsJ1850Struct.Header1;
            icsCANStruct.Data1 = icsJ1850Struct.Data1;
            icsCANStruct.Data2 = icsJ1850Struct.Data2;
            icsCANStruct.Data3 = icsJ1850Struct.Data3;
            icsCANStruct.Data4 = icsJ1850Struct.Data4;
            icsCANStruct.Data5 = icsJ1850Struct.Data5;
            icsCANStruct.Data6 = icsJ1850Struct.Data6;
            icsCANStruct.Data7 = icsJ1850Struct.Data7;
            icsCANStruct.Data8 = icsJ1850Struct.Data8;
            icsCANStruct.AckBytes1 = icsJ1850Struct.AckBytes1;
            icsCANStruct.AckBytes2 = icsJ1850Struct.AckBytes2;
            icsCANStruct.AckBytes3 = icsJ1850Struct.AckBytes3;
            icsCANStruct.AckBytes4 = icsJ1850Struct.AckBytes4;
            icsCANStruct.AckBytes5 = icsJ1850Struct.AckBytes5;
            icsCANStruct.AckBytes6 = icsJ1850Struct.AckBytes6;
            icsCANStruct.AckBytes7 = icsJ1850Struct.AckBytes7;
            icsCANStruct.AckBytes8 = icsJ1850Struct.AckBytes8;
            icsCANStruct.Value = icsJ1850Struct.Value;
            icsCANStruct.MiscData = icsJ1850Struct.MiscData;
        }

        public static string ConvertToHex(string sInput)
        {
            string sOut;
            uint uiDecimal = 0;

            try
            {
                //Convert text string to unsigned Int32eger
                uiDecimal = checked(Convert.ToUInt32(sInput));
            }
            catch (OverflowException)
            {
                sOut = "Overflow";
                return sOut;
            }
            //Format unsigned Int32eger value to hex 
            sOut = string.Format("{0:x2}", uiDecimal);
            return sOut;
        }

        public static int ConvertFromHex(string num)
        {
            //To hold our converted unsigned Int32eger32 value
            uint uiHex = 0;
            try
            {
                // Convert hex string to unsigned Int32eger
                uiHex = Convert.ToUInt32(num, 16);
            }
            catch (OverflowException)
            {
                //
            }
            return Convert.ToInt32(uiHex);
        }

        public static int GetNetworkIDfromString(ref string NetworkName)
        {
            switch (NetworkName)
            {
                case "DEVICE":
                    return ((int)eNETWORK_ID.NETID_DEVICE);
                case "HSCAN":
                    return ((int)eNETWORK_ID.NETID_HSCAN);
                case "MSCAN":
                    return ((int)eNETWORK_ID.NETID_MSCAN);
                case "SWCAN":
                    return ((int)eNETWORK_ID.NETID_SWCAN);
                case "LSFTCAN":
                    return ((int)eNETWORK_ID.NETID_LSFTCAN);
                case "J1708":
                    return ((int)eNETWORK_ID.NETID_J1708);
                case "JVPW":
                    return ((int)eNETWORK_ID.NETID_JVPW);
                case "ISO":
                    return ((int)eNETWORK_ID.NETID_ISO);
                case "ISO2":
                    return ((int)eNETWORK_ID.NETID_ISO2);
                case "ISO3":
                    return ((int)eNETWORK_ID.NETID_ISO3);
                case "ISO4":
                    return ((int)eNETWORK_ID.NETID_ISO4);
                case "HSCAN2":
                    return ((int)eNETWORK_ID.NETID_HSCAN2);
                case "HSCAN3":
                    return ((int)eNETWORK_ID.NETID_HSCAN3);
                case "LIN":
                    return ((int)eNETWORK_ID.NETID_LIN);
                case "LIN2":
                    return ((int)eNETWORK_ID.NETID_LIN2);
                case "LIN3":
                    return ((int)eNETWORK_ID.NETID_LIN3);
                case "LIN4":
                    return ((int)eNETWORK_ID.NETID_LIN4);
            }
            return (-1);
        }

        public static string GetStringForNetworkID(short lNetworkID)
        {
            string sTempOutput = "N/A";
            switch (lNetworkID)
            {
                case (short)eNETWORK_ID.NETID_DEVICE:
                    sTempOutput = "DEVICE";
                    break;

                case (short)eNETWORK_ID.NETID_HSCAN:
                    sTempOutput = "HSCAN";
                    break;

                case (short)eNETWORK_ID.NETID_MSCAN:
                    sTempOutput = "MSCAN";
                    break;

                case (short)eNETWORK_ID.NETID_SWCAN:
                    sTempOutput = "SWCAN";
                    break;

                case (short)eNETWORK_ID.NETID_LSFTCAN:
                    sTempOutput = "LSFTCAN";
                    break;

                case (short)eNETWORK_ID.NETID_J1708:
                    sTempOutput = "J1708";
                    break;

                case (short)eNETWORK_ID.NETID_JVPW:
                    sTempOutput = "JVPW";
                    break;

                case (short)eNETWORK_ID.NETID_ISO:
                    sTempOutput = "ISO";
                    break;

                case (short)eNETWORK_ID.NETID_ISO2:
                    sTempOutput = "ISO2";
                    break;

                case (short)eNETWORK_ID.NETID_ISO3:
                    sTempOutput = "ISO3";
                    break;

                case (short)eNETWORK_ID.NETID_ISO4:
                    sTempOutput = "ISO4";
                    break;

                case (short)eNETWORK_ID.NETID_HSCAN2:
                    sTempOutput = "HSCAN2";
                    break;

                case (short)eNETWORK_ID.NETID_HSCAN3:
                    sTempOutput = "HSCAN3";
                    break;

                case (short)eNETWORK_ID.NETID_LIN:
                    sTempOutput = "LIN1";
                    break;

                case (short)eNETWORK_ID.NETID_LIN2:
                    sTempOutput = "LIN2";
                    break;

                case (short)eNETWORK_ID.NETID_LIN3:
                    sTempOutput = "LIN3";
                    break;

                case (short)eNETWORK_ID.NETID_LIN4:
                    sTempOutput = "LIN4";
                    break;
            }
            return sTempOutput;
        }
    }
}
