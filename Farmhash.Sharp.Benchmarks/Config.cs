﻿using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace Farmhash.Sharp.Benchmarks
{
    public class Config : ManualConfig
    {
        public Config()
        {
#if CORE
            // dotnet cli toolchain supports only x64 compilation
            Add(new Job("core-64bit")
            {
                Env = { Runtime = Runtime.Core, Jit = Jit.RyuJit, Platform = Platform.X64 }
            });
#elif MONO
            Add(new Job("mono-32bit")
            {
                Env = { Runtime = Runtime.Mono, Jit = Jit.Llvm, Platform = Platform.X86 }
            });

            Add(new Job("mono-64bit")
            {
                Env = { Runtime = Runtime.Mono, Jit = Jit.Llvm, Platform = Platform.X64 }
            });
#else
            Add(new Job("net-legacy-32bit")
            {
                Env = { Runtime = Runtime.Clr, Jit = Jit.LegacyJit, Platform = Platform.X86 }
            });

            Add(new Job("net-legacy-64bit")
            {
                Env = { Runtime = Runtime.Clr, Jit = Jit.LegacyJit, Platform = Platform.X64 }
            });

            // Ryu is only for 64bit jobs
            Add(new Job("net-ryu-64bit")
            {
                Env = { Runtime = Runtime.Clr, Jit = Jit.RyuJit, Platform = Platform.X64 }
            });
#endif
        }
    }
}
