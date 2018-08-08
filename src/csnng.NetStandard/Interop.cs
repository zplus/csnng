using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Nanomsg2.Sharp
{
    /// <summary>
    /// Mostly based off https://github.com/mhowlett/NNanomsg
    /// </summary>
    static class LibraryLoader
    {
        public static string NativeLibraryPath { get; private set; }
        
        static LibraryLoader()
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix ||
                     Environment.OSVersion.Platform == PlatformID.MacOSX ||
                     // Legacy mono value.  See https://www.mono-project.com/docs/faq/technical/
                     (int)Environment.OSVersion.Platform == 128)
            {
                LoadPosixLibrary();
            }
            else
            {
                LoadWindowsLibrary();
            }
        }

        
        static void LoadWindowsLibrary()
        {
            const string libFile = "nng.dll";
            string rootDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string arch = Environment.Is64BitProcess ? "win-x64" : "win-x86";
            var paths = new[]
                {
                    Path.Combine(assemblyDirectory, arch, libFile),  
                    Path.Combine(assemblyDirectory, libFile),
                    
                    Path.Combine(rootDirectory, "runtimes", arch, "native", libFile),
                    Path.Combine(rootDirectory, libFile)
                };

            foreach (var path in paths)
            {
                if (path == null)
                {
                    continue;
                }

                if (File.Exists(path))
                {
                    var addr = LoadLibrary(path);
                    if (addr == IntPtr.Zero)
                    {
                        // Not using NanomsgException because it depends on nn_errno.
                        throw new Exception("LoadLibrary failed: " + path);
                    }
                    NativeLibraryPath = path;
                    return;
                }
            }

            throw new Exception("LoadLibrary failed: unable to locate library " + libFile + ". Searched: " + paths.Aggregate((a, b) => a + "; " + b));
        }

        static void LoadPosixLibrary()
        {
            const int RTLD_NOW = 2;
            const string libFile = "libnng.so";
            string rootDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string arch = Environment.Is64BitProcess ? "x64" : "x86";

            var paths = new[]
                {
                    Path.Combine(rootDirectory, "bin", arch, libFile),
                    Path.Combine(rootDirectory, arch, libFile),
                    Path.Combine(rootDirectory, libFile),
                    Path.Combine("/usr/local/lib", libFile),
                    Path.Combine("/usr/lib", libFile)
                };

            foreach (var path in paths)
            {
                if (path == null)
                {
                    continue;
                }

                if (File.Exists(path))
                {
                    var addr = dlopen(path, RTLD_NOW);
                    if (addr == IntPtr.Zero)
                    {
                        // Not using NanosmgException because it depends on nn_errno.
                        throw new Exception("dlopen failed: " + path + " : " + Marshal.PtrToStringAnsi(dlerror()));
                    }
                    NativeLibraryPath = path;
                    return;
                }
            }

            throw new Exception("dlopen failed: unable to locate library " + libFile + ". Searched: " + paths.Aggregate((a, b) => a + "; " + b));
        }

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("libdl.so")]
        static extern IntPtr dlopen(String fileName, int flags);

        [DllImport("libdl.so")]
        static extern IntPtr dlerror();

        [DllImport("libdl.so")]
        static extern IntPtr dlsym(IntPtr handle, String symbol);
    }
}
