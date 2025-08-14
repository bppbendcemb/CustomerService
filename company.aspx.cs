using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomerService
{
    public partial class company : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //LoadDropdownlistcompany();
            }
        }

        //private void LoadDropdownlistcompany()
        //{
        //    using (SqlConnection conn = new SqlConnection(connStr))
        //    {
        //        conn.Open();
        //        string query = @"
        //    SELECT 
        //        com.[companyid],
        //        com.[company],
        //        tax.[taxid]
        //    FROM [Condo].[dbo].[main.company] com
        //    LEFT JOIN [Condo].[dbo].[main.tax] tax
        //        ON com.companyid = tax.companyid
        //";

        //        using (SqlCommand cmd = new SqlCommand(query, conn))
        //        {
        //            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        //            {
        //                DataTable dt = new DataTable();
        //                da.Fill(dt);

        //                ddlCompany.DataSource = dt;
        //                ddlCompany.DataTextField = "company";
        //                ddlCompany.DataValueField = "companyid";
        //                ddlCompany.DataBind();

        //                ddlCompany.Items.Insert(0, new ListItem("-- Select Company --", ""));
        //            }
        //        }
        //    }
        //}

        protected void bttSearch_Click(object sender, EventArgs e)
        {
            string serach = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(serach))
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"
                SELECT 
                    com.[companyid],
                    com.[company],
                    tax.[taxid]
                FROM [Condo].[dbo].[main.company] com
                LEFT JOIN [Condo].[dbo].[main.tax] tax
                    ON com.companyid = tax.companyid
                WHERE com.company LIKE @search
                OR tax.taxid LIKE @search
                ORDER BY com.company
            ";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + serach + "%");
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            grv1.DataSource = dt;
                            grv1.DataBind();
                        }
                    }

                }
            }

            grvDetail.DataSource = null; // ล้างตารางรายละเอียด
            grvDetail.DataBind();
        }

        protected void grv1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string companyId = grv1.SelectedDataKey.Value.ToString();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = @"SELECT 
 A.[taxinvoice]
,A.[date]
,[companyid]
,[company]
,detail
,value
FROM (
SELECT [taxinvoice]
      ,[date]
      ,rchd.[companyid]
	  --,company.[companyid]
      ,company.[company]
      ,[value]
      ,[vat]
      ,[totalamount]
      ,[withholdingtax]
      ,[paidby]
      ,[bank]
      ,[chequeno]
      ,[paiddate]
      ,[amount]
      ,[balance]
      ,[note]
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
ORDER BY date DESC
        ";

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
    }
}