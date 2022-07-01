using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChatCSR.Tests
{
	public static class Reflection
	{
		public static T GetObjectInFiled<T,G>(G parent, string name, BindingFlags flags)
		{
			var handlerType = typeof(G);
			var fields = handlerType.GetFields(flags);
			var field = fields.First(x => x.Name == name);
			return (T) field.GetValue(parent)!;
		}

		public static T CallPrivateMethod<T>(
			this object instance,
			string methodName,
			BindingFlags flags,
			params object[] parameters
			)
		{
			Type type = instance.GetType();
			MethodInfo method = type.GetMethod(methodName, flags)!;

			return (T) method.Invoke(instance, parameters)!;
		}

		public static void CallPrivateMethodVoid(
			this object instance,
			string methodName,
			BindingFlags flags,
			params object[] parameters
			)
		{
			Type type = instance.GetType();
			MethodInfo method = type.GetMethod(methodName, flags)!;
			method.Invoke(instance, parameters);
		}
	}
}
