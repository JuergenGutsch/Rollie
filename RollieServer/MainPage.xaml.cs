using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Devkoes.Restup.WebServer.File;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Rest;
using RollieServer.Controller;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RollieServer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await InitializeWebServer();
        }

        private HttpServer _httpServer;
        private async Task InitializeWebServer()
        {
            // creating the http server
            var httpServer = new HttpServer(5000);
            _httpServer = httpServer;

            // register the api controller
            var restRouteHandler = new RestRouteHandler();
            restRouteHandler.RegisterController<RollieController>();

            // provide the web api 
            httpServer.RegisterRoute("api", restRouteHandler);
            // provide the web ui
            httpServer.RegisterRoute(new StaticFileRouteHandler(@"Web"));

            // starting the http server
            await httpServer.StartServerAsync();
        }
    }
}
