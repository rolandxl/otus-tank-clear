using System;
using System.Numerics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace OtusTankClear
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Старт программы!");
		}
	}


	public interface IUObject
	{
		object GetProperty(string key);
		void SetProperty(string key, object value);
	}


	//реализуем интерфейс в класс 
	public class Tank : IUObject
	{
		public Tank()
		{
			Propertys = new();
		}

		public object GetProperty(string key)
		{
			Propertys.TryGetValue(key, out object value);
			return value;
		}
		public void SetProperty(string key, object value)
		{
			if (Propertys.ContainsKey(key))
				Propertys[key] = value;
			else Propertys.TryAdd(key, value);
		}
		Dictionary<string, object> Propertys;
	}


	//создаем интерфейс команды для последующей реалиции его через конкретные механики 
	public interface ICommand
	{
		void Execute();
	}

	public class MacroCommand : ICommand
	{
		readonly ICommand[] commands;
		public MacroCommand(ICommand[] commands)
		{
			this.commands = commands;
		}

		public void Execute()
		{
			foreach (var i in commands)
				try
				{
					i.Execute();
				}
				catch (Exception e)
				{
					throw new Exception ($"One of command is error: {e.Message}");
				}
		}
	}
}

