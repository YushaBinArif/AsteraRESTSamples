

using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AsteraRESTSamples
{
    class Program
    {
        private static string _ServerUri = @"HTTPS://LOCALHOST:9260";
        private static string _PATH_TO_DATAFLOW = @"C:\Users\yousha.arif\Desktop\Delete Any time\Dataflow1.Df";               

        private static string _Username = "admin";
        private static string _Password = "Admin123";
        private static Int16 _RememberMe = 1;


        static async Task Main(string[] args)
        {

            
            RunningJob runningJob = new RunningJob();

            runningJob.ServerURI = _ServerUri;
            runningJob.Username = _Username;
            runningJob.Password = _Password;
            runningJob.RememberMe = _RememberMe;

            Authentication auth = await runningJob.GetAccessTokenFromServer();
            Job job = await runningJob.RunJobOnServer(auth, _PATH_TO_DATAFLOW );
            string status = await runningJob.CheckJobStatus(auth,job.JobID);

            Console.WriteLine($"Status: {status}");
            
        }


       

    }
}
