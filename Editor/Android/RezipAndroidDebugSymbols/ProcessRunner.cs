using System.Diagnostics;
using System.Text;

public static class ProcessRunner
{
    public static ProcessResult RunProcess(string workingDirectory, string fileName, string args)
    {
        UnityEngine.Debug.Log($"Executing {fileName} {args} (Working Directory: {workingDirectory}");
        
        var process = new Process();
        process.StartInfo.FileName               = fileName;
        process.StartInfo.Arguments              = args;
        process.StartInfo.UseShellExecute        = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError  = true;
        process.StartInfo.WorkingDirectory       = workingDirectory;
        process.StartInfo.CreateNoWindow         = true;
        var output = new StringBuilder();

        process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                output.AppendLine(e.Data);
            }
        });

        var error = new StringBuilder();
        process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                error.AppendLine(e.Data);
            }
        });

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        UnityEngine.Debug.Log($"{fileName} exited with {process.ExitCode}");
        
        return new ProcessResult(process.ExitCode, output.ToString(), error.ToString());
    }
}