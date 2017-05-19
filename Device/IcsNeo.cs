using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BtFlash.Device.Neo
{
    public class icsNeoDll
    {
        public const double NEOVI_TIMEHARDWARE2_SCALING = 0.1048576;
        public const double NEOVI_TIMEHARDWARE_SCALING = 0.0000016;

        public const double NEOVIPRO_VCAN_TIMEHARDWARE2_SCALING = 0.065536;
        public const double NEOVIPRO_VCAN_TIMEHARDWARE_SCALING = 0.000001;

        // med speed CAN
        public const Int16 NEO_CFG_MPIC_MS_CAN_CNF1 = 512 + 22;
        public const Int16 NEO_CFG_MPIC_MS_CAN_CNF2 = 512 + 21;
        public const Int16 NEO_CFG_MPIC_MS_CAN_CNF3 = 512 + 20;

        public const Int16 NEO_CFG_MPIC_SW_CAN_CNF1 = 512 + 34;
        public const Int16 NEO_CFG_MPIC_SW_CAN_CNF2 = 512 + 33;
        public const Int16 NEO_CFG_MPIC_SW_CAN_CNF3 = 512 + 32;

        public const Int16 NEO_CFG_MPIC_LSFT_CAN_CNF1 = 512 + 46;
        public const Int16 NEO_CFG_MPIC_LSFT_CAN_CNF2 = 512 + 45;
        public const Int16 NEO_CFG_MPIC_LSFT_CAN_CNF3 = 512 + 44;

        // Protocols
        public const Int16 SPY_PROTOCOL_CUSTOM = 0;
        public const Int16 SPY_PROTOCOL_CAN = 1;
        public const Int16 SPY_PROTOCOL_GMLAN = 2;
        public const Int16 SPY_PROTOCOL_J1850VPW = 3;
        public const Int16 SPY_PROTOCOL_J1850PWM = 4;
        public const Int16 SPY_PROTOCOL_ISO9141 = 5;
        public const Int16 SPY_PROTOCOL_Keyword2000 = 6;
        public const Int16 SPY_PROTOCOL_GM_ALDL_UART = 7;
        public const Int16 SPY_PROTOCOL_CHRYSLER_CCD = 8;
        public const Int16 SPY_PROTOCOL_CHRYSLER_SCI = 9;
        public const Int16 SPY_PROTOCOL_FORD_UBP = 10;
        public const Int16 SPY_PROTOCOL_BEAN = 11;
        public const Int16 SPY_PROTOCOL_LIN = 12;

        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoFindNeoDevices(UInt32 DeviceTypes, ref NeoDevice pNeoDevice, ref Int32 pNumDevices);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoOpenNeoDevice(ref NeoDevice pNeoDevice, ref Int32 hObject, ref byte bNetworkIDs, Int32 bConfigRead, Int32 bSyncToPC);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoClosePort(Int32 hObject, ref Int32 pNumberOfErrors);
        [DllImport("icsneo40.dll")]
        public static extern void icsneoFreeObject(Int32 hObject);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoOpenPortEx(Int32 lPortNumber, Int32 lPortType, Int32 lDriverType, Int32 lIPAddressMSB, Int32 lIPAddressLSBOrBaudRate, Int32 bConfigRead, ref byte bNetworkID, ref Int32 hObject);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoGetMessages(Int32 hObject, ref icsSpyMessage pMsg, ref Int32 pNumberOfMessages, ref Int32 pNumberOfErrors);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoTxMessages(Int32 hObject, ref icsSpyMessageJ1850 pMsg, Int32 lNetworkID, Int32 lNumMessages);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoTxMessages(Int32 hObject, ref icsSpyMessage pMsg, Int32 lNetworkID, Int32 lNumMessages);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoWaitForRxMessagesWithTimeOut(Int32 hObject, UInt32 iTimeOut);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoEnableNetworkRXQueue(Int32 hObject, Int32 iEnable);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoGetTimeStampForMsg(Int32 hObject, ref icsSpyMessage pMsg, ref double pTimeStamp);
        [DllImport("icsneo40.dll")]
        public static extern void icsneoGetISO15765Status(Int32 hObject, Int32 lNetwork, Int32 lClearTxStatus, Int32 lClearRxStatus, ref Int32 lTxStatus, ref Int32 lRxStatus);
        //[DllImport("icsneo40.dll")]
        //public static extern void icsneoSetISO15765RxParameters(Int32 hObject, Int32 lNetwork, Int32 lEnable, ref spyFilterLong pFF_CFMsgFilter, ref icsSpyMessage pTxMsg, Int32 lCFTimeOutMs, Int32 lFlowCBlockSize,Int32 lUsesExtendedAddressing, Int32 lUseHardwareIfPresent);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoGetConfiguration(Int32 hObject, ref byte pData, ref Int32 lNumBytes);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoSendConfiguration(Int32 hObject, ref byte pData, Int32 lNumBytes);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoGetFireSettings(Int32 hObject, ref SFireSettings pSettings, Int32 iNumBytes);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoSetFireSettings(Int32 hObject, ref SFireSettings pSettings, Int32 iNumBytes, Int32 bSaveToEEPROM);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoGetYellowSettings(Int32 hObject, ref sYellowSettings pSettings, Int32 iNumBytes);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoSetYellowSettings(Int32 hObject, ref sYellowSettings pSettings, Int32 iNumBytes, Int32 bSaveToEEPROM);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoGetVCAN3Settings(Int32 hObject, ref SVCAN3Settings pSettings, Int32 iNumBytes);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoSetVCAN3Settings(Int32 hObject, ref SVCAN3Settings pSettings, Int32 iNumBytes, Int32 bSaveToEEPROM);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoSetBitRate(Int32 hObject, Int32 BitRate, Int32 NetworkID);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoGetDeviceParameters(Int32 hObject, ref char pParameter, ref char pValues, Int16 ValuesLength);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoSetDeviceParameters(Int32 hObject, ref char pParmValue, ref Int32 pErrorIndex, Int32 bSaveToEEPROM);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoGetLastAPIError(Int32 hObject, ref UInt32 pErrorNumber);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoGetErrorMessages(Int32 hObject, ref Int32 pErrorMsgs, ref Int32 pNumberOfErrors);
        [DllImport("icsneo40.dll")]
        public static extern int icsneoGetErrorInfo(int iErrorNumber, StringBuilder sErrorDescriptionShort, StringBuilder sErrorDescriptionLong, ref int iMaxLengthShort, ref int iMaxLengthLong, ref int lErrorSeverity, ref int lRestartNeeded);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoValidateHObject(Int32 hObject);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoGetDLLVersion();
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoStartSockServer(Int32 hObject, Int32 iPort);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoStopSockServer(Int32 hObject);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoScriptStart(Int32 hObject, Int32 iLocation);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoScriptStop(Int32 hObject);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoScriptLoad(Int32 hObject, ref byte bin, UInt32 len_bytes, Int32 iLocation);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoScriptClear(Int32 hObject, Int32 iLocation);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoScriptStartFBlock(Int32 hObject, UInt32 fb_index);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoScriptGetFBlockStatus(Int32 hObject, UInt32 fb_index, ref Int32 piRunStatus);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoScriptStopFBlock(Int32 hObject, UInt32 fb_index);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoScriptGetScriptStatus(Int32 hObject, ref Int32 piStatus);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoScriptReadAppSignal(Int32 hObject, UInt32 iIndex, ref double dValue);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoScriptWriteAppSignal(Int32 hObject, UInt32 iIndex, double dValue);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoScriptReadRxMessage(Int32 hObject, UInt32 iIndex, ref icsSpyMessage pRxMessageMask, ref icsSpyMessage pRxMessageValue);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoScriptReadTxMessage(Int32 hObject, UInt32 iIndex, ref icsSpyMessage pTxMessage);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoScriptWriteRxMessage(Int32 hObject, UInt32 iIndex, ref icsSpyMessage pRxMessageMask, ref icsSpyMessage pRxMessageValue);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoScriptWriteTxMessage(Int32 hObject, UInt32 iIndex, ref icsSpyMessage pTxMessage);
        //[DllImport("icsneo40.dll")]
        //public static extern Int32 icsneoScriptReadISO15765_2_TxMessage(Int32 hObject, UInt32 iIndex, stCM_ISO157652_ref TxMessage pTxMessage);
        //[DllImport("icsneo40.dll")]
        //public static extern Int32 icsneoScriptWriteISO15765_2_TxMessage(Int32 hObject, UInt32 iIndex,  stCM_ISO157652_ref TxMessage pTxMessage);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoOpenPort(Int32 lPortNumber, Int32 lPortType, Int32 lDriverType, ref byte bNetworkID, ref byte bSCPIDs, ref Int32 hObject);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoEnableNetworkCom(Int32 hObject, Int32 Enable);
        [DllImport("icsneo40.dll")]
        public static extern Int32 icsneoFindAllCOMDevices(Int32 lDriverType, Int32 lGetSerialNumbers, Int32 lStopAtFirst, Int32 lUSBCommOnly, ref Int32 p_lDeviceTypes, ref Int32 p_lComPorts, ref Int32 p_lSerialNumbers, ref Int32 lNumDevices);

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
                uiDecimal = checked((uint)System.Convert.ToUInt32(sInput));
            }
            catch (System.OverflowException)
            {
                sOut = "Overflow";
                return sOut;
            }
            //Format unsigned Int32eger value to hex 
            sOut = String.Format("{0:x2}", uiDecimal);
            return sOut;
        }

        public static Int32 ConvertFromHex(string num)
        {
            //To hold our converted unsigned Int32eger32 value
            uint uiHex = 0;
            try
            {
                // Convert hex string to unsigned Int32eger
                uiHex = System.Convert.ToUInt32(num, 16);
            }
            catch (System.OverflowException)
            {
                //
            }
            return Convert.ToInt32(uiHex);
        }

        public static Int32 GetNetworkIDfromString(ref string NetworkName)
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

        public static string GetStringForNetworkID(Int16 lNetworkID)
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
