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

namespace POEMaps
{
    public class RequestResult
    {
        public List<PostResult> post_results { get; set; } //item_ids, id, map
        public List<UserResults> users { get; set; }
        public StackPanel sp;
        public TextBlock child = null;

        RequestClient rq = RequestClient.GetInstance();

        public RequestResult(StackPanel sp)
        {
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

        public void getRequestItems(List<String> ids)
        {
            Console.WriteLine("Get request:");
            foreach(String s in ids)
            {
                Console.WriteLine(s);
            }
        }


        public void addPostResult(PostResult pr)
        {

            post_results.Add(pr);

            Application.Current.Dispatcher.Invoke((Action)delegate {
                logInfo("Returned " + pr.nIds + " offers!");
            });

            for (int i = 0; i < 13; i++)
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

                getRequestItems(ids_requested);
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                int remainingMsToAvoidTimeout = 750 - (int)elapsedMs;

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
