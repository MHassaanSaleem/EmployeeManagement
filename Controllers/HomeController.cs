using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository emp, IHostingEnvironment hostingEnvironment)
        {
            _employeeRepository = emp;
            this.hostingEnvironment = hostingEnvironment;
        }

        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmpployees();
            return View(model);
            //return _employeeRepository.GetEmployee(1).Name;
        }

        public ViewResult Details(int? id)
        {
            // throw new Exception("Error in Details View");

            Employee employee = _employeeRepository.GetEmployee(id.Value);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employee,
                PageTitle = "Employee Details"
            };

            return View(homeDetailsViewModel);
        }

        // *****************************************************************************************************


        [HttpGet]
        [Authorize]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(EmployeeCreateViewModel employee)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(employee);


                // for upload single file

                //if (employee.Photos != null)
                //{
                //    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                //    uniqueFileName = Guid.NewGuid().ToString() + "_" + employee.Photo.FileName;
                //    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //    employee.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                //}


                Employee newEmployee = new Employee
                {
                    Name = employee.Name,
                    Email = employee.Email,
                    Department = employee.Department,
                    PhotoPath = uniqueFileName
                };

                _employeeRepository.Add(newEmployee);

                //Employee newEmployee =  _employeeRepository.Add(employee);

                return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();
        }



        // *****************************************************************************************************

        [HttpGet]
        [Authorize]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };

            return View(employeeEditViewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;

                if (model.Photos != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploadedFile(model);

                }

                _employeeRepository.Update(employee);

                return RedirectToAction("index");
            }
            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;

            // for uploading multiple files
            if (model.Photos != null && model.Photos.Count > 0)
            {
                foreach (IFormFile photo in model.Photos)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // memory leaks. 

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }
                }
            }

            return uniqueFileName;
        }
    }
}
