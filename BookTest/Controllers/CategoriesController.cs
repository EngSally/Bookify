﻿


namespace BookTest.Controllers
{
    public class categoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public categoriesController(ApplicationDbContext context,IMapper  mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var categories=_context.categories.AsNoTracking().ToList();

            var modelView=_mapper.Map< IEnumerable< CategoryViewModel>>(categories);
            return View(modelView);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView("_FormCategory");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoriesFormViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var category= _mapper.Map<Category>(model);
            _context.categories.Add(category);
            _context.SaveChanges();
            var categoryViewModel=_mapper.Map<CategoryViewModel>(category);

            return PartialView("_PartialRowCategory", categoryViewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int id)
        {

            var category = _context.categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }
            CategoriesFormViewModel categoryView = _mapper.Map<CategoriesFormViewModel>(category);

            return PartialView("_FormCategory", categoryView);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoriesFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var category = _context.categories.Find(model.Id);
            if (category is null) { return NotFound(); }
            category = _mapper.Map(model, category);
            category.LastUpdate = DateTime.Now;
            _context.SaveChanges();
            var viewModel=_mapper.Map<CategoryViewModel>(category);



            return PartialView("_PartialRowCategory", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeStatue(int id)
        {
            var category = _context.categories.Find(id);
            if (category is null)
            { return NotFound(); }
            category.Deleted = !category.Deleted;
            category.LastUpdate = DateTime.Now;
            _context.SaveChanges();
            return Ok(DateTime.Now.ToString());

        }


        public IActionResult AllowItem(CategoriesFormViewModel model)
        {
           var Category = _context.categories.SingleOrDefault(x=>x.Name == model.Name);
            bool allow=Category is null||Category.Id.Equals(model.Id);
            return Json(allow);
        }
       



    }
}


