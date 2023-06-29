/*********************************************
 * BFramework
 * JsonData扩展类
 * 创建时间：2023/05/17 10:34:24
 *********************************************/
using LitJson;

namespace Framework
{
    /// <summary>
    /// JsonData扩展类
    /// </summary>
    public static class Ex_JsonData
    {
        public static int ToInt(this JsonData jsonData, int defaultValue = 0)
        {
            if (jsonData == null)
            {
                return defaultValue;
            }

            return jsonData.ToString().ToInt();
        }

        public static long ToLong(this JsonData jsonData, long defaultValue = 0)
        {
            if (jsonData == null)
            {
                return defaultValue;
            }

            return jsonData.ToString().ToLong();
        }

        public static double ToDouble(this JsonData jsonData, double defaultValue = 0)
        {
            if (jsonData == null)
            {
                return defaultValue;
            }

            return jsonData.ToString().ToDouble();
        }

        public static double ToFloat(this JsonData jsonData, float defaultValue = 0)
        {
            if (jsonData == null)
            {
                return defaultValue;
            }

            return jsonData.ToString().ToFloat();
        }

        public static bool ToBool(this JsonData jsonData, bool defaultValue = false)
        {
            if (jsonData == null)
            {
                return defaultValue;
            }
            bool value = defaultValue;
            bool.TryParse(jsonData.ToString(), out value);
            return value;
        }

        public static bool TryToValue(this JsonData jsonData, string key, out JsonData value)
        {
            value = null;
            if (jsonData.Keys.Contains(key))
            {
                value = jsonData[key];
                return true;
            }
            return false;
        }

        public static bool TryToInt(this JsonData jsonData, string key, out int value)
        {
            value = 0;
            if (jsonData.Keys.Contains(key))
            {
                value = jsonData[key].ToString().ToInt();
                return true;
            }
            return false;
        }

        public static bool TryToLong(this JsonData jsonData, string key, out long value)
        {
            value = 0;
            if (jsonData.Keys.Contains(key))
            {
                value = jsonData[key].ToString().ToLong();
                return true;
            }
            return false;
        }

        public static bool TryToDouble(this JsonData jsonData, string key, out double value)
        {
            value = 0;
            if (jsonData.Keys.Contains(key))
            {
                value = jsonData[key].ToString().ToDouble();
                return true;
            }
            return false;
        }

        public static bool TryToFloat(this JsonData jsonData, string key, out float value)
        {
            value = 0;
            if (jsonData.Keys.Contains(key))
            {
                value = jsonData[key].ToString().ToFloat();
                return true;
            }
            return false;
        }
    }
}
