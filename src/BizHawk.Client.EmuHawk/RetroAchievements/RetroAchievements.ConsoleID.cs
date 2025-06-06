using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Atari.Jaguar;
using BizHawk.Emulation.Cores.Consoles.Nintendo.Gameboy;
using BizHawk.Emulation.Cores.Consoles.Nintendo.NDS;
using BizHawk.Emulation.Cores.Consoles.Sega.gpgx;
using BizHawk.Emulation.Cores.Consoles.Sega.PicoDrive;
using BizHawk.Emulation.Cores.Nintendo.SNES;

namespace BizHawk.Client.EmuHawk
{
	public abstract partial class RetroAchievements
	{
		public enum ConsoleID : int
		{
			UnknownConsoleID = 0,
			MegaDrive = 1,
			N64 = 2,
			SNES = 3,
			GB = 4,
			GBA = 5,
			GBC = 6,
			NES = 7,
			PCEngine = 8,
			SegaCD = 9,
			Sega32X = 10,
			MasterSystem = 11,
			PlayStation = 12,
			Lynx = 13,
			NeoGeoPocket = 14,
			GameGear = 15,
			GameCube = 16,
			Jaguar = 17,
			DS = 18,
			WII = 19,
			WIIU = 20,
			PlayStation2 = 21,
			Xbox = 22,
			MagnavoxOdyssey = 23,
			PokemonMini = 24,
			Atari2600 = 25,
			MSDOS = 26,
			Arcade = 27,
			VirtualBoy = 28,
			MSX = 29,
			C64 = 30,
			ZX81 = 31,
			Oric = 32,
			SG1000 = 33,
			VIC20 = 34,
			Amiga = 35,
			AtariST = 36,
			AmstradCPC = 37,
			AppleII = 38,
			Saturn = 39,
			Dreamcast = 40,
			PSP = 41,
			CDi = 42,
			ThreeDO = 43,
			Colecovision = 44,
			Intellivision = 45,
			Vectrex = 46,
			PC8800 = 47,
			PC9800 = 48,
			PCFX = 49,
			Atari5200 = 50,
			Atari7800 = 51,
			X68K = 52,
			WonderSwan = 53,
			CassetteVision = 54,
			SuperCassetteVision = 55,
			NeoGeoCD = 56,
			FairchildChannelF = 57,
			FMTowns = 58,
			ZXSpectrum = 59,
			GameAndWatch = 60,
			NokiaNGage = 61,
			Nintendo3DS = 62,
			Supervision = 63,
			SharpX1 = 64,
			Tic80 = 65,
			ThomsonTO8 = 66,
			PC6000 = 67,
			Pico = 68,
			MegaDuck = 69,
			Zeebo = 70,
			Arduboy = 71,
			WASM4 = 72,
			Arcadia2001 = 73,
			IntertonVC4000 = 74,
			ElektorTVGamesComputer = 75,
			PCEngineCD = 76,
			JaguarCD = 77,
			DSi = 78,
			TI83 = 79,
			Uzebox = 80,

			NumConsoleIDs
		}

		protected ConsoleID SystemIdToConsoleId()
		{
			return Emu.SystemId switch
			{
				VSystemID.Raw.A26 => ConsoleID.Atari2600,
				VSystemID.Raw.A78 => ConsoleID.Atari7800,
				VSystemID.Raw.Amiga => ConsoleID.Amiga,
				VSystemID.Raw.AmstradCPC => ConsoleID.AmstradCPC,
				VSystemID.Raw.AppleII => ConsoleID.AppleII,
				VSystemID.Raw.Arcade => ConsoleID.Arcade,
				VSystemID.Raw.C64 => ConsoleID.C64,
				VSystemID.Raw.ChannelF => ConsoleID.FairchildChannelF,
				VSystemID.Raw.Coleco => ConsoleID.Colecovision,
				VSystemID.Raw.DEBUG => ConsoleID.UnknownConsoleID,
				VSystemID.Raw.Dreamcast => ConsoleID.Dreamcast,
				VSystemID.Raw.GameCube => ConsoleID.GameCube,
				VSystemID.Raw.GB when Emu is IGameboyCommon { IsCGBMode: true } => ConsoleID.GBC,
				VSystemID.Raw.GB => ConsoleID.GB,
				VSystemID.Raw.GBA => ConsoleID.GBA,
				VSystemID.Raw.GBC => ConsoleID.GBC, // Not actually used
				VSystemID.Raw.GBL when Emu is ILinkedGameBoyCommon { First: { IsCGBMode: true } } => ConsoleID.GBC,
				VSystemID.Raw.GBL => ConsoleID.GB, // actually can be a mix of GB and GBC
				VSystemID.Raw.GEN when Emu is GPGX { IsMegaCD: true } => ConsoleID.SegaCD,
				VSystemID.Raw.GEN when Emu is PicoDrive { Is32XActive: true } => ConsoleID.Sega32X,
				VSystemID.Raw.GEN => ConsoleID.MegaDrive,
				VSystemID.Raw.GG => ConsoleID.GameGear,
				VSystemID.Raw.GGL => ConsoleID.GameGear, // ???
				VSystemID.Raw.INTV => ConsoleID.Intellivision,
				VSystemID.Raw.Jaguar when Emu is VirtualJaguar { IsJaguarCD: true } => ConsoleID.JaguarCD,
				VSystemID.Raw.Jaguar => ConsoleID.Jaguar,
				VSystemID.Raw.Libretro => ConsoleID.UnknownConsoleID,
				VSystemID.Raw.Lynx => ConsoleID.Lynx,
				VSystemID.Raw.MSX => ConsoleID.MSX,
				VSystemID.Raw.N3DS => ConsoleID.Nintendo3DS,
				VSystemID.Raw.N64 => ConsoleID.N64,
				VSystemID.Raw.NDS when Emu is NDS { IsDSi: true } => ConsoleID.DSi,
				VSystemID.Raw.NDS => ConsoleID.DS,
				VSystemID.Raw.NeoGeoCD => ConsoleID.NeoGeoCD,
				VSystemID.Raw.NES => ConsoleID.NES,
				VSystemID.Raw.NGP => ConsoleID.NeoGeoPocket,
				VSystemID.Raw.NULL => ConsoleID.UnknownConsoleID,
				VSystemID.Raw.O2 => ConsoleID.MagnavoxOdyssey,
				VSystemID.Raw.Panasonic3DO => ConsoleID.ThreeDO,
				VSystemID.Raw.PCE => ConsoleID.PCEngine,
				VSystemID.Raw.PCECD => ConsoleID.PCEngineCD,
				VSystemID.Raw.PCFX => ConsoleID.PCFX,
				VSystemID.Raw.PhillipsCDi => ConsoleID.CDi,
				VSystemID.Raw.Playdia => ConsoleID.UnknownConsoleID,
				VSystemID.Raw.PS2 => ConsoleID.PlayStation2,
				VSystemID.Raw.PSP => ConsoleID.PSP,
				VSystemID.Raw.PSX => ConsoleID.PlayStation,
				VSystemID.Raw.SAT => ConsoleID.Saturn,
				VSystemID.Raw.Sega32X => ConsoleID.Sega32X, // not actually used
				VSystemID.Raw.SG => ConsoleID.SG1000,
				VSystemID.Raw.SGB => ConsoleID.GB,
				VSystemID.Raw.SGX => ConsoleID.PCEngine, // ???
				VSystemID.Raw.SGXCD => ConsoleID.PCEngineCD, // ???
				VSystemID.Raw.SMS => ConsoleID.MasterSystem,
				VSystemID.Raw.SNES when Emu is LibsnesCore { IsSGB: true } => ConsoleID.GB,
				VSystemID.Raw.SNES => ConsoleID.SNES,
				VSystemID.Raw.TI83 => ConsoleID.TI83,
				VSystemID.Raw.TIC80 => ConsoleID.Tic80,
				VSystemID.Raw.UZE => ConsoleID.Uzebox,
				VSystemID.Raw.VB => ConsoleID.VirtualBoy,
				VSystemID.Raw.VEC => ConsoleID.Vectrex,
				VSystemID.Raw.Wii => ConsoleID.WII,
				VSystemID.Raw.WSWAN => ConsoleID.WonderSwan,
				VSystemID.Raw.ZXSpectrum => ConsoleID.ZXSpectrum,
				_ => ConsoleID.UnknownConsoleID,
			};
		}
	}
}
