using Audit.EntityFramework;
using HotelListing.API.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListing.API.Data
{
    [AuditIgnore]
    public class AuditLog
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public int? TablePk { get; set; }
        public string EntityType { get; set; }
        public string Title { get; set; }
        public string AuditData { get; set; }
        public string AuditAction { get; set; }
        public long? AuditUser { get; set; }
        public DateTime AuditDate { get; set; }
    }
}
