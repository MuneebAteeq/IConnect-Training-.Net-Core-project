using System;
using System.Collections.Generic;

#nullable disable

namespace IConnect_Training_.Net_Core_project.Models
{
    public partial class Specialization
    {
        public Specialization()
        {
            Doctors = new HashSet<Doctor>();
        }

        public long Id { get; set; }
        public string SpecializationName { get; set; }

        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
