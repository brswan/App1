using GLib;
using HarmonyLib;
using Silk.NET.Core.Loader;
using System;
using System.IO;
using Uno.UI.Runtime.Skia;
using Windows.UI.ViewManagement;

namespace App1.Skia.Gtk
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("THIS IS THE UPDATED VERSION!!!");

            ExceptionManager.UnhandledException += delegate (UnhandledExceptionArgs expArgs)
            {
                Console.WriteLine("GLIB UNHANDLED EXCEPTION" + expArgs.ExceptionObject.ToString());
                expArgs.ExitApplication = true;
            };

            RenderSurfaceType? mode = RenderSurfaceType.OpenGLES;
            bool applyMode = true;

            //JtttSharedLibrary.JtttApplication.DispatchHandler += (a) =>
            //{
            //    try
            //    {
            //        a?.Invoke();
            //    }
            //    catch { }
            //    //this.SuspendBindings();
            //    //Gtk.Application.Invoke((e, s) => { a?.Invoke(); });
            //    //this.ResumeBindings();
            //};

            try
            {
                string fmode = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "rendermode");
                if (File.Exists(fmode))
                {
                    mode = (RenderSurfaceType)Int32.Parse(File.ReadAllText(fmode));
                    applyMode = true;
                }
            }
            catch { }

            if (GetShouldRunPatches() && applyMode && mode == RenderSurfaceType.OpenGLES)
            {
                RunPatches();
            }

            var host = new GtkHost(() => new App() { });
            ApplicationView.PreferredLaunchViewSize = new Windows.Foundation.Size(600, 1024);

            if (applyMode)
            {
                host.RenderSurfaceType = mode;
            }

            //DEVce.Common.Lib.ViewModels.ConsoleVM.RenderModeStatic = (host.RenderSurfaceType.HasValue ? host.RenderSurfaceType.Value.ToString() : "NONE");
            string logMessage = "********************* RENDER MODE = " + (host.RenderSurfaceType.HasValue ? host.RenderSurfaceType.Value.ToString() : "NONE");
            Console.WriteLine(logMessage);
            Serilog.Log.Debug(logMessage);
            host.Run();
        }

        public static bool GetShouldRunPatches()
        {
            try
            {
                string patches = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "runglespatches");
                if (File.Exists(patches))
                {
                    return bool.Parse(File.ReadAllText(patches));
                }
            }
            catch { }

            return false;
        }

        public static void RunPatches()
        {
            var type = AccessTools.TypeByName("GLCoreLibraryNameContainer");
            SearchPathContainer path = Activator.CreateInstance(type) as SearchPathContainer;

            Console.WriteLine("############## BEFORE PATCH Linux=" + path.Linux);

            var harmony = new Harmony("com.example.patch");
            var mOriginalLinux = AccessTools.PropertyGetter(AccessTools.TypeByName("GLCoreLibraryNameContainer"), "Linux");
            var mOriginalAndroid = AccessTools.PropertyGetter(AccessTools.TypeByName("GLCoreLibraryNameContainer"), "Android");
            var mPrefix = AccessTools.Method(typeof(Program), "PrefixSo");
            var mPostfix = AccessTools.Method(typeof(Program), "PostfixSo");

            var mOriginalIsSupported = AccessTools.PropertyGetter(AccessTools.TypeByName("OpenGLESRenderSurface"), "IsSupported");

            try
            {
                var isSupported = (bool)mOriginalIsSupported.Invoke(null, null);
                Console.WriteLine("############## BEFORE PATCH IsSupported=" + isSupported);
            }
            catch
            {
                Console.WriteLine("IsSupported failed.");
            }

            var mPrefixIsSupported = AccessTools.Method(typeof(Program), "PrefixIsSupported");
            var mPostfixIsSupported = AccessTools.Method(typeof(Program), "PostfixIsSupported");

            harmony.Patch(mOriginalLinux, new HarmonyMethod(mPrefix), new HarmonyMethod(mPostfix));
            harmony.Patch(mOriginalAndroid, new HarmonyMethod(mPrefix), new HarmonyMethod(mPostfix));
            harmony.Patch(mOriginalIsSupported, new HarmonyMethod(mPrefixIsSupported), new HarmonyMethod(mPostfixIsSupported));

            try
            {
                var isSupported = (bool)mOriginalIsSupported.Invoke(null, null);
                Console.WriteLine("############## BEFORE PATCH IsSupported=" + isSupported);
            }
            catch
            {
                Console.WriteLine("IsSupported failed.");
            }

            path = Activator.CreateInstance(type) as SearchPathContainer;

            Console.WriteLine("############## AFTER PATCH Linux=" + path.Linux);
        }

        [HarmonyPrefix]
        public static void PrefixSo(ref SearchPathContainer __instance)
        {
        }

        [HarmonyPostfix]
        public static void PostfixSo(ref string __result, ref SearchPathContainer __instance)
        {
            __result = "libGLESv2.so";
        }

        [HarmonyPrefix]
        public static void PrefixIsSupported()
        {
        }

        [HarmonyPostfix]
        public static void PostfixIsSupported(ref bool __result)
        {
            __result = true;
        }
    }
}