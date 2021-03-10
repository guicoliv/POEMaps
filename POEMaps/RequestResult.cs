using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using POEMaps;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace POEMaps
{
    public class RequestResult
    {
        public List<PostResult> post_results { get; set; } //item_ids, id, map
        public List<UserResults> users { get; set; }
        public StackPanel sp;
        public Grid requestGrid;
        public TextBlock child = null;

        RequestClient rq = RequestClient.GetInstance();

        public RequestResult(StackPanel sp, Grid requestGrid)
        {
            this.requestGrid = requestGrid;
            post_results = new List<PostResult>();
            users = new List<UserResults>();
            this.sp = sp;
        }

        public void logInfo(string s)
        {
            TextBlock t = new TextBlock();
            t.Text = s;
            t.FontSize = 14;
            sp.Children.Add(t);
        }

        public void logInfoForMap(int ids, int miliseconds)
        {
            TextBlock t = new TextBlock();


            if (child == null)
            {
                logInfo("Getting items from ids, preventing timeout by sleeping sleeping for:");
                t.Text = "("+ids+") "+miliseconds+"ms";
                t.FontSize = 12;
                sp.Children.Add(t);
                child = (TextBlock)sp.Children[sp.Children.Count - 1];
            }
            else
            {
                child.Text += " | ("+ids+")" + miliseconds + " ms";
            }
        }

 

        public void getMapsFromIds(Map m, List<string> ids)
        {

            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

            string str = "";
            int k = 1;
            foreach(string s in ids)
            {
                str += s;
                if (k != ids.Count)
                    str += ",";
                k++;

            }

            Console.WriteLine("Sending this arg: " + str);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "C:\\Users\\Guilherme\\AppData\\Local\\Programs\\Python\\Python39\\python.exe";
            startInfo.Arguments = projectDirectory + "\\request.py " + str + " get";
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            //process.StartInfo = startInfo;

            using (Process process = Process.Start(startInfo))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.WriteLine("GET Result:\n" + result);

                    JObject json = JObject.Parse(result);
                    var nItems = json.Value<JArray>("result").Count;

                    JToken items = json["result"];

                    for (int i = 0; i < nItems; i++)
                    {
                        JToken item = items[i];
                        string stash = "";
                        string strX = "0";
                        string strY = "0";
                        /*try
                        {
                            stash = item["listing"]["stash"]["name"].ToString();
                            strX = item["listing"]["stash"]["x"].ToString();
                            strY = item["listing"]["stash"]["y"].ToString();
                        }catch(NullReferenceException e)
                        {
                            stash = "";
                            strX = "0";
                            strY = "0";
                        }*/
                        string currency = item["listing"]["price"]["currency"].ToString();
                        string strAmount = item["listing"]["price"]["amount"].ToString();
                        string account = item["listing"]["account"]["lastCharacterName"].ToString();

                        int x, y, amount;

                        

                        if (!Int32.TryParse(strX, out x) || !Int32.TryParse(strY, out y) || !Int32.TryParse(strAmount, out amount))
                        {
                            Console.WriteLine("Error parsing the results");
                            continue;

                        }
                        Console.Write("A MATAR O JTOKEN:");
                        Console.Write("stash: " +stash);
                        Console.Write("; x: " +x);
                        Console.Write("; y: " +y);
                        Console.Write("; currency: " +currency);
                        Console.Write("; amount: " +amount);
                        Console.Write("; account: " +account+"\n");

                        UserResults u = null;


                        bool existed = false;
                        foreach(UserResults user in users)
                        {
                            if (account.Equals(user.username))
                            {
                                existed = true;
                                u = user;
                                break;
                            }
                        }

                        if(u == null)
                        {
                            u = new UserResults(account, users.Count);
                            users.Add(u);
                        }


                        MapResult mapResult = new MapResult(m, currency, amount, stash, x, y, u.getNResults());
                        u.addMapResult(mapResult);
                        if (!existed)
                        {
                            Application.Current.Dispatcher.Invoke((Action)delegate
                            {
                                requestGrid.RowDefinitions.Add(new RowDefinition());
                                requestGrid.Children.Add(u.userGrid);
                            });
                        }
                        else
                        {
                            Console.WriteLine("Already existing user: " + account);
                        }
                    }                    
                }
            }
        }


        public void addPostResult(PostResult pr)
        {

            post_results.Add(pr);

            Application.Current.Dispatcher.Invoke((Action)delegate {
                logInfo("Returned " + pr.nIds + " offers!");
            });

            for (int i = 0; i < 10; i++)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                List<String> ids_requested = new List<string>();
                Console.WriteLine("TOTAL IDS: " + pr.nIds + "; READ IDS: " + pr.ids_read);
                int limit_id = pr.ids_read + 10;
                for(int j = pr.ids_read; j < limit_id  && j < pr.nIds; j++)
                {
                    ids_requested.Add(pr.item_ids[j].ToString());
                    pr.ids_read += 1;
                }

                getMapsFromIds(pr.m, ids_requested);
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                int remainingMsToAvoidTimeout = 1000 - (int)elapsedMs;

                if (remainingMsToAvoidTimeout < 0)
                    remainingMsToAvoidTimeout = 0;

                Application.Current.Dispatcher.Invoke((Action)delegate {
                    logInfoForMap(pr.ids_read, remainingMsToAvoidTimeout);
                });

                Thread.Sleep(remainingMsToAvoidTimeout);


                if (pr.ids_read == pr.nIds)
                    break;

            }

        }
    }
}
