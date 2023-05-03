using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MimeKit;
using Newtonsoft.Json;
using Stubble.Core.Builders;
using System.Net.Mail;
using System.Text;
using System.Text.Json.Nodes;

namespace tasklistDotNetReact.Services
{
  public class MailService
  {

    string path = "workspace/mails/";

    internal string Resolve(string name) => path + name;

    public JsonNode FindByName(string name)
    {
      string content = File.ReadAllText(Resolve(name));
      return System.Text.Json.JsonSerializer.Deserialize<JsonNode>(content);
    }

    public List<string> FindNames()
    {
      bool exists = Directory.Exists(path);

      if (!exists)
        Directory.CreateDirectory(path);
      return Directory
                .EnumerateFiles(path, "*", SearchOption.AllDirectories)
                .Select(Path.GetFileName).ToList();
    }

    public void Delete(string name)
    {
      File.Delete(Resolve(name));
    }

    public JsonNode Save(JsonNode mail)
    {
      mail["modified"] = DateTime.Now;

      File.WriteAllText(Resolve(mail["name"].ToString()), System.Text.Json.JsonSerializer.Serialize(mail));
      return mail;
    }

    public string Preview(JsonNode mail)
    {
      string template = mail["htmlTemplate"].ToString();
      JsonNode previewData = mail["previewData"];
      var dynamicVar = JsonConvert.DeserializeObject<dynamic>(previewData.ToJsonString())!;

      var stubble = new StubbleBuilder().Build();
      return stubble.Render(template, dynamicVar);
    }

    public async Task<string> sendMailAsync(string to, string subject, string template, string locale, JsonNode variables)
    {
      UserCredential credential;
      using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
      {
        string credPath = "token.json";
        credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            new[] { GmailService.Scope.GmailReadonly, GmailService.Scope.GmailSend },
            "user", CancellationToken.None,
                        new FileDataStore(credPath, true));
      }
      var service = new GmailService(
                new BaseClientService.Initializer { HttpClientInitializer = credential, ApplicationName = "My camunda app" }
            );
      JsonNode mail = FindByName(template + '-' + locale.ToLower());
      string mailTemplate = mail["htmlTemplate"].ToString();
      var dynamicVar = JsonConvert.DeserializeObject<dynamic>(variables.ToJsonString())!;
      var stubble = new StubbleBuilder().Build();
      string content = stubble.Render(mailTemplate, dynamicVar);
      var message = new Message();
      var mailMessage = new MailMessage();
      mailMessage.To.Add(to);
      mailMessage.ReplyToList.Add(to);
      mailMessage.Subject = subject;
      mailMessage.Body = content;
      mailMessage.IsBodyHtml = true;
      var mimeMessage = MimeMessage.CreateFromMailMessage(mailMessage);

      var gmailMessage = new Message
      {
        Raw = Base64UrlEncode(mimeMessage.ToString())
      };
      var request = service.Users.Messages.Send(gmailMessage, "me");
      return request.Execute().Id;
    }

    private static string Base64UrlEncode(string input)
    {
      var inputBytes = Encoding.UTF8.GetBytes(input);
      // Special "url-safe" base64 encode.
      return Convert.ToBase64String(inputBytes);
    }
  }
}

