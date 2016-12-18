using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace PsHtml.Controllers
{
  public class HomeController : ApiController
  {
    public HttpResponseMessage Get()
    {
      var path = Request.RequestUri.AbsolutePath.Remove(0, 1);
      if (path.EndsWith("/") || string.IsNullOrEmpty(path))  {
        path += "index"; 
      }
      string filename = (path + ".html.ps1");

      if (!File.Exists(filename))
      {
        throw new HttpResponseException(HttpStatusCode.NotFound);
      }
      filename = Path.GetFullPath(filename);

      var parameters = HttpUtility.ParseQueryString(Request.RequestUri.Query);

      using (PowerShell ps = PowerShell.Create())
      {
        ps.AddCommand(filename);

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
  }
}