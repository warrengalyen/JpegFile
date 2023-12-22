﻿using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;
using System.Threading.Tasks;

namespace JpegEncode
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new CommandLineBuilder();

            SetupEncodeCommand(builder.Command);

            builder.UseDefaults();

            Parser parser = builder.Build();
            await parser.InvokeAsync(args);
        }

        static void SetupEncodeCommand(Command command)
        {
            command.Description = "Encode JPEG image.";

            command.AddOption(Output());
            command.AddOption(Quality());
            command.AddOption(OptimizeCoding());

            command.AddArgument(new Argument<FileInfo>()
            {
                Name = "source",
                Description = "The file to encode. (BMP, PNG, JPG file)",
                Arity = ArgumentArity.ExactlyOne
            }.ExistingOnly());

            command.Handler = CommandHandler.Create<FileInfo, FileInfo, int, bool>(EncodeAction.Encode);

            static Option Output() =>
                new Option<string>(new[] { "--output", "--out", "-o" }, "Output JPEG file path.")
                {
                    Argument = new Argument<string> { Arity = ArgumentArity.ExactlyOne },
                    Name = "output",
                };

            static Option Quality() =>
                new Option<int>(new[] { "--quality" }, () => 75, "Output JPEG quality.")
                {
                    Argument = new Argument<string> { Arity = ArgumentArity.ExactlyOne },
                    Name = "quality",
                };

            static Option OptimizeCoding() =>
                new Option<bool>(new[] { "--optimize-coding" }, "Output JPEG quality.")
                {
                    Name = "optimizeCoding",
                };
        }
    }
}
