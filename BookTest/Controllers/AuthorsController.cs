using Bookify.Web.Core.ViewModels.Authors;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bookify.Web.Controllers
{
	[Authorize(Roles = AppRole.Archive)]
	public class AuthorsController : Controller
	{
       
		private readonly IAuthorsService _authorsService;
        private readonly IMapper _mapper;
        private readonly IValidator<AuthorsFormViewModel> _validator;
	
        public AuthorsController(IAuthorsService authorsService, IMapper mapper, IValidator<AuthorsFormViewModel> validator)
		{

          _authorsService = authorsService;
			_mapper = mapper;
			_validator = validator;
		}
		public IActionResult Index()
		{

			var authers=_authorsService.GetAll(true);
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
			//Fluent Validation
			var resultValidation=_validator.Validate(model);
            if (!resultValidation.IsValid)
                return BadRequest();

                //        if (!ModelState.IsValid)
                //return BadRequest();

                var author=_mapper.Map<Author>(model);
			author.CreatedById = User.GetUserId();
            author= _authorsService.Add(author);
                var viewModel=_mapper.Map<AuthorViewModel>(author);
			return PartialView("_PartialRowAuthors", viewModel);


		}
		[HttpGet]
		[AjaxOnly]
		public IActionResult Edit(int id)
		{
			var author=_authorsService.GetById(id);
			if (author is null) return NotFound();
			var autherModel=_mapper.Map<AuthorsFormViewModel>(author);
			return PartialView("_FormAuthor", autherModel);

		}

		[HttpPost]
	
		public IActionResult Edit(AuthorsFormViewModel model)
		{
            //Fluent Validation
            var resultValidation=_validator.Validate(model);
            if (!resultValidation.IsValid)
                return BadRequest();
            //if (!ModelState.IsValid) return BadRequest();
            var author=_authorsService.GetById(model.Id);
            if (author is null) return NotFound();
			author = _mapper.Map(model, author);
			author.LastUpdateOn = DateTime.Now;
			author.LastUpdateById = User.GetUserId();
            _authorsService.Update(author);
			var authorViewModel=_mapper.Map<AuthorViewModel>(author);
			return PartialView("_PartialRowAuthors", authorViewModel);


		}

		public IActionResult Allow(AuthorsFormViewModel model)
		{
		var author=_authorsService.Find(x=>x.Name == model.Name);
                //_context.Authors.SingleOrDefault(a=>a.Name == model.Name);
            bool allow=  author is null || author.Id.Equals(model.Id);
			return Json(allow);
		}

		[HttpPost]

		public IActionResult ChangeStatue(int id)
		{
			var author=_authorsService.GetById(id);
            if (author is null) return NotFound();
			author.Deleted = !author.Deleted;
			author.LastUpdateOn = DateTime.Now;
			author.LastUpdateById = User.GetUserId();
            _authorsService.Update(author);
            return Ok(author.LastUpdateOn.ToString());
		}

      

    }
}
