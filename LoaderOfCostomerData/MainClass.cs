using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public string GetData(string companyInfo)
        {
            var result = "";
            var structCompanyInfo = JsonConvert.DeserializeObject<Request>(companyInfo);
            
            return result;

        }
    }
}
