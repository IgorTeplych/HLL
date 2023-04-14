using HLL;

public static class Program
{
    public static void Main()
    {
        
    }


    static void TestUniqueItems(int sizeReg)
    {
        Console.WriteLine($"Размер регистра {sizeReg}");
        Console.WriteLine($"____________________________________________________________");
        Console.WriteLine($"|Число уникальных элементов |Оценка HLL         |Ошибка, % |");
        Console.WriteLine($"|__________________________________________________________|");

        for (int uniqueNum = 1; uniqueNum < 1_000_000_000; uniqueNum *= 10)
        {
            int[] ints = GetUniqueItems(uniqueNum).ToArray();
            HyperLogLog hyperLogLog = new HyperLogLog(sizeReg);

            foreach (var item in ints)
                hyperLogLog.Add(item);

            var est = hyperLogLog.EstimateCount(); 

            var err = (Math.Abs(est - uniqueNum) / uniqueNum) * 100;

            Console.WriteLine("|{0,27}|{1,19:0.0000}|{2,10:0.0000}|", uniqueNum, est, err);
        }
        Console.WriteLine($"|__________________________________________________________|");
    }
    static double TestUnique27Words(int regNum)
    {
        string path = Environment.CurrentDirectory + @"\" + "Unique size 27 Full size 1080000.txt";
        var words = File.ReadAllText(path).Trim().Split(' ');

        HyperLogLog hyperLogLog = new HyperLogLog(regNum);
        foreach (var item in words)
        {
            char[] chars = item.ToCharArray();
            hyperLogLog.Add(MurMurHash2.GetHash(chars));
        }
        var count = hyperLogLog.EstimateCount();
        return count;
    }
    static double TestUnique27WordsWithOnceUnique(int regNum)
    {
        string path = Environment.CurrentDirectory + @"\" + "Unique size 27 with one once Full size 1080000.txt";
        var words = File.ReadAllText(path).Trim().Split(' ');

        HyperLogLog hyperLogLog = new HyperLogLog(regNum);
        foreach (var item in words)
        {
            char[] chars = item.ToCharArray();
            hyperLogLog.Add(MurMurHash2.GetHash(chars));
        }
        var count = hyperLogLog.EstimateCount();
        return count;
    }
    static double TestWarAndPeace(int regNum)
    {
        string path = Environment.CurrentDirectory + @"\" + "А Толстой - Война и мир.txt";
        var words = File.ReadAllText(path).Trim().Split(' ');

        HyperLogLog hyperLogLog = new HyperLogLog(regNum);
        foreach (var item in words)
        {
            char[] chars = item.ToCharArray();
            hyperLogLog.Add(MurMurHash2.GetHash(chars));
        }
        var count = hyperLogLog.EstimateCount();
        return count;
    }
    static void CreateWords27UniqueText()
    {
        string[] trees = new string[]
        {
             "Oak", //1
             "Birch", //2
             "Ash", //3
             "Spruce", //4
             "Stump",//5
             "Maple", //6
             "Larch", //7
        };
        string[] writer = new string[]
        {
            "pen",
            "felt-tip",
            "pencil",
            "brush",
            "paper",
            "notebook",
            "album"
        };
        string[] countries = new string[]
        {
            "USA",
            "Germany",
            "Italy",
            "Georgia",
            "China",
            "Korea",
            "Kazachstan"
        };
        string[] devices = new string[]
        {
            "phone",
            "headphones",
            "camera",
            "powerbank",
            "charge",
            "tablet",
        };

        var unique = trees.Length + writer.Length + countries.Length + devices.Length;
        var count = 40_000;
        string path = Environment.CurrentDirectory + @"\" + $"Unique size {unique} Full size {unique * count}.txt";
        for (int i = 0; i < count; i++)
        {
            foreach (var item in trees)
                File.AppendAllText(path, item + " ");
            foreach (var item in writer)
                File.AppendAllText(path, item + " ");
            foreach (var item in countries)
                File.AppendAllText(path, item + " ");
            foreach (var item in devices)
                File.AppendAllText(path, item + " ");
        }
    }
    static void CreateWords28UniqueTextWhereElementIsOnce()
    {
        string[] trees = new string[]
        {
             "Oak", //1
             "Birch", //2
             "Ash", //3
             "Spruce", //4
             "Stump",//5
             "Maple", //6
             "Larch", //7
        };
        string[] writer = new string[]
        {
            "pen",
            "felt-tip",
            "pencil",
            "brush",
            "paper",
            "notebook",
            "album"
        };
        string[] countries = new string[]
        {
            "USA",
            "Germany",
            "Italy",
            "Georgia",
            "China",
            "Korea",
            "Kazachstan"
        };
        string[] devices = new string[]
        {
            "phone",
            "headphones",
            "camera",
            "powerbank",
            "charge",
            "tablet",
        };

        var unique = trees.Length + writer.Length + countries.Length + devices.Length;
        var count = 40_000;
        string path = Environment.CurrentDirectory + @"\" + $"Unique size {unique} with one once Full size {unique * count}.txt";
        for (int i = 0; i < count; i++)
        {
            foreach (var item in trees)
                File.AppendAllText(path, item + " ");
            foreach (var item in writer)
                File.AppendAllText(path, item + " ");
            foreach (var item in countries)
                File.AppendAllText(path, item + " ");
            foreach (var item in devices)
                File.AppendAllText(path, item + " ");
        }
        File.AppendAllText(path, "Valera");
    }

    static IEnumerable<int> GetUniqueItems(int number)
    {
        int[] unique = new int[number];
        while (number > 0)
        {
            unique[--number] = number.ToString().GetHashCode();
        }
        return unique;
    }
}
