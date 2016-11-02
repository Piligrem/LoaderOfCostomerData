using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoaderOfCostomerData
{
    class Owner
    {
       public Owner(bool _getOwner)
        {
            if (_getOwner)
            {
                Owner res;
                using (StreamReader sr = new StreamReader(Properties.Resources.ResourceManager.GetStream("OwnerRes.res")))
                {
                    string resJSON = sr.ReadToEnd();
                    res = JsonConvert.DeserializeObject<Owner>(resJSON);
                }
                Name = res.Name;
                Bin = res.Bin;
            }
        }

        public Owner(string _name, string _bin)
        {
            Name = _name;
            Bin = _bin;
        }

        public Owner()
        { }
        string Name { get; set; }

        string Bin { get; set; }
    }
    class Request
    {
       public Owner RequestOwner { get; set; }
       public string Name { get; set; }
       public string BIN { get; set; }
    }
}
