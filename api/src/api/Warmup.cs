// using Microsoft.Azure.Functions.Worker;
// using Microsoft.Extensions.Logging;

// namespace api;

// public static class Warmup
// {
//     [Function("Warmup")]
//     public static void RunWarmup([WarmupTrigger] object warmupContext, FunctionContext context)
//     {
//         ILogger logger = context.GetLogger("Warmup");

//         logger.LogInformation("Function App instance is now warm!");
//     }
// }
