using Report.Application.Dtos;
using Report.Application.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.Services.Concrete
{
    public class ReportService : IReportService
    {
        public Task<List<HotelsInfoByLocationDto>> GetHotelsInfoByLocationAsync(string location)
        {
            throw new NotImplementedException();
        }
    }
}
