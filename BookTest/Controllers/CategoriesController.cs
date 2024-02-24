
using Bookify.Infrastructure.Services;
using Bookify.Web.Core.ViewModels.Categories;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Bookify.Web.Controllers
{
	[Authorize(Roles = AppRole.Archive)]
	public class categoriesController : Controller
	{
		private readonly ICategoriesService _categoriesService;
		private readonly IMapper _mapper;
		private readonly IValidator<CategoriesFormViewModel> _validator;
		public categoriesController(ICategoriesService categoriesService, IMapper mapper, IValidator<CategoriesFormViewModel> validator)
        {
           _categoriesService = categoriesService;
            _mapper = mapper;
            _validator = validator;
        }

        public IActionResult Index()
		{

			var categories=_categoriesService.GetAll(true);
            var modelView=_mapper.Map< IEnumerable< CategoryViewModel>>(categories);

			return View(modelView);
		}



		[AjaxOnly]
		public IActionResult Create()
		{

			return PartialView("_FormCategory");
		}


		[HttpPost]

		public IActionResult Create(CategoriesFormViewModel model)
		{
			var validationResult=_validator.Validate(model);
			if (!validationResult.IsValid) return BadRequest();//fluent Validation

            //if (!ModelState.IsValid) return BadRequest();//data annotation
            var category= _mapper.Map<Category>(model);
			category.CreatedById = User.GetUserId();
            _categoriesService.Add(category);
            var categoryViewModel=_mapper.Map<CategoryViewModel>(category);

			return PartialView("_PartialRowCategory", categoryViewModel);
		}

		[HttpGet]
		[AjaxOnly]
		public IActionResult Edit(int id)
		{


			var category =_categoriesService.GetById(id);
			if (category is null)
			{
				return NotFound();
			}
			CategoriesFormViewModel categoryView = _mapper.Map<CategoriesFormViewModel>(category);

			return PartialView("_FormCategory", categoryView);
		}



		[HttpPost]

		public IActionResult Edit(CategoriesFormViewModel model)
		{
            var validationResult=_validator.Validate(model);
            if (!validationResult.IsValid) return BadRequest();//fluent Validation

            //if (!ModelState.IsValid) return BadRequest();//data annotation

            var category = _categoriesService.GetById(model.Id);
            if (category is null) { return NotFound(); }
			category = _mapper.Map(model, category);
			category.LastUpdateOn = DateTime.Now;
			category.LastUpdateById = User.GetUserId();
            _categoriesService.Update(category);
            var viewModel=_mapper.Map<CategoryViewModel>(category);
			return PartialView("_PartialRowCategory", viewModel);
		}

		[HttpPost]

		public IActionResult ChangeStatue(int id)
		{
			var category =_categoriesService.GetById(id);
            if (category is null)
			{ return NotFound(); }
			category.Deleted = !category.Deleted;
			category.LastUpdateOn = DateTime.Now;
			category.LastUpdateById = User.GetUserId();
			_categoriesService.Update(category);
			return Ok(DateTime.Now.ToString());

		}


		public IActionResult AllowItem(CategoriesFormViewModel model)
		{
			var Category = _categoriesService.Find(x=>x.Name == model.Name);
			bool allow=Category is null||Category.Id.Equals(model.Id);
			return Json(allow);
		}


	}
}


