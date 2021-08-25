using System;
using System.Collections.Generic;

#nullable disable

namespace IConnect_Training_.Net_Core_project.Models
{
    public partial class AppointmentType
    {
        public AppointmentType()
        {
            Appointments = new HashSet<Appointment>();
        }

        public long Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
