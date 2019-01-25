using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Get Login: login=admin senha=123456");
            Console.WriteLine(Get("http://localhost:57623/desafiomb/q?method=login&login=admin&senha=123456&sessionid=123456"));
            Console.ReadKey();
            Console.WriteLine("POST");
            Console.WriteLine(Post("http://localhost:57623/desafiomb/q","test", "text/plain"));

            Console.ReadKey();
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// GET async
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static async Task<string> GetAsync(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// POST
        /// Contains the parameter method in the event you wish to use other HTTP methods such as PUT, DELETE, ETC
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string Post(string uri, string data, string contentType, string method = "POST")
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = contentType;
            request.Method = method;

            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(dataBytes, 0, dataBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// POST async
        /// Contains the parameter method in the event you wish to use other HTTP methods such as PUT, DELETE, ETC
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string uri, string data, string contentType, string method = "POST")
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = contentType;
            request.Method = method;

            using (Stream requestBody = request.GetRequestStream())
            {
                await requestBody.WriteAsync(dataBytes, 0, dataBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
