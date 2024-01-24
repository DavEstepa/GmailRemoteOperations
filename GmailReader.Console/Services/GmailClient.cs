using GmailReader.Console.Domain.Extensions;
using GmailReader.Console.Domain.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmailReader.Console.Services
{
    public class GmailClient : IRemoteMailOperations
    {
        private GmailService _client {  get; set; }
        public async Task Authenticate()
        {
            string[] scopes = { GmailService.Scope.GmailModify };

            string serviceAccountKeyPath = "Assets/credentials_client.json";
            string credPath = "token.json";

            UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                            (await GoogleClientSecrets.FromFileAsync(serviceAccountKeyPath)).Secrets,
                            scopes,
                            "user",
                            CancellationToken.None,
                            new FileDataStore(credPath, true));
            System.Console.WriteLine("Credential file saved to: " + credPath);

            _client = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "",
            });
        }

        public async Task CleanMailbox(string emailAddress)
        {
            var users = _client.Users;
            var tempMessages = await users.Messages.List(userId: "me").ExecuteAsync();
            if (tempMessages == null) throw new Exception("No Messages retrieved");
            for (int i = 0; i < tempMessages.Messages.Count; i++)
            {
                var tempMessage = await users.Messages.Get("me", tempMessages.Messages[i].Id).ExecuteAsync();
                var header = tempMessage.Payload.Headers.Where(hd => hd.Name.Equals("From")).First();
                var date = DateTimeOffset.FromUnixTimeMilliseconds(tempMessage.InternalDate ?? 0).DateTime;
                var emails = header.Value.ExtractEmailInChars('<','>');
                if (emails.Contains(emailAddress))
                {
                    await users.Messages.Trash("me", tempMessages.Messages[i].Id).ExecuteAsync();
                }
            }
        }

        public async Task RetrieveDataByMailAddress(string emailAddress, DateTime initDate, DateTime endDate)
        {
            var users = _client.Users;
            var instruction = users.Messages.List(userId: "me");
            string query = $"from:{emailAddress} after:{initDate.ToString("yyyy/MM/dd")} before:{endDate.ToString("yyyy/MM/dd")}";
            instruction.Q = query;
            var msgs = await instruction.ExecuteAsync();
            throw new NotImplementedException();
        }
    }
}
