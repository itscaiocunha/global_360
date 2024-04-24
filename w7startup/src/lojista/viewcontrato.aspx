<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/src/lojista/principal.Master" AutoEventWireup="true" CodeBehind="viewcontrato.aspx.cs" Inherits="global.lojista.viewcontrato" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Title and Top Buttons Start -->
    <asp:HiddenField ID="hdfId" runat="server" />
    <div class="page-title-container">
        <div class="row">
            <!-- Title Start -->
            <div class="col-auto mb-3 mb-md-0 me-auto">
                <div class="w-auto sw-md-30">
                    <a href="#" class="muted-link pb-1 d-inline-block breadcrumb-back">
                        <i data-acorn-icon="chevron-left" data-acorn-size="13"></i>
                        <span class="text-small align-middle">Contrato</span>
                    </a>
                    <h1 class="mb-0 pb-0 display-6" id="title">Visualização online</h1>
                </div>
            </div>
            <!-- Title End -->
            <div class="col-sm-12 col-md-7 col-lg-4 col-xxl-10 text-end mb-1">
                <div class="d-inline-block">
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

            <div class="row gx-4 gy-5">
                <div class="col-12 col-xl-8 col-xxl-9 mb-n5">
                    <!-- Status Start -->
                    <div class="mb-5">
                        <div class="row g-2">
                            <asp:Label ID="lblContrato" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <!-- Status End -->
                </div>

            </div>
        </div>
    </div>
</asp:Content>
