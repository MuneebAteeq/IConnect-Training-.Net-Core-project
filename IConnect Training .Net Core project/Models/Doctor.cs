using System;
using System.Collections.Generic;

#nullable disable

namespace IConnect_Training_.Net_Core_project.Models
{
    public partial class Doctor
    {
        public Doctor()
        {
            Appointments = new HashSet<Appointment>();
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public decimal? MonthlySalary { get; set; }
        public string PhoneNumber { get; set; }
        public string Iban { get; set; }
        public string Email { get; set; }
        public long SpecializationId { get; set; }
        public string Country { get; set; }

        public virtual Specialization Specialization { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
