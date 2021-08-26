using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IConnect_Training_.Net_Core_project.Models;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;

namespace IConnect_Training_.Net_Core_project.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ClinicManagementSystemContext _context;

        public PatientsController(ClinicManagementSystemContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["LastNameSortParm"] = sortOrder == "last_name" ? "last_name_desc" : "last_name";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            var patinets = from p in _context.Patients
                           select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                patinets = patinets.Where(p => p.LastName.Contains(searchString)
                                       || p.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    patinets = patinets.OrderByDescending(p => p.FirstName);
                    break;
                case "last_name_desc":
                    patinets = patinets.OrderByDescending(p => p.LastName);
                    break;
                case "last_name":
                    patinets = patinets.OrderBy(p => p.LastName);
                    break;
                case "Date":
                    patinets = patinets.OrderBy(p => p.RegisterationDate);
                    break;
                case "date_desc":
                    patinets = patinets.OrderByDescending(p => p.RegisterationDate);
                    break;
                default:
                    patinets = patinets.OrderBy(p => p.FirstName);
                    break;
            }
            return View(await patinets.AsNoTracking().ToListAsync());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
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
        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Birthday,Gender,PhoneNumber,Email,Address,RegisterationDate,Ssn,Country")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            getAllCountries();
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,FirstName,LastName,Birthday,Gender,PhoneNumber,Email,Address,RegisterationDate,Ssn,Country")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var patient = await _context.Patients.FindAsync(id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(long id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
