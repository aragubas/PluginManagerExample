using PluginCore;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PluginManagerExample
{
    internal class Program
    {
        static ExecutingContext context = new();

        static void PrintWelcome()
        {
            Console.WriteLine("\nWelcome to PluginManagerExample");
            Console.WriteLine($"Plugins loaded: {context.Plugins.Count}");
        }

        static void CLILoop()
        {
            bool RunLoop = true;
            string LineTitle = "$:";
            int SelectedPluginID = -1;

        PluginList:
            for (int i = 0; i < context.Plugins.Count; i++)
            {
                if (!context.Plugins[i].Metadata.IsCommandProcessor) { break; }
                Console.WriteLine($"ID: {i}\nName: {context.Plugins[i].Metadata.Name}, Version: {context.Plugins[i].Metadata.Version}");
            }

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Choose a command processor from the list above");
            Console.ResetColor();
            Console.Write("\n");

            // Subfunction for printing invalid option without having to copy-paste loads of code
            void InvalidOption() 
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Invalid option");
                Console.ResetColor();
                Console.Write("\n");
            }

            while (RunLoop)
            {
                Console.Write($"{LineTitle} ");
                string input = Console.ReadLine();

                if (input == "exit")
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Goodbye!");
                    Console.ResetColor();
                    Console.Write("\n");
                    return;
                }

                try
                {
                    int pluginID = Convert.ToInt32(input);

                    // Check if pluginID is in range
                    if (pluginID >= context.Plugins.Count || pluginID < 0) 
                    { InvalidOption(); continue; }

                    if (!context.Plugins[pluginID].Metadata.IsCommandProcessor)
                    {
                        Console.WriteLine("This plugin is not a command processor!");
                        InvalidOption();
                        continue;
                    }

                    SelectedPluginID = pluginID;
                    RunLoop = false;
                    break;

                }
                catch (FormatException) { InvalidOption(); }
                catch (OverflowException) { InvalidOption(); }

            }

            if (SelectedPluginID == -1)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("No command processor has been selected!");
                Console.ResetColor();
                Console.Write("\n");

                // Jump to PluginList if no command processor has been selected
                goto PluginList;
            }

            // Calls the run function of the CommandProcessor
            context.Plugins[SelectedPluginID].Run();
        }

        static void InvalidPluginMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid plugin!");
            Console.ResetColor();
            Console.Write("\n");
        }

        static void Main(string[] args)
        {
            // Check if plugin directory exists
            if (!Directory.Exists("Plugins"))
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Error! could not find plugins path.");
                Console.ResetColor();
                Console.Write("\n");
                return;
            }

            List<string> DLLFiles = Directory.GetFiles("Plugins", "*.dll", SearchOption.AllDirectories).ToList<string>();

            DLLFiles.ForEach((file) =>
            {
                Console.Write($"\nTrying to load \"{file}\"... ");

                Assembly assembly = Assembly.LoadFrom(file);

                try
                {
                    Type[] types = assembly.GetTypes();
                    Type validType = null;

                    // Find a type that implements IPlugin interface
                    foreach (Type type in types)
                    {
                        // Check if current type implements IPlugin interface
                        if (typeof(IPlugin).IsAssignableFrom(type))
                        {
                            validType = type;
                            break;
                        }
                    }

                    // If no class implementing IPlugin was found
                    if (validType == null)
                    {
                        InvalidPluginMessage();
                        return;
                    }

                    // Check if type is not an interface (that may happen when trying to load PluginCore.dll)
                    TypeInfo info = validType.GetTypeInfo();
                    if (info.IsInterface)
                    {
                        InvalidPluginMessage();
                        return;
                    }

                    // Create an instance of validType
                    IPlugin plugin = (IPlugin)Activator.CreateInstance(validType);

                    // Check if plugin name doesn't contain space 
                    if (plugin.Metadata.Name.Contains(' '))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Invalid Plugin! Metadata name should not have any space");
                        Console.ResetColor();
                        Console.Write("\n");
                        return;
                    }

                    // Writes "Assembly loaded" to the console before calling Initialize.
                    // If the initialize method writes to the console it will make a mess of text :P
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Assembly loaded.\n");
                    Console.ResetColor();

                    try
                    {
                        // PluginInitialization struct contains status about initialization of
                        // the plugin. 
                        PluginInitialization initialization = plugin.Initialize(ref context);

                        // If plugin reported error when initializating
                        if (!initialization.Successful)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"Plugin initialization failed. {initialization.Message}");
                            Console.ResetColor();
                            Console.Write("\n");
                            return;
                        }

                        context.Plugins.Add(plugin);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"Plugin successfully loaded.");
                        Console.ResetColor();
                        Console.Write("\n");

                    }
                    catch (NotImplementedException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Invalid Plugin! no implementation of 'Initialize' method.");
                        Console.ResetColor();
                        Console.Write("\n");

                        return;
                    }

                }
                catch (ReflectionTypeLoadException exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Error while loading plugin!\n");
                    Console.WriteLine(exception.Message);
                    Console.WriteLine("\nSugestion: This plugin was probably compiled with a different version of PluginAPI and could not be loaded.");
                    Console.ResetColor();
                }


            });

            Console.WriteLine("\nPlugin initialization complete.\n\n");

            // Check if command processor plugins are available
            bool CommandProcessorFound = false;
            foreach(IPlugin plugin in context.Plugins)
            {
                if (plugin.Metadata.IsCommandProcessor) { CommandProcessorFound = true; break; }
            }

            if (!CommandProcessorFound)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("No command processor plugin installed.");
                Console.ResetColor();
                Console.Write("\n");
                return;
            }

            CLILoop();
        }
    }
}