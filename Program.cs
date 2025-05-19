using System;
using CommandLine;

class ListFilesApp {
    // adds support for cli (https://github.com/commandlineparser/commandline/wiki)
    public class Options {
        [Option("filename", Required = false, HelpText = "Input filename.")]
        public string? Filename { get; set; }
    }

    // returns a concated list of all files and directories in a root folder.
    string[] GetAllFiles(string filename) {
        string[] files = Directory.GetFiles(filename, "*", SearchOption.TopDirectoryOnly);
        string[] directories = Directory.GetDirectories(filename, "*", SearchOption.TopDirectoryOnly);
        return files.Concat(directories).ToArray();
    }

    void WriteFilesToDateNamedFile(string[] files) {
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string dateFileName = $"file_list_{timestamp}.txt";
        File.WriteAllLines(dateFileName, files);
        Console.WriteLine($"File list written to: {dateFileName}");
    }

    static int Main(string[] args) {
        var app = new ListFilesApp();
        
        // parsing cli args.
        Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o => {
            // if filename arg is passed via CLI
            if (o.Filename != null) {
                if(!Directory.Exists(o.Filename)) {
                    Console.WriteLine("Please input a valid, existing directory.");
                    throw new FileNotFoundException();
                }
                string[] allFiles = app.GetAllFiles(o.Filename);
                foreach(string files in allFiles) {
                    Console.WriteLine(files);
                }
                app.WriteFilesToDateNamedFile(allFiles);
            }
            // else run like console app
            else {
                Console.WriteLine("Input a valid, existing directory: ");
                string? filename = Console.ReadLine();

                if(!Directory.Exists(filename)) {
                    Console.WriteLine("Please input a valid, existing directory.");
                    throw new FileNotFoundException();
                }

                string[] allFiles = app.GetAllFiles(filename);
                foreach(string files in allFiles) {
                    Console.WriteLine(files);
                }
                app.WriteFilesToDateNamedFile(allFiles);
            }
        });
        return 0;
    }
}
