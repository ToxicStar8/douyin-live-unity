/*********************************************
 * BFramework
 * Base64工具类
 * 创建时间：2023/06/26 11:00:00
 *********************************************/
using System;
using System.IO;

namespace Framework
{
    /// <summary>
    /// Base64工具类
    /// </summary>
    public class Base64Util
    {
        /// <summary>
        /// 文件转base64
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="ContentType">需要浏览器直接打开base64时指定</param>
        /// <returns>Base64FileHelper</returns>
        public static Base64Helper FileToBase64(string FilePath, string ContentType = null)
        {
            using (FileStream filestream = new FileStream(FilePath, FileMode.Open))
            {
                string Extension = Path.GetExtension(FilePath);
                return FileToBase64(filestream, Extension, ContentType);
            }
        }

        /// <summary>
        /// 文件转base64
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="Extension">扩展名 示例:.jpg</param>
        /// <param name="ContentType">需要浏览器直接打开base64时指定</param>
        /// <returns>Base64FileHelper</returns>
        public static Base64Helper FileToBase64(System.IO.Stream stream, string Extension, string ContentType = null)
        {
            stream.Position = 0;
            byte[] bt = new byte[stream.Length];
            stream.Read(bt, 0, bt.Length);
            return FileToBase64(bt, Extension, ContentType);
        }

        /// <summary>
        /// 文件转base64
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="Extension">扩展名 示例:.jpg</param>
        /// <param name="ContentType">需要浏览器直接打开base64时指定</param>
        /// <returns>Base64FileHelper</returns>
        public static Base64Helper FileToBase64(byte[] bytes, string Extension, string ContentType = null)
        {
            var base64Str = Convert.ToBase64String(bytes);
            return new Base64Helper(Extension, base64Str, ContentType);
        }
    }

    public class Base64Helper
    {
        /// <summary>
        /// 给序列化使用的实例化
        /// </summary>
        public Base64Helper() { }

        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="Extension">文件扩展 例如.jpg</param>
        /// <param name="Base64">base64字符串</param>
        /// <param name="ContentType">需要浏览器直接打开base64时指定</param>
        public Base64Helper(string Extension, string Base64, string ContentType = null)
        {
            //先对扩展名赋值，因为假如扩展名不存在时在对Base64赋值会检索扩展名
            this.Extension = Extension;
            this.Base64 = Base64;
            this.ContentType = ContentType;
        }

        #region 私有参数
        /// <summary>
        /// 文件流
        /// </summary>
        private MemoryStream _stream { get; set; }
        /// <summary>
        /// 检索前100个字符，假如有,则去掉,前面部分
        /// </summary>
        private const int len = 100;

        /// <summary>
        /// base64字符串
        /// </summary>
        protected string _Base64 { get; set; }
        #endregion

        #region 公共属性
        /// <summary>
        /// 扩展文件名
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// base64字符串
        /// </summary>
        /// <remarks>如果Base64没有包含扩展名时，必须要设置Extension之后才能正常使用</remarks>
        public string Base64
        {
            get
            {
                return _Base64;
            }
            set
            {
                string file = value;
                if (string.IsNullOrEmpty(file))
                {
                    _Base64 = null;
                    Extension = null;
                    return;
                }

                int count = file.IndexOf(',', 0, len);

                if (count >= 0)
                {
                    string strExtension = file.Remove(file.IndexOf(';'));

                    if (string.IsNullOrEmpty(Extension))
                    {
                        Extension = "." + strExtension.Substring(strExtension.IndexOf('/') + 1);
                    }

                    if (string.IsNullOrEmpty(ContentType))
                    {
                        int Start = strExtension.IndexOf(':');
                        ContentType = strExtension.Substring(Start + 1);
                    }

                    file = file.Substring(count + 1);
                }

                _Base64 = file;
            }
        }

        /// <summary>
        /// 如果要使用HtmlBase64请指定ContentType
        /// 部分是支持浏览器直接查看，部分是下载
        /// </summary>
        /// <remarks>例如:application/pdf;image/jpeg;等</remarks>
        public string ContentType { get; set; }

        /// <summary>
        /// 支持浏览器打开的base64
        /// </summary>
        /// <remarks>只有在实例化时指定了ContentType才能用</remarks>
        public string HtmlBase64
        {
            get
            {
                if (string.IsNullOrEmpty(ContentType))
                {
                    throw new Exception("未指定ContentType");
                }
                return string.Format("data:{0};base64,{1}", ContentType, Base64);
            }
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long Length { get { return stream.Length; } }

        /// <summary>
        /// 文件流
        /// </summary>
        protected MemoryStream stream
        {
            get
            {
                if (IsNull)
                {
                    throw new Exception("base64字符串或扩展名为空");
                }
                if (_stream == null)
                {
                    string base64 = Base64.Replace(' ', '+');
                    _stream = new MemoryStream(Convert.FromBase64String(base64));
                }
                return _stream;
            }
        }

        /// <summary>
        /// base64字符串、扩展名是否为空
        /// </summary>
        public bool IsNull
        {
            get
            {
                return (string.IsNullOrEmpty(Base64) || string.IsNullOrEmpty(Extension));
            }
        }
        #endregion

        /// <summary>
        /// 将base64保存为文件并返回新的文件名(带后缀)
        /// </summary>
        /// <param name="path">文件保存路径</param>
        /// <param name="FileName">文件名，不带后缀</param>
        /// <param name="maxLen">文件最大长度,0不限制</param>
        public string Save(string path, string FileName, int maxLen = 0)
        {
            if (maxLen != 0 && Length > maxLen)
            {
                throw new Exception("文件大小不能超过" + (maxLen / 1024) + "kb");
            }

            //检查目录，没有就创建
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //检查路径是否需要\
            if (!path.EndsWith("\\"))
            {
                path += "\\";
            }

            //检查是否存在同名文件，存在就+(1)
            while (File.Exists(path + FileName + Extension))
            {
                FileName = FileName + "(1)";
            }

            //组成新的文件名
            string NewFileName = FileName + Extension;

            //写文件
            FileStream fs = new FileStream(path + FileName + Extension, FileMode.Create, FileAccess.Write);
            byte[] bytes = stream.ToArray();

            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
            return NewFileName;
        }
    }
}