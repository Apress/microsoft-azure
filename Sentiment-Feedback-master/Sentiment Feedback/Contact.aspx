<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="Sentiment_Feedback.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Please provide feedback.</h3>
    <p>
        <asp:TextBox ID="txt_Feedback" runat="server" Height="162px" Width="464px" TextMode="MultiLine"></asp:TextBox>
    </p>
    <address>
        <asp:Button ID="Btn_Submit" runat="server" OnClick="Btn_Submit_Click" Text="Submit" />
    </address>
    <address>
        Sentiment Analysis Score: <asp:TextBox ID="txt_Score" runat="server"></asp:TextBox>
    &nbsp;(value that is closer to 1 = positive)</address>
    <address>
        One Microsoft Way<br />
        Redmond, WA 98052-6399<br />
        <abbr title="Phone">P:</abbr>
        425.555.0100
    </address>

    <address>
        <strong>Support:</strong>   <a href="mailto:Support@example.com">Support@example.com</a><br />
        <strong>Marketing:</strong> <a href="mailto:Marketing@example.com">Marketing@example.com</a>
    </address>
</asp:Content>
