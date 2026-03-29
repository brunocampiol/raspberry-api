using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Running;
using RaspberryPi.Benchmarks;

var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
var config = ManualConfig.Create(DefaultConfig.Instance)
    .WithOptions(ConfigOptions.JoinSummary)
    .WithArtifactsPath($@"{Directory.GetCurrentDirectory()}\results-{timestamp}")
    .AddExporter(MarkdownExporter.GitHub);

BenchmarkRunner.Run<AutoMapperBenchmarks>(config);