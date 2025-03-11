using System.Diagnostics;

namespace GitHelper
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Generate a commit message with a timestamp.
                string commitMessage = $"Auto-update {DateTime.Now:yyyy-MM-dd HH:mm:ss}";

                Console.WriteLine("Adding changes...");
                RunCommand("git add .");

                Console.WriteLine("Committing changes...");
                try
                {
                    RunCommand($"git commit -m \"{commitMessage}\"");
                }
                catch (Exception)
                {
                    // If commit fails, it might be due to no changes to commit.
                    Console.WriteLine("No changes to commit or commit failed.");
                    return;
                }

                Console.WriteLine("Pushing changes...");
                RunCommand("git push");
                Console.WriteLine("Update pushed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Runs a command in a shell and returns the output.
        /// Throws an exception if the command fails.
        /// </summary>
        /// <param name="command">The shell command to execute.</param>
        static void RunCommand(string command)
        {
            // On Windows, using "cmd.exe". On Linux/macOS use "bash" instead.
            string shell = Environment.OSVersion.Platform == PlatformID.Win32NT ? "cmd.exe" : "/bin/bash";
            string shellArg = Environment.OSVersion.Platform == PlatformID.Win32NT ? $"/c {command}" : $"-c \"{command}\"";

            var processInfo = new ProcessStartInfo(shell, shellArg)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process();
            process.StartInfo = processInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string errorOutput = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Command '{command}' failed with error: {errorOutput}");
            }

            Console.WriteLine(output);
        }
    }
}