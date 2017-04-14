using System;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace PowerShellExecution
{

    public partial class Default : Page
    {
        // Page_Init
        protected void Page_Init(object sender, EventArgs e)
        {
            bool redirected = false;
            bool isScriptSelected = false;
            foreach (var key in Request.Form.Keys)
            {
                if (key.ToString().StartsWith("script"))
                {
                    SendScripts(Request.Form[key.ToString()]);
                    redirected = true;
                    isScriptSelected = true;
                }
                else if (key.ToString().StartsWith("directory"))
                {
                    SendDirectory(Request.Form[key.ToString()]);
                    redirected = true;
                }
            }
            if (!redirected)
            {
                Session["directory"] = "";
            }
            if (!isScriptSelected)
            {
                Session["scriptDirectory"] = "";
            }
        }

        // Page_Load
        protected void Page_Load(object sender, EventArgs e) {
            string relPath = "";
            if (Session["directory"] != null && !Session["directory"].Equals(""))
            {
                relPath = Session["directory"].ToString();
            }
            show_directories(relPath);
            show_scripts(relPath);
            //debugRequests();
        }

        protected void debugRequests()
        {
            Response.Write("Path: " + Session["directory"] + "<br/>");
            foreach (var key in Request.Form.Keys)
            {
                Response.Write("Key: " + key.ToString() + " | " + "Value: " + Request.Form[key.ToString()] + "<br/>");
            }
            foreach (var key in Session)
            {
                Response.Write("Key: " + key.ToString() + " | " + "Value: " + Session[key.ToString()] + "<br/>");
            }
            Response.Write("Unassigned keys <br/>");
            Response.Write(Request.Form.GetValues(null));
        }

        // ALL FILES AND FOLDERS CANNOT HAVE SPACES (for an unknown reason)
        protected string getSubDirectoriesPath(string additionalPath) {
            string projectDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            string subDirectoryPath = Path.Combine(projectDirectory, "powershell_scripts\\" + additionalPath);
            return subDirectoryPath;
        }

        protected void show_directories(string relPath)
        {
            folderPanel.Controls.Add(new LiteralControl("<p><u>Subdirectories: </u></p>"));
            DirectoryInfo directory = new DirectoryInfo(getSubDirectoriesPath(relPath));
            DirectoryInfo[] subdirectories = directory.GetDirectories();
            int id = 0;
            foreach (DirectoryInfo subdirectory in subdirectories)
            {
                Button dir = new Button();
                dir.ID = "directory" + id.ToString();
                dir.Text = subdirectory.ToString();
                dir.Attributes["class"] = "button folder";
                dir.ClientIDMode = ClientIDMode.Static;
                folderPanel.Controls.Add(dir);
                string lineBreakTags = "<br /><br />";
                folderPanel.Controls.Add(new LiteralControl(lineBreakTags));
                id += 1;
            }
        }

        // ALL FILES AND FOLDERS CANNOT HAVE SPACES (for an unknown reason)
        protected string getScriptPath(string additionalPath)
        {
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string scriptsDirectoryPath = Path.Combine(projectDirectory, "powershell_scripts\\" + additionalPath);
            return scriptsDirectoryPath;
        }

        protected void show_scripts(string relPath)
        {
            scriptsPanel.Controls.Add(new LiteralControl("<p><u>Scripts: </u><p>"));
            string scriptsDirectory = getScriptPath(relPath);
            DirectoryInfo dir = new DirectoryInfo(scriptsDirectory);
            string[] filePaths = Directory.GetFiles(scriptsDirectory, "*.*", SearchOption.TopDirectoryOnly);
            int id = 0;
            foreach (var file in dir.GetFiles())
            {
                Button newButton = new Button();
                newButton.Text = file.Name;
                newButton.ID = "script" + id.ToString();
                newButton.Attributes["class"] = "button script";
                scriptsPanel.Controls.Add(newButton);
                string lineBreakTags = "<br /><br />";
                scriptsPanel.Controls.Add(new LiteralControl(lineBreakTags));
                id += 1;
            }
        }

        protected void SendDirectory(string dirName)
        {
            Session["directory"] += dirName + "\\";
        }

        protected void SendScripts(string scriptName)
        {
            Session["scriptDirectory"] = Session["directory"] + scriptName;
            Response.Redirect("Scripts.aspx"); 
        }
    }
}