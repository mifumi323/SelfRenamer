using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SelfRenamer
{
	class Program
	{
		static void Main(string[] args) {
			if(args.Length > 0) {
				Console.WriteLine(args[0]);
				File.Delete(args[0]);
				return;
			}
			var assembly = Assembly.GetEntryAssembly();
			var filePath = assembly.Location;
			string newPath = GenerateNewPath(filePath);
			Console.WriteLine(newPath);
			File.Copy(filePath,newPath);
			Process.Start(newPath,filePath);
		}

		private static string GenerateNewPath(string filePath) {
			var dir = Path.GetDirectoryName(filePath);
			var pureName = Path.GetFileNameWithoutExtension(filePath);
			var extension = Path.GetExtension(filePath);
			var match = Regex.Match(pureName,@"^(.*?)(\d+)$");
			var baseName = match.Success ? match.Groups[1].Value : pureName;
			var number = match.Success ? int.Parse(match.Groups[2].Value) : 1;
			while(true) {
				number++;
				var newName = $"{baseName}{ number.ToString()}";
				var newPath = Path.Combine(dir,newName + extension);
				if(!File.Exists(newPath)) {
					return newPath;
				}
			}
		}
	}
}
