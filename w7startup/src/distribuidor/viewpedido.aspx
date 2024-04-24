<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/distribuidor/principal.Master" AutoEventWireup="true" CodeBehind="viewpedido.aspx.cs" Inherits="global.distribuidor.viewpedido" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfId" runat="server" />            
    <!-- Title and Top Buttons Start -->
    <div class="page-title-container">
        <div class="row">
            <!-- Title Start -->
            <div class="col-auto mb-3 mb-md-0 me-auto">
                <div class="w-auto sw-md-30">
                    <a href="#" class="muted-link pb-1 d-inline-block breadcrumb-back">
                        <i data-acorn-icon="chevron-left" data-acorn-size="13"></i>
                        <span class="text-small align-middle">Cliente</span>
                    </a>
                    <h1 class="mb-0 pb-0 display-4" id="title">Pedido</h1>
                </div>
            </div>
            <!-- Title End -->

        </div>
    </div>
    <!-- Title and Top Buttons End -->

    <!-- Standard Start -->
    <!-- card-print: removes shadow, margin and padding  -->
    <!-- print-me: removes everyting from main .container except the element with the class if main tag has print-restricted class -->
    <h2 class="small-title">Detalhes do Pedido</h2>
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
                                    <p style="font-size: 14px; text-decoration: none; line-height: 1; color: #7c7c7c; margin-top: 0px; margin-bottom: 0"><%# Eval("ean") %> unidade(s)</p>
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
                <asp:SqlDataSource ID="sdsDados" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select pp.idpedido, pp.idproduto, p.titulo, pp.valor, pp.qtde, pp.ean from pedido_produto pp
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

                </div>
                
                <br /><br />
                <h2>Atualizar Informações</h2>
                                    <div class="mb-3">
        <label class="form-label">Link da Nf</label>
        <asp:TextBox ID="txtLinkNF" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="mb-3">
        <label class="form-label">Data de Entrega</label>
        <asp:TextBox ID="txtDataEntrega" onkeyup="formataData(this,event);" MaxLength="10" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="mb-3">
        <label class="form-label">Prazo de Entrega</label>
        <asp:TextBox ID="txtPrazo" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
                         <div class="mb-3">
     <label class="form-label">Rastreio</label>
     <asp:TextBox ID="txtRastreio" runat="server" CssClass="form-control"></asp:TextBox>
 </div>   
    <div class="mb-3 w-100">
        <label class="form-label">Novo Status</label>
        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control shadow dropdown-menu-end">
            <asp:ListItem Text="Aguardando Pagamento" CssClass="dropdown-item"></asp:ListItem>
            <asp:ListItem Text="Em Rota" CssClass="dropdown-item"></asp:ListItem>
            <asp:ListItem Text="Entrega Realizada" CssClass="dropdown-item"></asp:ListItem>
            <asp:ListItem Text="Cancelado" CssClass="dropdown-item"></asp:ListItem>
            <asp:ListItem Text="Aguardando Envio" CssClass="dropdown-item"></asp:ListItem>
            <asp:ListItem Text="Pagamento Realizado" CssClass="dropdown-item"></asp:ListItem>
        </asp:DropDownList>
    </div>
</div>
                        <div class="modal-footer border-0">
                            <asp:Label ID="lblMensagem" runat="server" Text=""></asp:Label>
                            <br />
                            <asp:Button ID="btnSalvar" CssClass="btn btn-icon btn-icon-end btn-success" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
    

                <div class="separator separator-light"></div>
                <p style="color: #afafaf; font-size: 11px; text-align: center">
  Pedido gerado pela plataforma digital da Global 360.
</p>
        </div>
    </div>
    </div>
    
    <!-- Standard End -->   
</asp:Content>
