An NHibernate profiler. Uses log4net's UDP to transmit log messages to a WPF application. Please contribute and make it better!

## Getting Started

To use the profiler:

1. Clone the repository and build the project. 
2. After building, you need to reference the OpenProfiler.Bootstrapper.dll assembly in your NHibernate project
3. In your startup code, add the following:

    ```csharp
    OpenProfiler.Bootstrapper.Bootstrapper.Initialize();
    ```
4. Start the main application, OpenProfiler.WPF.exe (yes, I know it's a terrible name).
5. You should start to see SQL displayed in a crude application.

## Disclaimer

This profiler has not undergone any sort of performance testing. DO NOT use this in a production scenario.

## Limitations

[log4net](http://logging.apache.org/log4net/) is *required* to use this. Hopefully this dependency will be removed soon.