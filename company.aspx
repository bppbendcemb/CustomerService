<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="company.aspx.cs" Inherits="CustomerService.company" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
    <div>
        <%-- ตารางผลการค้นหาแรก --%>
    <asp:GridView ID="grv1" runat="server" CssClass="table table-bordered"
                  AutoGenerateColumns="False" DataKeyNames="companyid"
                  OnSelectedIndexChanged="grv1_SelectedIndexChanged">
        <Columns>
            <asp:BoundField DataField="companyid" HeaderText="Company ID" />
            <asp:BoundField DataField="company" HeaderText="Company" />
            <asp:BoundField DataField="taxid" HeaderText="Tax ID" />
            <asp:CommandField ShowSelectButton="True" SelectText="เลือก" />
        </Columns>
    </asp:GridView>

    <hr />

    <%-- ตารางรายละเอียด --%>
    <asp:GridView ID="grvDetail" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false">
        
       <%-- 
        taxinvoice	
        date	
        companyid	
        company	
        detail	
        value
           --%>
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
</asp:Content>
