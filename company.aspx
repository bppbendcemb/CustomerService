<%@ Page Title="Company" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="company.aspx.cs" Inherits="CustomerService.company" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="css/styles.css" rel="stylesheet" />

    <%-- Search bar --%>
    <div class="card shadow-sm p-3 m-2">
        <div class="d-flex justify-content-start">
            <div class="row w-75">
                <div class="d-flex justify-content-start gap-2 row">
                    <asp:TextBox ID="txtCompanyidChoose" runat="server" CssClass="form-control w-25" placeholder="รหัสบริษัทที่เลือก" Enabled="false"></asp:TextBox>
                    <asp:TextBox ID="txtConpanyNameChoose" runat="server" CssClass="form-control w-50" placeholder="ชื่อบริษัทที่เลือก" Enabled="false"></asp:TextBox>
                </div>


                <div class="d-flex justify-content-start gap-2 row">
                    <asp:TextBox ID="txtConpanyidChange" runat="server" CssClass="form-control w-25" placeholder="กรอกรหัสบริษัทที่เปลี่ยน"
                        OnTextChanged="txtConpanyidChange_TextChanged"
                        AutoPostBack="true"></asp:TextBox>
                    <asp:TextBox ID="txtCompanyNameChange" runat="server" CssClass="form-control w-50" placeholder="ชื่อบริษัทที่เปลี่ยน"></asp:TextBox>
                </div>



            </div>
            <div class="w-25">
                <asp:Button ID="btnChange" runat="server" CssClass="btn btn-primary" Text="บันทึกเปลี่ยนรหัส" OnClick="btnChange_Click" />
            </div>
        </div>
    </div>

    <div class="d-flexjustify-content-end m-2 p-2">
        <div class="input-group">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="พิมพ์คำค้นหา..."></asp:TextBox>
            <asp:Button ID="bttSearch" runat="server" CssClass="btn btn-primary" Text="ค้นหา" OnClick="bttSearch_Click" />
        </div>
        <div>
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
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
                        <asp:GridView ID="grv1" runat="server" CssClass="table table-bordered table-sm table-striped mb-0"
                            AutoGenerateColumns="False" DataKeyNames="companyid"
                            AutoGenerateSelectButton="False"
                            OnSelectedIndexChanged="grv1_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="companyid" HeaderText="ID" HeaderStyle-Width="60" />
                                <asp:BoundField DataField="company" HeaderText="Company" />
                                <asp:BoundField DataField="taxid" HeaderText="Tax ID" HeaderStyle-Width="130" />
                                <asp:BoundField DataField="count_reciept" HeaderText="bill" HeaderStyle-Width="40" />
                                <asp:CommandField ShowSelectButton="True" SelectText="เลือก" HeaderText="ประวัติ" HeaderStyle-Width="60" />
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
                                <asp:CommandField ShowSelectButton="True" SelectText="เลือก" HeaderText="รายละเอียด" />
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
