using System;
using System.Collections.Generic;

#nullable disable

namespace IConnect_Training_.Net_Core_project.Models
{
    public partial class Appointment
    {
        public long Id { get; set; }
        public long DoctorId { get; set; }
        public long PatientId { get; set; }
        public DateTime Reservation { get; set; }
        public long AppointmentId { get; set; }

        public virtual AppointmentType AppointmentNavigation { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
