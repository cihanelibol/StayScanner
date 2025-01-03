using Report.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.Dtos
{
    public record ReportDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public ReportStatus ReportStatus { get; set; }
    }
}
