<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Scripts.aspx.cs" Inherits="PowerShell.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Output</title>
    <script type="text/javascript" src="scripts/jquery-2.0.0.min.js"></script>
    <script type="text/javascript">
        function getOptions() {
            var optionPrompt = prompt("What parameters and args do you want to run with the script?");
            var jsonString = '{"options":' + '"' + encodeURIComponent(optionPrompt) + '"}';
            $.ajax({
                type: "POST",
                url: "Scripts.aspx/RunPowerShell",
                data: jsonString,
                contentType: 'application/json; charset=uft-8',
                dataType: 'json',
                success: function (result) {
                    $("#ResultBox").val(result.d);
                    alert("Successfully run!");
                },
                error: function (e) {
                    console.log("Error" + JSON.stringify(e));
                    alert("Error! Check console log for details!")
                }
            });
        }
    </script>
</head>
<body>
    <form id="directory" method="post" runat="server">
        <div>
            <table>
                <tr>
                    <td>&nbsp;</td>
                    <td><h1>PowerShell Web Interface</h1></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <h2 align="left">Output</h2>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <p>If there are errors, they will be shown below the output.</p>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="ResultBox" TextMode="MultiLine" Width="700" Height="200" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
