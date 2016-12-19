using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace PsHtml.Controllers
{
  public class HomeController : ApiController
  {
    const string MEDIA_TYPE_HEADER = "content-type: ";
    public static string DefaultContentType { get; set; } = "text/plain";

    public HttpResponseMessage Get()
    {
      var response = new HttpResponseMessage();

      try
      {
        string fileName = Request.RequestUri.AbsolutePath.Remove(0, 1);
        if (fileName.EndsWith("/") || string.IsNullOrEmpty(fileName))
        {
          fileName = "index";
        }

        string filePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, fileName + ".html.ps1");

        if (!File.Exists(filePath))
        {
          throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        var parameters = HttpUtility.ParseQueryString(Request.RequestUri.Query);

        using (PowerShell ps = PowerShell.Create())
        {                                                
          ps.AddCommand(filePath);

          foreach (var key in parameters.AllKeys)
          {
            ps.AddParameter(key, parameters[key]);
          }

          var output = ps.Invoke();
          var outputStrings = output.Select(o => o.ToString());

          if (outputStrings.Any())
          {
            string contentType = DefaultContentType;

            var first = outputStrings.FirstOrDefault();
            if (!string.IsNullOrEmpty(first) && first.StartsWith(MEDIA_TYPE_HEADER))
            {
              contentType = first.Remove(0, MEDIA_TYPE_HEADER.Length);
              outputStrings = outputStrings.Skip(1);
            }

            response.Content = new StringContent(string.Join(Environment.NewLine, outputStrings));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
          }

          return response;
        }
      }
      catch (Exception e)
      {
        response.Content = new StringContent(e.ToString(), Encoding.UTF8, "text/plain");
        return response;
      }
    }
  }
}