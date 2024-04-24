<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/geral.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="global.login" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="padding: 50px; max-width: 100%">
        <div style="margin-bottom: 25px">
            <img src="img/logo/logo_global.png" class="sw-17" alt="logo" />
        </div>
         <h2 class="small-title">Acesso a Plataforma Global 360</h2>
        <div class="mb-3">
            <label class="form-label">E-mail</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Required></asp:TextBox>
        </div>
        <div class="mb-3">
            <label class="form-label">Senha</label>
            <asp:TextBox ID="txtSenha" TextMode="Password" runat="server" CssClass="form-control" Required></asp:TextBox>
        </div>
        <div class="mb-3" style="text-align: left">
            <asp:Label ID="lblToken" runat="server" Text=""></asp:Label>
            <asp:Label ID="lblMensagem" runat="server" Text=""></asp:Label>
            <asp:LinkButton ID="lkbJasou" CssClass="" runat="server" OnClick="lkbJasou_Click">Não tenho cadastro!</asp:LinkButton>
            </div>
        <div class="mb-3" style="text-align: right">
            <asp:LinkButton ID="lkbEsqueceuSenha" CssClass="" runat="server" OnClick="lkbEsqueceuSenha_Click">Esqueceu a senha?</asp:LinkButton>
        </div>
        <asp:Button ID="btnSalvar" CssClass="btn btn-icon btn-icon-end btn-primary" runat="server" Text="Entrar" OnClick="btnSalvar_Click1"/>       
        
    </div>
</asp:Content>
