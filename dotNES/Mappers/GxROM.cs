﻿using dotNES.Core;

namespace dotNES.Mappers;

[MapperDef(66)]
class GxROM : BaseMapper
{
    protected int _prgBankOffset;
    protected int _chrBankOffset;

    public GxROM(Emulator emulator) : base(emulator)
    {
    }

    public override void InitializeMemoryMap(PPU ppu)
    {
        ppu.MapReadHandler(0x0000, 0x1FFF, addr => _chrROM[_chrBankOffset + addr]);
    }

    public override void InitializeMemoryMap(CPU cpu)
    {
        cpu.MapReadHandler(0x8000, 0xFFFF, addr => _prgROM[_prgBankOffset + (addr - 0x8000)]);

        cpu.MapWriteHandler(0x8000, 0xFFFF, (addr, val) =>
        {
            _prgBankOffset = (val >> 4 & 0x3) * 0x8000;
            _chrBankOffset = (val & 0x3) * 0x2000;
        });
    }
}
