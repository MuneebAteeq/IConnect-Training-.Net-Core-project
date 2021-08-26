using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IConnect_Training_.Net_Core_project.Models;
using System.Net;
using Newtonsoft.Json;

namespace IConnect_Training_.Net_Core_project.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ClinicManagementSystemContext _context;

        public DoctorsController(ClinicManagementSystemContext context)
        {
            _context = context;
        }

        // GET: Doctors
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["LastNameSortParm"] = sortOrder == "last_name" ? "last_name_desc" : "last_name";
            ViewData["Specialization"] = sortOrder == "specialization" ? "specialization_desc" : "specialization";
            ViewData["CurrentFilter"] = searchString;

            var doctors = from d in _context.Doctors.Include(d => d.Specialization)
                                                select d;
            if (!String.IsNullOrEmpty(searchString))
            {
                doctors = doctors.Where(d => d.LastName.Contains(searchString)
                                       || d.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    doctors = doctors.OrderByDescending(d => d.FirstName);
                    break;
                case "last_name_desc":
                    doctors = doctors.OrderByDescending(d => d.LastName);
                    break;
                case "last_name":
                    doctors = doctors.OrderBy(d => d.LastName);
                    break;
                case "specialization":
                    doctors = doctors.OrderBy(d => d.Specialization.SpecializationName);
                    break;
                case "specialization_desc":
                    doctors = doctors.OrderByDescending(d => d.Specialization.SpecializationName);
                    break;
                default:
                    doctors = doctors.OrderBy(d => d.FirstName);
                    break;
            }

            return View(await doctors.AsNoTracking().ToListAsync());
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.Specialization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            ViewData["SpecializationId"] = new SelectList(_context.Specializations, "Id", "Id");
            getAllCountries();
            return View();
        }
        void getAllCountries()
        {
            string url = "https://restcountries.eu/rest/v2/all";
            List<string> countries = new List<string>();

            using (WebClient webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(url);
                dynamic array = JsonConvert.DeserializeObject(json);
                foreach (var country in array)
                {
                    //Console.WriteLine(country.name);

                    countries.Add(Convert.ToString(country.name));
                }
            }
            ViewData["Countries"] = countries;
        }
        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Address,Notes,MonthlySalary,PhoneNumber,Iban,Email,SpecializationId,Country")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpecializationId"] = new SelectList(_context.Specializations, "Id", "Id", doctor.SpecializationId);
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            ViewData["SpecializationId"] = new SelectList(_context.Specializations, "Id", "Id", doctor.SpecializationId);
            getAllCountries();
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,FirstName,LastName,Address,Notes,MonthlySalary,PhoneNumber,Iban,Email,SpecializationId,Country")] Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpecializationId"] = new SelectList(_context.Specializations, "Id", "Id", doctor.SpecializationId);
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.Specialization)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorExists(long id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }
    }
}
