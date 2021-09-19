using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

public class Class1
{

        public static string Post(string word)
        {
       string  url = "https://fanyi.youdao.com/translate?smartresult=dict&smartresult=rule";
        Dictionary<string, string> dic = new Dictionary<string, string>();
       string  lts =Convert.ToString( new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds());


        string salt = lts + Convert.ToString(new Random().Next(0, 9));

        string tempstr = "fanyideskweb" + word + salt + "Y2FYu%TNSbMCxc3t2u^XT";
        string pwd = "";
        MD5 md5 = MD5.Create();//实例化一个md5对像
                               // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(tempstr));
        // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
        for (int ii = 0; ii < s.Length; ii++)
        {
            // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
            pwd = pwd + s[ii].ToString("x");

        }

        string sign = pwd;
        dic.Add("i",word);

          dic.Add(  "from", "AUTO");
        dic.Add(            "to", "AUTO");
        dic.Add(            "smartresult", "dict");
        dic.Add(            "client", "fanyideskweb");
        dic.Add(            "salt", salt);
        dic.Add(            "sign", sign);
        dic.Add(            "lts", lts);
        dic.Add(            "bv", "89e18957825871c419be045180c67d3b");
        dic.Add(            "doctype", "json");
        dic.Add(            "version", "2.1");
        dic.Add(            "keyfrom", "fanyi.web");
        dic.Add(            "action", "FY_BY_CLICKBUTTION");       










    string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
         //  req.CookieContainer.SetCookies(new Uri(url),"OUTFOX_SEARCH_USER_ID=-676104602@10.108.160.100");

        CookieContainer cc = new CookieContainer();


        cc.Add(new Uri(url), new Cookie("OUTFOX_SEARCH_USER_ID", "-676104602@10.108.160.100"));
        req.CookieContainer = cc;
        req.ContentType = "application/x-www-form-urlencoded";
            req.UserAgent = "Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 74.0.3724.8 Safari / 537.36";

            #region 添加Post 参数
        StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }

        string encString=result;
        Regex reg = new Regex("tgt\":\"(.*?)\"}");

        var result2 = reg.Match(encString).Groups;
        Thread.Sleep(new Random().Next(100, 200));
        string res=null;
        foreach (var item in result2)

        {
            string filter = item.ToString();
             res = filter.Replace("tgt\":\"", "").Replace(",,", ",");

            res = res.Replace("\"}", "").Replace(",,", ",");
           
        }
            return res;
        }

    public string md5(string input)
    {
        // Use input string to calculate MD5 hash
        MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        // Convert the byte array to hexadecimal string
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("X2"));
            // To force the hex string to lower-case letters instead of
            // upper-case, use he following line instead:
            // sb.Append(hashBytes[i].ToString("x2")); 
        }
        return sb.ToString();
    }
    public static string Main(string q,string appId, string secretKey)
    {
        // 原文
        //string q = "apple";
        // 源语言
        string from = "en";
        // 目标语言
        string to = "zh";
        // 改成您的APP ID
      //  string appId = "2015063000000001";
        Random rd = new Random();
        string salt = rd.Next(100000).ToString();
        // 改成您的密钥
      //  string secretKey = "12345678";
        string sign = EncryptString(appId + q + salt + secretKey);
        string url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
        url += "q=" + HttpUtility.UrlEncode(q);
        url += "&from=" + from;
        url += "&to=" + to;
        url += "&appid=" + appId;
        url += "&salt=" + salt;
        url += "&sign=" + sign;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.ContentType = "text/html;charset=UTF-8";
        request.UserAgent = null;
        request.Timeout = 6000;
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream myResponseStream = response.GetResponseStream();
        StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
        string retString = myStreamReader.ReadToEnd();
        string[] sArray = Regex.Split(retString, "dst\":\"", RegexOptions.IgnoreCase);
        string hhh="";
        
        for (int yyy=1; yyy<sArray.Length;yyy++)
        {
            string dj = Regex.Split(sArray[yyy], "\"}", RegexOptions.IgnoreCase)[0];
            dj = dj.Replace("\"", "");
            UTF8Encoding utf8 = new UTF8Encoding();
            Byte[] encodedBytes = utf8.GetBytes(dj);
            String decodedString = DeCode(utf8.GetString(encodedBytes));
            hhh += decodedString; 
            if (yyy < sArray.Length - 1)
            {
                hhh += "\n";
            }
        }
        myStreamReader.Close();
        myResponseStream.Close();
        Console.WriteLine(hhh);
        return hhh;
        Console.WriteLine(hhh);
        Console.WriteLine(retString);
        Console.ReadLine();
    }

    public static string DeCode(string str)
    {
        var regex = new Regex(@"\\u(\w{4})");

        string result = regex.Replace(str, delegate (Match m)
        {
            string hexStr = m.Groups[1].Value;
            string charStr = ((char)int.Parse(hexStr, System.Globalization.NumberStyles.HexNumber)).ToString();
            return charStr;
        });

        return result;
    }


        // 计算MD5值
        public static string EncryptString(string str)
    {
        MD5 md5 = MD5.Create();
        // 将字符串转换成字节数组
        byte[] byteOld = Encoding.UTF8.GetBytes(str);
        // 调用加密方法
        byte[] byteNew = md5.ComputeHash(byteOld);
        // 将加密结果转换为字符串
        StringBuilder sb = new StringBuilder();
        foreach (byte b in byteNew)
        {
            // 将字节转换成16进制表示的字符串，
            sb.Append(b.ToString("x2"));
        }
        // 返回加密的字符串
        return sb.ToString();
    }

}
