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
        //string connStr = ConfigurationManager.ConnectionStrings["myoffice"].ConnectionString;
        string connStr = ConfigurationManager.ConnectionStrings["myhome"].ConnectionString;

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

            txtCompanyidChoose.Text = "";
            txtConpanyNameChoose.Text = "";
            txtConpanyidChange.Text = "";
            txtCompanyNameChange.Text = "";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"SELECT 
    com.companyid,
    com.company,
    com.taxid,
    COUNT(rchd.companyid) AS count_reciept
FROM      [Condox].[dbo].[main.company] com
LEFT JOIN [Condox].[dbo].[tran.rchd] rchd
    ON com.companyid = rchd.companyid
WHERE com.company LIKE @search
GROUP BY com.companyid, com.company, com.taxid
HAVING COUNT(rchd.companyid) > 0;";

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
                           FROM [Condox].[dbo].[tran.rchd] rchd
                           LEFT JOIN [Condox].[dbo].[main.company] company
                                  ON rchd.companyid = company.companyid
                       ) A
                       LEFT JOIN (
                           SELECT taxinvoice, [date], detail
                           FROM (
                               SELECT *,
                                      ROW_NUMBER() OVER (PARTITION BY taxinvoice ORDER BY (SELECT NULL)) AS rn
                               FROM [Condox].[dbo].[tran.rcdt]
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
                string sqlCompany = @"SELECT 
       com.[companyid]
	  ,com.[company]
	  ,com.[taxid]
	  ,COUNT(rchd.companyid) count_reciept
 FROM [Condox].[dbo].[main.company] com
 LEFT JOIN [condox].[dbo].[tran.rchd] rchd
        ON com.companyid = rchd.companyid
 WHERE com.companyid = @companyid
 GROUP BY com.companyid, com.company, com.taxid;";

                using (SqlCommand cmd = new SqlCommand(sqlCompany, conn))
                {
                    cmd.Parameters.AddWithValue("@companyid", companyId);
                    conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        grv1.DataSource = dt;
                        grv1.DataBind();

                        if (dt.Rows.Count > 0)
                        {
                            txtCompanyidChoose.Text = dt.Rows[0]["companyid"].ToString();
                            txtConpanyNameChoose.Text = dt.Rows[0]["company"].ToString();
                        }
                    }
                }
            }
        }

        protected void grv2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // อ่านค่า tax จากแถวที่เลือก
            GridViewRow row = grv2.SelectedRow;

            int companyid = int.Parse(txtCompanyidChoose.Text);
            string texinvoice = row.Cells[0].Text;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT TOP (1000) [taxinvoice]
                                     ,[date]
                                     ,[detail]
                                 FROM [Condox].[dbo].[tran.rcdt]
                                 WHERE taxinvoice = @serachtax";

                string choostax = @"SELECT  A.[taxinvoice],
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
     FROM [Condox].[dbo].[tran.rchd] rchd
     LEFT JOIN [Condox].[dbo].[main.company] company
            ON rchd.companyid = company.companyid
 ) A
 LEFT JOIN (
     SELECT taxinvoice, [date], detail
     FROM (
         SELECT *,
                ROW_NUMBER() OVER (PARTITION BY taxinvoice ORDER BY (SELECT NULL)) AS rn
         FROM [Condox].[dbo].[tran.rcdt]
     ) AS t
     WHERE rn = 1
 ) B
     ON A.taxinvoice = B.taxinvoice
 WHERE companyid = @companyid AND A.taxinvoice = @serachtax
 ORDER BY date DESC";

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

                using (SqlCommand cmd = new SqlCommand(choostax, conn))
                {
                    cmd.Parameters.AddWithValue("@serachtax", texinvoice);
                    cmd.Parameters.AddWithValue("@companyid", companyid);
                    
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        grv2.DataSource = reader;
                        grv2.DataBind();
                    }
                }

            }
        }

        protected void txtConpanyidChange_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConpanyidChange.Text))
                return;

            int companyid;
            if (!int.TryParse(txtConpanyidChange.Text, out companyid))
                return; // ไม่ใช่เลข ไม่ query

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT [companyid], [company], [taxid]
                         FROM [Condox].[dbo].[main.company]
                         WHERE companyid = @companyid";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@companyid", SqlDbType.Int).Value = companyid;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        txtCompanyNameChange.Text = dt.Rows[0]["company"].ToString();
                    }
                    else
                    {
                        txtCompanyNameChange.Text = string.Empty;
                    }
                }
            }
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    string update = @"
            UPDATE Condox.dbo.[tran.rchd]
            SET [companyid] = @companyid_new
            WHERE [companyid] = @companyid_old;";

                    int companyid_old = int.Parse(txtCompanyidChoose.Text);
                    int companyid_new = int.Parse(txtConpanyidChange.Text);

                    using (SqlCommand cmd = new SqlCommand(update, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@companyid_old", companyid_old);
                        cmd.Parameters.AddWithValue("@companyid_new", companyid_new);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        // ตรวจสอบผลลัพธ์ก่อน commit
                        if (rowsAffected > 0)
                        {
                            tran.Commit();
                            lblMessage.Text = $"อัปเดตสำเร็จ {rowsAffected} แถว จาก {companyid_old} → {companyid_new}";
                        }
                        else
                        {
                            tran.Rollback();
                            lblMessage.Text = "ไม่มีแถวที่ถูกอัปเดต จึง rollback";
                        }
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    lblMessage.Text = $"เกิดข้อผิดพลาด: {ex.Message}";
                }
            }
        }
    }
}
