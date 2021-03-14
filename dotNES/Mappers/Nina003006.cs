﻿namespace dotNES.Mappers
{
    [MapperDef(79)]
    class Nina003006 : BaseMapper
    {
        protected int _prgBankOffset;
        protected int _chrBankOffset;

        public Nina003006(Emulator emulator) : base(emulator)
        {
        }

        public override void InitializeMemoryMap(PPU ppu)
        {
            ppu.MapReadHandler(0x0000, 0x1FFF, addr => _chrROM[_chrBankOffset + addr]);
        }

        public override void InitializeMemoryMap(CPU cpu)
        {
            cpu.MapReadHandler(0x8000, 0xFFFF, addr => _prgROM[_prgBankOffset + (addr - 0x8000)]);

            cpu.MapWriteHandler(0x4000, 0x5FFF, (addr, val) =>
            {
                if ((addr & 0b1110_0001_0000_0000) == 0b0100_0001_0000_0000)
                {
                    _prgBankOffset = ((val >> 4) & 0x3) * 0x8000;
                    _chrBankOffset = (val & 0x3) * 0x2000;
                }
            });
        }
    }
}