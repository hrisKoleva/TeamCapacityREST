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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;

namespace TeamCapacityREST
{
    public class Values
    {
        public List<TeamMember> teamMemberList { get; set; }
        public List<Activities> activitiesList { get; set; }
    }

    public class TeamMember
    {
        private string id { get; set; }
        private string displayName { get; set; }
        private DateTime[] daysOff { get; set; }

    }

    public class Activities
    {
        public int capacityPerDay { get; set; }
        public string name { get; set; }
    }


    public class Program
    {
        static void Main(string[] args)
        {
            //http://ptfs.partner.master.int:8080/tfs/Simulation/Nemo/Sputnik/_apis/work/TeamSettings/Iterations/bf3a9219-0758-41a3-865b-d3d4f74c0239/Capacities?api-version=2.0-preview.1


            HttpClientHandler authHandler = new HttpClientHandler()
            {
                UseDefaultCredentials = true

            };

            using (HttpClient client = new HttpClient(authHandler))
            {
                client.BaseAddress = new Uri("http://ptfs.partner.master.int:8080/tfs/Simulation/Nemo/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                using (HttpResponseMessage response = client.GetAsync("Sputnik/_apis/work/TeamSettings/Iterations/bf3a9219-0758-41a3-865b-d3d4f74c0239/Capacities?api-version=2.0-preview.1").Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        //I don't know how to read the response :-( 
                        TeamMember teamMemmbers = response.Content.ReadAsAsync<TeamMember>().Result;
                        Console.WriteLine(teamMemmbers);
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

       









