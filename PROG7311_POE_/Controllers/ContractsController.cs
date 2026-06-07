using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG7311_POE_.Data;
using PROG7311_POE_.Factories;
using PROG7311_POE_.Models;
using PROG7311_POE_.Observers;
using PROG7311_POE_.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROG7311_POE_.Controllers
{
    public class ContractsController : Controller
    {
        private readonly ContractApiService _apiService;
        private readonly ClientApiService _clientapiService;

        public ContractsController(ContractApiService apiService, ClientApiService clientapiService)
        {
            _apiService = apiService;
            _clientapiService = clientapiService;
        }

        // GET: Contracts
        public async Task<IActionResult> Index(string searchString, string status, string serviceLevel)
        {
            var token = HttpContext.Session.GetString("JWT");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var contracts = (await _apiService.GetContractsAsync(token)).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                contracts = contracts.Where(c =>c.ClientID.ToString().Contains(searchString));
            }

            if (!string.IsNullOrEmpty(status))
            {
                contracts = contracts.Where(c => c.Status == status);
            }

            if (!string.IsNullOrEmpty(serviceLevel))
            {
                contracts = contracts.Where(c => c.ServiceLevel == serviceLevel);
            }

            return View(contracts);
        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _apiService.GetContractByIdAsync(id.Value);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: Contracts/Create
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("JWT");

            var clients = await _clientapiService.GetClientsAsync(token);

            ViewBag.ClientID = new SelectList(clients, "ClientID", "Name");

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
                var token = HttpContext.Session.GetString("JWT");

                var clients = await _clientapiService.GetClientsAsync(token);
                ViewBag.ClientID = new SelectList(clients, "ClientID", "Name");

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

            await _apiService.CreateContractAsync(newContract);

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

            var contract = await _apiService.GetContractByIdAsync(id.Value);

            if (contract == null)
            {
                return NotFound();
            }
            
            return View(contract);
        }

        // POST: Contracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contract contract)
        {
            if (id != contract.ContractID)
            {
                return BadRequest("ID mismatch");
            }
                
            if (!ModelState.IsValid)
            {
                return View(contract);
            }
                
            var existing = await _apiService.GetContractByIdAsync(id);

            if (existing == null)
            {
                return NotFound();
            }

            contract.AgreementFile ??= existing.AgreementFile;

            await _apiService.UpdateContractAsync(contract);

            return RedirectToAction(nameof(Index));
        }

        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _apiService.GetContractByIdAsync(id.Value);

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
            var existing = await _apiService.GetContractByIdAsync(id);

            if (existing == null)
                return NotFound();

            await _apiService.DeleteContractAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ContractExists(int id)
        {
            return await _apiService.ContractExistsAsync(id);
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
