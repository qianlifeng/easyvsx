using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace easyVS.Core
{
    /// <summary>
    /// 延迟执行动作
    /// </summary>
    public class DelayExecute
    {
        /// <summary>
        /// 延迟执行
        /// </summary>
        /// <param name="timeout">延迟时间，毫秒</param>
        /// <param name="func"></param>
        public static void Execute(int timeout, Action func)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(timeout);
                func();
            }));

            thread.Start();
        }
    }
}
