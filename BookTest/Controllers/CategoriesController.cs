﻿
using BookTest.Core.ViewModels.Categories;
using System.Security.Claims;

namespace BookTest.Controllers
{
   // [Authorize(Roles = AppRole.Archive)]
    public class categoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public categoriesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            

             var categories=_context.Categories.AsNoTracking().ToList(); 
            var modelView=_mapper.Map< IEnumerable< CategoryViewModel>>(categories);
            return View(modelView);
        }

        public int MinimizedStringLength(string s)
        {
            Dictionary<char,int> dic=new Dictionary<char, int> ();
            char[] chars=s.ToCharArray();   
            foreach(char c in  chars)
            {
                dic.TryAdd(c, 1);
            }
            return dic.Count;
        }


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
            category.CreatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            _context.Categories.Add(category);
            _context.SaveChanges();
            var categoryViewModel=_mapper.Map<CategoryViewModel>(category);

            return PartialView("_PartialRowCategory", categoryViewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            

            var category = _context.Categories.Find(id);
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

            var category = _context.Categories.Find(model.Id);
            if (category is null) { return NotFound(); }
            category = _mapper.Map(model, category);
            category.LastUpdateOn = DateTime.Now;
            category.LastUpdateById= User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            _context.SaveChanges();
            var viewModel=_mapper.Map<CategoryViewModel>(category);
            return PartialView("_PartialRowCategory", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeStatue(int id)
        {
            var category = _context.Categories.Find(id);
            if (category is null)
            { return NotFound(); }
            category.Deleted = !category.Deleted;
            category.LastUpdateOn = DateTime.Now;
            category.LastUpdateById= User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            _context.SaveChanges();
            return Ok(DateTime.Now.ToString());

        }


        public IActionResult AllowItem(CategoriesFormViewModel model)
        {
            var Category = _context.Categories.SingleOrDefault(x=>x.Name == model.Name);
            bool allow=Category is null||Category.Id.Equals(model.Id);
            return Json(allow);
        }




    }
}


