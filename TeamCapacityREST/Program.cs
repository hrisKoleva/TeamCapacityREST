using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TeamCapacityREST
{

    public class DataObject
    {
        public string Name { get; set; }
        public int Hours { get; set; }

    }

    public class Program
    {
        static void Main(string[] args)
        {
            
            /*//http://ptfs.partner.master.int:8080/tfs/Simulation/Nemo/Sputnik/_apis/work/TeamSettings/Iterations/4529/Capacity?api-version= 2.0-preview.1*/

            var username = "username";
            var password = "password";


                   using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password))));


                using (HttpResponseMessage response = client.GetAsync("http://ptfs.partner.master.int:8080/tfs/Simulation/Nemo/Sputnik/_apis/work/TeamSettings/Iterations/4529/Capacity?api-version= 2.0-preview.1").Result)
                {
                    if (response.IsSuccessStatusCode)
                    {

                        var dataObjects = response.Content.ReadAsAsync<IEnumerable<DataObject>>().Result;
                        foreach (var d in dataObjects)
                        {
                            Console.WriteLine("Sputnik Iteration 163 capacity is {0}", d.Name);
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n !!! {0} ({1})", (int)response.StatusCode, response.ReasonPhrase);

                    }

                    Console.ReadLine();
                }
                   
            }
               
        }
    }
}

       









