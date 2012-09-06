# Exceptron.Client #

**Exceptron.Client** is a .NET wrapper for [exceptron's](https://www.exceptron.com) REST API.

## Requirements ##
.NET Framework 2.0 or Higher.

*`exceptron.client.dll` currently has no dependencies on any external libraries.*

## Download ##

You can download the latest binary version of `Exceptron.Client` from [nuget](http://nuget.org/packages/exceptron.client)

To install Exceptron.Client, run the following command in the Package Manager Console

`Install-Package Exceptron.Client`

## Usage ##

```c#
//Create a new instance of exceptron client.
var exceptron = new ExceptronClient();

try
{
	//Code that could cause an exceptions
}
catch (Exception e)
{
	exceptron.SubmitException(e, "Main", ExceptionSeverity.Fatal, "Couldn't call the broken method", "User1");
}
```
    
*Please note that Client initialization only needs to be done once per application lifecycle. Single exceptron client and be reused to submit subsequent exceptions. There is no need to create a new instance for each exception.*


###Response `ExceptionResponse`###
`SubmitException()` method returns an instance of `ExceptionResponse`

**`Exceptron.Client.Message.ExceptionResponse:`**

#####`[string] RefId` 
> This filed will be populated with the Reference ID of your exception report. similar exceptions will return the same RefId but each exception instance is still stored individually on the server. You can show this ID to the user which could provide to you. This can allow you to see exact detail of the exception on your dashboard.

#####`[bool] Successful` 
> `True` if the report was successfully received and processed by exception servers 

> `False` if the submission has failed for any reason.

#####`[Exception] Exception` 
> In cases where `ThrowExceptions` is set to `False` exceptron client will return an `ExceptionResponse` object with this field populated with the exception that would otherwise be thrown. You can log or inspect this exception to get more information on the reason why the call failed.

## Configuration ##

### Configuration Values ###
#####`[string] ApiKey` 
> The API key that identifies your application. you can find the API key in the application settings page.

#####`[bool] ThrowExceptions`
> Whether or not exceptron client is allowed to throw exceptions in an errors occures. Setting this value to `False` will cause the client not to throw exceptions under any circumstances.

>*We recommend that you enable exceptions during debug and development and disable in production* 

#####`[bool] IncludeMachineName` 
>Whether or not the machine name should be attached to the exception report.
Machine name can be useful in webfarm enviroments where multiple servers are running the same app and the issues could be machine specific. However, You might want to disable this feature on client applications for privacy reasons.</remarks>


### Configuring the Client Using Configuration File ###
exceptron client can be configured using application config files `web.config` or `app.config` using the following schema:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<configSections>
    	<section name="exceptron" type="Exceptron.Client.Configuration.ExceptronConfiguration,Exceptron.Client" />
    </configSections>
    <exceptron throwExceptions="true" apiKey="ABCD" includeMachineName="true"/>
</configuration>
```    


### Configuring the Client Programmatically ###
By default exceptron client tries to load configuration values from the application config file. If config section is not defined, or any of the values are missing a default value will be assigned. However all these values can be overwriten at runtime using the following syntax.

```c#
//Create a new configuration instance
var myConfig = new ExceptronConfiguration
	{
    	ApiKey = "YOUR_API_TOKE",
        IncludeMachineName = true,
        ThrowExceptions = true
    };

//Now you can create the client using the configuration object
var exceptron = new ExceptronClient(myConfig);

```
 

## Add-ons ##
There are add-ons that let you integrate exceptron into your existing workflow

**[Exceptron.Log4Net](https://github.com/Exceptron/Exceptron.Log4Net "Exceptron.Log4Net")** *Automatically reports all exceptions logged using log4net to exceptron*

**[Exceptron.Nlog](https://github.com/Exceptron/Exceptron.Log4Net "Exceptron.Nlog")** *Automatically reports all exceptions logged using NLog to exceptron*
