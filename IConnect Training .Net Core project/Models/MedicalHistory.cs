using System;
using System.Collections.Generic;

#nullable disable

namespace IConnect_Training_.Net_Core_project.Models
{
    public partial class MedicalHistory
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public long PatientId { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
