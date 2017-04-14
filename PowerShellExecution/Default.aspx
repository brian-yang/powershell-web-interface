<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PowerShellExecution.Default" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Reflection" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Alliance Bernstein Windows CSSA Group</title>
    <link rel="stylesheet" type="text/css" href="css/button.css" />
    <link rel="stylesheet" type="text/css" href="css/logo.css" />
    <style type="text/css">
    </style>
</head>
<body>
    <form id="main" runat="server">
        <div>
            <table align="center">
                <tr>
                    <td>&nbsp;</td>
                    <td><img class="logo" src="assets/logo.png"/></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><h2 align="center">Alliance Bernstein Windows CSSA Group</h2></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><h3 align="center">Powershell Web Interface</h3></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <b>Note</b>: Leave the prompt blank if the script you're trying to run doesn't take arguments.
                        <br />
                        <br />
                        <b>Example Use</b>:
                        <br />
                        &emsp; get-service.ps1: Runs get-service with no arguments. Leave the prompt blank.
                        <br />
                        &emsp; args-test.ps1: Takes a string arg to store in var computername and prints computername.
                        <br />
                        &emsp; &emsp; &emsp; &emsp; &emsp; &ensp; Put '-computername=ABCD' in the prompt to see what happens.
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <br />
                        <br />
                        <u>Path</u>: \<%= Session["directory"].ToString() %>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Panel runat="server" ID="folderPanel"></asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Panel runat="server" ID="scriptsPanel"></asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>