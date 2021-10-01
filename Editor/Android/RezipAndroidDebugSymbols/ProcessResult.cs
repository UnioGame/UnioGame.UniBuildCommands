public class ProcessResult
{
    public int    ExitCode { get; }
    public string StdOut   { get; }
    public string StdErr   { get; }

    internal bool Failure => ExitCode != 0;

    internal ProcessResult(int exitCode, string stdOut, string stdErr)
    {
        ExitCode = exitCode;
        StdOut   = stdOut;
        StdErr   = stdErr;
    }

    public override string ToString()
    {
        return $"Exit Code: {ExitCode}\nStdOut:\n{StdOut}\nStdErr:\n{StdErr}";
    }
}