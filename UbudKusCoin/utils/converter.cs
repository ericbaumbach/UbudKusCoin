using System.Text;
using Models;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Utils
{
    public static class Converter
    {

        public static byte[] DoSerialize(this object obj)
        {
            if(obj == null){
                return null;
            }

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
   
        public static object DoDeSerialize(this byte[] bytes)
        {
            try
            {
                var ms = new MemoryStream(bytes);
                var bf = new BinaryFormatter();
                ms.Seek(0, SeekOrigin.Begin);
                //ms.Position = 0;
                return bf.Deserialize(ms);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occur: {0}", ex.ToString());
            }

            return null;
        }

    
        /**
        Convert array of byte to string 
        */
        public static string ConvertToString(this byte[] arg)
        {
            return System.Text.Encoding.UTF8.GetString(arg, 0, arg.Length);
        }

        /**
        Convert string to array of byte
         */
        public static byte[] ConvertToBytes(this string arg)
        {
            return System.Text.Encoding.UTF8.GetBytes(arg);
        }

        public static byte[] ConvertToByte(this Transaction[] lsTrx)
        {
            var transactionsString = Newtonsoft.Json.JsonConvert.SerializeObject(lsTrx);
            return transactionsString.ConvertToBytes();
        }

        public static string ConvertToString(this Transaction[] lsTrx)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(lsTrx);
        }

        /*
        public static void PrintToDisplay(this byte[] arr)
        {
            Console.WriteLine(" byte lenght {0}", arr.Length);
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write("{0}-{1}", arr[i], "-");
            }
            Console.WriteLine();
        }
        */

        public static string ConvertToHexString(this byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] ConvertHexStringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }


        public static string ConvertToDateTime(this Int64 timestamp)
        {

            DateTime myDate = new DateTime(timestamp);
            var strDate = myDate.ToString("dd MMM yyyy hh:mm:ss");

            return strDate;

        }


    }

}