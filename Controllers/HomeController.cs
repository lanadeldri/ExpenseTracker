using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;

namespace ExpenseTracker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ExpensesDbContext _context;

    public HomeController(ILogger<HomeController> logger,  ExpensesDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult Expenses()
    {
        var allExpenses = _context.Expenses.ToList();
        var total = allExpenses.Sum(x => x.Value);
        ViewBag.Expenses = total;
        return View(allExpenses);
    }

    public IActionResult CreateEditExpense(int? Id)
    {
        
        if (Id != null)
        {
            var expenseInDb = _context.Expenses.SingleOrDefault(x => x.Id == Id);
            return View(expenseInDb);
        }

        return View();
    }
    public IActionResult DeleteExpense(int Id)
    {
        var expenseInDb = _context.Expenses.SingleOrDefault(x => x.Id == Id);
        _context.Expenses.Remove(expenseInDb);
        _context.SaveChanges();
        return RedirectToAction("Expenses");
    }
    public IActionResult CreateEditExpenseForm(Expense model)
    {
        if (model.Id == 0)
        {
            //add
            _context.Expenses.Add(model);
        }
        else
        {
            //edit
            _context.Expenses.Update(model);
        }

        _context.SaveChanges();
        
        return RedirectToAction("Expenses");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}