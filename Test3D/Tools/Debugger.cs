using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test3D.Tools
{
    public interface Debugger
    {
        void Print(string text);
        void Clear();
    }

    public class ConsoleDebugger : Debugger
    {
        public void Print(string text)
        {
            Console.WriteLine(text);
        }

        public void Clear()
        {
            Console.Clear();
        }
    }

    public class WindowDebugger : Debugger
    {
        private Game game;
        private string originalTitle;

        public WindowDebugger(Game game)
        {
            this.game = game;
            this.originalTitle = game.Window.Title;
        }

        public void Print(string text)
        {
            game.Window.Title = text;
        }

        public void Clear()
        {
            game.Window.Title = originalTitle;
        }
    }
}
