using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KCM.ServiciosInternet.Gigya.Services;

namespace KCM.ServiciosInternet.Plugins.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Bussiness.resetPassword(new Entities.Gigya.ResetPassWordData());
        }
    }
}
