
using BookTest.Core.ViewModels.Categories;
using System.Security.Claims;
using System.Text;

namespace BookTest.Controllers
{
    [Authorize(Roles = AppRole.Archive)]
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

      

     
        public IList<string> CommonChars(string[] words)
        {
            if (words.Length == 0) return null;
            Dictionary<char,int> dic = new Dictionary<char,int>();
            IList<string> res=new  List<string>();
            int count=words.Length;

              for (int i=0;i<words.Length;i++)
                {
                    for(int j = 0; j < words[i].Length;j++)
                    {
                    if (!dic.TryAdd(words[i][j], 1))
                        dic[words[i][j]]++;
                    }
                }
            int repet=0;
            foreach (var pair in dic)
            {
                if (pair.Value == count) res.Add(pair.Key.ToString());
                if(pair.Value>count)
                {
                    repet = pair.Value;
                    while (repet > count)
                    {
                        res.Add(pair.Key.ToString());
                        repet = repet - count;
                    }
                }
            }
            
            return res;
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


