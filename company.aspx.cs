using System;
using System.ComponentModel.Design;
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

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void bttSearch_Click(object sender, EventArgs e)
        {
            // ล้างข้อมูลเก่าก่อนค้นหาใหม่
            grv1.DataSource = null;
            grv1.DataBind();
            grv2.DataSource = null;
            grv2.DataBind();
            grv3.DataSource = null;
            grv3.DataBind();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"SELECT 
                           com.[companyid],
                           com.[company],
                           tax.[taxid]
                       FROM [Condo].[dbo].[main.company] com
                       LEFT JOIN [Condo].[dbo].[main.tax] tax
                            ON com.companyid = tax.companyid
                       WHERE com.company LIKE @search";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    string searchText = txtSearch.Text.Trim();
                    cmd.Parameters.AddWithValue("@search", string.IsNullOrEmpty(searchText) ? "%%" : "%" + searchText + "%");

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        grv1.DataSource = reader;
                        grv1.DataBind();
                    }
                }
            }
        }


        protected void grv1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // อ่านค่า companyid จากแถวที่เลือก
            GridViewRow row = grv1.SelectedRow;
            string companyId = row.Cells[0].Text;  // สมมติ Column 0 = companyid

            // ====== โหลดข้อมูล grv2 ======
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"SELECT 
                           A.[taxinvoice],
                           A.[date],
                           [companyid],
                           [company],
                           detail,
                           value
                       FROM (
                           SELECT [taxinvoice],
                                  [date],
                                  rchd.[companyid],
                                  company.[company],
                                  [value],
                                  [vat],
                                  [totalamount],
                                  [withholdingtax],
                                  [paidby],
                                  [bank],
                                  [chequeno],
                                  [paiddate],
                                  [amount],
                                  [balance],
                                  [note]
                           FROM [Condo].[dbo].[tran.rchd] rchd
                           LEFT JOIN [Condo].[dbo].[main.company] company
                                  ON rchd.companyid = company.companyid
                       ) A
                       LEFT JOIN (
                           SELECT taxinvoice, [date], detail
                           FROM (
                               SELECT *,
                                      ROW_NUMBER() OVER (PARTITION BY taxinvoice ORDER BY (SELECT NULL)) AS rn
                               FROM [Condo].[dbo].[tran.rcdt]
                           ) AS t
                           WHERE rn = 1
                       ) B
                           ON A.taxinvoice = B.taxinvoice
                       WHERE companyid = @companyid
                       ORDER BY date DESC";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@companyid", companyId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        grv2.DataSource = reader;
                        grv2.DataBind();
                    }
                }
            }

            // ====== ทำให้ grv1 เหลือแค่บริษัทที่เลือก ======
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sqlCompany = @"SELECT com.[companyid], com.[company], tax.[taxid]
                              FROM [Condo].[dbo].[main.company] com
                              LEFT JOIN [Condo].[dbo].[main.tax] tax
                                     ON com.companyid = tax.companyid
                              WHERE com.companyid = @companyid";

                using (SqlCommand cmd = new SqlCommand(sqlCompany, conn))
                {
                    cmd.Parameters.AddWithValue("@companyid", companyId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        grv1.DataSource = reader;
                        grv1.DataBind();
                    }
                }
            }
        }

        protected void grv2_SelectedIndexChanged(object sender, EventArgs e)
        {

            GridViewRow row = grv2.SelectedRow;
            string texinvoice = row.Cells[0].Text;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT TOP (1000) [taxinvoice]
                                     ,[date]
                                     ,[detail]
                                 FROM [Condo].[dbo].[tran.rcdt]
                                 WHERE taxinvoice = @serachtax";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@serachtax", texinvoice);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        grv3.DataSource = reader;
                        grv3.DataBind();
                    }
                }

            }
        }
    }
}
