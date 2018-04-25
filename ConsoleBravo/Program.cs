using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBravo
{
    class Program
    {
        static void Main(string[] args)
        {

            string articul = System.IO.File.ReadAllText(@"articul.txt", Encoding.Default);//Копируем нажные артикулы в фаил 

            articul = articul.Replace(" ", "");//убираем пробелы
            articul = articul.Replace("\r\n", ","); //переносы строк - заменяем на символ


            String value = articul;            Char delimiter = ',';

            String[] substrings = value.Split(delimiter);            // Console.WriteLine(substrings.Length);


            /*
            // Get the object used to communicate with the server.  
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://85.235.48.6:5251");
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            // This example assumes the FTP site uses anonymous logon.  
            request.Credentials = new NetworkCredential("bravo_guest", "Veseluha");
            */
            String[] fibarray = value.Split(delimiter);            foreach (string element in fibarray)            {

                FtpWebRequest frpWebRequest = (FtpWebRequest)FtpWebRequest.Create("ftp://85.235.48.6:5251/НОВИНКИ/" + element + ".jpg");
                frpWebRequest.Credentials = new NetworkCredential("bravo_guest", "Veseluha");
                frpWebRequest.KeepAlive = true;
                frpWebRequest.UsePassive = true;
                frpWebRequest.UseBinary = true;
                frpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                FtpWebResponse response = (FtpWebResponse)frpWebRequest.GetResponse();
                Stream stream = response.GetResponseStream();
                List<byte> list = new List<byte>();
                int b;
                while ((b = stream.ReadByte()) != -1)
                    list.Add((byte)b);
                File.WriteAllBytes("img/Б"+ element + ".jpg", list.ToArray());

                Console.WriteLine("Картинка " + element + ".jpg скачана");
                stream.Close();
                response.Close();
            }

            Console.ReadLine();
        }
    }
}
