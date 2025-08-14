using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomerService
{
    public partial class company : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

        private int SelectedRowIndex
        {
            get => ViewState["SelectedRowIndex"] != null ? (int)ViewState["SelectedRowIndex"] : -1;
            set => ViewState["SelectedRowIndex"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["LastSearch"] = "";
            }
            else
            {
                // bind GridView ก่อน postback
                string lastSearch = ViewState["LastSearch"].ToString();
                if (!string.IsNullOrEmpty(lastSearch))
                    BindCompanyGrid(lastSearch);
            }
        }

        protected void bttSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();
            ViewState["LastSearch"] = search;
            BindCompanyGrid(search);
        }

        private void BindCompanyGrid(string search)
        {
            if (string.IsNullOrEmpty(search)) return;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = @"
                    SELECT com.companyid, com.company, tax.taxid
                    FROM [Condo].[dbo].[main.company] com
                    LEFT JOIN [Condo].[dbo].[main.tax] tax
                    ON com.companyid = tax.companyid
                    WHERE com.company LIKE @search OR tax.taxid LIKE @search
                    ORDER BY com.company";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        grv1.DataSource = dt;
                        grv1.DataBind();
                    }
                }
            }

            grvDetail.DataSource = null;
            grvDetail.DataBind();
            SelectedRowIndex = -1;
        }

        protected void grv1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // คลิกทั้งแถว
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grv1, "Select$" + e.Row.RowIndex);
                e.Row.Style["cursor"] = "pointer";

                // ไฮไลต์แถวที่เลือก
                if (grv1.SelectedIndex == e.Row.RowIndex)
                {
                    e.Row.CssClass = "grv-selected";
                }
            }
        }

        protected void grv1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                SelectedRowIndex = rowIndex;

                string companyId = grv1.DataKeys[rowIndex].Value.ToString();
                LoadCompanyDetail(companyId);

                // ❌ ไม่ต้องเรียก DataBind อีก
            }
        }

        private void LoadCompanyDetail(string companyId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = @"
                    SELECT A.taxinvoice, A.date, companyid, company, detail, value
                    FROM (
                        SELECT rchd.taxinvoice, rchd.date, rchd.companyid, company.company, rchd.value
                        FROM [Condo].[dbo].[tran.rchd] rchd
                        LEFT JOIN [Condo].[dbo].[main.company] company
                        ON rchd.companyid = company.companyid
                    ) A
                    LEFT JOIN (
                        SELECT taxinvoice, [date], detail
                        FROM (
                            SELECT *, ROW_NUMBER() OVER (PARTITION BY taxinvoice ORDER BY (SELECT NULL)) AS rn
                            FROM [Condo].[dbo].[tran.rcdt]
                        ) t WHERE rn = 1
                    ) B
                    ON A.taxinvoice = B.taxinvoice
                    WHERE companyid = @companyid
                    ORDER BY date DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@companyid", companyId);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        grvDetail.DataSource = dt;
                        grvDetail.DataBind();
                    }
                }
            }
        }

        protected void grv1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string companyId = grv1.SelectedDataKey.Value.ToString();
            LoadCompanyDetail(companyId);
        }
    }
}
