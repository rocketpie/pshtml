using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace PsHtml.Controllers
{
  public class HomeController : ApiController
  {
    public HttpResponseMessage Get()
    {
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
          var response = new HttpResponseMessage();
          response.Content = new StringContent(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath);
          response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
          return response;

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

          var response = new HttpResponseMessage();
          response.Content = new StringContent(string.Join(Environment.NewLine, output.Select(o => o.ToString())));
          response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
          return response;
        }
      }
      catch (Exception e)
      {
        var response = new HttpResponseMessage();
        response.Content = new StringContent(e.ToString());
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        return response;
      }
    }
  }
}