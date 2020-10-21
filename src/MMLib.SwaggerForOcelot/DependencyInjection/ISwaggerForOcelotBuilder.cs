using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MMLib.SwaggerForOcelot.DependencyInjection
{
    public interface ISwaggerForOcelotBuilder
    {
        IServiceCollection Services { get; }

        IConfiguration Configuration { get; }

        ISwaggerForOcelotBuilder AddDelegatingHandler(Type type);

        ISwaggerForOcelotBuilder AddDelegatingHandler<T>()
            where T : DelegatingHandler;
    }
}
