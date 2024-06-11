<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MobileForm.aspx.cs" Inherits="WebApp.MobileForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td colspan="2">
                    <asp:Button ID="butLogin" runat="server" Text="LoginXML" OnClick="butLogin_Click" />
                    <asp:Button ID="butBillerList" runat="server" Text="Biller List" OnClick="butBiller_Click" />
                    <asp:Button ID="butInquiry" runat="server" Text="Inquiry" OnClick="butInquiry_Click" />
                    <asp:Button ID="butConfirm" runat="server" Text="Confirm" OnClick="butConfirm_Click" />
                    <asp:Button ID="butTxnList" runat="server" Text="Txn List" OnClick="butTxnList_Click" />
                    <asp:Button ID="buttxnDetail" runat="server" Text="Txn Detail" OnClick="buttxnDetail_Click" />
                    <asp:Button ID="butBatchList" runat="server" Text="Batch List" OnClick="butBatchList_Click" />
                    <asp:Button ID="butBatchDetail" runat="server" Text="Batch Detail" OnClick="butBatchDetail_Click" />
                    <asp:Button ID="butCloseShift" runat="server" Text="Close Shift" OnClick="butCloseShift_Click" />
                    <asp:Button ID="butResetPassword" runat="server" Text="ResetPassword" OnClick="butResetPassword_Click" />
                    <asp:Button ID="butChangePassword" runat="server" Text="ChangePassword" OnClick="butChangePassword_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    Mobile Request :
                </td>
                <td>
                    Mobile Response :
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtRequest" runat="server" TextMode="MultiLine" Width="500px" Rows="10" Wrap="false" spellcheck="false" Font-Size="Large"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtResponse" runat="server" TextMode="MultiLine" Width="500px" Rows="10" Wrap="false" spellcheck="false" Font-Size="Large"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="txtCompress" runat="server" TextMode="MultiLine" Width="500px" Rows="3" Wrap="false" spellcheck="false" Font-Size="Large"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="txtURL" runat="server" Width="490px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" text="Encode" OnClick="Button1_Click" />
                    <asp:Button ID="Button2" runat="server" text="Decode" OnClick="Button2_Click" />

                    <asp:Button ID="butSubmit" runat="server" text="Submit" OnClick="butSubmit_Click"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
