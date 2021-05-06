using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCM.ServiciosInternet.Google.Services.Data
{
    public struct ResponseCaptcha
    {
        public bool isSuccess { get; set; }
        public DateTime dtChallenge_ts { get; set; }
        public string hostname { get; set; }
        public string[] errorCodes { get; set; }

    }
}
