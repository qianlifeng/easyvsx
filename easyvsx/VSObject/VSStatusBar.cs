using System;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace easyvsx.VSObject
{
    public class VSStatusBar : VSBase
    {
        #region - 变量 -

        private static uint processBarCookie;
        public static uint lastValue;

        #endregion

        #region - 属性 -

        private static IVsStatusbar statusBar;
        public static IVsStatusbar StatusBarObject
        {
            get
            {
                if (statusBar == null)
                {
                    Object obj = Package.GetGlobalService(typeof(SVsStatusbar));
                    if (obj == null)
                    {
                        return null;
                    }
                    statusBar = obj as IVsStatusbar;
                }
                return statusBar;
            }
        }

        private static uint maxValue = 100;
        public static uint MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        /// <summary>
        /// 是否正在显示进度条
        /// </summary>
        private static bool isInProcess;

        #endregion

        #region - 公开方法 -

        /// <summary>
        /// 在状态栏上显示文字
        /// </summary>
        /// <param name="msg"></param>
        public static void SetText(string msg)
        {
            if (StatusBarObject != null && !string.IsNullOrEmpty(msg))
            {
                StatusBarObject.SetText(msg);
            }
        }

        /// <summary>
        /// 消除状态栏上的文字
        /// </summary>
        public static void ClearText()
        {
            if (StatusBarObject != null)
            {
                StatusBarObject.Clear();
            }
        }

        /// <summary>
        /// 显示进度条
        /// </summary>
        /// <param name="leftCornerText">左下角需要显示的文字</param>
        /// <param name="value">进度条的当前值，不超过最大值</param>
        public static void ShowProcessBar(string leftCornerText, uint value)
        {
            if (StatusBarObject != null)
            {
                lastValue = value;
                if (value <= MaxValue)
                {
                    StatusBarObject.Progress(ref processBarCookie, 1, leftCornerText, value, MaxValue);
                }
                else
                {
                    StatusBarObject.Progress(ref processBarCookie, 0, "", 0, 0);
                }
            }
        }

        public static void CancelProcessBar()
        {
            StatusBarObject.Progress(ref processBarCookie, 0, "", 0, 0);
        }

        /// <summary>
        /// 停止进度条滚动
        /// </summary>
        public static void StopProcessBar()
        {
            if (StatusBarObject != null)
            {
                StatusBarObject.Progress(ref processBarCookie, 0, "", 0, 0);
                processBarCookie = 0;
            }
        }

        #endregion
    }
}
