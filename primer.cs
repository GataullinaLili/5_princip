namespace Func;


public class Test
{
    
}


/*
==========================================
Класс демонстрирует ПРИЁМ ФУНКЦИЙ КАК АРГУМЕНТОВ
Суть: Функции высшего порядка могут принимать другие функции в качестве параметров.
Это позволяет создавать гибкие и переиспользуемые алгоритмы, где часть логики
инкапсулирована в передаваемой функции.
*/
public static class FunctionAsArgumentExample
{
    /*
    Метод ApplyOperation - функция высшего порядка.
    Принимает:
    - a, b: целочисленные аргументы для операции
    - operation: функция (делегат), которая будет применена к a и b
    
    Func<int, int, int> означает:
    - первые два int - типы входных параметров
    - последний int - тип возвращаемого значения
    */
    private static void ApplyOperation(int a, int b, Func<int, int, int> operation)
    {
        // Вызываем переданную функцию с параметрами a и b
        var result = operation(a, b);
        Console.WriteLine($"Результат операции: {result}");
    }

    // Стандартная функция сложения двух чисел
    private static int Add(int x, int y) => x + y;
    
    // Стандартная функция умножения двух чисел
    private static int Multiply(int x, int y) => x * y;

    public static void Demo()
    {
        Console.WriteLine("\n=== Приём функций как аргументов ===");
        
        // Передаём именованную функцию Add как аргумент
        ApplyOperation(5, 3, Add); // Сложение: 5 + 3 = 8
        
        // Передаём именованную функцию Multiply как аргумент
        ApplyOperation(5, 3, Multiply); // Умножение: 5 * 3 = 15
        
        // Используем лямбда-выражение для определения операции "на лету"
        ApplyOperation(5, 3, (x, y) => x - y); // Вычитание: 5 - 3 = 2
        
        // Ещё один пример с лямбдой - возведение в степень
        ApplyOperation(2, 10, (x, y) => (int)Math.Pow(x, y)); // 2^10 = 1024
    }
}

/*
==========================================
Класс демонстрирует ВОЗВРАЩЕНИЕ ФУНКЦИЙ ИЗ ФУНКЦИЙ
Суть: Функции могут не только принимать другие функции, но и возвращать их.
Это позволяет создавать "фабрики функций" - функции, которые генерируют другие функции
с определённым поведением.
*/
public static class FunctionAsReturnValueExample
{
    /*
    Метод CreateMultiplier создаёт новую функцию-умножитель.
    Принимает:
    - factor: множитель, который будет "запечён" в возвращаемой функции
    
    Возвращает:
    - Функцию, которая принимает int и возвращает int (Func<int, int>)
    */
    private static Func<int, int> CreateMultiplier(int factor)
    {
        // Возвращаем лямбда-функцию, которая запоминает factor
        // через механизм замыканий (closure)
        return x => x * factor;
    }

    public static void Demo()
    {
        Console.WriteLine("\n=== Возвращение функций из функций ===");
        
        // Создаём специализированные функции-умножители
        var doubleIt = CreateMultiplier(2); // Функция удвоения
        var tripleIt = CreateMultiplier(3); // Функция утроения
        var multiplyBy10 = CreateMultiplier(10); // Функция умножения на 10
        
        // Используем созданные функции
        Console.WriteLine($"Удвоение 5: {doubleIt(5)}"); // 10
        Console.WriteLine($"Утроение 5: {tripleIt(5)}"); // 15
        Console.WriteLine($"5 * 10: {multiplyBy10(5)}"); // 50
        
        // Можно использовать сразу без сохранения в переменную
        Console.WriteLine($"7 * 100: {CreateMultiplier(100)(7)}"); // 700
    }
}

/*
==========================================
Класс демонстрирует ВСТРОЕННЫЕ ФУНКЦИИ ВЫСШЕГО ПОРЯДКА В LINQ
Суть: В C# многие методы LINQ являются функциями высшего порядка.
Они принимают другие функции (предикаты, селекторы, аккумуляторы)
для реализации сложного поведения.
*/
public static class LinqHigherOrderFunctionsExample
{
    public static void Demo()
    {
        Console.WriteLine("\n=== Встроенные функции высшего порядка в LINQ ===");
        
        int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        
        // 1. Where - фильтрация (принимает предикат - функцию, возвращающую bool)
        var evens = numbers.Where(number => number % 2 == 0);
        Console.WriteLine("Чётные числа: " + string.Join(", ", evens));
        
        // 2. Select - преобразование (принимает функцию преобразования)
        var squares = numbers.Select(n => n * n);
        Console.WriteLine("Квадраты чисел: " + string.Join(", ", squares));
        
        // 3. Aggregate - агрегация (принимает начальное значение и функцию аккумуляции)
        var sum = numbers.Aggregate(0, (acc, n) => acc + n);
        Console.WriteLine("Сумма чисел: " + sum);
        
        // 4. OrderBy - сортировка (принимает функцию ключа сортировки)
        var sortedDesc = numbers.OrderBy(n => -n);
        Console.WriteLine("Числа по убыванию: " + string.Join(", ", sortedDesc));
    }
}

/*
==========================================
Класс демонстрирует КОМПОЗИЦИЮ ФУНКЦИЙ
Суть: Композиция функций - создание новой функции путём объединения существующих,
где результат одной функции передаётся в следующую.
Это позволяет строить сложные преобразования из простых компонентов.
*/
public static class FunctionCompositionExample
{
    /*
    Метод Compose создаёт композицию двух функций.
    Принимает:
    - func1: функция для второго шага преобразования
    - func2: функция для первого шага преобразования
    
    Возвращает:
    - Новую функцию, которая применяет func2, затем func1 к результату
    
    Общий вид: f ∘ g = f(g(x))
    */
    private static Func<T, TReturn2> Compose<T, TReturn1, TReturn2>(
        Func<TReturn1, TReturn2> func1, 
        Func<T, TReturn1> func2)
    {
        return x => func1(func2(x));
    }

    public static void Demo()
    {
        Console.WriteLine("\n=== Композиция функций ===");
        
        // Создаём простые функции-компоненты
        Func<int, int> square = x => x * x;
        Func<int, string> toString = x => $"Результат: {x}";
        Func<string, string> addExclamation = s => s + "!";
        
        // Композиция 1: сначала square, затем toString
        var squareAndStringify = Compose(toString, square);
        Console.WriteLine(squareAndStringify(5)); // "Результат: 25"
        
        // Композиция 2: цепочка из трёх функций
        var processNumber = Compose(addExclamation, Compose(toString, square));
        Console.WriteLine(processNumber(3)); // "Результат: 9!"
        
        // Альтернативный вариант композиции через лямбды
        Func<int, string> alternative = x => addExclamation(toString(square(x)));
        Console.WriteLine(alternative(4)); // "Результат: 16!"
    }
}

/*
==========================================
Класс демонстрирует КАРРИРОВАНИЕ
Суть: Каррирование - преобразование функции от многих аргументов
в последовательность функций одного аргумента.
Позволяет делать частичное применение функций.
*/
public static class CurryingExample
{
    // Обычная функция сложения двух чисел
    public static int Add(int x, int y) => x + y;
    
    // Каррированная версия функции сложения
    // Это значит: "функция, которая принимает int и возвращает функцию (int -> int)"
    private static Func<int, Func<int, int>> CurriedAdd = x => y => x + y;
    
    // Альтернативный способ записи каррированной функции
    // Альтернативный способ записи каррированной функции
    private static Func<int, int> CurriedAddMethod(int x)
    {
        return y => x + y;
    }

    public static void Demo()
    {
        Console.WriteLine("\n=== Каррирование ===");
        
        // Использование каррированной функции
        var add5 = CurriedAdd(5); // Фиксируем первый аргумент = 5
        Console.WriteLine(add5(3)); // 8 (5 + 3)
        Console.WriteLine(add5(10)); // 15 (5 + 10)
        
        // Прямой вызов каррированной функции
        Console.WriteLine(CurriedAdd(5)(3)); // 8
        
        // Частичное применение с альтернативной версией
        var add100 = CurriedAddMethod(100);
        Console.WriteLine(add100(50)); // 150
        
        // Пример с тремя аргументами
        Func<int, Func<int, Func<int, int>>> curriedAdd3 = 
            x => y => z => x + y + z;
            
        var add10 = curriedAdd3(10); // Фиксируем первый аргумент
        var add10And20 = add10(20); // Фиксируем второй аргумент
        Console.WriteLine(add10And20(30)); // 60 (10 + 20 + 30)
    }
}

class Program
{
    static void Main()
    {
        FunctionAsArgumentExample.Demo();
        FunctionAsReturnValueExample.Demo();
        LinqHigherOrderFunctionsExample.Demo();
        FunctionCompositionExample.Demo();
        CurryingExample.Demo();
    }
}
