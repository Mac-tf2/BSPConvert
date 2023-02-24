﻿using BSPConversionLib;
using CommandLine;

namespace BSPConversionCmd
{
	internal class Program
	{
		class Options
		{
			[Option("nopak", Required = false, HelpText = "Export materials into folders instead of embedding them in the BSP.")]
			public bool NoPak { get; set; }

			[Option("subdiv", Required = false, Default = 4, HelpText = "Displacement subdivisions [2-4].")]
			public int DisplacementPower { get; set; }

			[Option("oldbsp", Required = false, HelpText = "Use BSP version 20.")]
			public bool OldBSP { get; set; }

			[Option("prefix", Required = false, HelpText = "Prefix for the converted BSP's file name.")]
			public string Prefix { get; set; }

			[Value(0, MetaName = "input files", Required = true, HelpText = "Input Quake 3 BSP/PK3 file(s) to be converted.")]
			public IEnumerable<string> InputFiles { get; set; }

			[Option('o', "output", Required = false, HelpText = "Output game directory for converted BSP/materials.")]
			public string OutputDirectory { get; set; }
		}

		static void Main(string[] args)
		{
			//args = new string[]
			//{
			//	@"c:\users\tyler\documents\tools\source engine\bspconvert\dfwc2017-6.pk3",
			//	"--output", @"c:\users\tyler\documents\tools\source engine\bspconvert\output",
			//};

			Parser.Default.ParseArguments<Options>(args)
				.WithParsed(options =>
				{
					if (options.DisplacementPower < 2 || options.DisplacementPower > 4)
						throw new ArgumentOutOfRangeException("Displacement power must be between 2 and 4.");

					if (options.OutputDirectory == null)
						options.OutputDirectory = Path.GetDirectoryName(options.InputFiles.First());

					foreach (var inputEntry in options.InputFiles)
					{
						var converterOptions = new BSPConverterOptions()
						{
							noPak = options.NoPak,
							DisplacementPower = options.DisplacementPower,
							oldBSP = options.OldBSP,
							prefix = options.Prefix,
							inputFile = inputEntry,
							outputDir = options.OutputDirectory
						};
						var converter = new BSPConverter(converterOptions, new ConsoleLogger());
						converter.Convert();
					}
				});
		}
	}
}