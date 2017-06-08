
using System;
using System.Net;
using System.IO;
using System.Windows.Forms;



namespace LoaderOfCostomerData
{
    public class RequestConnector
    {
        private string uid { get; set; }
        string t { get; set; }

        public RequestConnector()
        {
            uid = generateUUID();
        }
        string generateUUID()
        {
            return System.Guid.NewGuid().ToString();
        }

        public string ConnectToSource()
        {

            string result = "";
            //WebClient client = new WebClient();
            t = generateUUID();
            CookieContainer reqCookies = new CookieContainer();
            Cookie cookie = new Cookie();
            //Пример получения исходного кода сайта
            string url = "http://kgd.gov.kz/apps/services/CaptchaWeb/generate?uid=" + uid + "&t=" + t + "";
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(url);
            request1.Accept = "image /webp,image/*,*/*;q=0.8";
            request1.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
            request1.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
            request1.KeepAlive = true;
            request1.Host = "www.kgd.gov.kz";
            request1.CookieContainer = reqCookies;
            request1.Referer = "http://www.kgd.gov.kz/ru/services/taxpayer_search";
            request1.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            request1.KeepAlive = false;
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
                        using (var capForm = new Capcha(ms.ToArray()))
                        {
                            var resultCapcha = capForm.ShowDialog();
                            if (resultCapcha == DialogResult.OK)
                            {
                                result = capForm.TextCapcha;
                            }
                        }
                    }
                }

                /*      using (Stream inputStream = response.GetResponseStream())
                      using (Stream outputStream = File.OpenWrite(@"g:\test.jpg"))
                      {
                          byte[] buffer = new byte[4096];
                          int bytesRead;
                          do
                          {
                              bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                              result = buffer.ToString();
                              outputStream.Write(buffer, 0, bytesRead);
                          } while (bytesRead != 0);
                      }*/
            }


          
            return result;
        }
    }
}