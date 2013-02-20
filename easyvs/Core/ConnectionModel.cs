/******************************************************************************
 *  作者：       scott
 *  创建时间：   2012/2/18 13:14:51
 *
 *
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyCodeGenerate.Core
{
    [Serializable]
    public class ConnectionModel
    {
        #region - Properties -

        public string ServerName { get; set; }

        public string DBName { get; set; }

        /// <summary>
        /// 0 密码验证，1 集成验证
        /// </summary>
        public int AuthorizationType { get; set; }

        public string UserName { get; set; }

        public string Pwd { get; set; }

        #endregion

    }
}
