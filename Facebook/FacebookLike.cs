using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace Facebook
{
    public class FacebookLike
    {
        private static string GetWebText(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = "A .NET Web Crawler";
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string htmlText = reader.ReadToEnd();
            return htmlText;
        }
        public static void PrintLikeCount(string pageUrl)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jsonCode = GetWebText(pageUrl);
            dynamic dy_item = serializer.Deserialize<object>(jsonCode);
            for (int i = 0; i < dy_item["posts"]["data"].Length; i++) {
                Console.WriteLine("Like = "+dy_item["posts"]["data"][i]["likes"]["summary"]["total_count"]);
            }
            
        }

    }
}
