

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
        

        static async Task Main(string[] args)
        {

            //var token = await GetBearerToken();
            //Console.WriteLine(token.BearerToken);                

            //var job = await RunJob();
            //Console.WriteLine(job.JobID);

            var status = await CheckJobStatus();
            Console.WriteLine(status);



            Console.ReadLine();
            
        }



        public static async Task<Token> GetBearerToken()
        {
            
            //Converting the object to a json string. NOTE: Make sure the object doesn't contain circular references.
            string json = JsonSerializer.Serialize(new
            {
                User = "admin",
                Password = "Admin123",
                RememberMe = 1
            });

            //Needed to setup the body of the request
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            //The url to post to.
            var url = "https://localhost:9260/api/account/login";
            X509Certificate2 certificate = new X509Certificate2("D:\\certificate.pfx", "Astera123");
            HttpClientHandler handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = (a, b, c, d) => { return true; };
            handler.ClientCertificates.Add(certificate);
            var client = new HttpClient(handler);


            //Pass in the full URL and the json string content
            var response = await client.PostAsync(url, data);
       
            //It would be better to make sure this request actually made it through
            string result = await response.Content.ReadAsStringAsync();

            var AccessToken = System.Text.Json.JsonSerializer.Deserialize<Token>(result.ToString());

            //close out the client
            client.Dispose();

            return AccessToken;
        }

        public static async Task<Job> RunJob()
        {
            
            string rawBody = JsonSerializer.Serialize(new
            {
                FilePath= "C:\\Users\\yousha.arif\\Desktop\\Delete Any time\\Dataflow1.Df"
            });

            
            var url = "https://localhost:9260/api/CommandLineProcessor";
            
            X509Certificate2 certificate = new X509Certificate2("D:\\certificate.pfx", "Astera123");
            HttpClientHandler handler = new HttpClientHandler();
            
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = (a, b, c, d) => { return true; };
            handler.ClientCertificates.Add(certificate);
            
           
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZG1pbkBjZW50ZXJwcmlzZS5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJzdWIiOiJhZG1pbiIsImp0aSI6ImJkNDE4ODFkLTU0ODEtNDIwZS04NGRmLWFiMDVjZTRjZmRlOCIsImlhdCI6MTYyMzczMDQ2Mywicm9sIjoiYXBpX2FjY2VzcyIsImlkIjoiMSIsIm5iZiI6MTYyMzczMDQ2MywiZXhwIjoxNjI2MzIyNDYzfQ.1BiBpP_wr8ngwK-HFGi6ZENyQWY0jdodoVJ4h95217g");
            
            StringContent data = new StringContent(rawBody, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, data);
            string result = await response.Content.ReadAsStringAsync();
            var job = System.Text.Json.JsonSerializer.Deserialize<Job>(result.ToString());

            client.Dispose();

            return job;

            
        }

        public static async Task<string> CheckJobStatus()
        {


            var url = "https://localhost:9260/api/Job/10116/Status";

            X509Certificate2 certificate = new X509Certificate2("D:\\certificate.pfx", "Astera123");
            HttpClientHandler handler = new HttpClientHandler();

            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = (a, b, c, d) => { return true; };
            handler.ClientCertificates.Add(certificate);


            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZG1pbkBjZW50ZXJwcmlzZS5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJzdWIiOiJhZG1pbiIsImp0aSI6ImJkNDE4ODFkLTU0ODEtNDIwZS04NGRmLWFiMDVjZTRjZmRlOCIsImlhdCI6MTYyMzczMDQ2Mywicm9sIjoiYXBpX2FjY2VzcyIsImlkIjoiMSIsIm5iZiI6MTYyMzczMDQ2MywiZXhwIjoxNjI2MzIyNDYzfQ.1BiBpP_wr8ngwK-HFGi6ZENyQWY0jdodoVJ4h95217g");
            
            var response = await client.GetAsync(url);
            string result = await response.Content.ReadAsStringAsync();
            var status = System.Text.Json.JsonSerializer.Deserialize<string>(result.ToString());

            client.Dispose();

            return status;


        }

    }
}
