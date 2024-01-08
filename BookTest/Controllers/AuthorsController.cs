using BookTest.Core.ViewModels.Authors;

namespace BookTest.Controllers
{
	[Authorize(Roles = AppRole.Archive)]
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

			var authers=_context.Authors.AsNoTracking().ToList();
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
		//                   added   at option.Filters.Add( new  AutoValidateAntiforgeryTokenAttribute());
		public IActionResult Create(AuthorsFormViewModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var author=_mapper.Map<Author>(model);
			author.CreatedById = User.GetUserId();
			_context.Authors.Add(author);
			_context.SaveChanges();
			var viewModel=_mapper.Map<AuthorViewModel>(author);
			return PartialView("_PartialRowAuthors", viewModel);


		}
		[HttpGet]
		[AjaxOnly]
		public IActionResult Edit(int id)
		{
			var author=_context.Authors.Find(id);
			if (author is null) return NotFound();
			var autherModel=_mapper.Map<AuthorsFormViewModel>(author);
			return PartialView("_FormAuthor", autherModel);

		}

		[HttpPost]

		public IActionResult Edit(AuthorsFormViewModel model)
		{
			if (!ModelState.IsValid) return BadRequest();
			var author=_context.Authors.Find(model.Id);
			if (author is null) return NotFound();
			author = _mapper.Map(model, author);
			author.LastUpdateOn = DateTime.Now;
			author.LastUpdateById = User.GetUserId();
			_context.SaveChanges();
			var authorViewModel=_mapper.Map<AuthorViewModel>(author);
			return PartialView("_PartialRowAuthors", authorViewModel);


		}

		public IActionResult Allow(AuthorsFormViewModel model)
		{
			var author=_context.Authors.SingleOrDefault(a=>a.Name == model.Name);
			bool allow=  author is null || author.Id.Equals(model.Id);
			return Json(allow);
		}

		[HttpPost]

		public IActionResult ChangeStatue(int id)
		{
			var author= _context.Authors.Find(id);
			if (author is null) return NotFound();
			author.Deleted = !author.Deleted;
			author.LastUpdateOn = DateTime.Now;
			author.LastUpdateById = User.GetUserId();
			_context.SaveChanges();
			return Ok(author.LastUpdateOn.ToString());
		}



	}
}
