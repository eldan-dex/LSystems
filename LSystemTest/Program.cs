using System;

namespace LSystemTest {
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Game(Environment.GetCommandLineArgs()))
                game.Run();
        }
    }
#endif
}

//blesk: 20 55 4F F=F-F+F[FF-]
//hrouda: 20 55 2F F=F-[FF+F-]F+F[FF-]+
//strom: 10 22 1FF-[2-F+F-F]+[2+F-F+F]
//vlak: 20 90 0-F F=F[+HG]F G=H[HH-G]