using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SkyLar
{
	/// <summary>
	/// Representa a classe compartilhada de instâncias de objetos globais.
	/// </summary>
	public static class Singleton<T>
		where T : class
	{
		/// <summary>
		/// Cache global de objetos guardados, mapeados pelo tipo e a instância do tipo.
		/// </summary>
		private static readonly Dictionary<Type, object> Pool
			= new Dictionary<Type, object>();

		/// <summary>
		/// Obtém a instância do tipo determinado do cache.
		/// </summary>
		/// <returns></returns>
		public static T Get() =>
			Instance;

		/// <summary>
		/// Determina se o determinado tipo tem uma instância já registrada.
		/// </summary>
		public static bool HasValue
		{
			get
			{
				bool state;

				lock (Pool)
					state = Pool.ContainsKey(typeof(T));

				return state;
			}
		}

		/// <summary>
		/// Obtém ou define a instância do tipo determinado do cache. Defina como nula para remover do cache.
		/// </summary>
		public static T Instance
		{
			get
			{
				lock (Pool)
				{
					if (Pool.TryGetValue(typeof(T), out var value))
						return (T)value;
					return default;
				}
			}
			set
			{
				lock (Pool)
				{
					if (!(value is null))
						Pool[typeof(T)] = value;
					else
					{
						if (Pool.ContainsKey(typeof(T)))
							Pool.Remove(typeof(T));
					}
				}
			}
		}
	}
}
