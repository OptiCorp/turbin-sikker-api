using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceApp.Model
{
    public enum InvoiceStatus
    {
        [Display(Name = "Active")]
        Active,
        [Display(Name = "Inactive")]
        Inactive
    }

    public class Invoice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime SentDate { get; set; }

        [EnumDataType(typeof(InvoiceStatus))]
        public InvoiceStatus Status { get; set; }

        public string Sender { get; set; }

        public string Reciever { get; set; }

        public int Amount { get; set; }

        public string PdfBlobLink { get; set; }
    }
}