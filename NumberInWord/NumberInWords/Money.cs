using System;

namespace NumberInWords
{
    /// <summary>
    /// Деньги
    /// </summary>
    public struct Money
    {
        #region Private Methods
        /// <summary>
        /// Внутреннее представление - количество копеек
        /// </summary>
        private long _value;
        #endregion Private Methods

        #region Constructors
        // Конструкторы
        public Money(double value)
        {
            this._value = (long)Math.Round(100 * value, 2);
        }
        public Money(long high, byte low)
        {
            if (low < 0 || low > 99)
                throw new ArgumentException();
            if (high >= 0)
                _value = 100 * high + low;
            else
                _value = 100 * high - low;
        }

        // Вспомогательный конструктор
        private Money(long copecks)
        {
            this._value = copecks;
        }
        #endregion Constructors

        #region Public Properties
        /// <summary>
        /// Количество гривень
        /// </summary>
        public long High
        {
            get
            {
                return _value / 100;
            }
        }
        /// <summary>
        /// Количество копеек
        /// </summary>
        public byte Low
        {
            get
            {
                return (byte)(_value % 100);
            }
        }
        #endregion Public Properties

        #region Public Methods
        /// <summary>
        /// Абсолютная величина
        /// </summary>
        /// <returns></returns>
        public Money Abs()
        {
            return new Money(Math.Abs(_value));
        }
        /// <summary>
        /// Сложение - функциональная форма
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public Money Add(Money r)
        {
            return new Money(_value + r._value);
        }
        /// <summary>
        /// Вычитание - функциональная форма 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public Money Subtract(Money r)
        {
            return new Money(_value - r._value);
        }
        /// <summary>
        /// Умножение - функциональная форма
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Money Multiply(double value)
        {
            return new Money((long)Math.Round(this._value * value, 2));
        }
        /// <summary>
        /// Деление - функциональная форма
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Money Divide(double value)
        {
            return new Money((long)Math.Round(this._value / value, 2));
        }
        /// <summary>
        /// Остаток от деления нацело - функциональная форма
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public long GetRemainder(uint n)
        {
            return _value % n;
        }
        /// <summary>
        /// Сравнение - функциональная форма
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public int CompareTo(Money r)
        {
            if (_value < r._value)
                return -1;
            else if (_value == r._value)
                return 0;
            else
                return 1;
        }
        /// <summary>
        /// Деление на одинаковые части
        /// Количество частей должно быть не меньше 2
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public Money[] Share(uint n)
        {
            if (n < 2)
                throw new ArgumentException();
            Money lowResult = new Money(_value / n);
            Money highResult =
                lowResult._value >= 0 ? new Money(lowResult._value + 1) : new Money(lowResult._value - 1);
            Money[] results = new Money[n];
            long remainder = Math.Abs(_value % n);
            for (long i = 0; i < remainder; i++)
                results[i] = highResult;
            for (long i = remainder; i < n; i++)
                results[i] = lowResult;
            return results;
        }
        /// <summary>
        /// Деление пропорционально коэффициентам
        /// Количество коэффициентов должно быть не меньше 2
        /// </summary>
        /// <param name="ratios"></param>
        /// <returns></returns>
        public Money[] Allocate(params uint[] ratios)
        {
            if (ratios.Length < 2)
                throw new ArgumentException();
            long total = 0;
            for (int i = 0; i < ratios.Length; i++)
                total += ratios[i];
            long remainder = _value;
            Money[] results = new Money[ratios.Length];
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = new Money(_value * ratios[i] / total);
                remainder -= results[i]._value;
            }
            if (remainder > 0)
            {
                for (int i = 0; i < remainder; i++)
                    results[i]._value++;
            }
            else
            {
                for (int i = 0; i > remainder; i--)
                    results[i]._value--;
            }
            return results;
        }
        #endregion Public Methods

        #region Public Overrided Object Methods
        // Перекрытые методы Object
        public override bool Equals(object value)
        {
            try
            {
                return this == (Money)value;
            }
            catch
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
        public override string ToString()
        {
            return ((double)this).ToString();
        }
        // Преобразования в строку аналогично double
        public string ToString(IFormatProvider provider)
        {
            if (provider is IMoneyToStringProvider)
                // здесь - формирование числа прописью
                return ((IMoneyToStringProvider)provider).MoneyToString(this);
            else
                // а здесь - обычный double с учетом стандартного провайдера
                return ((double)this).ToString(provider);
        }
        public string ToString(string format)
        {
            return ((double)this).ToString(format);
        }
        public string ToString(string format, IFormatProvider provider)
        {
            return ((double)this).ToString(format, provider);
        }
        #endregion Public Overrided Object Methods

        #region Operators
        // Унарные операторы
        public static Money operator +(Money r)
        {
            return r;
        }
        public static Money operator -(Money r)
        {
            return new Money(-r._value);
        }
        public static Money operator ++(Money r)
        {
            return new Money(r._value++);
        }
        public static Money operator --(Money r)
        {
            return new Money(r._value--);
        }
        // Бинарные операторы
        public static Money operator +(Money a, Money b)
        {
            return new Money(a._value + b._value);
        }
        public static Money operator -(Money a, Money b)
        {
            return new Money(a._value - b._value);
        }
        public static Money operator *(double a, Money b)
        {
            return new Money((long)Math.Round(a * b._value, 2));
        }
        public static Money operator *(Money a, double b)
        {
            return new Money((long)Math.Round(a._value * b, 2));
        }
        public static Money operator /(Money a, double b)
        {
            return new Money((long)Math.Round(a._value / b, 2));
        }
        public static Money operator %(Money a, uint b)
        {
            return new Money((long)(a._value % b));
        }
        public static bool operator ==(Money a, Money b)
        {
            return a._value == b._value;
        }
        public static bool operator !=(Money a, Money b)
        {
            return a._value != b._value;
        }
        public static bool operator >(Money a, Money b)
        {
            return a._value > b._value;
        }
        public static bool operator <(Money a, Money b)
        {
            return a._value < b._value;
        }
        public static bool operator >=(Money a, Money b)
        {
            return a._value >= b._value;
        }
        public static bool operator <=(Money a, Money b)
        {
            return a._value <= b._value;
        }
        // Операторы преобразования
        public static implicit operator double(Money r)
        {
            return (double)r._value / 100;
        }
        public static explicit operator Money(double d)
        {
            return new Money(d);
        }
        #endregion Operators
    }
}