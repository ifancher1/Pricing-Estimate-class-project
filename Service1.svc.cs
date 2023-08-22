using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Net;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.ServiceModel.Activation;

namespace PricingEstimate
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service1 : IService1
    {
        //private string url = "https://api.weatherapi.com/v1/current.json?key=35ef880d6ac94047a09173357222310&q=";
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        //Weather API key: 35ef880d6ac94047a09173357222310  
        public string estimate(string zip)//function to estimate the price of installation based on the weather at the location
        {
            string price = "";
            string url = "https://api.weatherapi.com/v1/current.json?key=35ef880d6ac94047a09173357222310&q=";
            string test = "no";
            string query = url + zip;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(query);
            try
            {//call the weather API to get the weather conditions at the given zip code
                WebResponse response = request.GetResponse();
                Stream tstream = response.GetResponseStream();
                StreamReader reader = new StreamReader(tstream);
                string textresult = reader.ReadToEnd();

                dynamic temp = JsonConvert.DeserializeObject(textresult);

                string weather = temp.current.condition.code;//get the condition code for the weather

                if (weather == "1000")//price if clear
                {
                    price = "$2,000";
                }

                else if (weather == "1003")//price if partly cloudy
                {
                    price = "$2500";
                }

                else if (weather == "1006")//price if cloudy
                {
                    price = "$3000";
                }

                else if (weather == "1009")//price if overcast
                {
                    price = "$3500";
                }

                else//price if weather is bad
                {
                    price = "$5000";
                }

                return price;
            }
            catch (Exception e)
            {
                test = e.Message.ToString();
                return test;
            }
        }
    }
}
