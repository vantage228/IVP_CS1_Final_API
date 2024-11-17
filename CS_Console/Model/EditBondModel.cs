using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BondConsoleApp.Models
{
    public class EditBondModel
    {
        public int SecurityID { get; set; }
        public string? SecurityDescription { get; set; }
        public string? SecurityName { get; set; }
        public decimal? Coupon { get; set; }
        public bool? IsCallable { get; set; }
        public DateTime? MaturityDate { get; set; }
        public DateTime? PenultimateCouponDate { get; set; }
        public string? FormPFCreditRating { get; set; }
        public decimal? AskPrice { get; set; }
        public decimal? BidPrice { get; set; }

        public bool? IsActive { get; set; }

    }
}
