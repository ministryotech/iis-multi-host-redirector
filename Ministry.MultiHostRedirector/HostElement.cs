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
    /// Definition for a host element.
    /// </summary>
    public class HostElement : ConfigurationElement
    {
        #region | Configuration Properties |

        /// <summary>
        /// Gets or sets the root URL.
        /// </summary>
        /// <value>
        /// The root URL.
        /// </value>
        [ConfigurationProperty("rootUrl", IsKey=true, IsRequired=true)]
        public string RootUrl
        {
            get { return this["rootUrl"].ToString(); }
            set { this["rootUrl"] = value; }
        }

        #endregion
    }


    /// <summary>
    /// Collection of host elements.
    /// </summary>
    public class HostElementCollection : ConfigurationElementCollection
    {
        #region | ConfigurationElementCollection Overrides |

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement() 
            => new HostElement();

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement"/> to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element) 
            => ((HostElement)element).RootUrl;

        #endregion
    }

}
