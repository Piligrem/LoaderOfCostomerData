using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoaderOfCostomerData
{
    public class Captcha
    {
        public string TextCapcha { get; set; }
        public string IdCaptcha { get; set; }

        public Captcha(string id)
        {
            IdCaptcha = id;
        }
    }
}
