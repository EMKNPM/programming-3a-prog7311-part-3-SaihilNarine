using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG7311_POE_.Data;
using PROG7311_POE_.Factories;
using PROG7311_POE_.Models;
using PROG7311_POE_.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROG7311_POE_.Controllers
{
    public class ContractsController : Controller
    {
        private readonly PROG7311_POE_Context _context;

        public ContractsController(PROG7311_POE_Context context)
        {
            _context = context;
        }

        // GET: Contracts
        public async Task<IActionResult> Index(string searchString, string status, string serviceLevel)
        {
            var contracts = _context.Contract
                .Include(c => c.Client)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                contracts = contracts.Where(c =>
                    c.ClientID.ToString().Contains(searchString));
            }

            if (!string.IsNullOrEmpty(status))
            {
                contracts = contracts.Where(c => c.Status == status);
            }

            if (!string.IsNullOrEmpty(serviceLevel))
            {
                contracts = contracts.Where(c => c.ServiceLevel == serviceLevel);
            }

            return View(await contracts.ToListAsync());
        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract
                .Include(c => c.Client)
                .FirstOrDefaultAsync(m => m.ContractID == id);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: Contracts/Create
        public IActionResult Create()
        {
            ViewBag.ClientID = new SelectList(_context.Client, "ClientID", "ClientID");
            return View();
        }

        // POST: Contracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ClientID = new SelectList(_context.Client, "ClientID", "ClientID", contract.ClientID);
                return View(contract);
            }

            IContractFactory factory =
                contract.ServiceLevel == "Premium"
                    ? new PremiumFactory()
                    : new StandardFactory();

            var newContract = factory.Create();

            newContract.ClientID = contract.ClientID;
            newContract.StartDate = contract.StartDate;
            newContract.EndDate = contract.EndDate;
            newContract.Status = contract.Status;
            newContract.ServiceLevel = contract.ServiceLevel;

            // FILE UPLOAD FIXED
            if (file != null && file.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = Guid.NewGuid() + ".pdf";
                var fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                newContract.AgreementFile = fileName;
            }

            _context.Add(newContract);
            await _context.SaveChangesAsync();

            var subject = new ContractSubject();
            subject.Attach(new Notification());
            subject.Notify("New Contract Created");

            return RedirectToAction(nameof(Index));
        }

        // GET: Contracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }
            ViewData["ClientID"] = new SelectList(_context.Client, "ClientID", "ClientID", contract.ClientID);
            return View(contract);
        }

        // POST: Contracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContractID,ClientID,StartDate,EndDate,Status,ServiceLevel,AgreementFile")] Contract contract)
        {
            if (id != contract.ContractID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractExists(contract.ContractID))
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
            ViewData["ClientID"] = new SelectList(_context.Client, "ClientID", "ClientID", contract.ClientID);
            return View(contract);
        }

        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _context.Contract
                .Include(c => c.Client)
                .FirstOrDefaultAsync(m => m.ContractID == id);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contract.FindAsync(id);
            if (contract != null)
            {
                _context.Contract.Remove(contract);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContractExists(int id)
        {
            return _context.Contract.Any(e => e.ContractID == id);
        }

        public IActionResult ViewFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return NotFound();

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", fileName);

            if (!System.IO.File.Exists(path))
                return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, "application/pdf");
        }

        public IActionResult DownloadFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return NotFound();

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", fileName);

            if (!System.IO.File.Exists(path))
                return NotFound();

            var bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/pdf", fileName);
        }
    }
}
