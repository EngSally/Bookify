
namespace BookTest.Controllers
{
    public class categoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public categoriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View(_context.categories.ToList());
        }


        public IActionResult Create()
        {
            return View("FormCategory");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoriesFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("FormCategory");
            }
            _context.categories.Add(new Category { Name = model.Name });
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            var category = _context.categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }
            var categoryView = new CategoriesFormViewModel { Id = id, Name = category.Name };

            return View("FormCategory", categoryView);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoriesFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("FormCategory", model);
            }

            var category = _context.categories.Find(model.Id);
            if (category is null) { return NotFound(); }
            category.Name = model.Name;
            category.LastUpdate = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeStatue(int id) {
            var category = _context.categories.Find(id);
            if(category is null)
            { return NotFound(); }
            category.Deleted=!category.Deleted;
            category.LastUpdate = DateTime.Now;
            _context.SaveChanges();
            return Ok(DateTime.Now.ToString());


        


        
        }
    }
}
    

