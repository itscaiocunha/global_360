<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/admin/principal.Master" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="global.dashboard" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Title and Top Buttons Start -->
    <div class="page-title-container">
        <div class="row">
            <!-- Title Start -->
            <div class="col-12 col-md-7">
                <a class="muted-link pb-2 d-inline-block hidden" href="#">
                    <span class="align-middle lh-1 text-small">&nbsp;</span>
                </a>
                <h1 class="mb-0 pb-0 display-4" id="title">Bem-vindo,
                    <asp:Label ID="lblNomeUsuario" runat="server" Text=""></asp:Label>!</h1>
            </div>
            <!-- Title End -->
        </div>
    </div>
    <!-- Title and Top Buttons End -->

    <!-- Stats Start -->
    <div class="row">
        <div class="col-12">
            <div class="mb-5">
                <div class="row g-2">
                    <div class="col-6 col-md-4 col-lg-2">
                        <div class="card h-100 hover-scale-up cursor-pointer">
                            <div class="card-body d-flex flex-column align-items-center">
                                <div class="sw-6 sh-6 rounded-xl d-flex justify-content-center align-items-center border border-primary mb-4">
                                    <i data-acorn-icon="user" class="text-primary"></i>
                                </div>
                                <div class="mb-1 d-flex align-items-center text-alternate text-small lh-1-25">Distribuidores</div>
                                <div class="text-primary cta-4">
                                    <asp:Label ID="lblQtdeDistribuidor" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-md-4 col-lg-2">
                        <div class="card h-100 hover-scale-up cursor-pointer">
                            <div class="card-body d-flex flex-column align-items-center">
                                <div class="sw-6 sh-6 rounded-xl d-flex justify-content-center align-items-center border border-primary mb-4">
                                    <i data-acorn-icon="user" class="text-primary"></i>
                                </div>
                                <div class="mb-1 d-flex align-items-center text-alternate text-small lh-1-25">Lojistas</div>
                                <div class="text-primary cta-4">
                                    <asp:Label ID="lblQtdeLojista" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-md-4 col-lg-2">
                        <div class="card h-100 hover-scale-up cursor-pointer">
                            <div class="card-body d-flex flex-column align-items-center">
                                <div class="sw-6 sh-6 rounded-xl d-flex justify-content-center align-items-center border border-primary mb-4">
                                    <i data-acorn-icon="user" class="text-primary"></i>
                                </div>
                                <div class="mb-1 d-flex align-items-center text-alternate text-small lh-1-25">Consumidores</div>
                                <div class="text-primary cta-4">
                                    <asp:Label ID="lblQtdeCliente" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-md-4 col-lg-2">
                        <div class="card h-100 hover-scale-up cursor-pointer">
                            <div class="card-body d-flex flex-column align-items-center">
                                <div class="sw-6 sh-6 rounded-xl d-flex justify-content-center align-items-center border border-primary mb-4">
                                    <i data-acorn-icon="dollar" class="text-primary"></i>
                                </div>
                                <div class="mb-1 d-flex align-items-center text-alternate text-small lh-1-25">Faturamento</div>
                                <div class="text-primary cta-4">
                                    <asp:Label ID="lblQtdeFaturamento" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-md-4 col-lg-2">
                        <div class="card h-100 hover-scale-up cursor-pointer">
                            <div class="card-body d-flex flex-column align-items-center">
                                <div class="sw-6 sh-6 rounded-xl d-flex justify-content-center align-items-center border border-primary mb-4">
                                    <i data-acorn-icon="tag" class="text-primary"></i>
                                </div>
                                <div class="mb-1 d-flex align-items-center text-alternate text-small lh-1-25">Produtos</div>
                                <div class="text-primary cta-4">
                                    <asp:Label ID="lblQtdeProdutos" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-md-4 col-lg-2">
                        <div class="card h-100 hover-scale-up cursor-pointer">
                            <div class="card-body d-flex flex-column align-items-center">
                                <div class="sw-6 sh-6 rounded-xl d-flex justify-content-center align-items-center border border-primary mb-4">
                                    <i data-acorn-icon="dollar" class="text-primary"></i>
                                </div>
                                <div class="mb-1 d-flex align-items-center text-alternate text-small lh-1-25">Vendas</div>
                                <div class="text-primary cta-4">
                                    <asp:Label ID="lblQtdeVendas" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Stats End -->

        <!-- Recent Orders Start -->
        <div class="col-xl-12 mb-12">
    <h2 class="small-title">Últimos Pedidos Realizados</h2>
    <div class="card sh-35 h-xl-100-card">
        <div class="card-body scroll-out h-100">
            <div class="scroll h-100">
                    <asp:GridView ID="gdvDados" runat="server" Width="100%" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" EmptyDataText="Não há dados para visualizar" DataSourceID="sdsDados">
                        <Columns>

                            <asp:BoundField DataField="vendedor" HeaderText="Vendedor" SortExpression="vendedor" />
<asp:BoundField DataField="comprador" HeaderText="Comprador" SortExpression="comprador" />
<asp:BoundField DataField="titulo" HeaderText="Produto" SortExpression="titulo" />
<asp:BoundField DataField="valor" HeaderText="Valor" DataFormatString="{0:c2}" SortExpression="valor" />
<asp:BoundField DataField="qtde" HeaderText="Quant." SortExpression="qtde" />
<asp:BoundField DataField="datacadastro" HeaderText="Data do Pedido" SortExpression="datacadastro" />
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
                    <asp:SqlDataSource ID="sdsDados" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select top 20 c2.nomecompleto as vendedor, c.nomecompleto as comprador, prod.titulo, pp.valor, pp.qtde, FORMAT(p.DATACADASTRO,'dd/MM/yyyy') as datacadastro from pedido p
join pedido_produto pp on pp.idpedido = p.id
join produto prod on prod.id = pp.idproduto
join cliente c on c.id = p.idconsumidor
join cliente c2 on c2.id = p.idlojista
order by datacadastro desc"></asp:SqlDataSource>
                </div>
            </div>
        </div>
            </div>
        <!-- Recent Orders End -->
    <hr />
    <div class="col-xl-10 mb-5">
    <h2 class="small-title">Últimos Lojistas/Distribuidores Cadastrados</h2>
    <div class="card sh-35 h-xl-100-card">
        <div class="card-body scroll-out h-100">
            <div class="scroll h-100">
                    <asp:GridView ID="gdvDados1" runat="server" Width="100%" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" EmptyDataText="Não há dados para visualizar" DataSourceID="sdsLojistas">
                        <Columns>

                            <asp:BoundField DataField="cnpj_cpf" HeaderText="CNPJ" SortExpression="cnpj_cpf" />
                            <asp:BoundField DataField="razao_social" HeaderText="Razão Social" SortExpression="razao_social" />
                            <asp:BoundField DataField="nomecompleto" HeaderText="Nome Fantasia" SortExpression="nomecompleto" />
                            <asp:BoundField DataField="cidade" HeaderText="Cidade" SortExpression="cidade" />
                            <asp:BoundField DataField="estado" HeaderText="UF" SortExpression="estado" />
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
                    <asp:SqlDataSource ID="sdsLojistas" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select top 20 cnpj_cpf, razao_social, nomecompleto, cidade, estado from cliente
where idtipocliente in(1,2)
order by datacadastro desc"></asp:SqlDataSource>
                </div>
            </div>
        </div>
      </div>
        <!-- Top Selling Items End -->

        <!-- Top Search Terms Start -->
        <div class="col-xl-10 mb-5">
            <h2 class="small-title">Últimos Clientes Cadastrados</h2>
            <div class="card sh-35 h-xl-100-card">
                <div class="card-body scroll-out h-100">
                    <div class="scroll h-100">
                        <asp:GridView ID="gdvDados2" runat="server" Width="100%" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" EmptyDataText="Não há dados para visualizar" DataSourceID="sdsClientes">
                            <Columns>

                                <asp:BoundField DataField="cnpj_cpf" HeaderText="CPf" SortExpression="cnpj_cpf" />
                                <asp:BoundField DataField="nomecompleto" HeaderText="Nome Completo" SortExpression="nomecompleto" />
                                <asp:BoundField DataField="cidade" HeaderText="Cidade" SortExpression="cidade" />
                                <asp:BoundField DataField="estado" HeaderText="UF" SortExpression="estado" />
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
                        <asp:SqlDataSource ID="sdsClientes" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select top 20 cnpj_cpf, nomecompleto, cidade, estado from cliente
where idtipocliente in(3)
order by datacadastro desc"></asp:SqlDataSource>
                    </div>
                </div>
            </div>
        </div>
        <!-- Top Search Terms End -->
</asp:Content>
