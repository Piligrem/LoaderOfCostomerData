using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;


namespace LoaderOfCostomerData
{
    [Guid("B1474B61-3C89-4EDC-B258-62230E866D85")]
    internal interface IMainClass
    {   // описываем методы которые можно будет вызывать из вне
        [DispId(1)]
        string GetData(string companyInfo);
        [DispId(1)]
        string GetLicense();
    }

    //определим интерфейс для COM-событий(GUID получаем и записываем с помощью утилиты guidgen.exe)
    [Guid("20EE9585-6E09-47FB-8A53-C8F50BC7E2D4"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IEvents
    {

    }

    [Guid("0137FC8A-541D-47B7-A8B3-D4228EACDB66"), ClassInterface(ClassInterfaceType.None), ComSourceInterfaces(typeof(IEvents))]
    public class MainClass : IMainClass
    {
        private string Uid { get; set; }
        string t { get; set; }

        public MainClass()
        {
            Uid = generateUUID();
        }
        string generateUUID()
        {
            return System.Guid.NewGuid().ToString();
        }

        public string Connect(Request companyInfo, string connectionType)
        {
            t = generateUUID();
            var endPoint = "http://kgd.gov.kz";
           

            string referer = "";
            if (companyInfo.Type == 1)
                referer = "http://kgd.gov.kz/ru/services/taxpayer_search";
            else if (companyInfo.Type == 2)
                referer = "http://kgd.gov.kz/ru/services/taxpayer_search/legal_entity";
            else if (companyInfo.Type == 3)
                referer = "http://kgd.gov.kz/ru/services/taxpayer_search/entrepreneur";
            var service = "/apps/services/CaptchaWeb/generate?uid=" + Uid + "&t=" + t + "";
            RestClient rcCaptcha = new RestClient(endPoint, service, HttpVerb.GET, "", referer);
            Captcha captcha = rcCaptcha.GetCaptcha(Uid);
            service = "/ru/system/ajax";
            RestClient rcData = new RestClient(endPoint, service, HttpVerb.POST, "", referer);
            string answer = rcData.GetData(captcha, companyInfo);
            return answer;

        }

        public void Close()
        {
            //var obj1C = V7Data.V7Object.AppDispatch
            //Marshal.ReleaseComObject(Marshal.GetIDispatchForObject(obj1C));
        }
        public string GetData(string companyInfo)
        {

            var result = "";
            Request structCompanyInfo = JsonConvert.DeserializeObject<Request>(companyInfo);
            result = Connect(structCompanyInfo, "");
            return result;

        }

        public string GetLicense()
        {
            string companyInfo = "";
            using (MemoryStream outputMS = new MemoryStream(Properties.Resources.OwnerRes))
            {
                var output = new StreamReader(outputMS, Encoding.Default);
                companyInfo = output.ReadToEnd();
            }
            return companyInfo;
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
