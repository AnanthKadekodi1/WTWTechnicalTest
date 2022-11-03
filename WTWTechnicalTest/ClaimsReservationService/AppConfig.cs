using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsReservationService
{
    public class AppConfig : IAppConfig
    {
        public string InputDataFilePath { get; set; }
        public string OutputDataFilePath { get; set; }
    }
}
