using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.XForms;
using System.Reflection;
using Xamarin.Forms;

namespace SharpGeo.Tests.WinPhone
{
    class TestApp : Xamarin.Forms.Application
    {
        public TestApp()
        {
            var runner = new TestRunner();
            // Add your test types/assemblies
            runner.Add(typeof(PositionTest).GetTypeInfo().Assembly);
            var page = new TestRunnerPage(runner);
            MainPage = new NavigationPage(page);
        }
    }
}
