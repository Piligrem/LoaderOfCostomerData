using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;

public enum HttpVerb
{
    GET,
    POST,
    PUT,
    DELETE
}

namespace LoaderOfCostomerData
{
    public class RestClient
    {
        private static readonly HttpClient client = new HttpClient();

        public string EndPoint { get; set; }
        public HttpVerb Method { get; set; }
        public string ContentType { get; set; }
        public string PostData { get; set; }
        public string Referer { get; set; }

        public RestClient()
        {
            EndPoint = "";
            Method = HttpVerb.GET;
            ContentType = "text/xml";
            PostData = "";
            Referer = "http://kgd.gov.kz/ru/services/taxpayer_search";
        }
        public RestClient(string endpoint)
        {
            EndPoint = endpoint;
            Method = HttpVerb.GET;
            ContentType = "text/xml";
            PostData = "";
            Referer = "http://kgd.gov.kz/ru/services/taxpayer_search";
        }
        public RestClient(string endpoint, HttpVerb method)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = "text/xml";
            PostData = "";
            Referer = "http://kgd.gov.kz/ru/services/taxpayer_search";
        }

        public RestClient(string endpoint, HttpVerb method, string postData)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = "text/xml";
            PostData = postData;
            Referer = "http://kgd.gov.kz/ru/services/taxpayer_search";
        }

        public RestClient(string endpoint, HttpVerb method, string postData, string referer)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = "text/xml";
            PostData = postData;
            Referer = referer;
        }

        public Captcha GetCaptcha(string uid)
        {
            Captcha captcha = new Captcha(uid);

            CookieContainer reqCookies = new CookieContainer();
            Cookie cookie = new Cookie();
            //Пример получения исходного кода сайта
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(EndPoint);

            request1.CookieContainer = reqCookies;
            request1.Referer = "http://www.kgd.gov.kz/ru/services/taxpayer_search";

            request1.KeepAlive = true;
            request1.Method = Method.ToString();

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

        public Captcha GetData(string uid)
        {
            Captcha captcha = new Captcha(uid);

            CookieContainer reqCookies = new CookieContainer();
            Cookie cookie = new Cookie();
            //Пример получения исходного кода сайта
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(EndPoint);

            request1.CookieContainer = reqCookies;
            request1.Referer = "http://www.kgd.gov.kz/ru/services/taxpayer_search";

            request1.KeepAlive = true;
            request1.Method = Method.ToString();

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

    }
}