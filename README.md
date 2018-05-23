# powershell-web-interface
## Installation

1. Enable IIS, ASP, and ASP.NET in Windows Features:

	1. Please enable IIS in Windows Features by selecting the box
	that says Internet Information Services.

	1. Enable all versions of ASP and ASP.NET by going to Internet Information
	Services -> World Wide Web Services -> Application Development Features and check
	all boxes that say ASP.

2. Install .NET Framework (requires .NET Framework >=v4.5.2).

3. Install Ajax for ASP v1.0.

4. Open the PowerShellExecution.sln in Visual Studio. Once it opens, go to the file tree on the right-hand side of Visual Studio. Right-click on the solution/project name, and then click ```Publish...``` 

	1. In the window that pops up, add a new Profile or use a previously created profile. Then in Connection, change the Publish Method to ```Web Deploy```. If you don't have a server to host the 	website on, use ```localhost``` as the server, and use ```localhost:<available port number>``` as the destination URL. The site name could be anything you want. 

	2. You may want to check ```Precompile before publishing``` in the Settings tab, but otherwise, you can now publish the site by clicking ```Publish```. Now a website folder (the compiled website) 		with the same name as the site name will be created in the parent folder of this GitHub repo.

5. Create a new site in IIS:

	1. Create a new site in IIS and point it to the website folder (which has the same name as the site name). 
	The name of the app pool should be the name of the site.

	2. You will have to give IIS_IUSRS and IUSR permissions to the site folder, and you may need to set up 
	basic authentication instead of anonymous authentication (if screen is loading for a long time).

6. Open the site in a browser:

	1. If you did not specify an IP address or localhost to use as the web address when you created the site in IIS, 
	the default address should be 127.0.0.1 or localhost along with the port number.

	2. If you want to run your own scripts, please put your scripts in the powershell_scripts folder in the site folder.
	Do not rename the powershell_scripts folder, or else the web interface will not be able to find the scripts.
	However, you are free to make subdirectories within the powershell_scripts folder.

## Important Notes

1. Please put all your scripts in the powershell_scripts folder. If you want to make subdirectories within that folder to organize your scripts please do so. 

2. When putting in parameters and arguments, regular parameters (parameters WITH arguments) and positional parameters (parameters WITHOUT arguments) are space-separated. If you wish to add an argument 
to a regular parameter, please do so in this format: '-param1=arg'. __There can only be one arg per parameter.__ Parameters and arguments are optional if the script does not require them.

_Example:_
```	
	parameter (non-positional, has arguments): "-param1=arg"
	paramter (positional, no arguments): "param2"
	full input: "-param1=arg param2"
```

3. If you see the error 'Access to path is denied', you must give IIS full permissions to the folder/file
that you are trying to access.

## Pushing PowerShell Scripts to Git

Currently, all files in the powershell_scripts folder are ignored. If you want to modify this, please edit the .gitignore file.
