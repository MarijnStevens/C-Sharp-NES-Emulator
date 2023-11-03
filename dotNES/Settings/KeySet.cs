using System;
using System.Windows.Forms;

namespace dotNES.Settings
{
    public class KeySet
    {
        public readonly static KeySet Default1 = new KeySet()
        {
        };

        public readonly static KeySet Default2 = new KeySet()
        {
            Up = 104, // Keys.NumPad8
            Down = 101, // Keys.NumPad5
            Left = 100, // Keys.NumPad4
            Right = 102, // Keys.NumPad6
            A = 80, // Keys.P
            B = 79, // Keys.O
            Select = 45, //Keys.Insert
            Start = 36, //Keys.Home
        };

        public byte TurboSpeed = 1;   // Not implemented.
        public int Up { get; set; } = 38; // Keys.Up;
        public int Down { get; set; } = 40; // Keys.Down;
        public int Left { get; set; } = 37; // Keys.Left;
        public int Right { get; set; } = 39; // Keys.Right;
        public int A { get; set; } = 88; // Keys.X;
        public int B { get; set; } = 90; // Keys.Z;
        public int TurboA { get; set; } = 83; // Keys.S;
        public int TurboB { get; set; } = 65; // Keys.A;
        public int Select { get; set; } = 161; // Keys.RShiftKey;
        public int Start { get; set; } = 13; // Keys.Enter;

        public int this[int keyCode]
        {
            get
            {
                if (keyCode == A || keyCode == TurboA) { return 7; }
                if (keyCode == B || keyCode == TurboB) { return 6; }
                if (keyCode == Select) { return 5; }
                if (keyCode == Start) { return 4; }
                if (keyCode == Up) { return 3; }
                if (keyCode == Down) { return 2; }
                if (keyCode == Left) { return 1; }
                if (keyCode == Right) { return 0; }

                return -1;
            }
        }

        public KeySet() { }

    }
}
