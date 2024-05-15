<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/admin/principal.Master" AutoEventWireup="true" CodeBehind="lote.aspx.cs" Inherits="global.admin.lote" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdfId" runat="server" />
            <asp:HiddenField ID="hdfIdUsuario" runat="server" />

            <!-- Title and Top Buttons Start -->
            <div class="page-title-container">
                <div class="row g-0">

                    <!-- Title -->
                    <div class="col-auto mb-3 mb-md-0 me-auto">
                        <div class="w-auto sw-md-30">
                            <a href="dashboard.aspx" class="muted-link pb-1 d-inline-block breadcrumb-back">
                                <i data-acorn-icon="chevron-left" data-acorn-size="13"></i>
                                <span class="text-small align-middle">Administrador</span>
                            </a>
                            <br />
                            <h1 class="mb-0 pb-0 display-4" id="title">Lote</h1>
                            <br />
                        </div>
                    </div>

                    <!-- Teste -->
                    <asp:Label ID="lblTeste" runat="server"></asp:Label> 

                    <!-- Add Lote -->
                    <div class="w-100 d-md-none"></div>
                    <div class="col-12 col-sm-6 col-md-auto d-flex align-items-end justify-content-end mb-2 mb-sm-0 order-sm-3">
                        <asp:LinkButton ID="lkbAdicionarPerfil" runat="server" CssClass="btn btn-outline-primary btn-icon btn-icon-start ms-0 ms-sm-1 w-100 w-md-auto" OnClick="LinkButton1_Click">
                         <i data-acorn-icon="plus"></i> Novo Lote</asp:LinkButton>                       
                    </div>
                </div>
            </div>

            <!-- Search -->
            <div class="row mb-2">
                <div class="col-sm-12 col-md-5 col-lg-4 col-xxl-2 mb-1">
                    <div class="">
                        <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Filtrar"></asp:TextBox>
                    </div>                        
                </div>
                <div class="col-sm-12 col-md-5 col-lg-4 col-xxl-2 mb-1">
                    <asp:LinkButton ID="lkbFiltro" runat="server" CssClass="btn btn-outline-primary btn-icon btn-icon-start ms-0 ms-sm-1 w-100 w-md-auto" OnClick="lkbFiltro_Click">
                    <i data-acorn-icon="search"></i> Filtrar</asp:LinkButton>     
                </div>
                <br />
            </div>

            <!-- Grid -->
            <div class="row">
                <div class="col-12 mb-5">
                    <asp:GridView ID="gdvDados" runat="server" Width="100%" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" EmptyDataText="Não há dados para visualizar" DataSourceID="sdsDados" OnRowCommand="gdvDados_RowCommand">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                            <asp:Button data-bs-offset="0,3" data-bs-toggle="modal" data-bs-target="#discountAddModal" ID="btnEditar" CssClass="btn btn-icon btn-icon-end btn-primary" CommandArgument='<%# Eval("idlote") %>' CommandName="Editar" runat="server" Text="Editar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="numlote" HeaderText="Número Lote" SortExpression="numlote" />
                            <asp:BoundField DataField="name_produto" HeaderText="Produto" SortExpression="name_produto" />
                            <asp:BoundField DataField="IMEI" HeaderText="IMEI" SortExpression="IMEI" />
                            <asp:BoundField DataField="quantidade" HeaderText="Quantidade" SortExpression="quantidade" />  
                            <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />
                            <asp:BoundField DataField="data_criacao" HeaderText="Desde de" SortExpression="data_criacao" />
                             <asp:TemplateField>
                                <ItemTemplate>
                                    <EditRowStyle BackColor="#7C6F57" />
                                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle />
                                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle Height="4em" BackColor="White" ForeColor="#a59e9e" CssClass="fix-margin" />
                                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                <asp:SqlDataSource ID="sdsDados" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand=
                    "select l.idlote, l.numlote, p.titulo as name_produto, l.IMEI, l.quantidade, l.[status], l.data_criacao from lote l  join produto p on l.name_produto = p.id where l.status = 'Ativo' order by l.name_produto DESC "></asp:SqlDataSource>
                </div>
            </div>

            <!-- Discount Add Modal Start -->
            <asp:Panel ID="pnlModal" runat="server" CssClass="modal-right" Visible="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Adicionar Lote</h5>
                        </div>
                        <div class="modal-body">
                            <div class="mb-3">
                                <label class="form-label">Número do Lote</label>
                                <asp:TextBox ID="txtLote" runat="server" CssClass="form-control" Required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Nome do Produto</label>
                                <asp:DropDownList ID="ddlProduto" runat="server" CssClass="form-control shadow dropdown-menu-end" DataSourceID="sdsProduto" DataTextField="nome" DataValueField="id"></asp:DropDownList>
                                    <asp:SqlDataSource ID="sdsProduto" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="select id, titulo +' : R$ '+ convert(varchar, valor) as nome from produto where status = 'Ativo'order by nome">
                                    </asp:SqlDataSource>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">IMEI</label>
                                <asp:TextBox ID="txtIMEI" runat="server" CssClass="form-control" Required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Quantidade</label>
                                <asp:TextBox ID="txtQuantidade" runat="server" CssClass="form-control" Required></asp:TextBox>
                            </div>
                            <div class="mb-3 w-100">
                                <label class="form-label">Status</label>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control shadow dropdown-menu-end">
                                    <asp:ListItem Text="Ativo" CssClass="dropdown-item"></asp:ListItem>
                                    <asp:ListItem Text="Inativo" CssClass="dropdown-item"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="modal-footer border-0">
                            <asp:Label ID="lblMensagem" runat="server" Text=""></asp:Label>
                            <br />
                            <asp:LinkButton ID="lkbFechar" runat="server" CssClass="btn btn-danger btn-icon btn-icon-start" OnClick="lkbFechar_Click"><i data-acorn-icon="close"></i> Fechar </asp:LinkButton>         
                            <asp:Button ID="btnSalvar" CssClass="btn btn-icon btn-icon-end btn-success" runat="server" Text="Salvar" OnClick="btnSalvar_Click1" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
