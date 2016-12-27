namespace CgfGames {

	public enum Difficulty {Easy, Medium, Hard}

	static class DifficultyMethods {

		public static Difficulty Next (this Difficulty d) {
			int dValue = (int)d;
			if (dValue < (int)Difficulty.Hard) {
				return (Difficulty)(dValue + 1);
			} else {
				return Difficulty.Hard;
			}
		}

		public static Difficulty Previous (this Difficulty d) {
			int dValue = (int)d;
			if (dValue > (int)Difficulty.Easy) {
				return (Difficulty)(dValue - 1);
			} else {
				return Difficulty.Easy;
			}
		}
	}
}
