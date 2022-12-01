//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Management.Automation;
//using System.Reflection;
//using System.Runtime.Loader;
//using System.Text;
//using System.Threading.Tasks;

//namespace MyTestModule2
//{
//    internal class TestModuleAssemblyLoadContext : AssemblyLoadContext
//    {
//        private readonly string _dependencyDirPath;

//        public TestModuleAssemblyLoadContext(string dependencyDirPath)
//        {
//            _dependencyDirPath = dependencyDirPath;
//        }

//        protected override Assembly Load(AssemblyName assemblyName)
//        {

//                // We do the simple logic here of looking for an assembly of the given name
//                // in the configured dependency directory.
//                string assemblyPath = Path.Combine(
//                    _dependencyDirPath,
//                    $"{assemblyName.Name}.dll");

//                if (File.Exists(assemblyPath))
//                {
//                    Console.WriteLine($"Resolving {assemblyName.Name}.dll in loadcontext");
//                    // The ALC must use inherited methods to load assemblies.
//                    // Assembly.Load*() won't work here.
//                    return LoadFromAssemblyPath(assemblyPath);
//                }

//                Console.WriteLine($"***** {assemblyName.Name} not found!!!!");
//            // For other assemblies, return null to allow other resolutions to continue.
//            return null;
//        }
//    }
//    public class TestModuleResolveEventHandler : IModuleAssemblyInitializer, IModuleAssemblyCleanup
//    {
//        // Get the path of the dependency directory.
//        // In this case we find it relative to the AlcModule.Cmdlets.dll location
//        //private static readonly string s_dependencyDirPath = Path.GetFullPath(
//        //    Path.Combine(
//        //        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
//        //        "Dependencies"));

//        private static readonly string s_dependencyDirPath = Path.GetFullPath(
//        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

//        private static readonly TestModuleAssemblyLoadContext s_dependencyAlc =
//            new TestModuleAssemblyLoadContext(s_dependencyDirPath);

//        public void OnImport()
//        {
//            // Add the Resolving event handler here
//            AssemblyLoadContext.Default.Resolving += ResolveAlcEngine;
//        }

//        public void OnRemove(PSModuleInfo psModuleInfo)
//        {
//            // Remove the Resolving event handler here
//            AssemblyLoadContext.Default.Resolving -= ResolveAlcEngine;
//        }

//        private static Assembly ResolveAlcEngine(AssemblyLoadContext defaultAlc, AssemblyName assemblyToResolve)
//        {
//            // We only want to resolve the Alc.Engine.dll assembly here.
//            // Because this will be loaded into the custom ALC,
//            // all of *its* dependencies will be resolved
//            // by the logic we defined for that ALC's implementation.
//            //
//            // Note that we are safe in our assumption that the name is enough
//            // to distinguish our assembly here,
//            // since it's unique to our module.
//            // There should be no other AlcModule.Engine.dll on the system.
//            //if (!assemblyToResolve.Name.Equals("AlcModule.Engine"))
//            //{
//            //    return null;
//            //}

//            if (assemblyToResolve.Name.Contains("sql", StringComparison.OrdinalIgnoreCase) || assemblyToResolve.Name.Contains("Entity"))
//            {
//                return s_dependencyAlc.LoadFromAssemblyName(assemblyToResolve);
//            }
//            return null;
//            // Allow our ALC to handle the directory discovery concept
//            //
//            // This is where Alc.Engine.dll is loaded into our custom ALC
//            // and then passed through into PowerShell's ALC,
//            // becoming the bridge between both
//        }
//    }
//}
