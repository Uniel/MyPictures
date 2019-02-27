using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace MyPictures
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        [System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public static void Main()
        {
            MyPictures.Library lib = new MyPictures.Library();
            Console.WriteLine("Test1");

            MyPictures.App window = new MyPictures.App();
            Console.WriteLine("Test2");

            window.InitializeComponent();
            //Console.WriteLine(lib.GetMetaData());
            lib.GetMediaPaths().ForEach(Console.WriteLine);
            //Console.WriteLine();

            window.Run();
        }
    }

}