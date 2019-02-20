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

using System.Configuration;

namespace Ministry.MultiHostRedirector
{
    /// <summary>
    /// Definition for a redirect configuration section.
    /// </summary>
    public class UrlRedirectConfigurationSection : ConfigurationSection
    {
        #region | Singletons |

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        public static UrlRedirectConfigurationSection Get() 
            => (UrlRedirectConfigurationSection)ConfigurationManager.GetSection(SectionName);

        /// <summary>
        /// Gets the name of the section.
        /// </summary>
        /// <value>
        /// The name of the section.
        /// </value>
        public static string SectionName => "multiHostRedirectData";

        #endregion

        #region | Configuration Properties |

        /// <summary>
        /// Gets the redirects.
        /// </summary>
        [ConfigurationProperty("redirects", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(RedirectElement), AddItemName = "redirect")]
        public RedirectElementCollection Redirects => (RedirectElementCollection)base["redirects"];

        /// <summary>
        /// Gets the hosts.
        /// </summary>
        [ConfigurationProperty("redirectHosts")]
        [ConfigurationCollection(typeof(HostElement), AddItemName = "host")]
        public HostElementCollection Hosts => (HostElementCollection)base["redirectHosts"];

        /// <summary>
        /// Gets the default redirect url.
        /// </summary>
        [ConfigurationProperty("defaultRedirectUrl")]
        public string DefaultRedirectUrl => (string)base["defaultRedirectUrl"];

        #endregion
    }
}
