using System;
using System.Windows.Forms;

namespace dotNES.Settings
{
    public class KeySet
    {
        public readonly static KeySet Default1 = new KeySet()
        {
            // restore default emulator from jeb495/C-Sharp-NES-Emulator
            //A = Keys.A,
            //B = Keys.S
        };

        public readonly static KeySet Default2 = new KeySet()
        {
            A = Keys.P,
            B = Keys.O,
            Select = Keys.Insert,
            Start = Keys.Home,
            Up = Keys.NumPad8,
            Down = Keys.NumPad5,
            Left = Keys.NumPad4,
            Right = Keys.NumPad6
        };

        public byte TurboSpeed = 1;   // Not implemented.
        public Keys Up { get; set; } = Keys.Up;
        public Keys Down { get; set; } = Keys.Down;
        public Keys Left { get; set; } = Keys.Left;
        public Keys Right { get; set; } = Keys.Right;
        public Keys A { get; set; } = Keys.X;
        public Keys B { get; set; } = Keys.Z;
        public Keys TurboA { get; set; } = Keys.S;
        public Keys TurboB { get; set; } = Keys.A;
        public Keys Select { get; set; } = Keys.RShiftKey;
        public Keys Start { get; set; } = Keys.Enter;

        public int this[Keys key]
        {
            get
            {
                if (key == A || key == TurboA) { return 7; }
                if (key == B || key == TurboB) { return 6; }
                if (key == Select) { return 5; }
                if (key == Start) { return 4; }
                if (key == Up) { return 3; }
                if (key == Down) { return 2; }
                if (key == Left) { return 1; }
                if (key == Right) { return 0; }

                return -1;
            }
        }

        public KeySet() { }

    }
}
