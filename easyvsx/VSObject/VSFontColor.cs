/******************************************************************************
 *  作者：       scott
 *  创建时间：   2012/2/18 13:54:03
 *
 *
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System.Drawing;
using Microsoft.VisualStudio;

namespace easyvsx.VSObject
{
    public class VSFontColor : VSBase
    {
        #region - 变量 -

        private static IVsFontAndColorStorage fontColorStorage;
        private static IVsFontAndColorCacheManager cacheManager;

        private static string textEditGuid = "A27B4E24-A735-4D1D-B8E7-9716E1E3D8E0";

        #endregion

        #region - 属性 -

        /// <summary>
        /// 获得全局的FontAndColor接口对象
        /// </summary>
        public static IVsFontAndColorStorage FontColorObject
        {
            get
            {
                if (fontColorStorage == null)
                {
                    Object obj = Package.GetGlobalService(typeof(SVsFontAndColorStorage));
                    if (obj == null)
                    {
                        throw new ArgumentNullException("获得SVsFontAndColorStorage失败");
                    }
                    fontColorStorage = obj as IVsFontAndColorStorage;
                }
                return fontColorStorage;
            }
        }

        public static IVsFontAndColorCacheManager FontColorCacheManagerObject
        {
            get
            {
                if (cacheManager == null)
                {
                    Object obj = Package.GetGlobalService(typeof(SVsFontAndColorCacheManager));
                    if (obj == null)
                    {
                        throw new ArgumentNullException("获得IVsFontAndColorCacheManager失败");
                    }
                    cacheManager = obj as IVsFontAndColorCacheManager;
                }
                return cacheManager;
            }
        }

        #endregion

        #region - 方法 -

        /// <summary>
        /// 获得textedit中的颜色
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Color GetTextEditColor(string name)
        {
            Guid textEditorGuid = new Guid(textEditGuid);
            FontColorObject.OpenCategory(ref textEditorGuid, (uint)__FCSTORAGEFLAGS.FCSF_NOAUTOCOLORS | (uint)__FCSTORAGEFLAGS.FCSF_LOADDEFAULTS);
            try
            {
                ColorableItemInfo[] info = new ColorableItemInfo[1];
                if (FontColorObject.GetItem(name, info) == VSConstants.S_OK)
                {
                    uint c = info[0].crForeground;
                    return ConvertWindowsRGBToColour((int)c);
                }
                return Color.Empty;
            }
            finally
            {
                FontColorObject.CloseCategory();
            }
        }

        /// <summary>
        /// 设置textedit中的颜色
        /// </summary>
        /// <param name="name">例如"String"，可以在工具-选项-字体和颜色中查看</param>
        /// <param name="foreColor">Color.Empty采用默认值</param>
        /// <param name="backColor">Color.Empty采用默认值</param>
        public static void SetTextEditColor(string name, Color foreColor, Color backColor)
        {
            var textEditorGuid = new Guid(textEditGuid);
            FontColorObject.OpenCategory(ref textEditorGuid, (uint)__FCSTORAGEFLAGS.FCSF_NOAUTOCOLORS | (uint)__FCSTORAGEFLAGS.FCSF_LOADDEFAULTS);
            try
            {
                ColorableItemInfo[] info = new ColorableItemInfo[1];
                if (FontColorObject.GetItem(name, info) == VSConstants.S_OK)
                {
                    if (foreColor != Color.Empty)
                    {
                        info[0].crForeground = (uint)ConvertColourToWindowsRGB(foreColor);

                    }
                    if (backColor != Color.Empty)
                    {
                        info[0].crBackground = (uint)ConvertColourToWindowsRGB(backColor);
                    }
                    FontColorObject.SetItem(name, info);
                }

            }
            finally
            {
                FontColorObject.CloseCategory();
            }
        }

        /// <summary>
        /// 设置textedit中的颜色
        /// </summary>
        /// <param name="name">例如"String"，可以在工具-选项-字体和颜色中查看</param>
        /// <param name="foreColor">-1采用默认值</param>
        /// <param name="backColor">-1采用默认值</param>
        public static void SetTextEditColor(string name, int foreColor, int backColor)
        {
            var textEditorGuid = new Guid(textEditGuid);
            FontColorObject.OpenCategory(ref textEditorGuid, (uint)__FCSTORAGEFLAGS.FCSF_NOAUTOCOLORS | (uint)__FCSTORAGEFLAGS.FCSF_LOADDEFAULTS);
            try
            {
                ColorableItemInfo[] info = new ColorableItemInfo[1];
                if (FontColorObject.GetItem(name, info) == VSConstants.S_OK)
                {
                    if (foreColor != -1)
                    {
                        info[0].crForeground = (uint)foreColor;

                    }
                    if (backColor != -1)
                    {
                        info[0].crBackground = (uint)backColor;
                    }
                    FontColorObject.SetItem(name, info);
                }

            }
            finally
            {
                FontColorObject.CloseCategory();
            }
        }


        private static int ConvertColourToWindowsRGB(Color dotNetColour)
        {
            int winRGB = 0;

            // windows rgb values have byte order 0x00BBGGRR
            winRGB |= (int)dotNetColour.R;
            winRGB |= (int)dotNetColour.G << 8;
            winRGB |= (int)dotNetColour.B << 16;

            return winRGB;
        }

        private static Color ConvertWindowsRGBToColour(int windowsRGBColour)
        {
            int r = 0, g = 0, b = 0;

            // windows rgb values have byte order 0x00BBGGRR
            r = (windowsRGBColour & 0x000000FF);
            g = (windowsRGBColour & 0x0000FF00) >> 8;
            b = (windowsRGBColour & 0x00FF0000) >> 16;

            Color dotNetColour = Color.FromArgb(r, g, b);

            return dotNetColour;
        }

        /// <summary>
        /// 刷新textedit缓存，使得更改有效
        /// </summary>
        public static void RefreshTextEditCache()
        {
            Guid categoryGuid = new Guid(textEditGuid);
            ErrorHandler.ThrowOnFailure(FontColorCacheManagerObject.RefreshCache(ref categoryGuid));
        }

        #endregion
    }
}
