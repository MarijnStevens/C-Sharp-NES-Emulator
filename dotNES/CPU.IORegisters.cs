﻿using System;
using System.Runtime.CompilerServices;

namespace dotNES.Core;

sealed partial class CPU
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteIORegister(uint reg, byte val)
    {
        switch (reg)
        {
            case 0x4014: // OAM DMA
                _emulator.PPU.PerformDMA(val);
                break;
            case 0x4016:
                _emulator.Controller1.Strobe(val == 1);
                _emulator.Controller2.Strobe(val == 1);
                break;
        }
        if (reg <= 0x401F) return; // APU write
        throw new NotImplementedException($"{reg.ToString("X4")} = {val.ToString("X2")}");
    }

    public uint ReadIORegister(uint reg)
    {
        switch (reg)
        {
            case 0x4016:
                return (uint)_emulator.Controller1.ReadState() & 0x1;

            case 0x4017:
                return (uint)_emulator.Controller2.ReadState() & 0x1;
        }
        return 0x00;
        
    }
}
