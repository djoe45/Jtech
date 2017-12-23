﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace PluginMerger {

    internal class Program {

        private readonly static Regex isUsingReg;
		private readonly static string pminsert = "//PM.INSERT(";
		private readonly static string pmdebug = "//PM.DEBUGENABLE";
		private readonly static string pmdebugstart = "//PM.DEBUGSTART";
		private readonly static string pmdebugend = "//PM.DEBUGEND";

		static Program() {
            Program.isUsingReg = new Regex("^using\\s+?[^\\(]+\\;$");
        }

        public Program() {}

        private static string GetNameSpace(string line) {
			return line.Trim(' ', '{').Remove(0, "namespace ".Length);
		}
        
        private static bool IsUsingLine(string line) {
            return Program.isUsingReg.IsMatch(line);
        }

		private static int Main(string[] args) {

			if ((int) args.Length != 4) {
				Console.WriteLine("Usage: PluginMerger.exe \"Source\" \"Target\" \"PluginName\" \"VersionNumber\".");
				return 1;
			}

			Console.WriteLine($"[PluginMerger] Merging Plugin - {args[2]} v{args[3]}" );

			string pluginauthor = "UNKNOWN";
			string pluginresourceid = "UNKNOWN";
			string pluginurl = "UNKNOWN";
			string githuburl = "UNKNOWN";

			using (StreamWriter streamWriter = File.CreateText(args[1])) { // write file to target (args[1])

				StringBuilder stringBuilder = new StringBuilder();

				HashSet<string> usings = new HashSet<string>();
				Dictionary<string, List<string>> namespaces = new Dictionary<string, List<string>>();

				bool isDebugEnabled = false;

                var files = Directory.EnumerateFiles(args[0], "*.cs", SearchOption.AllDirectories)
					.Where(s => !s.EndsWith("AssemblyInfo.cs"));

				Console.WriteLine($"[PluginMerger] Found {files.Count()} .cs files in {args[0]}");

				// look for PM.DEBUG flag
				foreach (string curfile in files) {
					foreach (string line in new List<string>(File.ReadAllLines(curfile))) { // for each line in file
						if (line.Contains(pmdebug)) {
							Console.WriteLine($"[PluginMerger] PM.DEBUGENABLED");
							isDebugEnabled = true;
							break;
						}
					}
				}
				
				foreach (string curfile in files) { // for each file in directory

					Console.WriteLine($"[PluginMerger] Parsing {curfile}");
					
					string curnamespace = string.Empty;
					int bracketdepth = 0;
					bool insideDebugBlock = false;

					foreach (string line in new List<string>(File.ReadAllLines(curfile))) { // for each line in file

						int newdepth = bracketdepth + (line.Count(f => f == '{') - line.Count(f => f == '}'));
						
						if (line.Contains(pmdebugstart)) {
							insideDebugBlock = true;
						}

						if (isDebugEnabled || !insideDebugBlock) {
							if (bracketdepth == 0) {
								if (line.TrimStart(' ').StartsWith("namespace ")) { // if namespace

									curnamespace = GetNameSpace(line); // extract the name of the namespace

									if (!namespaces.ContainsKey(curnamespace)) { // if this namespace hasn't already been registered

										namespaces.Add(curnamespace, new List<string>());
										Console.WriteLine($"[PluginMerger] Added Namespace {curnamespace}");
									}

								} else if (IsUsingLine(line)) // if using
									usings.Add(line);

							} else if (newdepth != 0) {

								if (line.Contains(pminsert)) {
									
									string[] insertargs = Regex.Match(line, @"\(([^)]*)\)").Groups[1].Value.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
									string indent = line.Substring(0, line.IndexOf('/'));

									Console.WriteLine($"[PluginMerger] Inserted {insertargs[0]}");

									if (insertargs[0] == "PluginInfo") {
										pluginauthor = insertargs[1];
										pluginurl = insertargs[2];
										pluginresourceid = Regex.Match(pluginurl, "([0-9]{4})").Groups[0].Value;
										githuburl = insertargs[3];
										namespaces[curnamespace].Add($"{indent}[Info(\"{args[2]}\", \"{pluginauthor}\", \"{args[3]}\", ResourceId = {pluginresourceid})]");
									}
								} else {
									namespaces[curnamespace].Add(line); // add line to namespace
								}
							}
						}

						if (line.Contains(pmdebugend)) {
							insideDebugBlock = false;
						}
						
						bracketdepth = newdepth;
					}
					
				}
				

				// create header

				List<string> header = new List<string>() {
					$"{args[2]}.cs generated by PluginMerger v{Assembly.GetExecutingAssembly().GetName().Version.ToString()} - {DateTime.Now.ToString("G")}",
					$"PluginInfo: Title = \"{args[2]}\", Author = \"{pluginauthor}\", Version = \"{args[3]}\", ResourceId = {pluginresourceid}",
					$"OxideMod: {pluginurl}",
					$"GitHub: {githuburl}"
				};

				if (isDebugEnabled)
					header.Add($"Flags: PM.DEBUGENABLE");


				// build the output file

				string longest = header.OrderByDescending(s => s.Length).First();
				string border = $"{new String('/', longest.Length + 5)}";

				// add header
				stringBuilder.AppendLine(border);
				foreach (string hs in header) {
					stringBuilder.AppendLine($"// {hs}");
				}
				stringBuilder.AppendLine(border);
				stringBuilder.AppendLine("\n");

				// add using lines
				stringBuilder.AppendLine(string.Join("\n", usings));

				// add code lines by namespace
				foreach (var ns in namespaces) {
					stringBuilder.AppendLine($"\nnamespace {ns.Key} {"{"}");
					stringBuilder.AppendLine(string.Join("\n", ns.Value));
					stringBuilder.AppendLine("}");
				}

				// write to file
				streamWriter.Write(stringBuilder.ToString()); 
            }

			Console.WriteLine($"[PluginMerger] Saved to {args[1]}");
			Console.WriteLine($"[PluginMerger] Done!");

			return 0;
        }
    }
}