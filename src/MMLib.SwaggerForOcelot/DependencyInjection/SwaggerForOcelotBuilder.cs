using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MMLib.SwaggerForOcelot.Configuration;
using MMLib.SwaggerForOcelot.ServiceDiscovery;
using MMLib.SwaggerForOcelot.Transformation;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MMLib.SwaggerForOcelot.DependencyInjection
{
    public class SwaggerForOcelotBuilder : ISwaggerForOcelotBuilder
    {
        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }

        public SwaggerForOcelotBuilder(IServiceCollection services, IConfiguration configurationRoot)
        {
            Configuration = configurationRoot;
            Services = services;

            Services
            .AddTransient<ISwaggerServiceDiscoveryProvider, SwaggerServiceDiscoveryProvider>()
            .AddTransient<ISwaggerJsonTransformer, SwaggerJsonTransformer>()
            .Configure<List<RouteOptions>>(options => Configuration.GetSection("Routes").Bind(options))
            .Configure<List<SwaggerEndPointOptions>>(options
                => Configuration.GetSection(SwaggerEndPointOptions.ConfigurationSectionName).Bind(options))
            .AddHttpClient("SwaggerForOcelot")
            .ConfigureHttpMessageHandlerBuilder(messageHandlerBuilder =>
            {
                var handlers = services.BuildServiceProvider().GetServices<SwaggerForOcelotHandler>();

                if (handlers != null)
                {
                    foreach (var handler in handlers)
                    {
                        messageHandlerBuilder.AdditionalHandlers.Add(handler.Handler);
                    }
                }
            });
        }

        public ISwaggerForOcelotBuilder AddDelegatingHandler(Type type)
        {
            if (!type.IsSubclassOf(typeof(DelegatingHandler)))
            {
                throw new InvalidOperationException("Type must be DelegatingHandler");    
            }

            Services.AddTransient(type);
            Services.AddTransient(serviceProvider =>
            {
                var handler = serviceProvider.GetService(type);
                return new SwaggerForOcelotHandler(handler as DelegatingHandler);
            });

            return this;
        }

        public ISwaggerForOcelotBuilder AddDelegatingHandler<T>() where T : DelegatingHandler
        {
            return AddDelegatingHandler(typeof(T));
        }
    }
}
