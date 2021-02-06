using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using dotdotdot.Services;

namespace dotdotdot
{
    public class Startup
    {
        readonly string FrontendCorsPolicy = "frontend";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddPolicy(
                    name: FrontendCorsPolicy,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:8080");
                        builder.WithMethods(new string[] {
                            "GET",
                            "POST",
                            "PUT",
                            "DELETE",
                            "OPTIONS",
                            "HEAD"
                        });
                        builder.WithHeaders(new string[] {
                            "Accept",
                            "Accept-Charset",
                            "Accept-Encoding",
                            "Accept-Language",
                            "Accept-Datetime",
                            "Access-Control-Request-Method",
                            "Access-Control-Request-Headers",
                            "Authorization",
                            "Cache-Control",
                            "Connection",
                            "Content-Length",
                            "Content-Type",
                            "Cookie",
                            "Date",
                            "Expect",
                            "Forwarded",
                            "From",
                            "Host",
                            "If-Match",
                            "If-Modified-Since",
                            "If-None-Match",
                            "If-Range",
                            "If-Unmodified-Since",
                            "Origin",
                            "Referer"
                        });
                    }
                );
            });

            services.AddMvc(config => {
            });

            services.AddScoped<ISaveFileReader, SaveFileReader>();
            services.AddControllersWithViews(config => {
            })
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.IncludeFields = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(options);
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors(FrontendCorsPolicy);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                /*
                endpoints.MapControllerRoute(
                    name: "SaveFileList",
                    pattern: "/api/save-file/list",
                    defaults: new {
                        controller = "SaveFile",
                        action = "List"
                    }
                );

                endpoints.MapControllerRoute(
                    name: "SaveFileRead",
                    pattern: "/api/save-file/{filename}",
                    defaults: new {
                        controller = "SaveFile",
                        action = "Read"
                    }
                );*/

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "/api",
                    defaults: new {
                        controller = "Home",
                        action = "Index"
                    }
                );
            });
        }
    }
}
