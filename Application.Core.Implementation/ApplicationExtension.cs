using System;
using System.Collections.Generic;
using System.Text;
using Application.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Implementation
{
    public static class ApplicationExtension
    {
        public static void UseApplication(this IServiceCollection services)
        {
            services.AddTransient<IRouletteApplication, RouletteApplication>();
            services.AddTransient<IBetApplication, BetApplication>();
        }
    }
}
