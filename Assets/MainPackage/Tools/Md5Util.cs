/*********************************************
 * BFramework
 * Md5工具
 * 创建时间：2022/12/29 20:13:41
 *********************************************/
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MainPackage
{
    /// <summary>
    /// Md5工具
    /// </summary>
    public static class Md5Util
    {
        private static MD5 _md5 = new MD5CryptoServiceProvider();
        private static StringBuilder _sb = new StringBuilder();

        /// <summary>
        /// 根据路径获得文件Md5
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetMd5ByPath(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    _sb.Clear();
                    byte[] hash = _md5.ComputeHash(fs);
                    for (int i = 0; i < hash.Length; i++)
                    {
                        _sb.Append(hash[i].ToString("x2"));
                    }

                    return _sb.ToString().ToUpper();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("获取MD5出错:" + ex.Message);
            }
        }
    }
}
