<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="company.aspx.cs" Inherits="CustomerService.company" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        /* ตัวอย่างใน <head> หรือไฟล์ CSS */
        .grv-hover tbody tr:hover {
            background-color: #f0f8ff;
        }

        .grv-selected {
            background-color: #add8e6 !important;
        }
    </style>
    <%--form control--%>
    <div>
        <div class="input-group mb-3">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="พิมพ์คำค้นหา..."></asp:TextBox>
            <div class="input-group-append">
                <asp:Button ID="bttSearch" runat="server" CssClass="btn btn-primary" Text="ค้นหา" OnClick="bttSearch_Click" />
            </div>
        </div>
    </div>

    <%--content--%>
    <div class="d-flex justify-content-start">
        <div class="col-4 card" style="min-height: 50rem;">
            <%-- ตารางผลการค้นหาแรก --%>
            <asp:GridView ID="grv1" runat="server" CssClass="table table-bordered"
                AutoGenerateColumns="False" DataKeyNames="companyid"
                AutoGenerateSelectButton="False" OnRowDataBound="grv1_RowDataBound"
                OnSelectedIndexChanged="grv1_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="companyid" HeaderText="Company ID" />
                    <asp:BoundField DataField="company" HeaderText="Company" />
                    <asp:BoundField DataField="taxid" HeaderText="Tax ID" />
                </Columns>
            </asp:GridView>

        </div>

        <div class="col-4 card" style="min-height: 50rem;">
            <%-- ตารางรายละเอียด --%>
            <asp:GridView ID="grvDetail" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="taxinvoice" HeaderText="taxinvoice" />
                    <asp:BoundField DataField="date" HeaderText="วันที่"
                        DataFormatString="{0:MM/yyyy}"
                        HtmlEncode="false" />
                    <asp:BoundField DataField="companyid" HeaderText="companyid" />
                    <asp:BoundField DataField="company" HeaderText="company" />
                    <asp:BoundField DataField="detail" HeaderText="detail" />
                    <asp:BoundField DataField="value" HeaderText="value" />
                </Columns>
            </asp:GridView>
        </div>


        <div class="col-4 card" style="min-height: 50rem;">
            xxxx
        </div>
    </div>

</asp:Content>
