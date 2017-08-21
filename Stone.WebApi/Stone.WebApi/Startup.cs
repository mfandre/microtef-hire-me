using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Stone.WebApi.Startup))]

namespace Stone.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
