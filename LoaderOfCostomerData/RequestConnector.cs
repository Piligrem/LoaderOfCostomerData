
using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;


namespace LoaderOfCostomerData
{
    public class RequestConnector
    {
        private static readonly HttpClient client = new HttpClient();
        private string uid { get; set; }
        string t { get; set; }

        public RequestConnector()
        {
        }
        string generateUUID()
        {
            return System.Guid.NewGuid().ToString();
        }
 
        public Captcha GetCaptcha()
        {
            t = generateUUID();
            uid = generateUUID();

            Captcha captcha = new Captcha(uid);

            CookieContainer reqCookies = new CookieContainer();
            Cookie cookie = new Cookie();
            //Пример получения исходного кода сайта
            string url = "http://kgd.gov.kz/apps/services/CaptchaWeb/generate?uid=" + uid + "&t=" + t + "";
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(url);
            
          //  request1.Accept = "image /webp,image/*,*/*;q=0.8";
          /*  request1.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            request1.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
            request1.KeepAlive = true;
            request1.Host = "www.kgd.gov.kz";*/
            request1.CookieContainer = reqCookies;
            request1.Referer = "http://www.kgd.gov.kz/ru/services/taxpayer_search";
            /*  request1.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";*/
            request1.KeepAlive = true;
            request1.Method = "GET";
            
            HttpWebResponse response = (HttpWebResponse)request1.GetResponse();
           
            if ((response.StatusCode == HttpStatusCode.OK ||
                 response.StatusCode == HttpStatusCode.Moved ||
                 response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {
                // if the remote file was found, download oit
                using (Stream inputStream = response.GetResponseStream())
                {
                    byte[] buffer = new byte[16 * 1024];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        using (var capForm = new CaptchaForm(ms.ToArray()))
                        {
                            var resultCapcha = capForm.ShowDialog();
                            if (resultCapcha == DialogResult.OK)
                            {
                                captcha.TextCapcha = captcha.TextCapcha;
                            }
                        }
                    }
                }
            }
          
            return captcha;
        }
        public async Task<string> GetData(Captcha captcha)
        {
            string result = "";

          

            var values = new Dictionary<string, string>
            {
                { "thing1", "hello" },
                { "thing2", "world" }
                };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(new HttpRequestMessage("http://kgd.gov.kz/ru/system/ajax"), content);

            var responseString = await response.Content.ReadAsStringAsync();


            /*       CookieContainer reqCookies = new CookieContainer();
                       Cookie cookie = new Cookie();
                       //Пример получения исходного кода сайта
                       string url = "http://kgd.gov.kz/ru/system/ajax";
                       HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                       request.CookieContainer = reqCookies;
                       request.Referer = "http://www.kgd.gov.kz/ru/services/taxpayer_search";
                       request.KeepAlive = true;
                       request.Method = "POST";

                       HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                       if ((response.StatusCode == HttpStatusCode.OK ||
                            response.StatusCode == HttpStatusCode.Moved ||
                            response.StatusCode == HttpStatusCode.Redirect) &&
                           response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                       {
                           // if the remote file was found, download oit
                           using (Stream inputStream = response.GetResponseStream())
                           {
                               byte[] buffer = new byte[16 * 1024];
                               using (MemoryStream ms = new MemoryStream())
                               {
                                   int read;
                                   while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                                   {
                                       ms.Write(buffer, 0, read);
                                   }
                                   using (var capForm = new CaptchaForm(ms.ToArray()))
                                   {
                                       var resultCapcha = capForm.ShowDialog();
                                       if (resultCapcha == DialogResult.OK)
                                       {
                                           captcha.TextCapcha = captcha.TextCapcha;
                                       }
                                   }
                               }
                           }
                       }
                       */
            return "";
        }

    }
}