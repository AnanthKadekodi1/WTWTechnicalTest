using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsReservationService.Model
{
    public class IncrementalDataModel
    {
        public string? Product { get; set; }
        public int OriginYear { get; set; }
        public int DevelopmentYear { get; set; }
        public double Increment { get; set; }
    }
}
