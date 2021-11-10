using System;
using System.Numerics;
using System.Collections.Generic;
using System.IO;

namespace OtusTankClear
{
	public interface ISort
	{
		string GetSourcePath();
		string GetResultPath();
	}

	public class SortableAdapter : ISort
	{
		readonly IUObject Obj;
		public SortableAdapter(IUObject Obj)
		{
			this.Obj = Obj;
		}
		public string GetSourcePath()
        {
			if (Obj.GetProperty("sourcePath") == null) throw new Exception("Error: sourcePath is null");
			return (string)Obj.GetProperty("sourcePath");
		}
		public string GetResultPath()
		{
			if (Obj.GetProperty("resultPath") == null) throw new Exception("Error: resultPath is null");
			return (string)Obj.GetProperty("resultPath");
		}
	}

	public class QSort : ICommand
	{
		readonly ISort sort;
		public QSort(ISort sort)
		{
			this.sort = sort;
		}

		public void Execute()
		{
			List<char> list = new List<char>();
			using (var file = new StreamReader(sort.GetSourcePath(), true))
			{	
				foreach (var i in file.ReadToEnd())
					list.Add(i);
				list.Sort();
			}

			using (var file = new StreamWriter(sort.GetResultPath(), true))
			{
				file.WriteLine("Standart sort");
				foreach (var i in list)
					file.WriteLine(i);
			}
		}
	}
}
