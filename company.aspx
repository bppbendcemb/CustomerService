<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="company.aspx.cs" Inherits="CustomerService.company" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .grv-hover tbody tr:hover {
            background-color: #f0f8ff;
        }

        .grv-selected {
            background-color: #add8e6 !important;
        }

        .table {
            width: 100%;
            table-layout: fixed;
            word-wrap: break-word;
        }

        .card-scroll {
            min-height: 45rem;
            max-height: 45rem;
            overflow-y: auto;
            overflow-x: auto;
        }

        .table thead th {
            position: sticky;
            top: 0;
            background: #fff; /* พื้นหลังหัวตาราง */
            z-index: 10;
        }
    </style>

    <%-- Search bar --%>
    <div class="container-fluid px-0 mb-3">
        <div class="input-group w-100">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="พิมพ์คำค้นหา..."></asp:TextBox>
            <asp:Button ID="bttSearch" runat="server" CssClass="btn btn-primary" Text="ค้นหา" OnClick="bttSearch_Click" />
        </div>
    </div>

    <%-- Content 3 คอลัมน์ --%>
    <div class="container-fluid">
        <div class="row">
            <!-- Card 1 -->
            <div class="col-4">
                <div class="card h-100">
                    <div class="card-header">
                        บริษัท
                    </div>
                    <div class="card-body p-0" style="max-height: 45rem; overflow-y: auto; overflow-x: auto;">
                        <asp:GridView ID="grv1" runat="server" CssClass="table table-bordered mb-0"
                            AutoGenerateColumns="False" DataKeyNames="companyid"
                            AutoGenerateSelectButton="False"
                            OnSelectedIndexChanged="grv1_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="companyid" HeaderText="Company ID" />
                                <asp:BoundField DataField="company" HeaderText="Company" />
                                <asp:BoundField DataField="taxid" HeaderText="Tax ID" />
                                <asp:CommandField ShowSelectButton="True" SelectText="เลือก" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <!-- Card 2 -->
            <div class="col-4">
                <div class="card h-100">
                    <div class="card-header">
                        ประวัติการซื้อ
                    </div>
                    <div class="card-body p-0" style="max-height: 45rem; overflow-y: auto; overflow-x: auto;">
                        <asp:GridView ID="grv2" runat="server" CssClass="table table-bordered mb-0"
                            AutoGenerateColumns="false" DataKeyNames="taxinvoice"
                            AutoGenerateSelectButton="false"
                            OnSelectedIndexChanged="grv2_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="taxinvoice" HeaderText="Tax Invoice" />
                                <asp:BoundField DataField="date" HeaderText="วันที่"
                                    DataFormatString="{0:MM/yyyy}" HtmlEncode="false" />
                                <asp:BoundField DataField="companyid" HeaderText="companyid" Visible="false" />
                                <asp:BoundField DataField="company" HeaderText="company" />
                                <asp:BoundField DataField="detail" HeaderText="detail" />
                                <asp:BoundField DataField="value" HeaderText="value" />
                                <asp:CommandField ShowSelectButton="True" SelectText="เลือก" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <!-- Card 3 -->
            <div class="col-4">
                <div class="card h-100">
                    <div class="card-header">
                        รายละเอียดการชำระ
                    </div>
                    <div class="card-body" style="max-height: 45rem; overflow-y: auto; overflow-x: auto;">
                        <asp:GridView ID="grv3" runat="server" CssClass="table table-bordered mb-0"
                            AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField DataField="detail" HeaderText="รายละเอียด" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
