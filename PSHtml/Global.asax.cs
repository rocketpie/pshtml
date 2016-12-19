using PsHtml.Controllers;
using System.Web.Http;

namespace PsHtml
{
  public class WebApiApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      GlobalConfiguration.Configure(WebApiConfig.Register);

      var defaultContentType = System.Configuration.ConfigurationManager.AppSettings["DefaultContentType"];
      if (defaultContentType != null) {
        HomeController.DefaultContentType = defaultContentType;
      }
    }
  }
}