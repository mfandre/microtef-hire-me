
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Stone.ClientApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Guid ClientCode = new Guid("56e0efe1-2fab-4489-9481-09b8e5a13777");
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //Stone.BusinessServices.Config.AutoMapperConfig.RegisterMappings();
        }
    }
}
