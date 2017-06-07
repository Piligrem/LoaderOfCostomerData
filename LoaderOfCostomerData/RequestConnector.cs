
using System;
using System.Net;
using System.IO;


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
                using (Stream outputStream = File.OpenWrite(@"g:\test.jpg"))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    do
                    {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        outputStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                }
            }


            //+* Привер записи Cookies и их восстановления при последующем запросе
            //Создаем соединение (получаем капчу)
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://kgd.gov.kz/");
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            request.AllowAutoRedirect = false;

            //Создаем куки и контейнер для куки
            request.CookieContainer = new CookieContainer();
            HttpWebResponse r1 = (HttpWebResponse)request.GetResponse();

            //По очередно записываем
            foreach (Cookie c in r1.Cookies)
            {
                //tb1.Text += "\r\n Cookie:" + c;
                request.CookieContainer.Add(c);
            }

            //Второй запрос (в котором отправляем данные вместе с предыдущими Cookies)
            HttpWebRequest h2 = (HttpWebRequest)WebRequest.Create("http://kgd.gov.kz/ru/services/taxpayer_search/legal_entity");
            h2.Proxy.Credentials = CredentialCache.DefaultCredentials;
            h2.AllowAutoRedirect = false;
            h2.CookieContainer = request.CookieContainer;
            HttpWebResponse r2 = (HttpWebResponse)h2.GetResponse();
            foreach (Cookie c in r2.Cookies)
            {
                //  tb1.Text += "\r\n Cookie:" + c;
            }

            return result;
        }
    }
}