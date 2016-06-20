using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace USBCAN
{
    public static class USBCAN
    {
        /// <summary>
        /// 错误码定义
        /// </summary>
        static Dictionary<uint, string> ERR = new Dictionary<uint, string>()
        {
            {0x00000001, "CAN控制器内部FIFO溢出" },
            {0x00000002, "CAN控制器错误报警" },
            {0x00000004, "CAN控制器消极错误" },
            {0x00000008, "CAN控制器仲裁丢失" },
            {0x00000010, "CAN控制器总线错误" },
            {0x00000020, "CAN控制器总线关闭" },
            {0x00000100, "设备已经打开" },
            {0x00000200, "打开设备错误" },
            {0x00000400, "设备没有打开" },
            {0x00000800, "缓冲区溢出" },
            {0x00001000, "此设备不存在" },
            {0x00002000, "装载动态库失败" },
            {0x00004000, "执行命令失败" },
            {0x00008000, "内存不足" },
        };
    }

}
