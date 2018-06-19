using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_SBAR.Entity {
    class OrderDeteilsView {
        public string Barcode { get; set; }
        public long InvoiceDetailsID { get; set; }
        public long InvoiceID { get; set; }
        public long ProductID { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public short GroupID { get; set; }
        public string GroupTitle { get; set; }
        public string ProductTitle { get; set; }
        public decimal SummOfPosition { get; set; }

        public OrderDeteilsView() {

            this.Barcode = "Undefined";
            this.InvoiceDetailsID = 0;
            this.InvoiceID = 0;
            this.ProductID = 0;
            this.Quantity = 0;
            this.Price = 0;
            this.GroupID = 0;
            this.GroupTitle = "";
            this.ProductTitle = "";
            this.SummOfPosition = Quantity * Price;

        }//OrderDeteilsView

        public OrderDeteilsView(string barcode, long invoiceDetailsID, long invoiceID, long productID,
            decimal  quantity, decimal price, short groupID, string groupTitle, string productTitle) {

            this.Barcode = barcode;
            this.InvoiceDetailsID = invoiceDetailsID;
            this.InvoiceID = invoiceID;
            this.ProductID = productID;
            this.Quantity = quantity;
            this.Price = price;
            this.GroupID = groupID;
            this.GroupTitle = groupTitle;
            this.ProductTitle = productTitle;
            this.SummOfPosition = Quantity * Price;

        }//OrderDeteilsView

    }
}
