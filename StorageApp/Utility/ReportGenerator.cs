using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using StorageApp.Dtos.Items;
using System.Data;
using System.Text;

namespace StorageApp.Utility
{
    /// <summary>
    /// Used for generating reports
    /// </summary>
    public static class ReportGenerator
    {
        /// <summary>
        /// Generates the report data table
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static DataTable GenerateReportDataTable(ItemsIndexDto dto)
        {
            DataTable dt = new DataTable("Report");

            dt.Columns.AddRange([
                new DataColumn("Id"),
                new DataColumn("ItemName"),
                new DataColumn("Quantity"),
                new DataColumn("Price"),
                new DataColumn("SupplierName")
                ]);

            foreach (var item in dto.AllItems)
            {
                dt.Rows.Add(item.Id, item.Name, item.Quantity, item.Price, item.Supplier);
            }

            return dt;
        }

        /// <summary>
        /// Generates a DataTable containing meta information about the report (what the report will be about).
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static DataTable GenerateInfoDataTable(ItemsIndexDto dto)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("This is an auto generated report for ");

            var itemName = string.IsNullOrWhiteSpace(dto.ItemName) ? "[ALL] items" : $"[{dto.ItemName}] items";
            sb.Append(itemName);

            var supplierName = string.IsNullOrWhiteSpace(dto.SupplierName) ? "[ANY]" : $"[{dto.SupplierName}]";
            sb.Append($", supplied from {supplierName} supplier");

            var priceRange = $", in the price range of [{dto.MinPrice}] <-> [{dto.MaxPrice}].";
            sb.Append(priceRange);

            DataTable infoDt = new DataTable("Info");

            infoDt.Columns.Add("Report Info");
            infoDt.Rows.Add(sb.ToString());
            infoDt.Rows.Add("Check the second worksheet for the data");

            return infoDt;
        }

        /// <summary>
        /// Generates an Excel workbook file containing an infoDataTable in the first worksheet and reportDataTable in the second worksheet.
        /// </summary>
        /// <param name="infoDataTable"></param>
        /// <param name="reportDataTable"></param>
        /// <param name="fileName"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public static FileContentResult GenerateExcelWorkbook(DataTable infoDataTable, DataTable reportDataTable, string fileName = "report", string fileExtension= ".xlsx")
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet1 = wb.Worksheets.Add(infoDataTable);
                workSheet1.Columns().AdjustToContents();

                var workSheet2 = wb.Worksheets.Add(reportDataTable);
                workSheet2.Columns().AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = fileName+fileExtension
                    };
                }
            }
        }
    }
}
