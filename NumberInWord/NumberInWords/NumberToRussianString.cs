namespace NumberInWords
{
    /// <summary>
    /// Преобразование числа в строку = число прописью
    /// </summary>
    public class NumberToRussianString
    {

        #region Public Enums
        /// <summary>
        /// Род единицы измерения
        /// </summary>
        public enum WordGender
        {
            /// <summary>
            /// мужской
            /// </summary>
            Masculine,
            /// <summary>
            /// женский
            /// </summary>
            Feminine,
            /// <summary>
            /// средний
            /// </summary>
            Neuter
        };
        /// <summary>
        /// Варианты написания единицы измерения 
        /// </summary>
        public enum WordMode
        {
            /// <summary>
            /// рубль
            /// </summary>
            Mode1,
            /// <summary>
            /// рубля
            /// </summary>
            Mode2_4,
            /// <summary>
            /// рублей
            /// </summary>
            Mode0_5
        };
        #endregion Public Enums

        #region Private Declarations Здесь можно поменять названия чисел
        // Строковые представления чисел
        private const string number0 = "нуль";
        private static readonly string[,] number1_2 =
        {
            { "один", "два" },
            { "одна", "дві" },
            { "однє", "два" }
        };
        private static readonly string[] number3_9 =
            { "три", "чотири", "п'ять", "шість", "сімь", "вісімь", "дев'ять" };
        private static readonly string[] number10_19 =
        {
            "десять", "одинадцять", "дванадцять", "тринадцять", "чотирнадцять", "п'ятнадцять",
            "шістнадцять", "сімнадцять", "вісімнадцять", "дев'ятнадцять" };
        private static readonly string[] number20_90 =
            { "двадцять", "тридцять", "сорок", "п'ятдесят", "шістдесят", "сімдесят", "вісімдесят", "дев'яносто" };
        private static readonly string[] number100_900 =
            { "сто", "двісті", "триста", "чотириста", "п'ятсот", "шістсот", "сімсот", "вісімсот", "дев'ятсот" };
        private static readonly string[,] ternaries =
        {
            { "тисяча", "тисячи", "тисяч" },
            { "мільон", "мільона", "мільонів" },
            { "мільярд", "мільярда", "мільярдів" },
            { "трилліон", "трилліонa", "трилліонів" },
            { "білліон", "білліона", "білліонів" }
        };
        private static readonly WordGender[] TernaryGenders =
        {
            WordGender.Feminine,	// тысяча - женский
            WordGender.Masculine, // миллион - мужской
            WordGender.Masculine, // миллиард - мужской
            WordGender.Masculine, // триллион - мужской
            WordGender.Masculine  // биллион - мужской
        };
        #endregion Private Declarations Здесь можно поменять названия чисел

        #region Private Methods
        /// <summary>
        /// Функция преобразования 3-значного числа, заданного в виде строки,
        /// с учетом рода (мужской, женский или средний).
        /// Род учитывается для корректного формирования концовки:
        /// "один" (рубль) или "одна" (тысяча)
        /// </summary>
        /// <param name="ternary"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        private static string TernaryToString(long ternary, WordGender gender)
        {
            string s = "";
            // учитываются только последние 3 разряда, т.е. 0..999	
            long t = ternary % 1000;
            int digit2 = (int)(t / 100);
            int digit1 = (int)((t % 100) / 10);
            int digit0 = (int)(t % 10);
            // сотни
            if (digit2 > 0)
                s = number100_900[digit2 - 1] + " ";
            if (digit1 > 1)
            {
                s += number20_90[digit1 - 2] + " ";
                if (digit0 >= 3)
                    s += number3_9[digit0 - 3] + " ";
                else if (digit0 > 0)
                    //s += number1_2[digit0 - 1, (int) gender] + " ";
                    s += number1_2[(int)gender, digit0 - 1] + " ";
            }
            else if (digit1 == 1)
                s += number10_19[digit0] + " ";
            else
            {
                if (digit0 >= 3)
                    s += number3_9[digit0 - 3] + " ";
                else if (digit0 > 0)
                    //s += number1_2[digit0 - 1, (int) gender] + " ";
                    s += number1_2[(int)gender, digit0 - 1] + " ";

                //!!! Чтобы писАл "нуль" раскоментировать следующие 2 строки :
                /*else 
					s += number0 + " ";*/
            }
            return s.TrimEnd();
        }
        //
        private static string TernaryToString(long value, byte ternaryIndex)
        {
            long ternary = value;
            for (byte i = 0; i < ternaryIndex; i++)
                ternary /= 1000;
            if (ternary == 0)
                return "";
            else
            {
                ternaryIndex--;
                return TernaryToString(ternary, TernaryGenders[ternaryIndex]) + " " +
                       ternaries[ternaryIndex, (int)GetWordMode(ternary)] + " ";
            }
        }
        #endregion Private Methods

        #region Public Methods
        /// <summary>
        ///Функция возвращает число прописью с учетом рода единицы измерения 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        public static string NumberToString(long value, WordGender gender)
        {
            if (value <= 0)
                return "";
            else
                return TernaryToString(value, 5) +
                       TernaryToString(value, 4) +
                       TernaryToString(value, 3) +
                       TernaryToString(value, 2) +
                       TernaryToString(value, 1) +
                       TernaryToString(value, gender);
        }

        /// <summary>
        /// Определение варианта написания единицы измерения по 3-х значному числу 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static WordMode GetWordMode(long number)
        {
            // достаточно проверять только последние 2 цифры,
            // т.к. разные падежи единицы измерения раскладываются
            // 0 рублей, 1 рубль, 2-4 рубля, 5-20 рублей, 
            // дальше - аналогично первому десятку		
            int digit1 = (int)(number % 100) / 10;
            int digit0 = (int)(number % 10);
            if (digit1 == 1)
                return WordMode.Mode0_5;
            else if (digit0 == 1)
                return WordMode.Mode1;
            else if (2 <= digit0 && digit0 <= 4)
                return WordMode.Mode2_4;
            else
                return WordMode.Mode0_5;
        }
        #endregion Public Methods
    }
}