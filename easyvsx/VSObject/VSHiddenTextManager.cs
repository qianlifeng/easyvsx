using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace easyvsx.VSObject
{
    public class VSHiddenTextManager : VSBase
    {
        #region - Variables -

        private IVsHiddenTextSession hiddenTextSession;

        private IVsHiddenTextManager hiddenTextManager;

        private IVsHiddenRegion hiddenRegion;

        public IVsHiddenTextClient hiddenTextClient;

        #endregion

        #region - Constructor -

        public VSHiddenTextManager(IVsTextView view)
        {
            hiddenTextSession = GetHiddenTextSession(view);
        }

        public VSHiddenTextManager(IVsTextView view, IVsHiddenTextClient client)
        {
            hiddenTextSession = GetHiddenTextSession(view);
            hiddenTextClient = client;
        }

        public VSHiddenTextManager()
        {

        }

        #endregion

        #region - Methods -

        private IVsHiddenTextSession GetHiddenTextSession(IVsTextView view)
        {
            if (view == null)
            {
                return null;
            }

            IVsTextLines txtLine;
            view.GetBuffer(out txtLine);
            hiddenTextSession = null;
            //每个session都与特定的Textbuffer关联，这里默认是当前窗口的buffer
            HiddenTextManager.GetHiddenTextSession(txtLine, out hiddenTextSession);
            if (hiddenTextSession == null)
            {
                HiddenTextManager.CreateHiddenTextSession(0, txtLine,/* hiddenTextClient  */hiddenTextClient, out hiddenTextSession);
            }
            return hiddenTextSession;
        }

        /// <summary>
        /// 得到enumHiddenRegions枚举中的region个数
        /// </summary>
        /// <param name="enumHiddenRegions"></param>
        /// <returns></returns>
        public uint GetHiddenRegionCount(IVsEnumHiddenRegions enumHiddenRegions)
        {
            uint count;
            enumHiddenRegions.GetCount(out count);
            return count;
        }

        /// <summary>
        /// 得到enumHiddenRegions中的所有region的span信息
        /// </summary>
        /// <param name="enumHiddenRegions"></param>
        /// <returns></returns>
        public IList<TextSpan> GetHiddenRegions(IVsEnumHiddenRegions enumHiddenRegions)
        {
            IList<TextSpan> regions = null;

            uint count = GetHiddenRegionCount(enumHiddenRegions);

            if (count > 0)
            {
                regions = new List<TextSpan>((int)count);

                var hiddenRegions = new IVsHiddenRegion[count];

                enumHiddenRegions.Reset();
                enumHiddenRegions.Next(count, hiddenRegions, out count);

                for (int i = 0; i < count; i++)
                {
                    IVsHiddenRegion hregion = hiddenRegions[i];

                    uint state = 0;
                    hregion.GetState(out state);
                    if ((state & (uint)HIDDEN_REGION_STATE.hrsExpanded) == 0)
                    {
                        TextSpan[] pSpan = new TextSpan[1];
                        hregion.GetSpan(pSpan);
                        regions.Add(pSpan[0]);
                    }
                }
            }

            return regions;
        }

        public void CreateHiddenRegion(string bannerText, TextSpan ts)
        {
            IVsHiddenTextSession session = HiddenTextSession;
            if (session != null)
            {
                NewHiddenRegion[] NewHiddenRegionArray = new NewHiddenRegion[1];
                NewHiddenRegionArray[0].dwBehavior = (uint)HIDDEN_REGION_BEHAVIOR.hrbClientControlled;
                //NewHiddenRegionArray[0].dwClient = 0x2cff;
                NewHiddenRegionArray[0].dwState = (uint)HIDDEN_REGION_STATE.hrsDefault;
                NewHiddenRegionArray[0].iType = (int)HIDDEN_REGION_TYPE.hrtCollapsible;
                NewHiddenRegionArray[0].pszBanner = bannerText;
                NewHiddenRegionArray[0].tsHiddenText = ts;
                int isOk = session.AddHiddenRegions(
                    (uint)CHANGE_HIDDEN_REGION_FLAGS.chrDefault,
                    1,
                    NewHiddenRegionArray,
                    null);

                if (isOk != VSConstants.S_OK)
                {
                    MessageBox.Show("error");
                }
            }
        }

        public IVsEnumHiddenRegions GetEnumHiddenRegions()
        {
            IVsHiddenTextSession session = HiddenTextSession;
            if (session != null)
            {
                IVsEnumHiddenRegions ppenum;
                session.EnumHiddenRegions((uint)FIND_HIDDEN_REGION_FLAGS.FHR_ALL_REGIONS, 0, null, out ppenum);
                return ppenum;
            }

            return null;
        }

        //todo:尽在apsx下面才有效，cs文件下无效，不知道为什么
        public void ToggleRegions()
        {
            IVsHiddenTextSession session = HiddenTextSession;
            TextSpan[] aspan = new TextSpan[1];
            aspan[0] = new VSTextView(VSTextView.ActiveTextView).GetWholeTextSpan();

            //获取IVsEnumHiddenRegions
            IVsEnumHiddenRegions ppenum;
            session.EnumHiddenRegions((uint)FIND_HIDDEN_REGION_FLAGS.FHR_ALL_REGIONS, 0, aspan, out ppenum);

            MessageBox.Show(GetHiddenRegionCount(ppenum).ToString());

            //遍历IVsEnumHiddenRegions
            uint fetched;
            IVsHiddenRegion[] aregion = new IVsHiddenRegion[1];
            while (ppenum.Next(1, aregion, out fetched) == VSConstants.S_OK && fetched == 1)
            {
                uint dwState;
                aregion[0].GetState(out dwState);
                dwState ^= (uint)HIDDEN_REGION_STATE.hrsExpanded;
                aregion[0].SetState(dwState, (uint)CHANGE_HIDDEN_REGION_FLAGS.chrDefault);
            }
        }

        #endregion

        #region - 属性 -

        public IVsHiddenTextManager HiddenTextManager
        {
            get
            {
                if (hiddenTextManager == null)
                {
                    Object obj = Package.GetGlobalService(typeof(SVsTextManager));
                    if (obj == null)
                    {
                        return null;
                    }
                    hiddenTextManager = obj as IVsHiddenTextManager;
                }
                return hiddenTextManager;
            }
        }

        public IVsHiddenTextSession HiddenTextSession
        {
            get
            {
                if (hiddenTextSession == null)
                {
                    GetHiddenTextSession(VSTextView.ActiveTextView);
                }
                return hiddenTextSession;
            }
            set
            {
                hiddenTextSession = value;
            }
        }

        #endregion

    }
}
