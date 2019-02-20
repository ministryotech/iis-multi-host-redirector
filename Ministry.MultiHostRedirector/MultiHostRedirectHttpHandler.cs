// Copyright (c) 2012 Minotech Ltd.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
// (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, 
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do 
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Ministry.MultiHostRedirector
{
    /// <summary>
    /// HTTP Request Handler to redirect Wordpress URLs to Umbraco
    /// </summary>
    public class MultiHostRedirectHttpHandler : IHttpHandler
    {
        #region | IHttpHandler Members |

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            var urlMapper = LoadUrls();
            var oldUrl = context.Request.Url.ToString().ToLower().Trim('/');
            var defaultRedirect = UrlRedirectConfigurationSection.Get().DefaultRedirectUrl;
            var desiredLocalFile = context.Request.ServerVariables["SCRIPT_NAME"];

            if (oldUrl.Contains("showredirects=true"))
            {
                ShowRedirectConfiguration(context, urlMapper);
            }
            else
            {
                var mapping = GetUrlMapping(oldUrl, urlMapper);
                if (mapping != null)
                {
                    CommitResponse(context, mapping.RedirectUrl);
                }
                else if (!string.IsNullOrEmpty(desiredLocalFile) && File.Exists(context.Server.MapPath(desiredLocalFile)))
                {
                    context.Response.Buffer = true;
                    context.Response.Clear();
                    context.Response.WriteFile(desiredLocalFile);
                }
                else if (!string.IsNullOrEmpty(defaultRedirect))
                {
                    CommitResponse(context, defaultRedirect);
                }
                else
                {
                    CommitResponse(context, null, 404);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is reusable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is reusable; otherwise, <c>false</c>.
        /// </value>
        public bool IsReusable => true;

        #endregion

        #region | Private Methods |

        /// <summary>
        /// Loads the urls from configuration.
        /// </summary>
        /// <returns>The config items.</returns>
        private List<RedirectModel> LoadUrls()
        {
            var retVal = new List<RedirectModel>();

            Configuration configuration;
            if (HttpContext.Current == null)
                configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            else
                configuration = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);

            var redirects = UrlRedirectConfigurationSection.Get().Redirects;
            var hosts = UrlRedirectConfigurationSection.Get().Hosts;

            if (hosts.Count == 0)
            {
                retVal.AddRange(redirects.Cast<RedirectElement>()
                    .Select(element => new RedirectModel
                    {
                        RequestedUrl = element.RequestedUrl.Trim('/'),
                        RedirectUrl = element.RedirectUrl.Trim('/')
                    }));
            }
            else
            {
                foreach (HostElement host in hosts)
                {
                    retVal.AddRange(redirects.Cast<RedirectElement>()
                        .Select(element => new RedirectModel
                        {
                            RequestedUrl = host.RootUrl.Trim('/') + "/" + element.RequestedUrl.Trim('/'),
                            RedirectUrl = element.RedirectUrl.Trim('/')
                        }));
                }
            }

            return retVal;
        }

        /// <summary>
        /// Gets the redirect URL for a specified request.
        /// </summary>
        /// <param name="requestedUrl">The requested URL.</param>
        /// <param name="urlMapper">The URL mapper.</param>
        /// <returns></returns>
        private RedirectModel GetUrlMapping(string requestedUrl, IEnumerable<RedirectModel> urlMapper) 
            => urlMapper.FirstOrDefault(mapper => mapper.RequestedUrl == requestedUrl.Trim('/'));

        /// <summary>
        /// Commits the HTTP Response.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="url">The URL.</param>
        /// <param name="statusCode">The status code.</param>
        private static void CommitResponse(HttpContext context, string url, int statusCode = 301)
        {
            if (!string.IsNullOrEmpty(url)) { context.Response.AddHeader("Location", url); }
            context.Response.StatusCode = statusCode;
            context.Response.End();
        }

        /// <summary>
        /// Outputs the redirect configuration to the provided HTTP Response stream.
        /// </summary>
        /// <param name="context">The context to output to the response stream of.</param>
        /// <param name="urlMapper">The URL mapper.</param>
        private static void ShowRedirectConfiguration(HttpContext context, List<RedirectModel> urlMapper)
        {
            context.Response.Write("<html><head></head><body><h1>Redirect Configuration</h1>");
            context.Response.Write("The URL Mapper has " + urlMapper.Count + " entries.");
            foreach (var mapper in urlMapper)
            {
                context.Response.Write("<p>");
                context.Response.Write("Requested: " + mapper.RequestedUrl);
                context.Response.Write("<br/>");
                context.Response.Write("Redirect: " + mapper.RedirectUrl);
                context.Response.Write("</p>");
            }
            context.Response.Write("</body></html>");
        }

        #endregion
    }
}
