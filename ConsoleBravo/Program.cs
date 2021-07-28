using System;
using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace brav_o.ru
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            string articul = System.IO.File.ReadAllText(@"articul.txt", Encoding.Default);//Копируем нужные артикулы из фаила 
                                                                                          // доработать - в место строки использовать массив
            articul = articul.Replace("Р‘", "Б");// костыльно убираем Б
            articul = articul.Replace("Б", "");// костыльно убираем Б
            articul = articul.Replace(" ", "");//убираем пробелы
            articul = articul.Replace("\r\n", ","); //переносы строк - заменяем на символ
            Console.WriteLine(articul);

            String value = articul;
            Char delimiter = ',';

            String[] substrings = value.Split(delimiter);
            //Console.WriteLine(substrings.Length);

            string Filepath = @"Text.csv";

            var wc = new WebClient();

            String[] fibarray = value.Split(delimiter);

            foreach (string element in fibarray)
            {
                var html = @"https://brav-o.ru/catalog/?q=" + element;
                HtmlWeb web = new HtmlWeb();
                var htmlDoc = web.Load(html);

                    try
                    {
                    //*[@id="bx_3966226736_17220_362ce596257894d11ab5c1d73d13c755"]/div/div[2]/div[1]/div[1]
                    foreach (HtmlNode node2 in htmlDoc.DocumentNode.SelectNodes("//*[@class='items-row']/div[1]/div[1]/div[1]/div[1]/a[1]"))
                        {

                            string value2 = node2.OuterHtml;


                            string[] mystring = value2.Split('"');

                            Console.WriteLine(mystring[1]);
                            var html2 = @"https://brav-o.ru" + mystring[1];
                            HtmlWeb web2 = new HtmlWeb();
                            var htmlDoc2 = web.Load(html2);

                            foreach (HtmlNode node3 in htmlDoc2.DocumentNode.SelectNodes("//*[@class='desc-box__txt']"))
                            {
                               

                                string testTar = node3.InnerText;

                                testTar = testTar.Replace("&#37;", "%"); //переносы строк - заменяем на символ
                                testTar = testTar.Replace("&quot;", "\""); //переносы строк - заменяем на символ
                                testTar = testTar.Replace("&nbsp;", " "); //переносы строк - заменяем на символ
                                testTar = testTar.Replace("\n", " "); //переносы строк - заменяем на символ
                                testTar = testTar.Replace("\r", " "); //переносы строк - заменяем на символ
                                testTar = testTar.Trim();


                            Console.WriteLine(element);
                                Console.WriteLine(testTar);

                                if (node3.InnerText != "")
                                {
                                    //File.AppendAllText(Filepath, node3.InnerText + '\n', Encoding.UTF8);

                                    string first = "\"" + element + "\"";
                                    string second = testTar;
                                    string csv = string.Format("{0} ; {1}\n", first, second);
                                    File.AppendAllText(Filepath, csv);

                                }
                                else
                                {
                                    string first = element;
                                    string second = " ";
                                    string csv = string.Format("{0} ; {1}\n", first, second);
                                    File.AppendAllText(Filepath, csv);
                                }
                            }
                        }
                    }
                    catch
                    {
                        string first = "\"" + element + "\"";
                        string second = "";
                        string csv = string.Format("{0} ; {1}\n", first, second);
                        File.AppendAllText(Filepath, csv);
                    }
                }
            System.Console.WriteLine();
            Console.Read();
        }
    }
}
