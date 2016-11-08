using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace LoaderOfCostomerData
{
    [Guid("B1474B61-3C89-4EDC-B258-62230E866D85")]
    internal interface IMainClass
    {   // описываем методы которые можно будет вызывать из вне
        [DispId(1)]
        string GetData(string companyInfo);
    }

    //определим интерфейс для COM-событий(GUID получаем и записываем с помощью утилиты guidgen.exe)
    [Guid("20EE9585-6E09-47FB-8A53-C8F50BC7E2D4"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IEvents
    {

    }
    [Guid("0137FC8A-541D-47B7-A8B3-D4228EACDB66"), ClassInterface(ClassInterfaceType.None), ComSourceInterfaces(typeof(IEvents))]
    public class MainClass : IMainClass
    {

        public string Connect(string companyInfo, string connectionType)
        {
            string result = "";
            //WebClient client = new WebClient();

            //Пример получения исходного кода сайта
            string url = "http://kgd.gov.kz/ru/services/taxpayer_search/legal_entity";
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request1.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            //TexBox1.Text = sr.ReadToEnd();
            sr.Close();


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
                //tb1.Text += "\r\n Cookie:" + c;
            }
            //-*


            return result;

        }

        public string GetData(string companyInfo)
        {
            var result = "";
            var structCompanyInfo = JsonConvert.DeserializeObject<Request>(companyInfo);

            return result;

        }


    }
}

    //MSDN EXAMPLE
    //+*
    // Get the response and write cookies to isolated storage.
    //private void ReadCallback(IAsyncResult asynchronousResult)
    //{
    //    HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
    //    HttpWebResponse response = (HttpWebResponse)
    //        request.EndGetResponse(asynchronousResult);
    //    using (IsolatedStorageFile isf =
    //        IsolatedStorageFile.GetUserStoreForSite())
    //    {
    //        using (IsolatedStorageFileStream isfs = isf.OpenFile("CookieExCookies",
    //            FileMode.OpenOrCreate, FileAccess.Write))
    //        {
    //            using (StreamWriter sw = new StreamWriter(isfs))
    //            {
    //                foreach (Cookie cookieValue in response.Cookies)
    //                {
    //                    sw.WriteLine("Cookie: " + cookieValue.ToString());
    //                }
    //                sw.Close();
    //            }
    //        }
    //    }
    //}

    //private void ReadFromIsolatedStorage()
    //{
    //    using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForSite())
    //    {
    //        using (IsolatedStorageFileStream isfs =
    //           isf.OpenFile("CookieExCookies", FileMode.Open))
    //        {
    //            using (StreamReader sr = new StreamReader(isfs))
    //            {
    //                tb1.Text = sr.ReadToEnd();
    //                sr.Close();
    //            }
    //        }
    //    }
    //}
    //-*



