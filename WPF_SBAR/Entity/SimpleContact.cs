using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_SBAR.Entity {
public class SimpleContact {
        public int ContactID { get; set; }
        public string contactType { get; set; }
        public string companyName { get; set; }
        public string contact { get; set; }
        public string description { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public SimpleContact() {
            ContactID = -1;
            contactType = "";
            companyName = "";
            contact = "";
            description = "";
            firstName = "";
            lastName = "";
        }//SimpleContact
    }//SimpleContact
}
