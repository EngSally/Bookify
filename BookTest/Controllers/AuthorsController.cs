using BookTest.Core.ViewModels.Authors;

namespace BookTest.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public AuthorsController(ApplicationDbContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var authers=_context.authors.AsNoTracking().ToList();
            var modelView=_mapper.Map<IEnumerable<AuthorViewModel>>(authers);

            return View(modelView);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Create()
        {

            return PartialView("_FormAuthor");
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(AuthorsFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var author=_mapper.Map<Author>(model);
            _context.authors.Add(author);
            _context.SaveChanges();
            var viewModel=_mapper.Map<AuthorViewModel>(author);
            return PartialView("_PartialRowAuthors", viewModel);


        }
        [HttpGet]
        [AjaxOnly]
        public IActionResult Edit(int id)
        {
            var author=_context.authors.Find(id);
            if (author is null) return NotFound();
            var autherModel=_mapper.Map<AuthorsFormViewModel>(author);
            return PartialView("_FormAuthor", autherModel);

        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(AuthorsFormViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var author=_context.authors.Find(model.Id);
            if (author is null) return NotFound();
            author = _mapper.Map(model, author);
            author.LastUpdate = DateTime.Now;
            _context.SaveChanges();
            var authorViewModel=_mapper.Map<AuthorViewModel>(author);
            return PartialView("_PartialRowAuthors", authorViewModel);


        }

        public IActionResult Allow(AuthorsFormViewModel model)
        {
            var author=_context.authors.SingleOrDefault(a=>a.Name == model.Name);
            bool allow=  author is null || author.Id.Equals(model.Id);
            return Json(allow);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult ChangeStatue(int id)
        {
            var author= _context.authors.Find(id);
            if (author is null) return NotFound();
            author.Deleted = !author.Deleted;
            author.LastUpdate = DateTime.Now;
            _context.SaveChanges();
            return Ok(author.LastUpdate.ToString());
        }



    }
}
