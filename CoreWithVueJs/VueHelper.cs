using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CoreWithVueJs
{
    public static class VueHelper
    {
        // Default port number of 'npm run server
        private static int Port { get; } = 8080;
        private static Uri DevelopmentServerEndpoint { get; } = new Uri($"http://localhost:{Port}");
        private static TimeSpan Timeout { get; } = TimeSpan.FromSeconds(30);
        // Done message of 'npm run serve' command.
        private static string DoneMessage { get; } = "DONE Compiled successfully in";

        public static void UseVueDevelopmentServer(this ISpaBuilder spa)
        {
            spa.UseProxyToSpaDevelopmentServer(async () =>
            {
                var loggerFactory = (ILoggerFactory)spa.ApplicationBuilder.ApplicationServices.GetService(typeof(ILoggerFactory));
                var logger = loggerFactory.CreateLogger("Vue");
                // if 'npm run serve' command was executed yourself, then just return the endpoint.
                if (IsRunning())
                {
                    return DevelopmentServerEndpoint;
                }

                // launch vue.js development server
                var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                var processInfo = new ProcessStartInfo
                {
                    FileName = isWindows ? "cmd" : "npm",
                    Arguments = $"{(isWindows ? "/c npm " : "")}run serve",
                    WorkingDirectory = "client-app",
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                };
                var process = Process.Start(processInfo);
                var tcs = new TaskCompletionSource<int>();

                _ = Task.Run(async () =>
                {
                    try
                    {
                        string line;
                        while ((line = await process.StandardOutput.ReadLineAsync().ConfigureAwait(false)) != null)
                        {
                            logger.LogInformation(line);
                            if (!tcs.Task.IsCompleted && line.Contains(DoneMessage))
                            {
                                tcs.SetResult(1);
                            }
                        }
                    }
                    catch (EndOfStreamException ex)
                    {
                        logger.LogError(ex.ToString());
                        tcs.SetException(new InvalidOperationException("'npm run serve' failed.", ex));
                    }
                });

                _ = Task.Run(async () =>
                {
                    try
                    {
                        string line;
                        while ((line = await process.StandardError.ReadLineAsync().ConfigureAwait(false)) != null)
                        {
                            logger.LogError(line);
                        }
                    }
                    catch (EndOfStreamException ex)
                    {
                        logger.LogError(ex.ToString());
                        tcs.SetException(new InvalidOperationException("'npm run serve' failed.", ex));
                    }
                });

                var timeout = Task.Delay(Timeout);
                if (await Task.WhenAny(timeout, tcs.Task).ConfigureAwait(false) == timeout)
                {
                    throw new TimeoutException();
                }

                return DevelopmentServerEndpoint;
            });
        }

        private static bool IsRunning() => IPGlobalProperties.GetIPGlobalProperties()
            .GetActiveTcpListeners()
            .Select(x => x.Port)
            .Contains(Port);
    }
}
