using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

public class EmployeeController(DataContext db, UserManager<AppUser> usrMgr) : Controller
{
  // this controller depends on the DataContext & UserManager classes
  private readonly DataContext _dataContext = db;
  private readonly UserManager<AppUser> _userManager = usrMgr;

  [Authorize(Roles = "northwind-employee")]
  public IActionResult Register() => View();
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async System.Threading.Tasks.Task<IActionResult> Register(EmployeeWithPassword employeeWithPassword)
  {
    if (ModelState.IsValid)
    {
      Employee employee = new Employee
      {
        Email = employeeWithPassword.Email,
        LastName = employeeWithPassword.LastName,
        FirstName = employeeWithPassword.FirstName,
        Title = employeeWithPassword.Title,
        TitleOfCourtesy = employeeWithPassword.TitleOfCourtesy,
        BirthDate = employeeWithPassword.BirthDate,
        HireDate = employeeWithPassword.HireDate,
        Address = employeeWithPassword.Address,
        City = employeeWithPassword.City,
        Region = employeeWithPassword.Region,
        PostalCode = employeeWithPassword.PostalCode,
        Country = employeeWithPassword.Country,
        HomePhone = employeeWithPassword.HomePhone,
        Extension = employeeWithPassword.Extension,
        ReportsTo = employeeWithPassword.ReportsTo
      };
        AppUser user = new AppUser
        {
          // email and username are synced - this is by choice
          Email = employee.Email,
          UserName = employee.Email
        };
        // Add user to Identity DB
        IdentityResult result = await _userManager.CreateAsync(user, employeeWithPassword.Password);
        if (!result.Succeeded)
        {
          AddErrorsFromResult(result);
        }
        else
        {
          // Assign user to employee Role
          result = await _userManager.AddToRoleAsync(user, "northwind-employee");

          if (!result.Succeeded)
          {
            // Delete User from Identity DB
            await _userManager.DeleteAsync(user);
            AddErrorsFromResult(result);
          }
          else
          {
            // Create employee (Northwind)
            _dataContext.AddEmployee(employee);
            return RedirectToAction("Index", "Home");
          }
        }
    }
    return View();
  }
  private void AddErrorsFromResult(IdentityResult result)
  {
    foreach (IdentityError error in result.Errors)
    {
      ModelState.AddModelError("", error.Description);
    }
  }
}