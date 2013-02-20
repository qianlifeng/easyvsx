/******************************************************************************
 *  Author：       SQ1000
 *  CreateDate：   17/02/2012 10:44:01 AM
 *
 *
 ******************************************************************************/
// Guids.cs
// MUST match guids.h

using System;

namespace Easy
{
    internal static class GuidList
    {
        public const string guidPackageTestPkgString = "ad30597b-10a5-4848-a122-a71d2602eef3";


        public const string guidPackageTestCmdSetString = "29d441c5-58be-4476-b35c-8a4955793e26";

        /// <summary>
        /// 在代码视图右击后的命令集合
        /// </summary>
        public const string CodeRightClick_CmdSetString = "1496A755-94DE-11D0-8C3F-00C04FC2AAE2";

        /// <summary>
        /// 在解决方案右击后“添加”项的命令集合
        /// </summary>
        public const string SolutionRightClick_Add_CmdSetString = "68017D43-B813-4ACE-8CE5-EA6A393088B6";

        /// <summary>
        /// 在文件右击后“添加”项的命令集合
        /// </summary>
        public const string FileRightClickCmdSetString = "DAF9DA28-285E-4D61-A350-3FB771B46D66";

        /// <summary>
        /// File菜单下的命令集合
        /// </summary>
        public const string FileMenuCmdSetString = "AAF9DA28-285E-4D61-1234-3FB771B46D55";

        /// <summary>
        /// 纯粹的快捷键指令集合
        /// </summary>
        public const string NonMenuCommand_CmdSetString = "98D17D48-B813-4ACE-8EE5-BA6A395588B6";

        public static readonly Guid guidPackageTestCmdSet = new Guid(guidPackageTestCmdSetString);
    };
}