using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cfb
{
    public class LanguagesExtensionsDict
    {
        public Dictionary<string, Tuple<List<String>, List<String>>> _languagesExtentions;
        public LanguagesExtensionsDict()
        {
            _languagesExtentions = new Dictionary<string, Tuple<List<string>, List<string>>>()
            {
                { "PYTHON", new Tuple<List<string>, List<string>>(new List<string> { ".py", ".ipynb" }, new List<string> { "__pycache__", "pyc", "pyo", ".env" }) },
                { "CSHARP", new Tuple<List<string>, List<string>>(new List<string> { ".cs" }, new List<string> { "bin", "obj", "*.suo" }) },
                { "JAVA", new Tuple<List<string>, List<string>>(new List<string> { ".java" }, new List<string> { "target", "*.class", ".idea" }) },
                { "JAVASCRIPT", new Tuple<List<string>, List<string>>(new List<string> { ".js" }, new List<string> { "node_modules", "npm-debug.log", "dist" }) },
                { "PHP", new Tuple<List<string>, List<string>>(new List<string> { ".php" }, new List<string> { "vendor", ".env", "composer.lock" }) },
                { "RUBY", new Tuple<List<string>, List<string>>(new List<string> { ".rb" }, new List<string> { "log", "tmp" }) },
                { "GO", new Tuple<List<string>, List<string>>(new List<string> { ".go" }, new List<string> { "bin", "vendor" }) },
                { "SWIFT", new Tuple<List<string>, List<string>>(new List<string> { ".swift" }, new List<string> { "build", "xcodeproj" }) },
                { "KOTLIN", new Tuple<List<string>, List<string>>(new List<string> { ".kt" }, new List<string> { "build", "iml" }) },
                { "HTML", new Tuple<List<string>, List<string>>(new List<string> { ".html", ".htm" }, new List<string> { }) },
                { "CSS", new Tuple<List<string>, List<string>>(new List<string> { ".css" }, new List<string> { }) },
                { "SQL", new Tuple<List<string>, List<string>>(new List<string> { ".sql" }, new List<string> { }) },
                { "TYPESCRIPT", new Tuple<List<string>, List<string>>(new List<string> { ".ts"}, new List<string> { "node_modules"}) },
                { "DART", new Tuple<List<string>, List<string>>(new List<string> { ".dart" }, new List<string> { "build", "dart_tool" }) },
                { "PERL", new Tuple<List<string>, List<string>>(new List<string> { ".pl" }, new List<string> { "pl", "blib" }) },
                { "HASKELL", new Tuple<List<string>, List<string>>(new List<string> { ".hs" }, new List<string> { "hi", "o" }) },
                { "LUA", new Tuple<List<string>, List<string>>(new List<string> { ".lua" }, new List<string> { "luac" }) },
                { "SCALA", new Tuple<List<string>, List<string>>(new List<string> { ".scala" }, new List<string> { "target", "class" }) },
                { "CLOJURE", new Tuple<List<string>, List<string>>(new List<string> { ".clj" }, new List<string> { "target" }) },
                { "COBOL", new Tuple<List<string>, List<string>>(new List<string> { ".cob", ".cbl" }, new List<string> { "gnt" }) },
                { "FORTRAN", new Tuple<List<string>, List<string>>(new List<string> { ".f", ".for" }, new List<string> { "o" }) },
                { "MATLAB", new Tuple<List<string>, List<string>>(new List<string> { ".m" }, new List<string> { "asv", "autosave" }) },
                { "OBJECTIVEC", new Tuple<List<string>, List<string>>(new List<string> { ".m" }, new List<string> { "build", "xcodeproj" }) },
                { "VB.NET", new Tuple<List<string>, List<string>>(new List<string> { ".vb" }, new List<string> { "bin", "obj" }) },
                { "ASSEMBLY", new Tuple<List<string>, List<string>>(new List<string> { ".asm", ".s" }, new List<string> { "o" }) },
                { "C", new Tuple<List<string>, List<string>>(new List<string> { ".c" }, new List<string> { "o" }) },
                { "R", new Tuple<List<string>, List<string>>(new List<string> { ".R", ".r" }, new List<string> { "Rhistory", "RData" }) },
                { "SAS", new Tuple<List<string>, List<string>>(new List<string> { ".sas" }, new List<string> { "log", "lst" }) },
                { "JULIA", new Tuple<List<string>, List<string>>(new List<string> { ".jl" }, new List<string> { "jl", "julia_history" }) },
                { "COFFEESCRIPT", new Tuple<List<string>, List<string>>(new List<string> { ".coffee" }, new List<string> { }) },
                { "REACT", new Tuple<List<string>, List<string>>(new List<string> { ".jsx", ".tsx" }, new List<string> { "node_modules" }) },
                { "ELIXIR", new Tuple<List<string>, List<string>>(new List<string> { ".ex", ".exs" }, new List<string> { "build", "deps" }) },
                { "OCAML", new Tuple<List<string>, List<string>>(new List<string> { ".ml", ".mli" }, new List<string> { "cmo", "cmi" }) },
                { "F#", new Tuple<List<string>, List<string>>(new List<string> { ".fs", ".fsi" }, new List<string> { "bin", "obj" }) }
            };
        }

        public List<String> GetLanguages()
        {
            return _languagesExtentions.Keys.ToList();
        }
    }
}
