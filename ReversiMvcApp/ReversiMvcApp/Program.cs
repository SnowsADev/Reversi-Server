using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace ReversiMvcApp
{
    public class Program
    {

        public static void Main(string[] args)
        {
            //string file = "fullchain.pem";
            //X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            //store.Open(OpenFlags.ReadOnly);
            //store.Add(new X509Certificate2(X509Certificate2.CreateFromCertFile(file)));
            //store.Close();

            CreateHostBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("https://0.0.0.0:5001", "http://0.0.0.0:5000")
                .UseStartup<Startup>();
    }
}
