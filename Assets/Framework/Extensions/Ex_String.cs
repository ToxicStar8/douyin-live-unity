/*********************************************
 * BFramework
 * 字符串方法扩展类
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static class Ex_String
    {
        public static Color32 ToColor32(this string colorStr)
        {
            if (!colorStr.Contains('#'))
            {
                colorStr = '#' + colorStr;
            }
            ColorUtility.TryParseHtmlString(colorStr, out var color);
            return color;
        }

        /// <summary>
        /// 转换成int或者默认值
        /// </summary>
        public static int ToInt(this string str, int defaultValue = int.MinValue)
        {
            if (int.TryParse(str, out int result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// 转换成long或者默认值
        /// </summary>
        public static long ToLong(this string strValue, long defaultValue = long.MinValue)
        {
            if (long.TryParse(strValue, out long v))
            {
                return v;
            }
            return defaultValue;
        }

        /// <summary>
        /// 转换成float或者默认值
        /// </summary>
        public static float ToFloat(this string strValue, float defaultValue = float.MinValue)
        {
            if (float.TryParse(strValue, out float v))
            {
                return v;
            }
            return defaultValue;
        }

        /// <summary>
        /// 转换成double或者默认值
        /// </summary>
        public static double ToDouble(this string strValue, double defaultValue = double.MinValue)
        {
            if (double.TryParse(strValue, out double v))
            {
                return v;
            }
            return defaultValue;
        }

        /// <summary>
        /// 转换成Vector3或者默认值  x,y,z
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Vector3 ToVector3(this string strValue, Vector3 defaultValue, char split = ',')
        {
            if (strValue.IsNullOrEmpty())
            {
                return defaultValue;
            }
            string[] strs = strValue.Split(split);
            if (strs.Length >= 3)
            {
                Vector3 v = new Vector3(strs[0].ToFloat(defaultValue.x), strs[1].ToFloat(defaultValue.y), strs[2].ToFloat(defaultValue.z));
                return v;
            }
            return defaultValue;
        }

        /// <summary>
        /// 转换成Vector2或者默认值  x,y
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Vector2 ToVector2(this string strValue, Vector2 defaultValue, char split = ',')
        {
            if (strValue.IsNullOrEmpty())
            {
                return defaultValue;
            }
            string[] strs = strValue.Split(split);
            if (strs.Length >= 2)
            {
                return new Vector2(strs[0].ToFloat(defaultValue.x), strs[1].ToFloat(defaultValue.y));
            }
            return defaultValue;
        }

        /// <summary>
        /// 字符串切割为int数组
        /// </summary>
        public static int[] SplitToIntArr(this string str, char separator)
        {
            if (str.IsNullOrEmpty())
            {
                return new int[0];
            }

            var strArr = str.Split(separator);
            var intArr = new int[strArr.Length];
            for (int i = 0,length = strArr.Length; i < length; i++)
            {
                intArr[i] = strArr[i].ToInt();
            }
            return intArr;
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        public static string FirstToUpper(this string strValue)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                return strValue;
            }

            return strValue.Substring(0, 1).ToUpper() + strValue.Substring(1);
        }

        /// <summary>
        /// 首字母小写
        /// </summary>
        public static string FirstToLower(this string strValue)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                return strValue;
            }

            return strValue.Substring(0, 1).ToLower() + strValue.Substring(1);
        }

        /// <summary>
        /// 按字符c分割，并转化成int List
        /// </summary>
        public static List<int> SplitToIntList(this string strValue, char c)
        {
            List<int> list = new List<int>();
            foreach (string item in strValue.Split(c))
            {
                if (!item.IsNullOrEmpty())
                {
                    list.Add(int.Parse(item));
                }
            }
            return list;
        }

        /// <summary>
        /// 按字符c分割，并转化成long List
        /// </summary>
        public static List<long> SplitToLongList(this string strValue, char c)
        {
            List<long> list = new List<long>();
            foreach (string item in strValue.Split(c))
            {
                if (!item.IsNullOrEmpty())
                {
                    list.Add(long.Parse(item));
                }
            }
            return list;
        }

        /// <summary>
        /// 按字符c分割，并转化成float List
        /// </summary>
        public static List<float> SplitToFloatList(this string strValue, char c)
        {
            List<float> list = new List<float>();
            if (!strValue.IsNullOrEmpty())
            {
                foreach (string item in strValue.Split(c))
                {
                    if (!item.IsNullOrEmpty())
                    {
                        list.Add(float.Parse(item));
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 按字符c分割，并转化成double List
        /// </summary>
        public static List<double> SplitToDoubleList(this string strValue, char c)
        {
            List<double> list = new List<double>();
            foreach (string item in strValue.Split(c))
            {
                if (!item.IsNullOrEmpty())
                {
                    list.Add(double.Parse(item));
                }
            }
            return list;
        }

        /// <summary>
        /// 是否为null或者empty
        /// </summary>
        public static bool IsNullOrEmpty(this string strValue)
        {
            return string.IsNullOrEmpty(strValue);
        }

        public static string ToJson(this string str)
        {
            return JsonMapper.ToJson(str);
        }
    }
}
