
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ServiceWebAPI
{
    /// <summary>
    /// 加密类
    /// </summary>
    public static class Encrypt
    {
        /// <summary>
        /// 默认密钥
        /// </summary>
        public const string DefaultKey = "ABCDEFGABCDEFG12ABCDEFGABCDEFG12";

        /// <summary>
        /// 获取Aes32位密钥
        /// </summary>
        /// <param name="key">Aes密钥字符串</param>
        /// <returns>Aes32位密钥</returns>
        private static byte[] GetAesKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", "Aes密钥不能为空");
            }
            if (key.Length < 32)
            {
                // 不足32补全
                key = key.PadRight(32, '0');
            }
            if (key.Length > 32)
            {
                key = key.Substring(0, 32);
            }
            return Encoding.UTF8.GetBytes(key);
        }

        /// <summary>
        /// Aes加密
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="key">aes密钥，长度必须32位</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptAes(string source, string key)
        {
            try
            {
                using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
                {
                    aesProvider.Key = GetAesKey(key);
                    aesProvider.Mode = CipherMode.ECB;
                    aesProvider.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor())
                    {
                        byte[] inputBuffers = Encoding.UTF8.GetBytes(source);
                        byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                        aesProvider.Clear();
                        aesProvider.Dispose();
                        return Convert.ToBase64String(results, 0, results.Length);
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Aes解密
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="key">aes密钥，长度必须32位</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptAes(string source, string key)
        {
            try
            {
                using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
                {
                    aesProvider.Key = GetAesKey(key);
                    aesProvider.Mode = CipherMode.ECB;
                    aesProvider.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor())
                    {
                        byte[] inputBuffers = Convert.FromBase64String(source);
                        byte[] results = cryptoTransform.TransformFinalBlock(inputBuffers, 0, inputBuffers.Length);
                        aesProvider.Clear();
                        return Encoding.UTF8.GetString(results);
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// JSON序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>JSON字符串</returns>
        public static string SerializeObject(this object obj, string dateFormatString)
        {
            try
            {
                var jsonSerializerSettings = new JsonSerializerSettings { DateFormatString = dateFormatString };
                return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// JSON序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>JSON字符串</returns>
        public static string SerializeObject(this object obj)
        {
            return SerializeObject(obj, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="json">JSON字符串</param>
        /// <returns>对象</returns>
        public static T DeserializeObject<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <returns>对象</returns>
        public static dynamic DeserializeObject(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject(json);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取一个新的Key
        /// </summary>
        /// <returns>key</returns>
        public static string GetNewKey()
        {
            return Guid.NewGuid().ToString().Replace("-", string.Empty).ToUpper();
        }

        /// <summary>
        /// 构造返回参数，自动加密、解压
        /// </summary>
        /// <param name="model">业务model</param>
        /// <param name="token">令牌</param>
        /// <returns>返回字符串</returns>
        public static string ToJson(this object model, string token)
        {
            var retjson = model.SerializeObject();
            retjson = EncryptAes(retjson, UserManager.LoginTokenDataList[token].Encryptionkey);
            return retjson;
        }
    }
}