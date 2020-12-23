using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Gigya.Socialize.SDK;

namespace KCM.ServiciosInternet.Plugins
{
    class Program
    {
        static void Main(string[] args)
        {

            //var key = new byte[32];
            //using (var generator = RandomNumberGenerator.Create())
            //    generator.GetBytes(key);
            //string apiKeys = Convert.ToBase64String(key);

            string apiKey = "3_tZ5kn2zQOZSMJ2firwgDhbob4CbC7wsA26bGt3IRct2imQ6q4fw4zYE5Z0G3Gaj6";
            string secretKey = "JHDvUwIrggNYQUhasE5edz8Z4WyH9zWWfhLrvDjbU00=";
            string userKey;
            string method = "accounts.login";
            GSRequest request  = new GSRequest(apiKey, secretKey, method, true);

            request.SetParam("loginID", "jorge_kain@yahoo.com");
            request.SetParam("password", "Atend2016");

            GSResponse response  = request.Send();

            if (response.GetErrorCode() == 0)
            {
                // Everything's okay
                GSObject resObj = response.GetData();

                // Do something with the data
            }
            else
            {
                Console.WriteLine("Uh-oh, we got the following error:{0}", response.GetLog());
            }

            secretKey = "lVQan+TL/DJRy4d1A+SGP5zyJYpo8BLN";
            userKey = "AJQdGovWswIy";

            method = "accounts.getAccountInfo";
            request = new GSRequest(apiKey, secretKey, method, null, true, userKey);

            request.SetParam("UID", "d9f65b81a01844c6bc8e71f0f8ca21cf");
            request.SetParam("include", "loginIDs, profile, data, lastLoginLocation");
            //request.SetParam("extraProfileFields", "languages,address,phones, education, honors, publications,  patents, certifications, professionalHeadline, bio, industry, specialties, work, skills, religion, politicalView, interestedIn, relationshipStatus, hometown, favorites, followersCount, followingCount, username, locale, verified, timezone, likes, samlData");

            response = request.Send();

            if (response.GetErrorCode() == 0)
            {
                // Everything's okay
                GSObject resObj = response.GetData();

                // Do something with the data
            }
            else
            {
                Console.WriteLine("Uh-oh, we got the following error:{0}", response.GetLog());
            }
            var dedi1 = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(response.GetData().GetString("profile"), new { email = " "});

        }
    }
}
