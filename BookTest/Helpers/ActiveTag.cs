using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BookTest.Helpers
{
	[HtmlTargetElement("a", Attributes = "active-when")]
	public class ActiveTag : TagHelper
	{
		public string? ActiveWhen { set; get; }


		[ViewContext]
		[HtmlAttributeNotBound]
		public ViewContext? ViewContextData { get; set; }




		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (string.IsNullOrEmpty(ActiveWhen))
			{
				return;
			}


			var currentController = ViewContextData?.RouteData.Values["controller"]?.ToString()?? string.Empty;
			if (currentController == ActiveWhen)
			{
				if (output.Attributes.ContainsName("class"))
				{
					output.Attributes.SetAttribute("class", $"{output.Attributes["class"].Value}  active");
				}
				else
				{
					output.Attributes.SetAttribute("class", " active");
				}


			}
		}
	}
}
