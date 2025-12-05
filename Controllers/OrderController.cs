using Microsoft.AspNetCore.Mvc;
// Don't know if needed
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

public class OrderController : Controller
{
  // this controller depends on the NorthwindRepository
  private DataContext _dataContext;
  public OrderController(DataContext db) => _dataContext = db;
  
  // [Authorize(Roles = "northwind-employee")]
  public IActionResult ViewOrders() => View(_dataContext.Orders.OrderBy(o => o.RequiredDate));
}
