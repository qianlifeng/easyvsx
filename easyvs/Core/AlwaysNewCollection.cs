/******************************************************************************
 *  作者：       scott
 *  创建时间：   2012/2/18 17:55:44
 *
 *
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using System.Linq;

namespace easyVS.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class AlwaysNewCollection
    {
        #region - Variables -

        private int capacity;

        private List<Document> list = new List<Document>();

        #endregion

        #region - Constructor -

        public AlwaysNewCollection(int capacity)
        {
            this.capacity = capacity;
        }

        #endregion

        public int Capacity
        {
            get { return capacity; }
            set
            {
                capacity = value;
            }
        }

        #region - Methods -

        public void Add(Document item)
        {
            if (list.Count < capacity)
            {
                //如果已经存在，则删除原有位置，并加入到最后
                if (list.Contains(item))
                {
                    list.Remove(item);
                    list.Add(item);
                }
                else
                {
                    list.Add(item);
                }
            }
            else
            {
                //如果已经存在，则删除原有位置，并加入到最后
                if (list.Contains(item))
                {
                    list.Remove(item);
                    list.Add(item);
                }
                else
                {
                    //如果不存在，则删除最前的一个
                    list.RemoveAt(0);
                    list.Add(item);
                }
            }
        }

        public void Delete(Document item)
        {
            list.Remove(item);
        }

        public bool Contains(Document item)
        {
            return list.Contains(item);
        }

        /// <summary>
        /// 得到当前列表不为空的文件个数
        /// </summary>
        /// <returns></returns>
        public int ValidCount()
        {
            return list.Where(doc => doc != null).Count();
        }

        #endregion
    }
}
