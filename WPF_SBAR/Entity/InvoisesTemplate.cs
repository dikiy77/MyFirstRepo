using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_SBAR.Entity {

    class InvoisesTemplate {

        public long InvoiceID { get; set; }
        public int SupplierID { get; set; }
        public string CompanyName { get; set; }
        public System.DateTime Date { get; set; }
        public int EmployeeID { get; set; }
        public bool Paid { get; set; }
        public Nullable<System.DateTime> PayToDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal? Overhead { get; set; }
        public bool Official { get; set; }
        public decimal? TotalSumm { get; set; }
    }
}
