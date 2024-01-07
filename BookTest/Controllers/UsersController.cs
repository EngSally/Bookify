using BookTest.Core.ViewModels.Users;
using BookTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace BookTest.Controllers
{
    [Authorize(Roles =AppRole.Admin)] 
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManger;
        private readonly IEmailSender _emailSender;
        private readonly IEmailBodyBuilder _emailBodyBuilder;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private  readonly IMapper _mapper;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManger,
            IEmailSender emailSender, IEmailBodyBuilder emailBodyBuilder,
            IWebHostEnvironment webHostEnvironment , IMapper mapper)
        {
            _userManager = userManager;
            _roleManger = roleManger;
            _emailSender = emailSender;
            _emailBodyBuilder = emailBodyBuilder;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            
        }

        public async Task<IActionResult> Index()
        {
            
            var users=await _userManager.Users.ToListAsync();
            var userViewModel=_mapper.Map<IEnumerable<UserViewModel>>(users);
            return View(userViewModel);
        }



    


        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> Create()
        {
            var model=new UserFormViewModel
            {
                Roles=await _roleManger.Roles.Select(
                  r=>new SelectListItem
                  {
                      Text=r.Name,
                      Value=r.Name//Assign Role To User By RoleName
                  }).ToListAsync()
            };
            return PartialView("_Form", model);
       
        }
        [HttpPost]
                         
        public  async  Task<IActionResult>   Create(UserFormViewModel model)
        {
            if(!ModelState.IsValid) {

                return BadRequest();
            }

            var user=new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                CreatedById=User.GetUserId()
            };
         var result=   await _userManager.CreateAsync(user,model.Password);
            if(result.Succeeded)
            {
               await  _userManager.AddToRolesAsync(user, model.SelectedRoles);


                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code },
                    protocol: Request.Scheme);

                var placeholders = new Dictionary<string, string>()
                {
                    { "imageUrl", "https://res.cloudinary.com/devcreed/image/upload/v1668732314/icon-positive-vote-1_rdexez.svg" },
                    { "header", $"Hey {user.FullName}, thanks for joining us!" },
                    { "body", "please confirm your email" },
                    { "url", $"{HtmlEncoder.Default.Encode(callbackUrl!)}" },
                    { "linkTitle", "Active Account!" }
                };

                var body = _emailBodyBuilder.GetEmailBody(EmailTemplates.Email, placeholders);

                await _emailSender.SendEmailAsync(user.Email, "Confirm your email", body);
                UserViewModel userViewModel=_mapper.Map<UserViewModel>(user);
                return PartialView("_PartialRowUser", userViewModel);
            }
            return BadRequest(string.Join(',', result.Errors.Select(e => e.Description)));

        }

        [HttpGet]
        [AjaxOnly]

        public async Task<IActionResult> Edit(string  id)
        {
            var user=_userManager.FindByIdAsync(id).Result;
            if (user is null) return NotFound();

            var model=_mapper.Map<UserFormViewModel>(user);
            model.SelectedRoles= await _userManager.GetRolesAsync(user);
            model.Roles = await _roleManger.Roles.Select(
                 r => new SelectListItem
                 {
                     Text = r.Name,
                     Value = r.Name//Assign Role To User By RoleName
                 }).ToListAsync();

            return PartialView("_Form", model);

        }
        [HttpPost]
                         
        public  async Task <IActionResult> Edit(UserFormViewModel model )
        {
            if(!ModelState.IsValid)  return BadRequest(ModelState);
            var user = await  _userManager.FindByIdAsync(model.Id);
            if(user is null) return NotFound();
           
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FullName = model.FullName;
                user.LastUpdateOn = DateTime.Now;
                user.LastUpdateById = User.GetUserId();
               var  result= await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                IEnumerable<string>oldRols=   await  _userManager.GetRolesAsync(user) ;
                await _userManager.RemoveFromRolesAsync(user, oldRols);
                await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                var RowUserModel=_mapper.Map<UserViewModel>(user);
                return PartialView("_PartialRowUser", RowUserModel);

            }
          
            return BadRequest(string.Join(',', result.Errors.Select(e => e.Description)));

        }

        [HttpPost]
                         
        public async Task<IActionResult> ChangeStatue(string Id)
        {
            var user = await   _userManager.FindByIdAsync(Id); 
            if(user is null) return NotFound();
            user.Deleted = !user.Deleted;
            user.LastUpdateOn = DateTime.Now;
            user.LastUpdateById = User.GetUserId();
           await  _userManager.UpdateAsync(user);
            return Ok(DateTime.Now.ToString());

        }


        [HttpPost]
                         
        public async Task<IActionResult> UnLockAccount(string Id)
        {
            var user = await   _userManager.FindByIdAsync(Id);
            if (user is null) return NotFound();

         if(await   _userManager.IsLockedOutAsync(user)) 
                await _userManager.SetLockoutEndDateAsync(user, null);
            return Ok();

        }

        
        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> RestPassword(string Id)
        {
            var user= await  _userManager.FindByIdAsync(Id);
            if (user is null) return NotFound();

            var model=_mapper.Map<RestPasswordViewModel>(user);
            return PartialView("_RestPassword", model);
        }

        [HttpPost]
                         
        public async Task<IActionResult> RestPassword(RestPasswordViewModel model )
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);  
            var user= await  _userManager.FindByIdAsync(model.Id);
            if(user is null) return NotFound();
            var OldHashPassword=user.PasswordHash;
            await  _userManager.RemovePasswordAsync(user);
            var result=    await  _userManager.AddPasswordAsync(user, model.Password);
            if(result.Succeeded) 
            { 
                user.LastUpdateOn= DateTime.Now;
                user.LastUpdateById = User.GetUserId();
                await _userManager.UpdateAsync(user);
                var RowUserModel=_mapper.Map<UserViewModel>(user);
                return PartialView("_PartialRowUser", RowUserModel);

            }

            user.PasswordHash = OldHashPassword;
            await _userManager.UpdateAsync(user);
            return BadRequest(string.Join(',', result.Errors.Select(e => e.Description)));
        }

            public  async  Task<IActionResult> AllowEmail(string Id, string email)
        {

            var user=await   _userManager.FindByEmailAsync(email);
            var isAllow= (user is null || user.Id == Id);
            return Json (isAllow);

        }
        public  async Task  <IActionResult>  AllowUserName( string Id,string UserName)
        {
            var user =await _userManager.FindByNameAsync(UserName);
            var isAllow= (user is null || user.Id == Id);
            return Json(isAllow);
        }
       

       



    }
}
