using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace GcExtractor.Service
{
    public class GmailCusService
    {
        private GmailService _gmailSvc;

        public GmailCusService() {
            UserCredential credential;
            var scopes = new[] {GmailService.Scope.GmailReadonly, GmailService.Scope.GmailModify};
            using (var stream = new FileStream("client_secret.json",
                 FileMode.Open,FileAccess.Read)) 
            {
                var credPath = Environment.GetFolderPath(
                    Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/gmail-auth.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)
                ).Result;
            }

            _gmailSvc = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Gmail API"
            });
        }

        public IList<string> GetLabels()
        {
            var request = _gmailSvc.Users.Labels.List("me");
            return request.Execute().Labels.Select(c => c.Name).ToList();
        }
    }
}
