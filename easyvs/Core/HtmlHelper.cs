using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using easyVS.Forms.Setting.UC;

namespace EasyCodeGenerate.Core
{
    public class HtmlHelper
    {
        #region - Methods -

        /// <summary>
        /// 获得基础流
        /// </summary>
        /// <param name="uri">网址</param>
        /// <param name="cc">cookie容器，可以为NULL</param>
        /// <returns></returns>
        public static Stream GetBaseStream(string uri, CookieContainer cc)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);   //用指定Uri创建一个request
                if (cc != null)
                {
                    request.CookieContainer = cc;
                }
                //浏览器欺骗
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = @"application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                request.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/533.2 (KHTML, like Gecko) Chrome/5.0.342.9 Safari/533.2 ChromePlus/1.3.9.0";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();      //根据创建的request得到响应response
                Stream responseStream = response.GetResponseStream();  //创建一个流来获得响应体
                return responseStream;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(@"操作失败：" + ex.Message);
                return null;
            }
        }

        ///// <summary>
        ///// 获得网页
        ///// </summary>
        ///// <param name="uri">网址</param>
        ///// <param name="postDate"></param>
        ///// <param name="cc">cookie容器，可以为null</param>
        ///// <param name="encoding">网页编码</param>
        ///// <returns></returns>
        //public static string GetHtmlString(string uri, string postDate, CookieContainer cc, Encoding encoding,IWebProxy proxy)
        //{
        //    try
        //    {
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
        //        request.Proxy = proxy;
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.AllowAutoRedirect = true;
        //        request.Accept = "application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
        //        request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/533.2 (KHTML, like Gecko) Chrome/5.0.342.9 Safari/533.2 ChromePlus/1.3.9.0";
        //        request.CookieContainer = cc;     //设置request产生cookie的容器
        //        if (postDate != null)
        //        {
        //            request.Method = "Post";
        //            byte[] byterequest = Encoding.UTF8.GetBytes(postDate);
        //            request.ContentLength = byterequest.Length;
        //            using (Stream stream = request.GetRequestStream())
        //            {
        //                stream.Write(byterequest, 0, byterequest.Length);
        //            }

        //        }

        //        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //        {
        //            using (Stream responsestream = response.GetResponseStream())
        //            {
        //                StreamReader sr = new StreamReader(responsestream, encoding);
        //                string html = sr.ReadToEnd();
        //                return html;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // MessageBox.Show(@"发生错误:" + ex.Message);
        //        return null;
        //    }
        //}

        public static string GetHtmlString(string uri, string postDate, CookieContainer cc, Encoding encoding)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

                //尝试系统默认代理，如果成功则调用系统默认代理，忽略手动设置
                if (CheckSystemProxyWorks())
                {
                    request.Proxy = WebRequest.GetSystemWebProxy();
                }
                else
                {
                    if (BaseUCSetting.SettingModel.NetWorkModel.Address != string.Empty)
                    {
                        WebProxy proxy = new WebProxy(string.Format("{0}:{1}", BaseUCSetting.SettingModel.NetWorkModel.Address, BaseUCSetting.SettingModel.NetWorkModel.Port));
                        if (BaseUCSetting.SettingModel.NetWorkModel.User != string.Empty)
                        {
                            proxy.Credentials = new NetworkCredential(BaseUCSetting.SettingModel.NetWorkModel.User, BaseUCSetting.SettingModel.NetWorkModel.Pwd);
                        }
                        request.Proxy = proxy;
                    }
                }

                request.ContentType = "application/x-www-form-urlencoded";
                request.AllowAutoRedirect = true;
                request.Accept = "application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
                request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/533.2 (KHTML, like Gecko) Chrome/5.0.342.9 Safari/533.2 ChromePlus/1.3.9.0";
                request.CookieContainer = cc;     //设置request产生cookie的容器
                if (postDate != null)
                {
                    request.Method = "Post";
                    byte[] byterequest = Encoding.UTF8.GetBytes(postDate);
                    request.ContentLength = byterequest.Length;
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(byterequest, 0, byterequest.Length);
                    }

                }
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responsestream = response.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(responsestream, encoding);
                        string html = sr.ReadToEnd();
                        return html;
                    }
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(@"发生错误:" + ex.Message);
                return null;
            }
        }

        private static bool CheckSystemProxyWorks()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.baidu.com");
            request.Proxy = WebProxy.GetDefaultProxy();
            request.ContentType = "application/x-www-form-urlencoded";
            request.AllowAutoRedirect = true;
            request.Accept = "application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
            request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/533.2 (KHTML, like Gecko) Chrome/5.0.342.9 Safari/533.2 ChromePlus/1.3.9.0";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responsestream = response.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(responsestream, Encoding.UTF8);
                        string html = sr.ReadToEnd();
                        return html.Length > 0;
                    }
                }
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 从字符串中返回匹配多个的集合值（网页抽取特定部分有效）
        /// </summary>
        /// <param name="start">开始html tag</param>
        /// <param name="end">结束html tag</param>
        /// <param name="html">html</param>
        /// <returns></returns>
        public static List<string> GetStrings(string start, string end, string html)
        {
            List<string> list = new List<string>();
            try
            {
                string pattern = string.Format("{0}(?<g>(.|[\r\n])+?){1}", start, end);//匹配URL的模式,并分组    //理解这个正则
                MatchCollection mc = Regex.Matches(html, pattern);//满足pattern的匹配集合
                if (mc.Count != 0)
                {
                    foreach (Match match in mc)
                    {
                        GroupCollection gc = match.Groups;
                        list.Add(gc["g"].Value);
                    }
                }
            }
            catch
            { }
            return list;
        }

        /// <summary>
        /// 中文url编码
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string UrlEncode(string buffer)
        {
            byte[] bty = Encoding.Default.GetBytes(buffer);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bty.Length; i++)
            {
                builder.Append("%");
                builder.Append(bty[i].ToString("x2"));
            }
            return builder.ToString();
        }

        #endregion

    }
}
