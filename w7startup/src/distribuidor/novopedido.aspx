<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/distribuidor/principal.Master" AutoEventWireup="true" CodeBehind="novopedido.aspx.cs" Inherits="global.distribuidor.novopedido" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../js/mascara.js"></script>
    <asp:HiddenField ID="hdfId" runat="server" />
    <asp:HiddenField ID="hdfIdProduto" runat="server" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

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
                            <h1 class="mb-0 pb-0 display-4" id="title">Número #:
                                <asp:Label ID="lblNumeroPedido" runat="server" Text=""></asp:Label></h1>
                        </div>
                    </div>
                    <!-- Title End -->
                </div>
            </div>
            <!-- Title and Top Buttons End -->

            <div id="card-body">
                <%--<div class="mb-3">
                    <h2 class="small-title">Informações Gerais</h2> 
                    <label class="form-label">Vendedor</label>
                    <asp:DropDownList ID="ddlVendedor" Enabled="false" runat="server" CssClass="form-control shadow dropdown-menu-end" DataSourceID="sdsVendedor" DataTextField="nomecompleto" DataValueField="id">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="sdsVendedor" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="select id, nomecompleto from cliente where idtipocliente in (1,2) and id = 7
order by nomecompleto"></asp:SqlDataSource>
                </div><br />
                                <div class="mb-3">
                    <label class="form-label">Comprador</label>
                    <asp:DropDownList ID="ddlCliente" Enabled="false" runat="server" CssClass="form-control shadow dropdown-menu-end" DataSourceID="sdsCliente" DataTextField="nome" DataValueField="id">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="sdsCliente" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="select id, isnull(razao_social, nomecompleto) as nome from cliente
order by nome">
                        <SelectParameters>
    <asp:ControlParameter ControlID="hdfId" Name="id" PropertyName="Value" />
</SelectParameters>
                    </asp:SqlDataSource>
                </div>--%>

                <h2 class="small-title">Carrinho de Produtos</h2>
                <div class="mb-3">
                    <label class="form-label">Produto</label>
                    <asp:DropDownList ID="ddlProduto" runat="server" CssClass="form-control shadow dropdown-menu-end" DataSourceID="sdsProduto" DataTextField="nome" DataValueField="id">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="sdsProduto" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="select id, titulo +' : R$ '+ convert(varchar, valor) as nome from produto where [status] = 'Ativo' order by nome"></asp:SqlDataSource>
                </div>

                <div class="mb-3">
                    <label class="form-label">Quant.</label>
                    <asp:TextBox ID="txtQtde" onkeyup="formataInteiro(this,event);" runat="server" Text="1" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="mb-3">
                    <label class="form-label">Lote</label>
                    <asp:TextBox ID="txtLote" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="mb-3">
                    <label class="form-label">Código Único</label>
                    <asp:TextBox ID="txtEAN" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <asp:Button ID="btnAdicionarItem" CssClass="btn btn-icon btn-icon-end btn-success" runat="server" Text="Adicionar ao carrinho" OnClick="btnAdicionarItem_Click" />
                <br />
                <br />
                <!-- car -->
                <div class="row">
                    <div class="col-12 mb-5">
                        <asp:GridView ID="gdvDados" runat="server" Width="100%" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" EmptyDataText="Não há dados produtos no carrinho" DataSourceID="sdsDados" OnRowCommand="gdvDados_RowCommand">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" ImageAlign="AbsMiddle" Width="50px" ImageUrl='<%# Eval("imagem") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="idpedido" HeaderText="#Cód." SortExpression="idpedido" />
                                <asp:BoundField DataField="titulo" HeaderText="Produto" SortExpression="titulo" />
                                <asp:BoundField DataField="qtde" HeaderText="Quant." SortExpression="qtde" />
                                <asp:BoundField DataField="valor" HeaderText="Valor" SortExpression="valor" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button data-bs-offset="0,3" data-bs-toggle="modal" data-bs-target="#discountAddModal" ID="btnEditar" CssClass="btn btn-icon btn-icon-end btn-danger" CommandArgument='<%# Eval("id") %>' CommandName="Excluir" runat="server" Text="Retirar" />
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
                        <asp:SqlDataSource ID="sdsDados" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select pp.idproduto as id, imagem, pp.idpedido, titulo, qtde, pp.valor from pedido_produto pp
join produto p on p.id = pp.idproduto 
where pp.idpedido = @id">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="lblNumeroPedido" Name="id" PropertyName="Text" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>
                                <div class="mb-3">
    <label class="form-label">Cupom</label>
    <asp:TextBox ID="txtCupom" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:Button ID="btnValidarCupom" runat="server" Text="Validar Cupom"  CssClass="btn btn-icon btn-icon-end btn-success" OnClick="btnValidarCupom_Click" />
                    <asp:Label ID="lblMsgCupom" runat="server" Text=""></asp:Label>
</div>
                <div class="mb-3">
                    <label class="form-label">Haverá Pagamento?</label>
                    <asp:CheckBox ID="CkbPagamento" runat="server" AutoPostBack="true" OnCheckedChanged="CkbPagamento_CheckedChanged" />
                </div>
                <br />
                <asp:Panel ID="pnlPagamento" runat="server" Visible="false">
                    <h2 class="small-title">Informações de Pagamento</h2>
                    <label class="form-label"><i data-acorn-icon="user" class="icon" data-acorn-size="18"></i>Número do Cartão</label>
                    <div class="form-line">
                        <asp:TextBox ID="txtNumeroCartao" MaxLength="19" onkeyup="formataCartaoCredito(this,event);" CssClass="form-control" runat="server" placeHolder="" Required></asp:TextBox>
                    </div>
                    <label class="form-label"><i data-acorn-icon="user" class="icon" data-acorn-size="18"></i>Data de Validade</label>
                    <div class="form-line">
                        <asp:TextBox ID="txtDataValidade" onkeyup="formataMesAno(this,event);" MaxLength="5" CssClass="form-control" runat="server" placeHolder="  /  " Required></asp:TextBox>
                    </div>
                    <label class="form-label"><i data-acorn-icon="user" class="icon" data-acorn-size="18"></i>CVC/CVV</label>
                    <div class="form-line">
                        <asp:TextBox ID="txtCodSegunraca" onkeyup="formataInteiro(this,event);" CssClass="form-control" runat="server" placeHolder="" MaxLength="3" Required></asp:TextBox>
                    </div>
                    <label class="form-label"><i data-acorn-icon="user" class="icon" data-acorn-size="18"></i>Nome no Cartão</label>
                    <div class="form-line">
                        <asp:TextBox ID="txtNomeCartao" CssClass="form-control" runat="server" placeHolder="" Required></asp:TextBox>
                    </div>
                </asp:Panel>
                <hr />
                <%--<div class="mb-3">
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
                    <label class="form-label">Status</label>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control shadow dropdown-menu-end">
                        <asp:ListItem Text="Aguardando Pagamento" CssClass="dropdown-item"></asp:ListItem>
                        <asp:ListItem Text="Em Rota" CssClass="dropdown-item"></asp:ListItem>
                        <asp:ListItem Text="Entrega Realizada" CssClass="dropdown-item"></asp:ListItem>
                        <asp:ListItem Text="Cancelado" CssClass="dropdown-item"></asp:ListItem>
                        <asp:ListItem Text="Aguardando Envio" CssClass="dropdown-item"></asp:ListItem>
                        <asp:ListItem Text="Pagamento Realizado" CssClass="dropdown-item"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="mb-3">
                    <label class="form-label">Observações</label>
                    <asp:TextBox ID="txtObservacoes" TextMode="MultiLine" Rows="3" MaxLength="500" runat="server" CssClass="form-control"></asp:TextBox>
                </div>--%>
            </div>
            <div class="modal-footer border-0">
                 <p>Sub-total: R$ <asp:Label ID="lblSUbTotal" runat="server" Text="0,00"></asp:Label></p>
                 <p>Desconto: R$ <asp:Label ID="lblDesconto" runat="server" Text="0,00"></asp:Label></p>
                 <p>Total: R$ <asp:Label ID="lblValorTotal" runat="server" Text="0,00"></asp:Label></p>
                <asp:Label ID="lblMensagem" runat="server" Text=""></asp:Label>
                <br />
                <asp:Button ID="btnSalvar" CssClass="btn btn-icon btn-icon-end btn-success" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
                <asp:Button ID="Button1" CssClass="btn btn-icon btn-icon-end btn-success" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
                <asp:UpdateProgress ID="LoaderBar" runat="server" DisplayAfter="300" DynamicLayout="true">
                    <ProgressTemplate>
                    <style type="text/css">
                        .updateprogress-overlay {
                             position: absolute;
                             top: 0;
                             left: 0;
                             width: 100%;
                             height: 100%;
                             background-color: rgba(0, 0, 0, 0.5);
                             z-index: 1000; 
                         }

                        .updateprogress-centered {
                            position: absolute;
                            top: 50%;
                            left: 50%;
                            transform: translate(-50%, -50%);
                            z-index: 1001; 
                        }

                        h1 {
                            font-size: 20px;
                            color: white;
                        }
                        </style>
                        <div class="updateprogress-centered">
                            <h1>Salvando... Por favor aguarde!</h1>
                        </div>
                        <div class="updateprogress-overlay"></div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
