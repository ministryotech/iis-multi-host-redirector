# The Multi-Host Redirector #

The Multi-Host redirector was originally designed as a method of redirecting the old WordPress URLs to this site to maintain rankings. I wanted a product that would allow multiple sites and URLs either complete or compiled from host and path elements to be redirected.

The Multi-Host Redirector achieves all of this and can be run either in it's own site (recommended configuration) or within an existing site.

The Multi-Host Redirector will support multiple configuration types. Currently *.config XML configuration is the only configuration type supported. Feel free to raise issues for alternative configuration types that may suit your needs.

## Usage and Sample Configurations ##

The following samples are included in the source code.

Configuration based redirection is best done using separate config files, but the main web.config file needs to have the following elements added to it to support this...

```
#!xml
  <configSections>
    <section name="multiHostRedirectData" type="Ministry.MultiHostRedirector.UrlRedirectConfigurationSection" allowLocation="true" allowDefinition="Everywhere"/>
  </configSections>

  <multiHostRedirectData configSource="redirects-sample-1.config"/>
```

Simply replace 'redirects-sample-1.config' with your redirection file using the samples below as a starting point.

You also need to instruct IIS to use the Http Handler that drives the redirection. This is done using the following config...

```
#!xml
  <system.webServer>
    <handlers>
      <add name="MultiHostRedirectHttpHandler" verb="*" path="*" type="Ministry.MultiHostRedirector.MultiHostRedirectHttpHandler, Ministry.MultiHostRedirector" />
    </handlers>    
  </system.webServer>
```

### Using The Samples ###

The configuration samples are located in the web-redirector project in the source code repository. These files configure various test URLs to point to the web-destination project. In order to see the redirector at work, simply set up both of these projects in IIS and add the following URL mappings to your hosts file...

```
# MultiHostRedirector Mappings
127.0.0.1 		redirector1.local
127.0.0.1 		redirector2.local
127.0.0.1 		redirector3.local
127.0.0.1 		destination.local
```

### Multi-Site Redirection ###

In this sample all key elements are fully defined. Redirects from other attached bindings to the site will not work.
This approach is ideal where the redirector resides with the new website but redirection of URLs in the new domain are not desirable.
  
Note the default redirect; This will cause a failover to the home page if no mapping is found for a valid host.

Here's the example config:

```
#!xml
<multiHostRedirectData defaultRedirectUrl="http://destination.local">
  <redirectHosts>
    <host rootUrl="http://redirector1.local"/>
    <host rootUrl="http://redirector2.local"/>
    <host rootUrl="http://redirector3.local"/>
  </redirectHosts>
  <redirects>
    <redirect requestedUrl="" redirectUrl="http://destination.local"/>
    <redirect requestedUrl="/home" redirectUrl="http://destination.local/index.htm"/>
    <redirect requestedUrl="/item/?p=1" redirectUrl="http://destination.local/destination1.htm"/>
    <redirect requestedUrl="/item/?p=2" redirectUrl="http://destination.local/destination2.htm"/>
    <redirect requestedUrl="/item/?p=3" redirectUrl="http://destination.local/destination3.htm"/>
    <redirect requestedUrl="/item/?p=4" redirectUrl="http://destination.local/destination4.htm"/>
    <redirect requestedUrl="/item/?p=5" redirectUrl="http://destination.local/destination5.htm"/>
    <redirect requestedUrl="/ministry" redirectUrl="http://www.ministryotech.co.uk"/>
    <redirect requestedUrl="/google" redirectUrl="http://www.google.co.uk"/>
  </redirects>
</multiHostRedirectData>
```

### Single Site Redirection ###

In this sample no hosts are defined so all hosts bound to the website in IIS will redirect UNLESS full URLs are given.
This approach is ideal for an isolated redirection site (My preferred solution).
  
Note that the home URL mappings will only work on the redirector1 site. 
Note also the absence of a default redirect; This will cause a 404 to be thrown rather than a failover to the home page.

Here's the example config:

```
#!xml
<multiHostRedirectData>
  <redirects>
    <redirect requestedUrl="http://redirector1.local" redirectUrl="http://destination.local"/>
    <redirect requestedUrl="http://redirector1.local/home" redirectUrl="http://destination.local/index.htm"/>
    <redirect requestedUrl="/item/?p=1" redirectUrl="http://destination.local/destination1.htm"/>
    <redirect requestedUrl="/item/?p=2" redirectUrl="http://destination.local/destination2.htm"/>
    <redirect requestedUrl="/item/?p=3" redirectUrl="http://destination.local/destination3.htm"/>
    <redirect requestedUrl="/item/?p=4" redirectUrl="http://destination.local/destination4.htm"/>
    <redirect requestedUrl="/item/?p=5" redirectUrl="http://destination.local/destination5.htm"/>
    <redirect requestedUrl="/ministry" redirectUrl="http://www.ministryotech.co.uk"/>
    <redirect requestedUrl="/google" redirectUrl="http://www.google.co.uk"/>
  </redirects>
</multiHostRedirectData>
```

## The Ministry of Technology Open Source Products ##
Welcome to The Ministry of Technology open source products. All open source Ministry of Technology products are distributed under the MIT License for maximum re-usability. Details on more of our products and services can be found on our website at http://www.ministryotech.co.uk

Our other open source repositories can be found here...

[https://bitbucket.org/ministryotech](https://bitbucket.org/ministryotech)

[https://github.com/tiefling](https://github.com/tiefling)

Most of our content is stored on BitBucket, but we also do some Umbraco related projects and with Umbraco itself hosted on GitHub it made sense to host those there too.

### Where can I get it? ###
You can download the package for this project from any of the following package managers...

- **NUGET** - [https://nuget.org/packages/Ministry.MultiHostRedirector](https://nuget.org/packages/Ministry.MultiHostRedirector)

### Contribution guidelines ###
If you would like to contribute to the project, please contact me.

The source code can be used in a simple text editor or within Visual Studio using NodeJS Tools for Visual Studio.

### Who do I talk to? ###
* Keith Jackson - keith@ministryotech.co.uk