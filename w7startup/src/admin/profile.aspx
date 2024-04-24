<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/admin/principal.Master" AutoEventWireup="true" CodeBehind="profile.aspx.cs" Inherits="global.profile" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../js/mascara.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdfId" runat="server" />
            <!-- Title and Top Buttons Start -->
            <div class="page-title-container">
                <div class="row g-0">
                    <!-- Title Start -->
                    <div class="col-auto mb-3 mb-md-0 me-auto">
                        <div class="w-auto sw-md-30">
                            <a href="#" class="muted-link pb-1 d-inline-block breadcrumb-back">
                                <i data-acorn-icon="chevron-left" data-acorn-size="13"></i>
                                <span class="text-small align-middle">Administrador</span>
                            </a>
                            <h1 class="mb-0 pb-0 display-4" id="title">Meus Dados</h1>
                        </div>
                    </div>
                    <!-- Title End -->
                </div>
            </div>
            <!-- Title and Top Buttons End -->

            <div class="row">
                <div class="col-12 mb-5">
                    <div class="mb-3">
                        <label class="form-label">Contrato</label>
                        <asp:HyperLink ID="hplContrato" runat="server" Target="_blank">Ver Contrato</asp:HyperLink>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Nome do Cliente</label>
                        <asp:TextBox ID="txtNomeCliente" runat="server" CssClass="form-control" Required></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">CPF/CNPJ</label>
                        <asp:TextBox ID="txtCPFCNPJ" Enabled="false" onkeyup="formataCPF(this,event);" MaxLength="14" runat="server" CssClass="form-control" Required></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">RG</label>
                        <asp:TextBox ID="txtRG" onkeyup="formataRG(this,event);" MaxLength="12" runat="server" CssClass="form-control" ></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Celular</label>
                        <asp:TextBox ID="txtCelular" onkeyup="formataTelefone(this,event);" MaxLength="15" runat="server" CssClass="form-control" Required></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">E-mail</label>
                        <asp:TextBox ID="txtEmail" Enabled="false" runat="server" CssClass="form-control" Required></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">CEP</label>
                        <asp:TextBox ID="txtCEP" onkeyup="formataCEP(this,event);" MaxLength="10" runat="server" CssClass="form-control" AutoPostBack="True" OnTextChanged="txtCEP_TextChanged" Required></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Endereço</label>
                        <asp:TextBox ID="txtEndereco" runat="server" CssClass="form-control" Required></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Num</label>
                        <asp:TextBox ID="txtNum" runat="server" CssClass="form-control" Required></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Bairro</label>
                        <asp:TextBox ID="txtBairro" runat="server" CssClass="form-control" Required></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Complemento</label>
                        <asp:TextBox ID="txtComplemento" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Estado</label>
                        <asp:DropDownList runat="server" ID="ddlUF" CssClass="form-control">
                            <asp:ListItem Text="Acre - AC" Value="AC" />
                            <asp:ListItem Text="Alagoas - AL" Value="AL" />
                            <asp:ListItem Text="Amapá - AP" Value="AP" />
                            <asp:ListItem Text="Amazonas - AM" Value="AM" />
                            <asp:ListItem Text="Bahia - BA" Value="BA" />
                            <asp:ListItem Text="Ceará - CE" Value="CE" />
                            <asp:ListItem Text="Espiríto Santo - ES" Value="ES" />
                            <asp:ListItem Text="Goiás - GO" Value="GO" />
                            <asp:ListItem Text="Maranhão - MA" Value="MA" />
                            <asp:ListItem Text="Mato Grosso - MT" Value="MT" />
                            <asp:ListItem Text="Mato Grosso do Sul - MS" Value="MS" />
                            <asp:ListItem Text="Minas Gerais - MG" Value="MG" />
                            <asp:ListItem Text="Pará - PA" Value="PA" />
                            <asp:ListItem Text="Paraíba - PB" Value="PB" />
                            <asp:ListItem Text="Paraná - PR" Value="PR" />
                            <asp:ListItem Text="Pernambuco - PE" Value="PE" />
                            <asp:ListItem Text="Piauí - PI" Value="PI" />
                            <asp:ListItem Text="Rio de Janeiro - RJ" Value="RJ" />
                            <asp:ListItem Text="Rio Grande do Norte - RN" Value="RN" />
                            <asp:ListItem Text="Rio Grande do Sul - RS" Value="RS" />
                            <asp:ListItem Text="Rondônia - RO" Value="RO" />
                            <asp:ListItem Text="Roraima - RR" Value="RR" />
                            <asp:ListItem Text="Santa Catarina - SC" Value="SC" />
                            <asp:ListItem Text="São Paulo - SP" Value="SP" />
                            <asp:ListItem Text="Sergipe - SE" Value="SE" />
                            <asp:ListItem Text="Tocantins - TO" Value="TO" />
                            <asp:ListItem Text="Distrito Federal - DF" Value="DF" />
                        </asp:DropDownList>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Cidade</label>
                        <asp:TextBox ID="txtCidade" runat="server" CssClass="form-control" Required></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Nova Senha</label>
                        <asp:TextBox ID="txtNovaSenha" TextMode="Password" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="modal-footer border-0">
                        <asp:Label ID="lblMensagem" runat="server" Text=""></asp:Label>
                        <br />
                        <asp:Button ID="btnSalvar" CssClass="btn btn-icon btn-icon-end btn-success" runat="server" Text="Salvar" OnClick="btnSalvar_Click1" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
