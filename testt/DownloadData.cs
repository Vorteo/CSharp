using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using testt.Models;

namespace testt
{
    public class DownloadData
    {
        public static string url = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";
        public static List<Kurz>? kurzy { get; set; }
        public static async Task<List<Kurz>> Download()
        {
            kurzy = new List<Kurz>();

            if (!File.Exists("kurzy.xml"))
            {
                using HttpClient client = new HttpClient();
                using HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);

                using HttpResponseMessage resp = await client.SendAsync(req);

                string txt = await resp.Content.ReadAsStringAsync();

                string[] data = txt.Split("\n");

                for( int i =  2; i < data.Length - 1; i++)
                {
                    string[] currency = data[i].Split("|");
                    Kurz kurz = new Kurz()
                    {
                        country = currency[0],
                        currency = currency[1],
                        count = int.Parse(currency[2]),
                        code = currency[3],
                        exchangeRate = double.Parse(currency[4])
                    };

                    kurzy.Add(kurz);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(List<Kurz>));
                TextWriter writer = new StreamWriter("kurzy.xml");
                serializer.Serialize(writer, kurzy);
                writer.Close();
            }
            else
            {    
                FileStream file = new FileStream("kurzy.xml", FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Kurz>));

                kurzy = serializer.Deserialize(file) as List<Kurz>;
            }

            return kurzy;
        }
    }
}
