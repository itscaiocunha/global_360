<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/geral.Master" AutoEventWireup="true" CodeBehind="cadastro.aspx.cs" Inherits="global.cadastro" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="js/mascara.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div style="padding: 50px; max-width: 100%">
                <div style="margin-bottom: 25px">
                    <img src="img/logo/logo_global.png" class="sw-25" alt="logo" />
                </div>
                <h2>Cadastro na Plataforma Digital</h2>
                <br />
                <div class="mb-3 w-100">
                    <label class="form-label">Escolha uma opção</label>
                    <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control shadow dropdown-menu-end" AutoPostBack="true" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged">
                        <asp:ListItem Text="Pessoa Jurídica" Value="PJ" CssClass="dropdown-item"></asp:ListItem>
                        <asp:ListItem Text="Pessoa Física" Value="PF" CssClass="dropdown-item"></asp:ListItem>

                    </asp:DropDownList>
                </div>

                <%-- PJ --%>
                <asp:Panel ID="pnlPJ" runat="server">
                    <div class="mb-3">
                        <label class="form-label">Nome Fantasia</label>
                        <asp:TextBox ID="txtNomeFantasia" runat="server" CssClass="form-control" Required></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Razão Social</label>
                        <asp:TextBox ID="txtRazaoSocial" runat="server" CssClass="form-control" Required></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">CNPJ</label>
                        <asp:TextBox ID="txtCNPJ" onkeyup="formataCNPJ(this,event);" MaxLength="18" runat="server" CssClass="form-control" placeholder="00.000.000/0000-00" Required></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Inscrição Estadual</label>
                        <asp:TextBox ID="txtIE" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                   <div class="mb-3">
                     <label class="form-label">Qtde de Lojistas</label>
                     <asp:TextBox ID="txtQtdeLojistas" onkeyup="formataNumero(this,event);" MaxLength="15" runat="server" CssClass="form-control"></asp:TextBox>
                   </div>
                </asp:Panel>

                <%-- PF --%>
                <asp:Panel ID="pnlPF" runat="server" Visible="false">
                    <div class="mb-3">
                        <label class="form-label">Nome Completo</label>
                        <asp:TextBox ID="txtNomeCompleto" runat="server" CssClass="form-control" Required></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">CPF</label>
                        <asp:TextBox ID="txtCPF" onkeyup="formataCPF(this,event);" MaxLength="18" runat="server" CssClass="form-control" placeholder="000.000.000-00" Required></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">RG</label>
                        <asp:TextBox ID="txtRG" runat="server" CssClass="form-control" placeholder="00.000.000-0" Required></asp:TextBox>
                    </div>
                </asp:Panel>

                <%-- Tipo de Cadastro --%>
                <hr />
                <div class="mb-3 w-100">
                    <label class="form-label">Tipo de Cadastro</label>
                    <asp:DropDownList ID="ddlTipoCliente" runat="server" CssClass="form-control shadow dropdown-menu-end" DataSourceID="sdsTipoCliente" DataTextField="descricao" DataValueField="id" AppendDataBoundItems="True">
                        <asp:ListItem Text="Selecionar Cadastro" Value="" />
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="sdsTipoCliente" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select id, descricao from tipo_cliente where status = 'ATIVO'"></asp:SqlDataSource>
                </div>
                <div class="mb-3">
                    <label class="form-label">Celular</label>
                    <asp:TextBox ID="txtCelular" onkeyup="formataTelefone(this,event);" MaxLength="15" runat="server" CssClass="form-control" placeholder="(00) 00000-0000" Required></asp:TextBox>
                </div>
                <div class="mb-3">
                    <label class="form-label">E-mail</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="seu@exemplo.com" Required></asp:TextBox>
                </div>
                <div class="mb-3">
                    <label class="form-label">CEP</label>
                    <asp:TextBox ID="txtCEP" onkeyup="formataCEP(this,event);" MaxLength="10" runat="server" CssClass="form-control" AutoPostBack="True" OnTextChanged="txtCEP_TextChanged" placeholder="000.000.000-00" Required></asp:TextBox>
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
                    <label class="form-label">Senha</label>
                    <asp:TextBox ID="txtSenha" TextMode="Password" runat="server" CssClass="form-control" placeholder="Mínimo de 6 dígitos" Required></asp:TextBox>
                </div>
                <div class="mb-3 w-100">
                    <label class="form-label">Status</label>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control shadow dropdown-menu-end">
                        <asp:ListItem Text="Ativo" CssClass="dropdown-item"></asp:ListItem>
                        <asp:ListItem Text="Inativo" CssClass="dropdown-item"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="mb-3">
                    <label class="form-label">
                        Concluíndo este cadastrado, está confirmando que aceita as políticas do
                        <asp:HyperLink ID="hplTermoUso" NavigateUrl="https://www.global360.app.br/src/termo.pdf" runat="server">Termo de Uso</asp:HyperLink>
                        do sistema</label>

                </div>
                <div class="" style="text-align: right">
                    <asp:Label ID="lblMensagem" Font-Size="1.5em" runat="server" Text=""></asp:Label>
                </div>
                <div class="mb-3" style="text-align: right">
                    <asp:LinkButton ID="lkbJatenhoconta" CssClass="" runat="server" OnClick="lkbJatenhoconta_Click">Já possuo conta!</asp:LinkButton>
                </div>
                <asp:Button ID="btnSalvar" CssClass="btn btn-icon btn-icon-end btn-success" runat="server" Text="Salvar" OnClick="btnSalvar_Click1" />
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
