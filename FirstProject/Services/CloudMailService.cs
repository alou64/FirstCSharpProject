using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace FirstProject.Services
{
  public class CloudMailService : IMailService
  {
    private readonly IConfiguration _configuration;

    // constructor injection
    public CloudMailService(IConfiguration configuration)
    {
      // null check
      _configuration = configuration ??
        throw new ArgumentNullException(nameof(configuration));
    }


    public void Send(string subject, string message)
    {
      // send mail -> output to debug window
      Debug.WriteLine($"Mail from {_configuration["mailSettings:mailFromAddress"]} to {_configuration["mailSettings:mailToAddress"]}, with LocalMailService");
      Debug.WriteLine($"Subject: {subject}");
      Debug.WriteLine($"Message: {message}");
    }
  }
}
