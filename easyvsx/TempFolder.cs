using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace easyvsx
{
    /// <summary>
    /// 临时文件夹类
    /// </summary>
    public class TempFolder : IDisposable
    {
        private DirectoryInfo directoryInfo;

        /// <summary>
        /// 创建一个临时目录并返回路径
        /// </summary>
        /// <returns></returns>
        public string CreateTempFolder()
        {
            Guid folderName = Guid.NewGuid();
            directoryInfo = Directory.CreateDirectory(folderName.ToString());
            Thread.Sleep(300);  //休眠一段时间，确保文件已经创建
            return directoryInfo.FullName;
        }

        public void Dispose()
        {
            if (directoryInfo != null)
            {
                directoryInfo.Delete(true);
            }
        }

    }
}
