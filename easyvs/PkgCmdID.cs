/******************************************************************************
 *  Author：       SQ1000
 *  CreateDate：   17/02/2012 10:43:51 AM
 *
 *
 ******************************************************************************/
// PkgCmdID.cs
// MUST match PkgCmdID.h

namespace Easy
{
    internal static class PkgCmdIDList
    {
        #region 帮助

        public const uint cmdSuggestion = 0x100;
        public const uint cmdAbout = 0x101;
        public const uint cmdUpdate = 0x102;

        #endregion

        public const uint cmdSetting = 0x110;

        #region 代码视图右键单击（普通的cs文件）

        public const uint CodeRightClick_UpdateModelFromDB = 9999;
        public const uint CodeRIghtClick_QuickRegion = 10000;
        public const uint CodeRightClick_MoveToRegion = 10001;

        #endregion

        #region 解决方案右击->添加

        public const uint SolutionRightClick_AddModelProject = 10;

        #endregion

        #region 文件右击

        public const uint FileRightClick_OpenInExplore = 10;

        #endregion

        #region File菜单

        public const uint FileMenu_RestartVS = 10;

        #endregion

        #region 纯粹的快捷键命令

        public const uint NonMenuCommand_CloneCurrentLine = 10;

        public const uint NonMenuCommand_GenerateVariable = 20;

        #endregion
    };
}