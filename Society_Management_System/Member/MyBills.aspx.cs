using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Society_Management_System.Member
{
    public partial class MyBills : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindBills();
        }

        private void BindBills()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString))
            {
                string query = @"SELECT mb.bill_id, s.name AS SocietyName, b.name AS BuildingName,
                                u.unit_no, mb.bill_month, mb.due_date, mb.total_amount, mb.status
                                FROM maintenance_bills mb
                                INNER JOIN units u ON mb.unit_id = u.unit_id
                                INNER JOIN buildings b ON u.building_id = b.building_id
                                INNER JOIN societies s ON b.society_id = s.society_id";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                GvMyBills.DataSource = dt;
                GvMyBills.DataBind();
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            int billId = Convert.ToInt32((sender as System.Web.UI.WebControls.Button).CommandArgument);
            GenerateBillPDF(billId);
        }

        private void GenerateBillPDF(int billId)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString))
            {
                string query = @"
            SELECT mb.*, s.name AS SocietyName, s.address_line1, s.city, s.state, s.pincode,
                   b.name AS BuildingName, u.unit_no, m.full_name, m.phone, m.email
            FROM maintenance_bills mb
            INNER JOIN units u ON mb.unit_id = u.unit_id
            INNER JOIN buildings b ON u.building_id = b.building_id
            INNER JOIN societies s ON b.society_id = s.society_id
            INNER JOIN unit_occupancies oc ON u.unit_id = oc.unit_id
            INNER JOIN members m ON oc.member_id = m.member_id
            WHERE mb.bill_id = @bill_id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@bill_id", billId);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    MemoryStream ms = new MemoryStream();
                    Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                    PdfWriter.GetInstance(doc, ms);
                    doc.Open();

                    // 🔹 Add Logo + Society Header
                    PdfPTable headerTable = new PdfPTable(2);
                    headerTable.WidthPercentage = 100;
                    float[] widths = new float[] { 20f, 80f };
                    headerTable.SetWidths(widths);

                    // Change logo path according to your project folder
                    string logoPath = Server.MapPath("~/assets/images/society-logo.png");
                    if (File.Exists(logoPath))
                    {
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                        logo.ScaleAbsolute(70, 70);
                        PdfPCell logoCell = new PdfPCell(logo);
                        logoCell.Border = Rectangle.NO_BORDER;
                        headerTable.AddCell(logoCell);
                    }
                    else
                    {
                        headerTable.AddCell(new PdfPCell(new Phrase(" ")) { Border = Rectangle.NO_BORDER });
                    }

                    PdfPCell titleCell = new PdfPCell();
                    titleCell.Border = Rectangle.NO_BORDER;
                    titleCell.AddElement(new Paragraph(rdr["SocietyName"].ToString(), FontFactory.GetFont("Arial", 18, Font.BOLD)));
                    titleCell.AddElement(new Paragraph($"{rdr["address_line1"]}, {rdr["city"]}, {rdr["state"]} - {rdr["pincode"]}", FontFactory.GetFont("Arial", 10)));
                    headerTable.AddCell(titleCell);
                    doc.Add(headerTable);

                    // Line separator
                    LineSeparator line = new LineSeparator(1f, 100f, BaseColor.LIGHT_GRAY, Element.ALIGN_CENTER, -2);
                    doc.Add(new Chunk(line));

                    // 🔹 Resident & Bill Info
                    PdfPTable infoTable = new PdfPTable(2);
                    infoTable.WidthPercentage = 100;
                    infoTable.SpacingBefore = 10f;
                    infoTable.SpacingAfter = 10f;
                    infoTable.DefaultCell.Border = Rectangle.NO_BORDER;

                    infoTable.AddCell(new Phrase($"Resident: {rdr["full_name"]}", FontFactory.GetFont("Arial", 11)));
                    infoTable.AddCell(new Phrase($"Contact: {rdr["phone"]} | {rdr["email"]}", FontFactory.GetFont("Arial", 11)));
                    infoTable.AddCell(new Phrase($"Building: {rdr["BuildingName"]} | Unit: {rdr["unit_no"]}", FontFactory.GetFont("Arial", 11)));
                    infoTable.AddCell(new Phrase($"Bill Month: {Convert.ToDateTime(rdr["bill_month"]).ToString("MMMM yyyy")}", FontFactory.GetFont("Arial", 11)));
                    infoTable.AddCell(new Phrase($"Due Date: {Convert.ToDateTime(rdr["due_date"]).ToString("dd MMM yyyy")}", FontFactory.GetFont("Arial", 11)));
                    infoTable.AddCell(new Phrase($"Status: {rdr["status"]}", FontFactory.GetFont("Arial", 11, Font.BOLD, BaseColor.BLUE)));
                    doc.Add(infoTable);

                    rdr.Close();

                    // 🔹 Bill Items Table
                    string itemQuery = "SELECT description, amount FROM bill_items WHERE bill_id = @bill_id";
                    SqlCommand itemCmd = new SqlCommand(itemQuery, con);
                    itemCmd.Parameters.AddWithValue("@bill_id", billId);
                    SqlDataReader itemRdr = itemCmd.ExecuteReader();

                    PdfPTable table = new PdfPTable(2);
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 10f;
                    table.SpacingAfter = 10f;
                    table.SetWidths(new float[] { 70, 30 });

                    PdfPCell header1 = new PdfPCell(new Phrase("Description", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.WHITE)));
                    header1.BackgroundColor = BaseColor.GRAY;
                    header1.HorizontalAlignment = Element.ALIGN_CENTER;
                    header1.Padding = 6;
                    table.AddCell(header1);

                    PdfPCell header2 = new PdfPCell(new Phrase("Amount (₹)", FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.WHITE)));
                    header2.BackgroundColor = BaseColor.GRAY;
                    header2.HorizontalAlignment = Element.ALIGN_CENTER;
                    header2.Padding = 6;
                    table.AddCell(header2);

                    decimal total = 0;
                    while (itemRdr.Read())
                    {
                        table.AddCell(new PdfPCell(new Phrase(itemRdr["description"].ToString())) { Padding = 5 });
                        table.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(itemRdr["amount"]).ToString("N2"))) { Padding = 5, HorizontalAlignment = Element.ALIGN_RIGHT });
                        total += Convert.ToDecimal(itemRdr["amount"]);
                    }
                    itemRdr.Close();

                    PdfPCell totalCell = new PdfPCell(new Phrase("Total", FontFactory.GetFont("Arial", 12, Font.BOLD)));
                    totalCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalCell.Padding = 5;
                    table.AddCell(totalCell);

                    PdfPCell totalValue = new PdfPCell(new Phrase("₹ " + total.ToString("N2"), FontFactory.GetFont("Arial", 12, Font.BOLD)));
                    totalValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                    totalValue.Padding = 5;
                    table.AddCell(totalValue);

                    doc.Add(table);

                    // 🔹 Footer Section
                    doc.Add(new Paragraph("Generated On: " + DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), FontFactory.GetFont("Arial", 10)));
                    doc.Add(new Paragraph(" "));
                    doc.Add(new Chunk(line));
                    doc.Add(new Paragraph("Thank you for being a valued resident!", FontFactory.GetFont("Arial", 10, Font.ITALIC, BaseColor.DARK_GRAY)));
                    doc.Add(new Paragraph("Please contact the society office for payment-related queries.", FontFactory.GetFont("Arial", 9, BaseColor.GRAY)));

                    doc.Close();

                    byte[] pdfBytes = ms.ToArray();
                    ms.Close();

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=MaintenanceBill_" + billId + ".pdf");
                    Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                    Response.BinaryWrite(pdfBytes);
                    Response.End();
                }
                con.Close();
            }
        }
    }
}