using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Core.DTOs
{
    public abstract class BaseGuidIdDto
    {
        public Guid Id { get; set; }
        public DateTime OlusturulanTarih { get; set; }
    }
}
