using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MMLib.SwaggerForOcelot.DependencyInjection
{
    internal class SwaggerForOcelotHandler
    {
        public DelegatingHandler Handler { get; }

        public SwaggerForOcelotHandler(DelegatingHandler handler)
        {
            Handler = handler;
        }
    }
}
