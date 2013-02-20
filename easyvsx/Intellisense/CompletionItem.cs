using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace easyvsx.Intellisense
{
    /// <summary>
    /// 自动完成列表中的一项
    /// </summary>
    public struct CompletionItem
    {
        public CompletionItem(string displayText,string description,string commitText) : this()
        {
            DisplayText = displayText;
            Description = description;
            CommitText = commitText;
        }

        public string DisplayText { get; set; }

        public string Description { get; set; }

        public string CommitText { get; set; }
    }
}
