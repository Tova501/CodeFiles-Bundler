using cfb;
using System.Collections.Immutable;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Linq;

var rootCommand = new RootCommand();

//Set the options
var outputOption = new Option<FileInfo?>("--output", 
            "Path to your bundled destination file");
outputOption.AddAlias("-o");

var noteOption = new Option<bool>("--note", 
            "Add a comment with the source of the code");
noteOption.AddAlias("-n");
var sortOption = new Option<string?>("--sort",
            "Sort the files according to fileName or fileType").FromAmong("name", "type");
sortOption.AddAlias("-s");

var relOption = new Option<bool>("--remove-empty-lines");
relOption.AddAlias("-r");

var authorOption = new Option<string?>("--author", 
            "Add the provided author to the head of the file");
authorOption.AddAlias("-a");

var languageOption = new Option<List<string>>("--language",
            "Software languages to include in your bundled file (e.g., 'PYTHON, C' or 'ALL')")
{ IsRequired = true, 
  AllowMultipleArgumentsPerToken = true
};
languageOption.AddAlias("-l");

//Set the command
var bundleCommand = new Command("bundle", "Bundle code files into one file");
bundleCommand.AddOption(languageOption);
bundleCommand.Add(noteOption);
bundleCommand.AddOption(sortOption);
bundleCommand.AddOption(outputOption);
bundleCommand.AddOption(authorOption);
bundleCommand.AddOption(relOption);

//Set the handler
bundleCommand.SetHandler((languages, sort, output, removeEmptyLines, note, author) =>
{
    try
    {
        string outputPath = output?.FullName ?? Path.Combine(Environment.CurrentDirectory, "files.txt");
        var files = FilterFiles(Directory.GetCurrentDirectory(), languages);
        CreateBundledFile(files, outputPath, note, sort, removeEmptyLines, author);
    }
    catch (IOException ex) {
        Console.WriteLine(ex.Message);
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}, languageOption, sortOption, outputOption, relOption, noteOption, authorOption);

var createRspBundle = new Command("create-rsp", "Create response file to bundle command");

//Option: Create default response file
var defaultOption = new Option<bool>("--default", "Create a defaultive response file");
defaultOption.AddAlias("-d");
createRspBundle.AddOption(defaultOption);

createRspBundle.SetHandler((d) =>
{
    string commandText;
    if (d)
    {
        commandText = "-l all";
    }
    else {
        commandText = InputOptions();
    }
    try
    {
        using (StreamWriter sw = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "rsp")))
        {
            sw.WriteLine(commandText);
        }
        Console.WriteLine("Operation was successful. \nTo bundle the file write: cfb bundle @rsp");
    }
    catch (IOException exp)
    {
        Console.WriteLine(exp.Message);
    }
}, defaultOption);


//Add subCommands to rootCommand
rootCommand.AddCommand(bundleCommand);
rootCommand.AddCommand(createRspBundle);

await rootCommand.InvokeAsync(args);

List<string> FilterFiles(string path, List<string> languages)
{
    var langExtensions = new LanguagesExtensionsDict();
    var langDict = langExtensions._languagesExtentions;
    var supportedLangs = langExtensions.GetLanguages();
    foreach (var lang in languages)
    {
        if (!supportedLangs.Contains(lang.ToUpper()))
        {
            throw new Exception($"ERROR: This language is not supported in the bundle command: {lang}");
        }
    }
    List<string> res = new List<string>();

    var directories = Directory.GetDirectories(path);
    var files = Directory.GetFiles(path);

    foreach (var dir in directories)
    {
        Console.WriteLine(path);
        if (!languages.Any(lang =>
        {
            var (Extensions, ignores) = langDict[lang.ToUpper()];
            return ignores.Any(ignore => dir.EndsWith(ignore));
        }))
        {
            res.AddRange(FilterFiles(dir, languages));
        }
    }
    if (languages.Contains("ALL", StringComparer.OrdinalIgnoreCase))
    {
        res.AddRange(files);
        return res;
    }
    foreach (var file in files)
    {
        if (languages.Any(lang =>
        {
            var (extensions, ignores) = langDict[lang.ToUpper()];
            return extensions.Contains(Path.GetExtension(file));
        }))
            res.Add(file);
    }
    return res;
}

static void CreateBundledFile(List<string> sourceFiles, string output, bool note, string? sort, bool removeEmptyLines, string? author)
{
    if (!string.IsNullOrEmpty(sort) && sort.Equals("type", StringComparison.OrdinalIgnoreCase))
    {
        sourceFiles = sourceFiles.OrderBy(f => Path.GetExtension(f)).ToList();
    }

    try
    {
        using (StreamWriter writer = new StreamWriter(output))
        {
            if (!string.IsNullOrEmpty(author))
            {
                writer.WriteLine($"// Author: {author}");
            }

            foreach (var file in sourceFiles)
            {
                if (note)
                {
                    writer.WriteLine($"// Source: {file}");
                }

                var lines = File.ReadAllLines(file);
                foreach (var line in lines)
                {
                    if (removeEmptyLines && string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    writer.WriteLine(line);
                }
            }
        }

        Console.WriteLine($"Bundle created successfully: {output}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error saving bundle: {ex.Message}");
    }
}

string InputOptions()
{
    Console.WriteLine("Enter the software languages to include in your bundled file (separated by spaces)");
    var languagesInput = Console.ReadLine();
    var languages = languagesInput?.Split(' ') ?? [];

    Console.WriteLine("Enter the output file path (if you press enter the file will be generated in the current directory):");
    var outputPath = Console.ReadLine();

    Console.WriteLine("Include source file path as comment? (yes/no):");
    var noteInput = Console.ReadLine();
    var note = noteInput?.ToLower() == "yes";

    Console.WriteLine("Remove empty lines? (yes/no):");
    var removeEmptyLinesInput = Console.ReadLine();
    var removeEmptyLines = removeEmptyLinesInput?.ToLower() == "yes";

    Console.WriteLine("Enter the sort order ('name' or 'type'):");
    var sort = Console.ReadLine();

    Console.WriteLine("Add the provided author to the head of the file? (yes/no):");
    string addAuthor = Console.ReadLine();
    string author = "";
    if (addAuthor.ToLower().Equals("yes"))
    {
        Console.WriteLine("Enter the author name:");
        author = Console.ReadLine();
    }

    string commandText = $" -l {string.Join(" ", languages)} " +
                      $" {(!string.IsNullOrEmpty(outputPath) ? $"-o {outputPath} " : "")}" +
                      $"{(note ? "-n " : "")}" +
                      $"{(!string.IsNullOrEmpty(sort) ? $"-s {sort} " : "")}" +
                      $"{(removeEmptyLines ? "-r " : "")}" +
                      $"{(addAuthor.ToLower().Equals("yes") ? $"-a {author}" : "")}";
    return commandText;
}
