using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft_Clone_with_Open_GL
{
    class Program
    {
        public static  string __DIR__ = Directory.GetCurrentDirectory().ToString();
        public static string __DIR1__ = @"C:\Users\c1419\source\repos\Minecraft Clone with Open GL\Minecraft Clone with Open GL\";
        public static Window win = new Window(GameWindowSettings.Default, new NativeWindowSettings()
        {
            Size = new Vector2i(Global.SCREEN.WIDTH, Global.SCREEN.HEIGHT),
            Title = "c14190069 c14190114 c14190115"
        });
        private static void CopyFilesRecursively(string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                try
                {
                    File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);

                }
                catch (System.IO.IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        static void Main(string[] args)
        {

            Console.WriteLine();
            //gameWindowSetting mengatur hz/frekuensi layar
            //.Default untuk memberi nilai default.
            //using disini seperti nge bundle agar mengoptimasi penggunakan memory.
            //agar apapun yang ada di dalam scope akan terdestroy bila sudah tidak digunakan

            win.Run();
            Console.WriteLine("Hello World!");
        }

    }
}
