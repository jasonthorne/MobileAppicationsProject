using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplicationsProject
{
    public class FindCards
    {
       

        public static async Task<RootObject> GetCardData(string httpCardName)  
        {

            var http = new HttpClient(); //set up client

            var response = await http.GetAsync("https://api.deckbrew.com/mtg/cards/" + httpCardName); //get response

            var jsonMsg = await response.Content.ReadAsStringAsync(); //read in string

            var serializer = new DataContractJsonSerializer(typeof(RootObject)); //deserialize json into classes

            var mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonMsg)); //memory stream of json bytes for deserializer
            var result = (RootObject)serializer.ReadObject(mStream);

            return result; //return result object

        }
    }

    
     
   
    public class Formats
    {
        public string commander { get; set; }

        public string legacy { get; set; }

        public string modern { get; set; }

        public string vintage { get; set; }

        public string standard { get; set; }
    }

   
    public class Price
    {
        public int low { get; set; }

        public int median { get; set; }

        public int high { get; set; }
    }

    public class Edition
    {
        public string set { get; set; }

        public string set_id { get; set; }

        public string rarity { get; set; }

        public string artist { get; set; }

        public int multiverse_id { get; set; }

        public string flavor { get; set; }

        public string number { get; set; }

        public string layout { get; set; }

        public Price price { get; set; }

        //public string url { get; set; }

        public string image_url { get; set; }

        public string set_url { get; set; }

        public string store_url { get; set; }

        public string watermark { get; set; }
    }

    public class RootObject
    {
        public string name { get; set; }

        public string id { get; set; }

        public string url { get; set; }

        public string store_url { get; set; }

        public List<string> types { get; set; }

        public List<string> subtypes { get; set; }

        public List<string> colors { get; set; }

        public int cmc { get; set; }

        public string cost { get; set; }

        public string text { get; set; }

        public string power { get; set; }

        public string toughness { get; set; }

        public Formats formats { get; set; }

        public List<Edition> editions { get; set; }

        public List<string> supertypes { get; set; }
    }
   

}
