using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Language.Intellisense;
using System.Collections.ObjectModel;

namespace easyVSSmartTag
{
    public class EasyVSSmartTag : SmartTag
    {
        public EasyVSSmartTag(ReadOnlyCollection<SmartTagActionSet> actionSets) :
            base(SmartTagType.Factoid, actionSets)
        {

        }
    }
}
