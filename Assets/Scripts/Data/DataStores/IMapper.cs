using System.Collections.Generic;
using System.Linq;

namespace CgfGames {

	public interface IMapper<From, To> {

		IEnumerable<To> Map (IEnumerable<From> from);

		To Map (From from);
	}

	public abstract class Mapper<From, To> : IMapper<From, To> {

		// public To[] Map (From[] from) {
		// 	return from.Select (x => this.Map (x)).ToArray ();
		// }

		public IEnumerable<To> Map (IEnumerable<From> from) {
			return from.Select (x => this.Map (x));
		}

		public abstract To Map (From from);
	}
}