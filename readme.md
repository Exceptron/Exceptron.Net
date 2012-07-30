# Exceptron.Client #

**Exceptron.Client** is a .NET wrapper for [exceptron](https://www.exceptron.com "exceptron") API.

## Download ##

You can download the binary version of `Exceptron.Client` from nuget [here](http://nuget.org/packages/exceptron.client "here")

To install Exceptron.Client, run the following command in the Package Manager Console

`PM> Install-Package Exceptron.Client -Pre`

## Usage ##

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
    
*Please note that Client initialization can be done once per application life cycle and reused to submit subsequent exceptions. There is no need to create a new instance for each exception.*


###Response `ExceptionResponse`###
`SubmitException()` method returns an instance of `ExceptionResponse`


#####`[string] RefId` 
> This filed will be populated with the Reference ID of you exception report. similar exceptions will return the same RefId but each exception instance is still stored individually on the server. You could show this ID to the user which will allow you to see exact detail of the exception on your exceptron dashboard.

#####`[bool] Successful` 
> `True` if the report was successfully received and processed by exception servers, `False` if the submission failed for any reason.

#####`[Exception] Exception` 
> In cases where `ThrowExceptions` is set to `False` exceptron client will return an `ExceptionResponse` object with this field populated with the exception that would otherwise be thrown. You can log or inspect this exception to get more information on the reason why the report failed.



## Configuration ##

### Configuration Values ###
#####`[string] ApiKey` 
> The API key that identifies your application. you can find API key in the application settings page.

#####`[bool] ThrowExceptions`
> If exceptron client is allowed to throw an exception when an error occurs while submitting an exception. Setting this value to `False` will cause exceptron client not to throw any exceptions under any circumstances.

>*We recommend that you enable exceptions during debug and development and disable in production* 

#####`[bool] IncludeMachineName` 
>If the machine name should be attached to the exception report.
Machine name can be useful in webfarm enviroments when multiple servers are running the same app and the issue could be machine specific. However, You might want to disable this feature for privacy reasons.</remarks>


### Configuring the Client Using Configuration File ###
exceptron client can be configured using application config files `web.config or app.config` using the following schema

    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
        <configSections>
            <section name="exceptron" type="Exceptron.Client.Configuration.ExceptronConfiguration,Exceptron.Client" />
        </configSections>
        <exceptron throwExceptions="true" apiKey="ABCD" includeMachineName="true" />
    </configuration>
    


### Configuring the Client Programmatically ###
By default exceptron client tries to load configuration values from the config file. if config section is not defined, or any of the values are missing a default value will be assigned. However all these values can be overwrite on runtime using the following syntax.

    //Create a new configuration instance
     var myConfig = new ExceptronConfiguration
           {
            ApiKey = "YOUR_API_TOKE",
            IncludeMachineName = true,
            ThrowExceptions = true
        };

    //Now you can create the client using the configuration object
    var exceptron = new ExceptronClient(myConfig);

    


## Add-ons ##
There are add-ons that lets you integrate exceptron into your existing workflow

**[Exceptron.Log4Net](https://github.com/Exceptron/Exceptron.Log4Net "Exceptron.Log4Net")** *Automatically report all exceptions logged using log4net to exceptron*

**[Exceptron.Nlog](https://github.com/Exceptron/Exceptron.Log4Net "Exceptron.Nlog")** *Automatically report all exceptions logged using NLog to exceptron*
