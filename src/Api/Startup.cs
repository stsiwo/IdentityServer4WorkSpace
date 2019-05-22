using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940


        /**
         * role of middleware is to make sure following: 
         *  1. validate the incoming token to make sure it is coming from a trusted issuer (identity server created just right before)
         *  2. validate that the token is valid to be used with this api (audience)
         **/
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            // adds the authenticaiton services to DI and configures "Bearer" as default scheme
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "api1"; 
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

//            app.Run(async (context) =>
//            {
//                await context.Response.WriteAsync("Hello World!");
//            });

            // adds the authentication middleware to the pipeline so authentication will be performed automatically on evey call into the host
            // must before "app.UseMvc()"
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
