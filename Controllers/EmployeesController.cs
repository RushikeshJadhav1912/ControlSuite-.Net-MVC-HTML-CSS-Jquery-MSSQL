using ASPNETMVCCRUD.Data;
using ASPNETMVCCRUD.Models;
using ASPNETMVCCRUD.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace ASPNETMVCCRUD.Controllers
{


    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly MVCDemoDbContext mvcDemoDbContext;

        public EmployeesController(MVCDemoDbContext mvcDemoDbContext)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }


        [HttpGet]
        /* public async Task<IActionResult> Index2(int pg)
		 {
			 *//* var employees = await mvcDemoDbContext.Employees.ToListAsync();*//*
			 List<Employee> emp = await mvcDemoDbContext.Employees.ToListAsync();
			  const int pageSize = 5;
			 if (pg < 1)
				 pg = 1;
			 int resCount=emp.Count();
			 var pager=new Pager(resCount, pg, pageSize);
			 int recSkip=(pg-1)*pageSize;
			 var data = emp.Skip(recSkip).Take(pager.PageSize).ToList();
			 this.ViewBag.Pager = pager;

			*//* return View(emp);*//*
			return View(data);

		 }*/
        /* public async Task<IActionResult> Index(int pageNumber=1,int pageSize=1)
		 {
			 int ExcludeRecords = (pageSize * pageNumber) - pageSize;
			 var employees = await mvcDemoDbContext.Employees
				 .Skip(ExcludeRecords)
				 .Take(ExcludeRecords)
				 .ToListAsync();




			  return View(employees);
		 }*/
        
        public async Task<IActionResult> Index(int? pg, string searchTerm)
        {
            const int pageSize = 5;
            var pageNumber = pg ?? 1;

            var query = mvcDemoDbContext.Employees.AsQueryable();

            // Apply search filter if search term is provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e =>
                    e.Name.Contains(searchTerm) ||
                    e.Email.Contains(searchTerm) ||
                    e.Department.Contains(searchTerm)
                );
            }

            var employees = await query.ToPagedListAsync(pageNumber, pageSize);

            ViewBag.SearchTerm = searchTerm;

            return View(employees);
        }

        [HttpGet]
        [Authorize(Roles ="Admin,SuperAdmin")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequestt)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequestt.Name,
                Email = addEmployeeRequestt.Email,
                Salary = addEmployeeRequestt.Salary,
                Department = addEmployeeRequestt.Department,
                DateOfBirth = addEmployeeRequestt.DateOfBirth
            };

           await mvcDemoDbContext.Employees.AddAsync(employee);
            await mvcDemoDbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Employee added successfully";

            /*  return RedirectToAction("Index");*/
            return RedirectToAction("Add");

        }



        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
           var employee= await mvcDemoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth,

                };
                return await Task.Run(()=>View("View",viewModel));
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel Model)
        {
            
            var employee= await mvcDemoDbContext.Employees.FindAsync(Model.Id);

            if (employee != null)
            {
                employee.Name = Model.Name;
                employee.Email = Model.Email;
                employee.Salary = Model.Salary;
                employee.DateOfBirth = Model.DateOfBirth;
                employee.Department = Model.Department;

                await mvcDemoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
           
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult LiveSearch(string searchTerm)
        {
            var query = mvcDemoDbContext.Employees.AsQueryable();

            // Apply search filter if search term is provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e =>
                    e.Name.Contains(searchTerm) ||
                    e.Email.Contains(searchTerm) ||
                    e.Department.Contains(searchTerm)
                );
            }

            var searchResults = query.ToList();

            return PartialView("_SearchResultsPartial", searchResults);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee=await mvcDemoDbContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                mvcDemoDbContext.Employees.Remove(employee);
                await mvcDemoDbContext.SaveChangesAsync(); 
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }



    }
}
