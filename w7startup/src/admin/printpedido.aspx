<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/admin/principal.Master" AutoEventWireup="true" CodeBehind="printpedido.aspx.cs" Inherits="global.printpedido" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <asp:HiddenField ID="hdfId" runat="server" />            
    <!-- Title and Top Buttons Start -->
    <div class="page-title-container">
        <div class="row">
                       <!-- Top Buttons Start -->
            <div class="col-12 col-md-10 d-flex align-items-center justify-content-end">
                <a onclick="window.print(); return false;" class="btn btn-outline-primary btn-icon btn-icon-start w-100 w-md-auto" href="#">
                    <i data-acorn-icon="print"></i>
                    <span>Imprimir</span>
                </a>
            </div>
            <!-- Top Buttons End -->
        </div>
    </div>
    <!-- Title and Top Buttons End -->

    <div class="card mb-5 card-print print-me">
        <div class="card-body">
            <div class="row d-flex flex-row align-items-center">
                <div class="col-12 col-md-6">
                    <img src="../img/logo/logo_global.png" class="sw-17" alt="logo" />
                </div>
                <div class="col-12 col-md-6 text-end">
                    <div class="mb-2">
                        <asp:Label ID="lblRazaoSocial" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="text-small text-muted">
                        <asp:Label ID="lblCidadeUf" runat="server" Text=""></asp:Label></div>
                    <div class="text-small text-muted">
                        <asp:Label ID="lblTelefone" runat="server" Text=""></asp:Label></div>
                </div>
            </div>
            <div class="separator separator-light mt-5 mb-5"></div>
            <div class="row g-1 mb-5">
                <div class="col-12 col-md-8">
                    <div class="py-3">
                        <div>
                            <asp:Label ID="lblCliente" runat="server" Text=""></asp:Label></div>
                        <div>
                            <asp:Label ID="lblEndereco" runat="server" Text=""></asp:Label></div>
                        <div>
    Prazo de Entrega: <asp:Label ID="lblPrazoEntrega" runat="server" Text=""></asp:Label></div>
                        <div>
    Data da Entrega: <asp:Label ID="lblDataEntrega" runat="server" Text=""></asp:Label></div>
                        <div>
    Rastreio: <asp:Label ID="lblRastreio" runat="server" Text=""></asp:Label></div>
                    </div>
                </div>
                <div class="col-12 col-md-4">
                    <div class="py-3 text-md-end">
                        <div>Pedido #: 
                            <asp:Label ID="lblNumeroPedido" runat="server" Text=""></asp:Label></div>
                        <div>Valor total R$ 
    <asp:Label ID="lblValorTotal" runat="server" Text=""></asp:Label></div>
                        <div>Nota Fiscal: 
   <asp:HyperLink ID="hplNF" runat="server" Target="_blank">Visualizar NF</asp:HyperLink></div>
                        <div>
     <asp:Label ID="lblStatusAtual" runat="server" Text=""></asp:Label></div>
                        <div>
                            <asp:Label ID="lblDataPedido" runat="server" Text=""></asp:Label></div>
                    </div>
                </div>
            </div>

            <div>
                 <h2 class="small-title">Lista de Produtos</h2> 
                <asp:GridView ID="gdvDados" runat="server" Width="100%" ShowHeader="false" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" EmptyDataText="Não há dados para visualizar" DataSourceID="sdsDados">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div style="padding-top: 0px; padding-bottom: 5px">
                                    <h6 style="font-size: 14px; line-height: 1; margin-bottom: 0; color: #4e4e4e; font-weight: 500; margin-top: 10px">
                                        <%# Eval("titulo") %>
  </h6>
                                </div>
                                <div>
                                    <p style="font-size: 14px; text-decoration: none; line-height: 1; color: #7c7c7c; margin-top: 0px; margin-bottom: 0">IMEI: <%# Eval("ean") %> - Ano de Fabricação: <%# Eval("ano_fabricacao") %>  - Ano do Modelo: <%# Eval("ano_modelo") %> - Chassi: <%# Eval("chassi") %>  - Cor: <%# Eval("cor") %> - Marca: <%# Eval("marca") %> - Modelo: <%# Eval("modelo") %> - Placa: <%# Eval("placa") %> - Renavam: <%# Eval("renavam") %> </p>
                                </div>
                                <div style="padding-top: 0px; padding-bottom: 0; text-align: right">
                                    <p
                                        style="font-size: 14px; line-height: 1; color: #4e4e4e; margin-bottom: 0; margin-top: 0; vertical-align: top; white-space: nowrap;">
                                        <%# Eval("valor") %>
                                    </p>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
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
                <asp:SqlDataSource ID="sdsDados" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select pp.idpedido, pp.idproduto, p.titulo, pp.valor, pp.qtde, pp.ean, pp.ano_fabricacao, pp.ano_modelo, pp.chassi, pp.cor, pp.marca, pp.modelo, pp.placa, pp.renavam  from pedido_produto pp
join produto p on p.id = pp.idproduto
where idpedido = @id">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdfId" Name="id" PropertyName="Value" />
                    </SelectParameters>
                </asp:SqlDataSource>

                            <div class="separator separator-light mt-5 mb-5"></div>
           
            <div>
                  <h2 class="small-title">Histórico do Pedido</h2> 
                <asp:GridView ID="gdvHistorico" runat="server" Width="100%" ShowHeader="False" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" EmptyDataText="Não há dados para visualizar" DataSourceID="sdsHistorico">
                    <Columns>
                        <asp:BoundField DataField="status" HeaderText="status" SortExpression="status" />
                        <asp:BoundField DataField="datacadastro" HeaderText="datacadastro" SortExpression="datacadastro" />
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
                <asp:SqlDataSource ID="sdsHistorico" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select status, datacadastro from pedido_historico where idpedido = @id order by datacadastro desc">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hdfId" Name="id" PropertyName="Value" />
                    </SelectParameters>
                </asp:SqlDataSource>

                            <div class="separator separator-light"></div>
                <br /><br /><br />
                <div class="separator separator-light"></div>
                <p style="color: #afafaf; font-size: 11px; text-align: center">
  Pedido gerado pela plataforma digital da Global 360.
</p>
        </div>
    </div>
    </div></div>
    
    
    <!-- Standard End -->   
</asp:Content>
