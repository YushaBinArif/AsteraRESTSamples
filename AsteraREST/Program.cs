

using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace AsteraRESTSamples
{
    class Program
    {
        private static string _ServerUri = @"HTTPS://LOCALHOST:9260";
        private static string _PATH_TO_DATAFLOW = @"C:\Users\yousha.arif\Desktop\Delete Any time\Dataflow1.Df";               

        private static string _Username = "admin";
        private static string _Password = "Admin123";
        private static Int16 _RememberMe = 1;
        private static List<string> parametersForJob;


        static async Task Main(string[] args)
        {
            parametersForJob = new List<string>();
            PrintDoc();
            RunningJob runningJob = new RunningJob();

            runningJob.ServerURI = _ServerUri;
            runningJob.Username = _Username;
            runningJob.Password = _Password;
            runningJob.RememberMe = _RememberMe;

            Authentication auth = await runningJob.GetAccessTokenFromServer();
            
            Console.WriteLine($"Access Token: {auth.AccessToken}");


            //Job job = await runningJob.RunJobOnServer(auth, _PATH_TO_DATAFLOW );

            parametersForJob.Add("someParameterValue");
            parametersForJob.Add("anotherParameterValue");
            Job jobWithParams = await runningJob.RunJobOnServerWithParameters(auth, _PATH_TO_DATAFLOW, parametersForJob);

            //Console.WriteLine($"Job ID: {job.JobID}");
            Console.WriteLine($"Job ID: {jobWithParams.JobID}");
            
            
            //string status = await runningJob.CheckJobStatus(auth,job.JobID);
            string status = await runningJob.CheckJobStatus(auth,jobWithParams.JobID);

            Console.WriteLine($"Status: {status}");
            
        }

        private static void PrintDoc()
        {
            
            Console.WriteLine(File.ReadAllText("DOC.txt"));
        }
    }
}
