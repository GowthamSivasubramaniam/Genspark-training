using Microsoft.EntityFrameworkCore;
using ShoppingApp.Context;
using ShoppingApp.Models;
using ShoppingApp.Services.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;

using System.IO;
using ShoppingApp.Dtos;

namespace ShoppingApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly ShoppingContext _context;

        public OrderService(ShoppingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync(int page = 1, int pageSize = 5)
        {
            return await _context.Orders
                .OrderByDescending(o => o.OrderID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderID == id);
        }

public async Task<Order> CreateAsync(OrderDto order)
{
    var neworder = new Order
    {
        OrderName = order.OrderName,
        OrderDate = order.OrderDate,
        PaymentType = order.PaymentType,
        Status = order.Status,
        CustomerName = order.CustomerName,
        CustomerPhone = order.CustomerPhone,
        CustomerEmail = order.CustomerEmail,
        CustomerAddress = order.CustomerAddress
    };

    _context.Orders.Add(neworder);
    await _context.SaveChangesAsync();
    return neworder;
}
        

        public async Task<bool> UpdateAsync(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Orders.AnyAsync(o => o.OrderID == order.OrderID))
                    return false;
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;
            _context.Orders.Remove(order);
            return await _context.SaveChangesAsync() > 0;
        }
        public byte[] ExportOrdersToExcel()
{
    var orders = _context.Orders.OrderBy(o => o.OrderID).ToList();

#pragma warning disable CS0618 // Type or member is obsolete
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
#pragma warning restore CS0618 // Type or member is obsolete

            using var package = new ExcelPackage();
    var worksheet = package.Workbook.Worksheets.Add("Orders");

    // Headers
    worksheet.Cells[1, 1].Value = "OrderID";
    worksheet.Cells[1, 2].Value = "OrderName";
    worksheet.Cells[1, 3].Value = "OrderDate";
    worksheet.Cells[1, 4].Value = "PaymentType";
    worksheet.Cells[1, 5].Value = "Status";
    worksheet.Cells[1, 6].Value = "CustomerName";
    worksheet.Cells[1, 7].Value = "CustomerPhone";
    worksheet.Cells[1, 8].Value = "CustomerEmail";
    worksheet.Cells[1, 9].Value = "CustomerAddress";

    // Data
    int row = 2;
    foreach (var order in orders)
    {
        worksheet.Cells[row, 1].Value = order.OrderID;
        worksheet.Cells[row, 2].Value = order.OrderName;
        worksheet.Cells[row, 3].Value = order.OrderDate.ToString("yyyy-MM-dd");
        worksheet.Cells[row, 4].Value = order.PaymentType;
        worksheet.Cells[row, 5].Value = order.Status;
        worksheet.Cells[row, 6].Value = order.CustomerName;
        worksheet.Cells[row, 7].Value = order.CustomerPhone;
        worksheet.Cells[row, 8].Value = order.CustomerEmail;
        worksheet.Cells[row, 9].Value = order.CustomerAddress;
        row++;
    }

    return package.GetAsByteArray();
}

       public byte[] ExportOrdersToPdf()
        {
            var orders = _context.Orders.OrderBy(o => o.OrderID).ToList();

            using var ms = new MemoryStream();
            var doc = new Document(PageSize.A4, 10, 10, 10, 10);
            PdfWriter.GetInstance(doc, ms);
            doc.Open();

            var font = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            var table = new PdfPTable(9) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 1f, 3f, 2f, 2f, 2f, 3f, 3f, 4f, 4f });

           
            string[] headers = { "OrderID", "OrderName", "OrderDate", "PaymentType", "Status", "CustomerName", "CustomerPhone", "CustomerEmail", "CustomerAddress" };
            foreach (var header in headers)
            {
                var cell = new PdfPCell(new Phrase(header, font))
                {
                    
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                table.AddCell(cell);
            }

           
            foreach (var order in orders)
            {
                table.AddCell(new PdfPCell(new Phrase(order.OrderID.ToString(), font)));
                table.AddCell(new PdfPCell(new Phrase(order.OrderName, font)));
                table.AddCell(new PdfPCell(new Phrase(order.OrderDate.ToString("yyyy-MM-dd"), font)));
                table.AddCell(new PdfPCell(new Phrase(order.PaymentType, font)));
                table.AddCell(new PdfPCell(new Phrase(order.Status, font)));
                table.AddCell(new PdfPCell(new Phrase(order.CustomerName, font)));
                table.AddCell(new PdfPCell(new Phrase(order.CustomerPhone, font)));
                table.AddCell(new PdfPCell(new Phrase(order.CustomerEmail, font)));
                table.AddCell(new PdfPCell(new Phrase(order.CustomerAddress, font)));
            }

            doc.Add(table);
            doc.Close();

            return ms.ToArray();
        }


    }
}
