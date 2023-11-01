using dotNES;
using dotNES.Settings;
using System.Collections.Generic;
using System.Windows.Forms;

namespace dotNES.Controllers
{
    class NES001Controller : IController
    {
        private int data;
        private int serialData;
        private bool strobing;
        private KeySet keyset;

        public NES001Controller(KeySet set)
        {
            keyset = set;
        }

        public void Strobe(bool on)
        {
            serialData = data;
            strobing = on;
        }

        public int ReadState()
        {
            int ret = ((serialData & 0x80) > 0).AsByte();
            if (!strobing)
            {
                serialData <<= 1;
                serialData &= 0xFF;
            }
            return ret;
        }

        public void PressKey(KeyEventArgs e)
        {
            var key = keyset[e.KeyCode];
            if (key >= 0)
            {
                data |= 1 << key;
            }
        }

        public void ReleaseKey(KeyEventArgs e)
        {

            var key = keyset[e.KeyCode];
            if (key >= 0)
            {
                data &= ~(1 << key);
            }
        }
    }
}
