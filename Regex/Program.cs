using System.Text.RegularExpressions;
using System.Xml;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("https://www.lupa.cz/rss/clanky/");
            foreach(XmlNode item in xDoc.SelectNodes("/rss/channel/item"))
            {   
                // nesmi byt lomitko na zacatku
                string title  = item.SelectSingleNode("title/text()").Value;
                Console.WriteLine(title);
                string publicationDate = item.SelectSingleNode("pubDate/text()").Value;
                Console.WriteLine(publicationDate);
                Console.WriteLine();
            }   

            /*
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("test.xml");

            foreach(XmlNode txtnode in xDoc.SelectNodes("/customerList/customer/name/text()"))
            {
                Console.WriteLine(txtnode.Value);
                txtnode.ParentNode.ParentNode.RemoveChild(txtnode.ParentNode);
            }
            xDoc.Save("text3.xml");
            */

            /*
            Console.WriteLine(xDoc.DocumentElement.Name);
            Console.WriteLine(xDoc.DocumentElement.ChildNodes[1].Name);
            Console.WriteLine(xDoc.DocumentElement.ChildNodes[1].Attributes["id"].Value);

            Console.WriteLine(xDoc.DocumentElement.ChildNodes[1].ChildNodes[0].FirstChild.Value);


            xDoc.DocumentElement.RemoveChild(xDoc.DocumentElement.ChildNodes[1]);
            xDoc.Save("text2.xml");
            */

            /*
            XmlDocument xDoc = new XmlDocument();

            xDoc.AppendChild(xDoc.CreateXmlDeclaration("1.0","UTF-8","yes"));

            XmlNode root = xDoc.CreateElement("customerList");
            xDoc.AppendChild(root);

            for (int i = 0; i < 5; i++)
            {
                XmlNode customer = xDoc.CreateElement("customer");
                root.AppendChild(customer);

                XmlAttribute idAttr = xDoc.CreateAttribute("id");
                idAttr.Value = i.ToString();
                customer.Attributes.Append(idAttr);

                XmlNode name = xDoc.CreateElement("name");
                customer.AppendChild(name);

                name.AppendChild(xDoc.CreateTextNode("Zakaznik" + i));

                XmlNode age = xDoc.CreateElement("age");
                customer.AppendChild(age);
                name.AppendChild(xDoc.CreateTextNode((20 + i).ToString()));
            }
            xDoc.Save("test.xml");
            */

            /*
            string loginRegexStr = @"^[A-Z]{3,4}[0-9]{3,4}$";
            string emailRegexStr = @"^[a-z0-9\.\-]+@[a-z0-9\.\-]+\.[a-z]{2,}$";
            string urlRegexStr = @"^(https?):\/\/([a-z0-9]+\.)?([a-z0-9]+\.[a-z]{2,})(\/|\?|$|#)";
            string placeholderRegexStr = @"\{\s*([A-Za-z]+)\s*\}";

            Regex placeholderRegex = new Regex(placeholderRegexStr, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string text = "Ahoj {name}. Tvá objednávka „{orderName}“ v ceně {price} byla úspěšně uhrazena.";

            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "name", "Martin" },
                { "orderName", "Auto" },
                { "price", "200 000 Kč" }
            };

            string result = placeholderRegex.Replace(text, (Match match) => {
                string key = match.Groups[1].Value;
                return data[key];
            });

            Console.WriteLine(result);
            */

            /*
            Match match = urlRegex.Match(url);
            if(match.Success)
            {
                string protocol = match.Groups[1].Value;
                string subdomain = match.Groups[2].Value;
                string domain = match.Groups[3].Value;
                Console.WriteLine($"{protocol} | {subdomain} | {domain}");
            }
            else
            {
                Console.WriteLine("Neni url");
            }
            */

            /*
            while (true)
            {
                string txt = Console.ReadLine();

                if (loginRegex.IsMatch(txt))
                {
                    Console.WriteLine("Validni");
                }
                else
                {
                    Console.WriteLine("Chyba");
                }
            }
            */
        }
    }
}