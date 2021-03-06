﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using System.Security.Cryptography;
using System.Text;

#if !CORE
using System.Data.HashFunction;
using xxHashSharp;
#endif

namespace Farmhash.Sharp.Benchmarks
{
    public class Config32 : ManualConfig
    {
        public Config32()
        {
            Add(new BaseConfig());
            Add(new TagColumn("Kind", _ => "32bit hash"));
        }
    }

    public class HashBenchmark32
    {
        private byte[] data;
        private string dataStr;
        private static readonly MD5 md5 = MD5.Create();

        [GlobalSetup]
        public void SetupData()
        {
            dataStr = new string('.', PayloadLength);
            data = Encoding.ASCII.GetBytes(dataStr);
        }


        [Params(4, 11, 25, 100, 1000, 10000)]
        public int PayloadLength { get; set; }

        [Benchmark]
        public uint FarmHash() => Farmhash.Hash32(data, data.Length);

        [Benchmark]
        public byte[] Md5() => md5.ComputeHash(data);

        [Benchmark]
        public int StringHashCode() => dataStr.GetHashCode();

        [Benchmark]
        public uint SparrowXXHash() => SparrowHashing.XXHash32.Calculate(data, data.Length);

#if !CORE
        private static readonly System.Data.HashFunction.CityHash hcity = new System.Data.HashFunction.CityHash(32);
        private static readonly SpookyHashV2 Spooky = new SpookyHashV2(32);

        [Benchmark]
        public uint XXHash() => xxHash.CalculateHash(data);

        [Benchmark]
        public uint CityHashNet() => CityHash.CityHash.CityHash32(dataStr);

        [Benchmark]
        public byte[] HFCityHash() => hcity.ComputeHash(data);

        [Benchmark]
        public byte[] SpookyHash() => Spooky.ComputeHash(data);


        [Benchmark]
        public unsafe uint Spookily()
        {
            fixed (byte* buffer = data)
            {
                return SpookilySharp.SpookyHash.Hash32(buffer, data.Length, 0);
            }
        }
#endif
    }
}
