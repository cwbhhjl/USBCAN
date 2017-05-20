using System.Management;

namespace BtFlash.Util
{
    public class UsbUtil
    {
        private static ManagementObjectCollection collection = null;

        /// <summary>
        /// 根据设备属性值判断USB设备是否存在
        /// </summary>
        /// <param name="property">USB设备属性</param>
        /// <param name="value">USB设备属性对应值</param>
        /// <returns>存在返回<c>true</c>,否则返回<c>false</c></returns>
        public static bool HasUsbDevice(string property, string value)
        {
            if (collection == null)
            {
                using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub"))
                {
                    collection = searcher.Get();
                }
            }

            foreach (var d in collection)
            {
                if ((string)d.GetPropertyValue(property) == value)
                {
                    return true;
                }
            }
            return false;
        }
    }
}