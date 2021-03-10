using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

using System.Threading.Tasks;

namespace POEMaps
{
    public sealed class RequestClient
    {

        private static RequestClient instance;


        public PostResult postToGetFirst100Maps(Map m)
        {

            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;




            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "C:\\Users\\Guilherme\\AppData\\Local\\Programs\\Python\\Python39\\python.exe";
            startInfo.Arguments = projectDirectory + "\\request.py " + m.searchCode + " post";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            //process.StartInfo = startInfo;

            using (Process process = Process.Start(startInfo))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.WriteLine("Result:\n" + result);

                    JObject json = JObject.Parse(result);
                    var roles = json.Value<JArray>("result");
                    var count = roles.Count;


                    JToken jjj = json["result"];

                    Console.WriteLine(json["id"].ToString());
                    string sss = "";
                    for( int i = 0; i < 20; i++)
                    {
                        try
                        {
                            sss += jjj[i].ToString() + ",";
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine("deu exception a ler o jtoken (provavelmente nao trouxe 200 valores)" + e);
                        }
                    }
                    Console.WriteLine(sss);

                    if (jjj.Any())
                        return new PostResult(jjj, (json["id"].ToString()), m, count);
                    else
                        return null;
                }
            }
        }


        static RequestClient()
        {
        }


        public static RequestClient GetInstance()
        {
            if (instance == null)
            {
                instance = new RequestClient();
            }
            return instance;
        }

    }
}
