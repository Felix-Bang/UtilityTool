// =============================================================================
// File      : CryptographyTool.cs
// Author    : 
// Create    : 2017-00-00 00:00
// Copyright : Copyright (c) 2014-2017 fsnmt.com, All rights reserved.   
// 功能说明  :   
// =============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace FelixBangTools
{
    /// <summary>
    /// 加密和解密
    /// </summary>
    public class CryptographyTool
    {
        /// 密钥。
        private const string _ENCRYPT_KEY = "";
        private const string _ENCRYPT_IV = "";

        #region MD5
        /// <summary>
        /// MD5加密（返回32位加密串）（默认小写）。
        /// </summary>
        /// <param name="input">需要加密的字符串。</param>
        /// <returns>返回已经加密好的字符串。</returns>
        public static string EncryptMD5(string input)
        {
            return EncryptMD5(input, new UTF8Encoding());
        }

        /// <summary>
        /// MD5加密（返回32位加密串）（默认小写）。
        /// </summary>
        /// <param name="input">需要进行加密的值。</param>
        /// <param name="encoding">编码格式。</param>
        /// <returns>返回已经加密好的字符串。</returns>
        public static string EncryptMD5(string input, Encoding encoding, bool lowercase = true)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(encoding.GetBytes(input));
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                if (lowercase)
                    sb.Append(bytes[i].ToString("x2"));
                else
                    sb.Append(bytes[i].ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// MD5加密（返回32位加密串）（默认小写）。
        /// </summary>
        /// <param name="stream">需要进行加密的文件流。</param>
        /// <returns>返回已经加密好的字符串。</returns>
        public static string EncryptMD5(Stream stream)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(stream);
            StringBuilder sb = new StringBuilder();

            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        /// <summary>
        /// MD5加密（返回16位加密串）。
        /// </summary>
        /// <param name="input">需要进行加密的值。</param>
        /// <param name="encoding">编码格式。</param>
        /// <returns>返回16位加密串。</returns>
        public static string EncryptMD5_16(string input, Encoding encoding)
        {
            MD5 md5 = MD5.Create();
            string result = BitConverter.ToString(md5.ComputeHash(encoding.GetBytes(input)), 4, 8);
            result = result.Replace("-", "");
            return result;
        }
        #endregion

        #region DES
        /// <summary>
        /// DES加密字符串。
        /// </summary>
        /// <param name="input">待加密的字符串。</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源字符串。</returns>
        public static string EncryptDES(string input)
        {
            return EncryptDES(input, _ENCRYPT_KEY, _ENCRYPT_IV);
        }

        /// <summary>
        /// DES加密字符串。
        /// </summary>
        /// <param name="input">待加密的字符串。</param>
        /// <param name="key">8位字符的密钥字符串。</param>
        /// <param name="iv">8位字符的初始化向量字符串。</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源字符串。</returns>
        public static string EncryptDES(string input, string key, string iv)
        {
            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
                byte[] data = Encoding.UTF8.GetBytes(input);
                DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, descsp.CreateEncryptor(keyBytes, ivBytes), CryptoStreamMode.Write);
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();
                cStream.Close();

                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return input;
            }
        }

        /// <summary>
        /// DES加密字节数组。
        /// </summary>
        /// <param name="bytes">待加密的字节数组。</param>
        /// <returns>加密成功返回加密后的字节数组，失败返回源字节数组。</returns>
        public static byte[] EncryptDES(byte[] bytes)
        {
            return EncryptDES(bytes, _ENCRYPT_KEY, _ENCRYPT_IV);
        }

        /// <summary>
        /// DES加密字节数组。
        /// </summary>
        /// <param name="bytes">待加密的字节数组。</param>
        /// <param name="key">8位字符的密钥字符串。</param>
        /// <param name="iv">8位字符的初始化向量字符串。</param>
        /// <returns>加密成功返回加密后的字节数组，失败返回源字节数组。</returns>
        public static byte[] EncryptDES(byte[] bytes, string key, string iv)
        {
            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
                byte[] data = bytes;
                DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, descsp.CreateEncryptor(keyBytes, ivBytes), CryptoStreamMode.Write);
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();
                cStream.Close();

                return mStream.ToArray();
            }
            catch
            {
                return bytes;
            }
        }

        /// <summary>
        /// DES解密字符串。
        /// </summary>
        /// <param name="input">待解密的字符串。</param>
        /// <returns>解密成功返回解密后的字符串，失败返回源字符串。</returns>
        public static string DecryptDES(string input)
        {
            return DecryptDES(input, _ENCRYPT_KEY, _ENCRYPT_IV);
        }

        /// <summary>
        /// DES解密字符串。
        /// </summary>
        /// <param name="input">待解密的字符串。</param>
        /// <param name="key">8位字符的密钥字符串（需要和加密时相同）。</param>
        /// <param name="iv">8位字符的初始化向量字符串（需要和加密时相同）。</param>
        /// <returns>解密成功返回解密后的字符串，失败返回源字符串。</returns>
        public static string DecryptDES(string input, string key, string iv)
        {
            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
                byte[] data = Convert.FromBase64String(input);
                DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, descsp.CreateDecryptor(keyBytes, ivBytes), CryptoStreamMode.Write);
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();
                cStream.Close();

                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return input;
            }
        }

        /// <summary>
        /// DES解密字节。
        /// </summary>
        /// <param name="bytes">待解密的字节数组。</param>
        /// <returns>解密成功返回解密后的字节数组，失败返回源字节数组。</returns>
        public static byte[] DecryptDES(byte[] bytes)
        {
            return DecryptDES(bytes, _ENCRYPT_KEY, _ENCRYPT_IV);
        }

        /// <summary>
        /// DES解密字节。
        /// </summary>
        /// <param name="bytes">待解密的字节数组。</param>
        /// <param name="key">8位字符的密钥字符串（需要和加密时相同）。</param>
        /// <param name="iv">8位字符的初始化向量字符串（需要和加密时相同）。</param>
        /// <returns>解密成功返回解密后的字节数组，失败返回源字节数组。</returns>
        public static byte[] DecryptDES(byte[] bytes, string key, string iv)
        {
            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
                byte[] data = bytes;
                DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, descsp.CreateDecryptor(keyBytes, ivBytes), CryptoStreamMode.Write);
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();
                cStream.Close();

                return mStream.ToArray();
            }
            catch
            {
                return bytes;
            }
        }
        #endregion

        #region Base64
        /// <summary>
        /// Base64加密。
        /// </summary>
        /// <param name="input">需要加密的字符串。</param>
        /// <returns>返回加密字符串。</returns>
        public static string EncryptBase64(string input)
        {
            return EncryptBase64(input, new UTF8Encoding());
        }

        /// <summary>
        /// Base64加密。
        /// </summary>
        /// <param name="input">需要加密的字符串。</param>
        /// <param name="encoding">字符编码。</param>
        /// <returns>返回加密字符串。</returns>
        public static string EncryptBase64(string input, Encoding encoding)
        {
            return Convert.ToBase64String(encoding.GetBytes(input));
        }

        public static string EncryptBase64(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// Base64解密。
        /// </summary>
        /// <param name="input">需要解密的字符串。</param>
        /// <returns>返回解密字符串。</returns>
        public static string DecryptBase64(string input)
        {
            return DecryptBase64(input, new UTF8Encoding());
        }

        /// <summary>
        /// Base64解密。
        /// </summary>
        /// <param name="input">需要解密的字符串。</param>
        /// <param name="encoding">字符的编码。</param>
        /// <returns>返回解密字符串。</returns>
        public static string DecryptBase64(string input, Encoding encoding)
        {
            return encoding.GetString(DecryptBase64ToBytes(input));
        }

        public static byte[] DecryptBase64ToBytes(string input)
        {
            return Convert.FromBase64String(input);
        }
        #endregion
    }

}
