using System;
using System.Collections.Generic;

#nullable disable

namespace IConnect_Training_.Net_Core_project.Models
{
    public partial class Patient
    {
        public Patient()
        {
            Appointments = new HashSet<Appointment>();
            MedicalHistories = new HashSet<MedicalHistory>();
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime? RegisterationDate { get; set; }
        public string Ssn { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<MedicalHistory> MedicalHistories { get; set; }
    }
}
