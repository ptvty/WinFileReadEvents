# Windows File Read Events

A Windows CLI utility to watch the file system for file *read* events.

It writes to the console the file path along with the offset and IO size in bytes.

## Usage

To watch a specific file:

    WinFileReadEvents.exe C:\test\target.txt

Or without any argument to watch system-wide:

    WinFileReadEvents.exe

Press `ctrl+c` to stop watching.

## Output format

Outputs to the stdout any file read event including the file path, along with the offset and read size in the following format:

    >|<offset>|<io_size>|<file_path>

In case of an error, a line will be written to the stdout in the following format:

    x|<error_message>

This utility uses Event Tracing for Windows (ETW) via [`Microsoft.Diagnostics.Tracing.TraceEvent`](https://www.nuget.org/packages/Microsoft.Diagnostics.Tracing.TraceEvent/) package.
