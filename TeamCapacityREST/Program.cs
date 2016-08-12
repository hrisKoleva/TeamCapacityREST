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

            HttpClient client = new HttpClient();

            //This is code that tests the connection, I can print workitems but seems I'm not authorized to execute the request, 
            //so I don't know what's the problem
            TfsTeamProjectCollection tfs = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri("http://ptfs.partner.master.int:8080/tfs/Simulation"));

            ReadOnlyCollection<CatalogNode> collectionNodes =
                tfs.ConfigurationServer.CatalogNode.QueryChildren(new[] { CatalogResourceTypes.ProjectCollection }, false,
                    CatalogQueryOptions.None);

            foreach (CatalogNode collectionNode in collectionNodes)
            {
                Guid collectionId = new Guid(collectionNode.Resource.Properties["InstanceId"]);

                TfsTeamProjectCollection teamProjectCollection =
                    tfs.ConfigurationServer.GetTeamProjectCollection(collectionId);

                  WorkItemStore workItemStore = (WorkItemStore)teamProjectCollection.GetService(typeof(WorkItemStore));


                WorkItemCollection exceptions = workItemStore.Query(
                    "Select [ID], [Title],  [Assigned To]" +
                    "From WorkItems " +
                    "Where [Work Item Type] = 'Bug' AND  [Created Date] = @Today");

                foreach (WorkItem item in exceptions)
                {
                    Console.WriteLine(item.Id + "\t" + item.Title);
                }
            }

            //my request.... 
            
            var x =  client.GetAsync("http://ptfs.partner.master.int:8080/tfs/Simulation/Nemo/Sputnik/_apis/work/TeamSettings/Iterations/4529/Capacity?api-version= 2.0-preview.1");  // Blocking call!
           
            HttpResponseMessage response = x.Result;

            if (response.IsSuccessStatusCode)
            {
                
                var dataObjects = response.Content.ReadAsAsync<IEnumerable<DataObject>>().Result;
                foreach (var d in dataObjects)
                {
                    Console.WriteLine("Sputnik Iteration 163 capacity is {0}", d.Hours);
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

       









