<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/admin/principal.Master" AutoEventWireup="true" CodeBehind="distribuidor.aspx.cs" Inherits="global.admin.distribuidor" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <script src="../js/mascara.js"></script>    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdfId" runat="server" />
            <asp:HiddenField ID="hdfIdUsuario" runat="server" />
            <asp:HiddenField ID="hdfContrato" runat="server" />
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
                            <h1 class="mb-0 pb-0 display-4" id="title">Distribuidor</h1>
                        </div>
                    </div>
                    <!-- Title End -->
                    <asp:Label ID="lblTeste" runat="server"></asp:Label>
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
                    <asp:GridView ID="gdvDados" runat="server" Width="100%" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" EmptyDataText="Não há dados para visualizar" DataSourceID="sdsDados" OnRowCommand="GdvDados_RowCommand">
                        <Columns>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button data-bs-offset="0,3" data-bs-toggle="modal" data-bs-target="#discountAddModal" ID="btnExcluir" CssClass="btn btn-icon btn-icon-start btn-danger" CommandArgument='<%# Eval("id") %>' CommandName="Excluir" runat="server" Text="Excluir" style="width: 75px; height: 36px;" />
                                    <asp:UpdateProgress ID="LoaderBar" runat="server" DisplayAfter="300" DynamicLayout="true">
                                        <ProgressTemplate>
                                        <style type="text/css">
                                            .updateprogress-overlay {
                                                position: fixed;
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
                                                <h1>Excluindo...</h1>
                                            </div>
                                            <div class="updateprogress-overlay"></div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <asp:Button data-bs-offset="0,3" data-bs-toggle="modal" data-bs-target="#discountAddModal" ID="btnEditar" CssClass="btn btn-icon btn-icon-end btn-primary" CommandArgument='<%# Eval("id") %>' CommandName="Editar" runat="server" Text="Editar" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="id" HeaderText="#Cod" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                            <asp:BoundField DataField="razao_social" Visible="false" HeaderText="razao_social" SortExpression="razao_social" />
                            <asp:BoundField DataField="cnpj_cpf" HeaderText="CPF" SortExpression="cnpj_cpf" />
                            <asp:BoundField DataField="email" HeaderText="E-mail" SortExpression="email" />
                            <asp:BoundField DataField="telefone" Visible="false" HeaderText="telefone" SortExpression="telefone" />
                            <asp:BoundField DataField="celular" HeaderText="Celular" SortExpression="celular" />
                            <asp:BoundField DataField="nomecompleto" HeaderText="Distribuidor" SortExpression="nomecompleto" />
                            <asp:BoundField DataField="cidade" HeaderText="Cidade" SortExpression="cidade" />
                            <asp:BoundField DataField="estado" HeaderText="UF" SortExpression="estado" />                          
                            <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />
                            <asp:BoundField DataField="datacadastro" HeaderText="Desde de" SortExpression="datacadastro" />
                                                        <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HyperLink ID="lkbContrato" runat="server" Text="Ver Contrato" NavigateUrl='<%# "http://global360.app.br/src/admin/viewcontrato.aspx?id=" + Eval("token")%>'></asp:HyperLink></ItemTemplate>
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
                    <asp:SqlDataSource ID="sdsDados" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select * from cliente where idtipocliente = 1 order by nomecompleto"></asp:SqlDataSource>
                </div>
            </div>

            <!-- Discount Add Modal Start -->
            <asp:Panel ID="pnlModal" runat="server" CssClass="modal-right" Visible="false">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">Adicionar Distribuidor</h5>
                        </div>
                        <div class="modal-body">
    <div class="mb-3">
        <label class="form-label">Nome Fantasia</label>
        <asp:TextBox ID="txtNomeCliente" runat="server" CssClass="form-control" Required></asp:TextBox>
    </div>
                            <div class="mb-3">
    <label class="form-label">Razão Social</label>
    <asp:TextBox ID="txtRazaoSocial" runat="server" CssClass="form-control" Required></asp:TextBox>
</div>
    <div class="mb-3">
        <label class="form-label">CNPJ</label>
        <asp:TextBox ID="txtCPFCNPJ" onkeyup="formataCNPJ(this,event);" MaxLength="18" runat="server" CssClass="form-control" Required></asp:TextBox>
    </div>
                                                        <div class="mb-3">
    <label class="form-label">Inscrição Estadual</label>
    <asp:TextBox ID="txtIE" runat="server" CssClass="form-control" Required></asp:TextBox>
</div>
    <div class="mb-3">
        <label class="form-label">Celular</label>
        <asp:TextBox ID="txtCelular" onkeyup="formataTelefone(this,event);" MaxLength="15" runat="server" CssClass="form-control" Required></asp:TextBox>
    </div>
    <div class="mb-3">
        <label class="form-label">E-mail</label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Required></asp:TextBox>
        <asp:Label ID="lblEmail" runat="server" Text="" Visible="false"></asp:Label>
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
                            <asp:LinkButton ID="lkbFechar" runat="server" CssClass="btn btn-danger btn-icon btn-icon-start" OnClick="lkbFechar_Click">
<i data-acorn-icon="close"></i> Fechar </asp:LinkButton>    
                            <asp:Button ID="btnSalvar" CssClass="btn btn-icon btn-icon-end btn-success" runat="server" Text="Salvar" OnClick="btnSalvar_Click1" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <!-- Discount Add Modal End -->
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
