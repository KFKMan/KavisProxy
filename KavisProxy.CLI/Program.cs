using Hook;
using KavisProxy.CLI;
using Log = KavisProxy.CLI.LogManger;

Console.WriteLine("Hello, World!");

Log.SetFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "proxy.txt"));
Log.Clear();
Log.WriteLine($"KavisProxy Started at {DateTime.Now.ToString()}");

var hook = new HookSystem();