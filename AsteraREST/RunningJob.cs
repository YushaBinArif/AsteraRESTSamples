#
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


using System;
using System.Collections.Generic;
using System.Text;

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


        private Token _AccessToken;
        /// <summary>
        /// This contains the server which we can then use to run jobs
        /// </summary>
        public Token AccessToken
        {
            get
            {
                if (_AccessToken== null)
                {
                    _AccessToken = GetServer();
                }
                return _AccessToken;
            }
        }

        #endregion


        #region Authentication Methods

        /// <summary>
        /// Grabs the Access token from the server
        /// </summary>
        /// <param name="job">The job to be run</param>
        /// <returns>The jobId</returns>
        public long RunJobOnServer(Job job)
        {
            // start the job and return the jobId
            long jobId = ServerProxy.StartJob(job);
            return jobId;
        }

        private ITransferServerProxy _Server;
        /// <summary>
        /// Gets the Server Proxy from the ServerConnection (server name and port number)
        /// </summary>
        /// <returns>A ServerProxy that can be used to start jobs</returns>
        private ITransferServerProxy GetServer()
        {

            try
            {
                if (_Server == null)
                {
                    CreateServer();
                }
                return _Server;

            }
            catch (ServerException ex)
            {

                // throw any exeption when failing to grab the server proxy specified
                throw ex;

            }

        }

        private void CreateServer()
        {
            AsteraAppContext.Servers = new ClientApis();
            AsteraAppContext.Servers.CurrentServerUri = ServerURI;
            // In order to run any job, we will first need the server connection. 
            // This will only work if a server is already configured for use.
            _Server = new TransferClient(ServerURI, Timeout, true);
            var result = _Server.LoginUser(new LoginModel { User = Username, Password = Password, RememberMe = true });
            if (result == null)
                throw new CPException("Login failed.");
        }

        #endregion
    }
}
