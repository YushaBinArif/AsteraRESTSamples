#region DOCUMENTATION
/*
 *  Following samples are present in the file:
 *  
 *  1. Get bearer access token from the Server
 *      
 *     https://<SERVERNAME>:9260/api/account/login
 *  
 *  
 *  2. Start a Job on the server
 *  
 *      https://<SERVERNAME>:9260/api/CommandLineProcessor
 *      
 *  3. Check status of a job.
 *      
 *     https://<SERVERNAME>:9260/api/Job/569/Status
 *  
 *  
 *  
 *  
 *
 *
*/

#endregion

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AsteraRESTSamples
{
    class RunningJob
    {

        #region Variables

        /// <summary>
        /// The server URI with the port number.
        /// </summary>
        public string ServerURI { get; set; }

        /// <summary>
        /// The username used to login to the server.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password required for the username to login to the server.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Set this value to 1 if you want to remain signed in, otherwise assign 0
        /// </summary>
        public Int16 RememberMe { get; set; }

        

        #endregion

        #region Job Related Functions


        public async Task<Job> RunJobOnServer(Authentication auth, string jobPath)
        {
           
            var url = $"{ServerURI}/api/CommandLineProcessor";
            
            StringContent requestBody = CreateRequestBodyForJob(jobPath);
            HttpClientHandler sslCertificateSettings = CreateCertificateSettings();
            var client = new HttpClient(sslCertificateSettings);
            Job job = await DeserializeRunJobResponse(client, url, requestBody, auth);
            
            client.Dispose();

            return job;


        }

        private async Task<Job> DeserializeRunJobResponse(HttpClient client, string url, StringContent requestBody, Authentication auth)
        {
            
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {auth.AccessToken}");
            var response = await client.PostAsync(url, requestBody);
            string result = await response.Content.ReadAsStringAsync();
            var job = System.Text.Json.JsonSerializer.Deserialize<Job>(result.ToString());

            return job;
        }

        private StringContent CreateRequestBodyForJob(string jobPath)
        {

            string requestBodyParameters = JsonSerializer.Serialize(new
            {
                FilePath = jobPath
            });

            StringContent requestBody = new StringContent(requestBodyParameters, Encoding.UTF8, "application/json");

            return requestBody;
        }

        public async Task<string> CheckJobStatus(Authentication auth,long jobId) {

            var url = $"{ServerURI}/api/Job/{jobId}/Status";

            HttpClientHandler sslCertificateSettings = CreateCertificateSettings();
            var client = new HttpClient(sslCertificateSettings);
            string status = await DeserializeCheckJobStatusResponse(client, url, auth);

            client.Dispose();

            return status;

        }

        private async Task<string> DeserializeCheckJobStatusResponse(HttpClient client, string url, Authentication auth)
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {auth.AccessToken}");
            var response = await client.GetAsync(url);
            string result = await response.Content.ReadAsStringAsync();
            var status = System.Text.Json.JsonSerializer.Deserialize<string>(result.ToString());

            return status;
        }

        #endregion

        #region Authentication Functions

        public async Task<Authentication> GetAccessTokenFromServer()
        {
            Authentication auth;
            try
            {
                auth = await CreateAccessToken(ServerURI);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return auth;

        }

        public  async Task<Authentication> CreateAccessToken(string serverUri)
        {

            string url = $"{serverUri}/api/account/login";
            StringContent requestBody = CreateRequestBodyForAuthentication();
            HttpClientHandler sslCertificateSettings = CreateCertificateSettings();
            HttpClient client = new HttpClient(sslCertificateSettings);

            var authentication = await DeserializeAuthResponse(client, url, requestBody); ;

            client.Dispose();

            return authentication;
        }

        private static async Task<Authentication> DeserializeAuthResponse(HttpClient client, string url, StringContent requestBody)
        {

            var response = await client.PostAsync(url, requestBody);

            string result = await response.Content.ReadAsStringAsync();

            var authentication = System.Text.Json.JsonSerializer.Deserialize<Authentication>(result.ToString());

            return authentication;
        }

        private static HttpClientHandler CreateCertificateSettings()
        {
            HttpClientHandler handler = new HttpClientHandler(); 
            handler.ServerCertificateCustomValidationCallback = (a, b, c, d) => { return true; };  

            return handler;
        }

        private  StringContent CreateRequestBodyForAuthentication()
        {
            string requestBodyParameters = JsonSerializer.Serialize(new
            {
                User = Username,
                Password = Password,
                RememberMe = RememberMe
            });


            StringContent requestBody = new StringContent(requestBodyParameters, Encoding.UTF8, "application/json");

            return requestBody;
        }

        #endregion
    }
}
