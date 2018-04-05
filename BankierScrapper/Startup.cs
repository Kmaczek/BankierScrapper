using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using BankierScrapper.Common;
using BankierScrapper.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BankierScrapper.Repositories;

namespace BankierScrapper
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            var connection = @"Data Source=DK-GE70\SQL2016;Database=Bankier;User ID=sa;Password=ergo.1234;Integrated Security=False;MultipleActiveResultSets=True;";
            services.AddDbContext<BankierContext>(options => options.UseSqlServer(connection));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<BankierService>().As<IBankierService>();
            builder.RegisterType<RecommandationFactory>().As<IRecommandationFactory>();
            builder.RegisterInstance(new ConfigProvider(Configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
