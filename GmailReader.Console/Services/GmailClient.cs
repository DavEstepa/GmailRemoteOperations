using GmailReader.Console.Domain.Constants;
using GmailReader.Console.Domain.Extensions;
using GmailReader.Console.Domain.Interfaces;
using GmailReader.Console.Domain.Types;
using GmailReader.Console.Domain.Utilities;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;


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
            var tempMessages = await users.Messages.List(userId: GmailValues.ME).ExecuteAsync();
            if (tempMessages == null) throw new Exception("No Messages retrieved");
            for (int i = 0; i < tempMessages.Messages.Count; i++)
            {
                var tempMessage = await users.Messages.Get(GmailValues.ME, tempMessages.Messages[i].Id).ExecuteAsync();
                var header = tempMessage.Payload.Headers.Where(hd => hd.Name.Equals(GmailValues.FROM)).First();
                var date = DateTimeOffset.FromUnixTimeMilliseconds(tempMessage.InternalDate ?? 0).DateTime;
                var emails = header.Value.ExtractEmailInChars(GmailValues.EMAIL_INF_LIMIT, GmailValues.EMAIL_SUP_LIMIT);
                if (emails.Contains(emailAddress))
                {
                    await users.Messages.Trash(GmailValues.ME, tempMessages.Messages[i].Id).ExecuteAsync();
                }
            }
        }

        public async Task RetrieveDataByMailAddress(string emailAddress, DateTime initDate, DateTime endDate)
        {
            var users = _client.Users;
            var instruction = users.Messages.List(userId: GmailValues.ME);
            string query = $"from:{emailAddress} after:{initDate.ToString(GmailValues.FORMAT_DATE)} before:{endDate.ToString(GmailValues.FORMAT_DATE)}";
            instruction.Q = query;
            var msgs = await instruction.ExecuteAsync();

            foreach (var msg in msgs.Messages)
            {
                var fullMsg = await users.Messages.Get(GmailValues.ME, msg.Id).ExecuteAsync();
                var payload = fullMsg.Payload;
                foreach (var part in payload.Parts)
                {
                    var g = part.Body;
                    MimeTypesEnum convertedMimeType = part.MimeType switch
                    {
                        "text/plain" => MimeTypesEnum.TextPlain,
                        "text/html" => MimeTypesEnum.TextHtml,
                        _ => throw new Exception("Invalid MimeType")
                    };

                    switch (convertedMimeType)
                    {
                        case MimeTypesEnum.TextPlain:
                            var textContent = Base64URLHelpers.Base64UrlDecode(part.Body.Data);
                            break;
                        case MimeTypesEnum.TextHtml:
                            var htmlContent = Base64URLHelpers.Base64UrlDecode(part.Body.Data);
                            break;
                        default:
                            break;
                    }

                }

            }
        }
    }
}
