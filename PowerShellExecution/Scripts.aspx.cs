using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;

namespace PowerShell
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //debugRequests();
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:getOptions(); ", true);
        }

        protected void debugRequests()
        {
            Response.Write("Path: " + Session["directory"] + "<br/>");
            foreach (var key in Request.Form.Keys)
            {
                Response.Write("Key: " + key.ToString() + " - " + "Value: " + Request.Form[key.ToString()] + "<br/>");
            }
            foreach (var key in Session)
            {
                Response.Write("Key: " + key.ToString() + " - " + "Value: " + Session[key.ToString()] + "<br/>");
            }
            Response.Write("Unassigned keys <br/>");
            Response.Write(Request.Form.GetValues(null));
        }

        [WebMethod]
        public static string RunPowerShell(string options)
        {
            options = Uri.UnescapeDataString(options);
            PowerShellWrapper pswrapper = new PowerShellWrapper();
            Dictionary<string, string> optionDict;
            if (!string.IsNullOrEmpty(options))
            {
                optionDict = parseOptions(options);
            }
            else
            {
                optionDict = null;
            }
            return pswrapper.runPowerShell(options, optionDict);
        }

        private static Dictionary<string, string> parseOptions(string options)
        {
            Collection<string> optionList = new Collection<string>(Regex.Split(options.Trim(), "\\s+"));
            Dictionary<string, string> parameter_arguments = new Dictionary<string, string>();
            foreach (string option in optionList)
            {
                if (option.StartsWith("-"))
                {
                    Collection<string> optionParts = new Collection<string>(option.Split(new char[] { '=' }, 2));
                    if (optionParts.Count > 1)
                    {
                        parameter_arguments.Add(optionParts[0], optionParts[1]);
                    }
                    else
                    {
                        parameter_arguments.Add(optionParts[0], "");
                    }
                }
                else
                {
                    parameter_arguments.Add(option, "");
                }

            }
            return parameter_arguments;
        }

        //private static string DecodeUrlString(string url)
        //{
        //    string newUrl;
        //    while ((newUrl = Uri.UnescapeDataString(url)) != url)
        //    {
        //        url = newUrl;
        //    }
        //    return newUrl;
        //}

        protected class PowerShellWrapper
        {
            // ALL FILES AND FOLDERS CANNOT HAVE SPACES (for an unknown reason)
            protected string getScriptPath(string additionalPath)
            {
                string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string scriptsDirectoryPath = Path.Combine(projectDirectory, "powershell_scripts\\" + additionalPath);
                return scriptsDirectoryPath;
            }

            protected Runspace getRunspace()
            {
                InitialSessionState session = InitialSessionState.CreateDefault();
                session.AuthorizationManager = new AuthorizationManager("sessionManager");
                Runspace runspace = RunspaceFactory.CreateRunspace(session);

                return runspace;
            }

            /* NOTE: You can see what the ExecutionPolicy is by doing "Get-ExecutionPolicy" (without double quotes).
             * Set the ExecutionPolicy with "Set-ExecutionPolicy -ExecutionPolicy <Restricted/AllSigned/RemoteSigned/Unrestricted>".
             * Add "-Scope CurrentUser" along with Set-ExecutionPolicy to set the ExecutionPolicy for ONLY the CURRENT USER.
             */
            public string runPowerShell(string options, Dictionary<string, string> optionDict)
            {
                // Get script directory and clear session variable
                string scriptDirectory = HttpContext.Current.Session["scriptDirectory"].ToString();
                HttpContext.Current.Session["scriptDirectory"] = "";

                // Clean the Result TextBox
                string output = "Running script... \r\n";

                // Powershell engine
                Runspace runspace = getRunspace();
                using (runspace)
                {
                    runspace.Open();
                    using (System.Management.Automation.PowerShell shell = System.Management.Automation.PowerShell.Create())
                    {
                        shell.Runspace = runspace;

                        // Get script path
                        string scriptPath = getScriptPath(scriptDirectory);
                        output += "Script path: " + scriptPath + "\r\n";
                        output += "Input: " + options + "\r\n";

                        if (File.Exists(scriptPath))
                        {
                            // Add script
                            shell.AddCommand(scriptPath);

                            if (!string.IsNullOrEmpty(options))
                            {
                                // Add parameters and arguments
                                output += "Parsed parameters and arguments: " + string.Join("; ", optionDict.Select(dict => dict.Key + "=" + dict.Value).ToArray()) + "\r\n";
                                foreach (string key in optionDict.Keys)
                                {
                                    if (key.StartsWith("-"))
                                    {
                                        shell.AddParameter(key, optionDict[key]);
                                    }
                                    else
                                    {
                                        shell.AddArgument(key);
                                    }
                                }
                            }
                            shell.AddCommand("Out-String");
                            output += invokeScript(shell);
                        }
                        else
                        {
                            output += "File not found. Please check the file path. Otherwise, please go back to the home page and then try again.";
                        }
                        shell.Dispose();
                    }
                    runspace.Close();
                }
                return output;
            }

            protected string invokeScript(System.Management.Automation.PowerShell shell)
            {
                // Execute the script
                try
                {
                    Collection<PSObject> results = shell.Invoke();
                    string output = "Invoking... \r\n\r\n";
                    // display results, with BaseObject converted to string
                    // Note : use |out-string for console-like output
                    if (results.Count > 0)
                    {
                        //ResultBox.Text += results.Count;
                        // We use a string builder ton create our result text
                        var builder = new StringBuilder();

                        output += "Output: \r\n";
                        foreach (var psObject in results)
                        {
                            // Convert the Base Object to a string and append it to the string builder.
                            // Add \r\n for line breaks
                            builder.Append(psObject.BaseObject.ToString() + "\r\n");
                        }

                        // Encode the string in HTML (prevent security issue with 'dangerous' characters like < >
                        output += HttpContext.Current.Server.HtmlEncode(builder.ToString());
                    }
                    else
                    {
                        output += "No output found. \r\n";
                    }

                    // Display error
                    if (shell.Streams.Error.Count > 0)
                    {
                        output += "Errors: \r\n";
                        foreach (var err in shell.Streams.Error)
                        {
                            output += err.ToString() + "\r\n";
                        }
                    }
                    else
                    {
                        output += "Script successfully run!";
                    }
                    return output;
                }
                catch (NullReferenceException error)
                {
                    return "Caught null reference exception. \r\n" + error.StackTrace;
                }
                catch (Exception error)
                {
                    return "Caught exception. \r\n" + error.StackTrace;
                }
            }
        }
    }
}