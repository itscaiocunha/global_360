<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/admin/principal.Master" AutoEventWireup="true" CodeBehind="estoque.aspx.cs" Inherits="global.estoque" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../js/mascara.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <contenttemplate>
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
                            <h1 class="mb-0 pb-0 display-4" id="title">Estoque</h1>
                        </div>
                    </div>
                    <!-- Title End -->                   
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
                        <i data-acorn-icon="search"></i>Filtrar Produto</asp:LinkButton>
                </div>
                <!-- Search End -->

                <div class="col-sm-12 col-md-7 col-lg-4 col-xxl-10 text-end mb-1">
                    <div class="d-inline-block">
                        <%--<!-- Print Button Start -->
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
                            </div>--%>

                        </div>
                        <!-- Export Dropdown End -->
                    </div>
                </div>
            </div>
            <!-- Controls End -->
            <!-- Discount List Start -->
            <div class="row">
                <div class="col-12 mb-5">
                    <asp:GridView ID="gdvDados" runat="server" Width="100%" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" EmptyDataText="Não há dados para visualizar" DataSourceID="sdsDados">
                        <columns>                            
                            <asp:BoundField DataField="nomecompleto" HeaderText="Vendedor" SortExpression="nomecompleto" ReadOnly="True" />
                            <asp:BoundField DataField="produto" HeaderText="Produto" SortExpression="produto" ReadOnly="True" />
                            <asp:BoundField DataField="lote" HeaderText="Lote" SortExpression="lote" ReadOnly="True" />
                            <asp:BoundField DataField="qtde_distribuidor" HeaderText="Quant. Distribuidor" SortExpression="qtde_distribuidor" ReadOnly="True" />
                            
                            <asp:BoundField DataField="qtde_lojista" HeaderText="Quant. Lojista" SortExpression="qtde_lojista" ReadOnly="True" />
                            <asp:BoundField DataField="qtde_cliente" HeaderText="Quant. Cliente" SortExpression="qtde_cliente" ReadOnly="True" />
                        </columns>
                        <editrowstyle backcolor="#7C6F57" />
                        <footerstyle backcolor="#1C5E55" font-bold="True" forecolor="White" />
                        <headerstyle />
                        <pagerstyle backcolor="#666666" forecolor="White" horizontalalign="Center" />
                        <rowstyle height="4em" backcolor="White" forecolor="#a59e9e" cssclass="fix-margin" />
                        <selectedrowstyle backcolor="#C5BBAF" font-bold="True" forecolor="#333333" />
                        <sortedascendingcellstyle backcolor="#F8FAFA" />
                        <sortedascendingheaderstyle backcolor="#246B61" />
                        <sorteddescendingcellstyle backcolor="#D4DFE1" />
                        <sorteddescendingheaderstyle backcolor="#15524A" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="sdsDados" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand=
                        "select idlojista, idconsumidor, max(nomecompleto) as nomecompleto, max(produto) as produto, SUM(qtde_distribuidor) as qtde_distribuidor, lote, sum(qtde_lojista) as qtde_lojista, sum(qtde_cliente) as qtde_cliente from (
                        select p.idlojista, p.idconsumidor, max(c.nomecompleto) as nomecompleto, max(prod.titulo) as produto, SUM(qtde) as qtde_distribuidor, pp.lote, 0 as qtde_lojista, 0 as qtde_cliente from pedido p 
                        join pedido_produto pp on pp.idpedido = p.id 
                        join produto prod on prod.id = pp.idproduto
                        join cliente c on c.id = p.idlojista and idtipocliente = 1
                        group by p.idlojista, p.idconsumidor, pp.lote
                        union all
                        select p.idlojista, p.idconsumidor,max(c.nomecompleto) as nomecompleto, max(prod.titulo) as produto, SUM(qtde) as qtde_distribuidor, pp.lote, 0 as qtde_lojista, 0 as qtde_cliente from pedido p 
                        join pedido_produto pp on pp.idpedido = p.id 
                        join produto prod on prod.id = pp.idproduto 
                        join cliente c on c.id = p.idlojista and idtipocliente = 2
                        group by p.idlojista, p.idconsumidor, pp.lote
                        union all
                        select p.idlojista, p.idconsumidor, max(c.nomecompleto) as nomecompleto, max(prod.titulo) as produto, SUM(qtde) as qtde_distribuidor, pp.lote, 0 as qtde_lojista, 0 as qtde_cliente from pedido p 
                        join pedido_produto pp on pp.idpedido = p.id 
                        join produto prod on prod.id = pp.idproduto
                        join cliente c on c.id = p.idlojista and idtipocliente = 3
                        group by p.idlojista, p.idconsumidor, pp.lote) as tab
                        group by idlojista, idconsumidor, lote">
                    </asp:SqlDataSource>
                </div>
            </div>
           
        </contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
