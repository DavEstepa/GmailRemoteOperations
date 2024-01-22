using System;
using System.Text.RegularExpressions;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;


namespace GmailReader.Console;

public static class OAuthExecution
{
    public static string EmailToTrash = "empleos_co@computrabajo.com";
    public static async Task Execute()
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

        var client = new GmailService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Your Application Name",
        });

        //await RetrieveInfo(client);

        var users = client.Users;
        var tempMessages = await users.Messages.List(userId: "me").ExecuteAsync();
        if (tempMessages == null) throw new Exception("No Messages retrieved");
        for (int i = 0; i < tempMessages.Messages.Count; i++)
        {
            var tempMessage = await users.Messages.Get("me", tempMessages.Messages[i].Id).ExecuteAsync();
            var header = tempMessage.Payload.Headers.Where(hd => hd.Name.Equals("From")).First();
            var date = DateTimeOffset.FromUnixTimeMilliseconds(tempMessage.InternalDate??0).DateTime;
            var emails = ExtractEmails(header.Value);
            if (emails.Contains(EmailToTrash))
            {
                await users.Messages.Trash("me", tempMessages.Messages[i].Id).ExecuteAsync();

            }
        }
    }

    private static List<string> ExtractEmails(string completeValue)
    {
        string pattern = @"<([^>]*)>";

        MatchCollection matches = Regex.Matches(completeValue, pattern);
        var returnedValue = new List<string>();
        foreach (Match match in matches)
        {
            string extractedString = match.Groups[1].Value;
            returnedValue.Add( extractedString);
        }
        return returnedValue;
    }

    private static async Task RetrieveInfo(GmailService service)
    {
        var messages = await service.Users.Messages.List("empleos_co@computrabajo.com").ExecuteAsync();
    }
}

