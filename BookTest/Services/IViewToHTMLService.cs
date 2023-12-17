namespace BookTest.Services
{
    public interface IViewToHTMLService
    {
        Task<string> RenderViewToStringAsync(ControllerContext actionContext, string viewPath, object model);
    }
}
