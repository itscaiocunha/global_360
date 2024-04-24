<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/geral.Master" AutoEventWireup="true" CodeBehind="sessao.aspx.cs" Inherits="global.sessao" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="padding: 50px; max-width: 100%">
        <div style="margin-bottom: 25px">
            <img src="img/logo/logo_global.png" class="sw-17" alt="logo" />
        </div>
        <div class="mb-3">
            <label class="form-label">Sua sessão de acesso expirou. Acesse novamente!</label>
        </div>     
        <asp:Button ID="btnSalvar" CssClass="btn btn-icon btn-icon-end btn-primary" runat="server" Text="Ir para Login" OnClick="btnSalvar_Click"/>       
        
    </div>
</asp:Content>
