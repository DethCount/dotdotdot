#pragma checksum "C:\Users\Count\Documents\projects\dotdotdot\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fa2c2aa84058b5fcb8e784e8657c64f91027854a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Count\Documents\projects\dotdotdot\Views\_ViewImports.cshtml"
using dotdotdot;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Count\Documents\projects\dotdotdot\Views\_ViewImports.cshtml"
using dotdotdot.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fa2c2aa84058b5fcb8e784e8657c64f91027854a", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"35f04cd4364e6dbd4a44ca5a4a6a2d2a5ebf192a", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\Count\Documents\projects\dotdotdot\Views\Home\Index.cshtml"
  

    ViewData["Title"] = "Satisfactory - Save file viewer";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"row\">\r\n    <table class=\"table table-striped\">\r\n        <thead>\r\n            <th>Safe file</th>\r\n            <th>Last Modified</th>\r\n        </thead>\r\n        <tbody>\r\n");
#nullable restore
#line 13 "C:\Users\Count\Documents\projects\dotdotdot\Views\Home\Index.cshtml"
         foreach (ValueTuple<string,DateTime> file in (List<ValueTuple<string,DateTime>>) ViewData["files"])
        {
            string filename = file.Item1.Replace((string) ViewData["basepath"], "");


#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <td><a");
            BeginWriteAttribute("href", " href=\"", 500, "\"", 557, 1);
#nullable restore
#line 18 "C:\Users\Count\Documents\projects\dotdotdot\Views\Home\Index.cshtml"
WriteAttributeValue("", 507, Url.RouteUrl("File", new { filename = filename }), 507, 50, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 18 "C:\Users\Count\Documents\projects\dotdotdot\Views\Home\Index.cshtml"
                                                                            Write(Html.Encode(filename));

#line default
#line hidden
#nullable disable
            WriteLiteral("</a></td>\r\n                <td>");
#nullable restore
#line 19 "C:\Users\Count\Documents\projects\dotdotdot\Views\Home\Index.cshtml"
               Write(Html.Encode(file.Item2));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            </tr>\r\n");
#nullable restore
#line 21 "C:\Users\Count\Documents\projects\dotdotdot\Views\Home\Index.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </tbody>\r\n    </table>\r\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591