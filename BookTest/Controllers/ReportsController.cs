using BookTest.Core.ViewModels.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookTest.Controllers
{
    public class ReportsController : Controller
    {
       private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ReportsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Books()
        {
            var authors=_context.Authors.Where(a=> !a.Deleted).OrderBy(a=>a.Name).AsNoTracking().ToList();
            var category=_context.Categories.Where(c=>!c.Deleted).OrderBy(c=>c.Name).AsNoTracking().ToList();
            var viewModel=new BooksReportViewModel();
            viewModel.Authors = _mapper.Map<IEnumerable<SelectListItem>>(authors);
            viewModel.Categories = _mapper.Map<IEnumerable<SelectListItem>>(category);
           
            return View(viewModel); 
        }

    }
}
