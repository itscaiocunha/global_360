<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/admin/principal.Master" AutoEventWireup="true" CodeBehind="contratos.aspx.cs" Inherits="global.contratos" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                            <h1 class="mb-0 pb-0 display-4" id="title">Contratos</h1>
                        </div>
                    </div>
                    <!-- Title End -->

                    <!-- Top Buttons Start -->
                    <div class="w-100 d-md-none"></div>
                    <div class="col-12 col-sm-6 col-md-auto d-flex align-items-end justify-content-end mb-2 mb-sm-0 order-sm-3">
                        <asp:LinkButton ID="lkbAdicionarPerfil" runat="server" CssClass="btn btn-outline-primary btn-icon btn-icon-start ms-0 ms-sm-1 w-100 w-md-auto" OnClick="LinkButton1_Click">
                         <i data-acorn-icon="plus"></i> Novo Cadastro</asp:LinkButton>
                    </div>
                    <!-- Top Buttons End -->
                </div>
            </div>
            <!-- Title and Top Buttons End -->

            <!-- Controls Start -->
            <div class="row mb-2">
                <!-- Search Start -->
                <div class="col-sm-12 col-md-5 col-lg-4 col-xxl-2 mb-1">
                    <div class="">
                        <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Filtrar"></asp:TextBox>
                    </div>
                </div>
                <div class="col-sm-12 col-md-5 col-lg-4 col-xxl-2 mb-1">
                    <asp:LinkButton ID="lkbFiltro" runat="server" CssClass="btn btn-outline-primary btn-icon btn-icon-start ms-0 ms-sm-1 w-100 w-md-auto" OnClick="lkbFiltro_Click">
<i data-acorn-icon="search"></i> Filtrar</asp:LinkButton>
                </div>
                <!-- Search End -->

                <div class="col-sm-12 col-md-7 col-lg-4 col-xxl-10 text-end mb-1">
                    <div class="d-inline-block">
                        <!-- Print Button Start -->
                        <asp:LinkButton ID="btnImprimir" runat="server" CssClass="btn btn-icon btn-icon-only btn-foreground-alternate shadow"><i data-acorn-icon="print"></i></asp:LinkButton>
                        <!-- Print Button End -->

                        <!-- Export Dropdown Start -->
                        <div class="d-inline-block">
                            <button class="btn p-0" data-bs-toggle="dropdown" type="button" data-bs-offset="0,3">
                                <span
                                    class="btn btn-icon btn-icon-only btn-foreground-alternate shadow dropdown"
                                    data-bs-delay="0"
                                    data-bs-placement="top"
                                    data-bs-toggle="tooltip"
                                    title="Export">
                                    <i data-acorn-icon="download"></i>
                                </span>
                            </button>
                            <div class="dropdown-menu shadow dropdown-menu-end">
                                <asp:LinkButton ID="btnDownloadExcel" runat="server" CssClass="dropdown-item export-excel">Excel</asp:LinkButton>
                                <asp:LinkButton ID="btnDownloadPDf" runat="server" CssClass="dropdown-item export-pdf">Pdf</asp:LinkButton>
                                <asp:LinkButton ID="btnDownloadCSV" runat="server" CssClass="dropdown-item export-cvs">Csv</asp:LinkButton>
                            </div>

                        </div>
                        <!-- Export Dropdown End -->
                    </div>
                </div>
            </div>
            <!-- Controls End -->
            <!-- Discount List Start -->
            <div class="row">
                <div class="col-12 mb-5">
                    <asp:GridView ID="gdvDados" runat="server" Width="100%" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" EmptyDataText="Não há dados para visualizar" DataSourceID="sdsDados" OnRowCommand="gdvDados_RowCommand">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button data-bs-offset="0,3" data-bs-toggle="modal" data-bs-target="#discountAddModal" ID="btnEditar" CssClass="btn btn-icon btn-icon-end btn-primary" CommandArgument='<%# Eval("id") %>' CommandName="Editar" runat="server" Text="Editar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="id" HeaderText="#Id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                            <asp:BoundField DataField="descricao" HeaderText="Contrato" SortExpression="descricao" />
                            <asp:BoundField DataField="tipocliente" HeaderText="Tipo de Cliente" SortExpression="tipocliente" />
                            <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />
                        </Columns>
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
                    </asp:GridView>
                    <asp:SqlDataSource ID="sdsDados" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select c.id, c.descricao, c.conteudo, c.status, t.descricao as tipocliente, c.datacadastro from contrato c
join tipo_cliente t on t.id = c.idtipocliente"></asp:SqlDataSource>
                </div>
            </div>
            <!-- Discount Add Modal Start -->
            <asp:Panel ID="pnlModal" runat="server" CssClass="modal-right" Visible="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Adicionar Contratos</h5>
                        </div>
                        <div class="modal-body">
                            <div class="mb-3">
                                <label class="form-label">Título do Contrato</label>
                                <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                             <div class="mb-3">
     <label class="form-label">Parâmetros do Sistema</label>
                         <asp:GridView ID="GridView1" runat="server" Width="100%" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" EmptyDataText="Não há dados para visualizar" DataSourceID="sdsParametros">
                        <Columns>                            
                            <asp:BoundField DataField="descricao" HeaderText="Parâmetro" SortExpression="descricao" />
                            <asp:BoundField DataField="observacao" HeaderText="Descrição" SortExpression="observacao" />
                        </Columns>
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
                    </asp:GridView>
                    <asp:SqlDataSource ID="sdsParametros" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select * from parametro where status = 'ATIVO'"></asp:SqlDataSource>
 </div>
                            <div class="mb-3">
                                <label class="form-label">Conteúdo</label>
                                <asp:TextBox ID="txtConteudo" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="50"></asp:TextBox>
                            </div>
                            <div class="mb-3 w-100">
                                <label class="form-label">Status</label>
                                <asp:DropDownList ID="ddlTipoCliente" runat="server" CssClass="form-control shadow dropdown-menu-end" DataSourceID="sdsTipoCliente" DataTextField="descricao" DataValueField="id">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="sdsTipoCliente" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select id, descricao from tipo_cliente"></asp:SqlDataSource>
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
                            <asp:Button ID="btnSalvar" CssClass="btn btn-icon btn-icon-end btn-success" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
                            <asp:LinkButton ID="lkbFechar" runat="server" CssClass="btn btn-danger btn-icon btn-icon-start" OnClick="lkbFechar_Click">
<i data-acorn-icon="close"></i> Fechar </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- Discount Add Modal End -->
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
