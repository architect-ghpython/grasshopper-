using System;

public class Class1
{
	public Class1()
	{
		string url = 'http://fanyi.youdao.com/translate?smartresult=dict&smartresult=rule';

        public static MemoryStream Http(string i)
        {
            WebClient d = new WebClient();
            //  i = "https://www.baidu.com";
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(i);
            Request.
            // Request.Timeout = 20 * 1000;//请求超时。
            // Request.AllowAutoRedirect = true; //网页自动跳转。
            Request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36";//伪装成谷歌爬虫。
                                                                                                                                                      //  Request.Headers.Add("Referer", "https://www.amap.com");
            Request.Referer = "https: // fanyi.youdao.com /";

         

           // Request.Method = "GET"; //获取数据的方法。GET
            Request.Method = "POST";//POST
           Request.ContentType = "application/json";上传的格式JSON
            Request.KeepAlive = true; //保持
        
            HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
            Stream s = Response.GetResponseStream();

            var memoryStream = new MemoryStream();
            //将基础流写入内存流
            // int bufferLength = (int)100000000;
            //  //MessageBox.Show(bufferLength.ToString());
            // byte[] buffer = new byte[bufferLength];
            /// int actual = s.Read(buffer, 0, bufferLength);
            //  //MessageBox.Show(actual.ToString());
            //  if (actual > 0)
            //  {
            //       memoryStream.Write(buffer, 0, actual);
            //   }

            const int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int count = 0;
            while ((count = s.Read(buffer, 0, bufferLen)) > 0)
            {
                memoryStream.Write(buffer, 0, count);
            }


            memoryStream.Position = 0;
            Response.Close();

            return memoryStream;

        }


    }
}
