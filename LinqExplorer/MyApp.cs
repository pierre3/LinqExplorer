﻿using ConsoleAppFramework;

partial class MyApp
{

    //1. CountUpEnumerator と CountUpEnumerable
    [Command("sample-1")]
    public void Sample_1()
    {
        ConsoleEx.WriteLine("# IEnumerable<T> と IEnumerator<T>", ConsoleColor.Magenta);
        foreach (var n in new CountUpEnumerable(1))
        {
            ConsoleEx.WriteLine(n, ConsoleColor.Green);
            if (n == 10) { break; }
        }
    }

    //2. イテレータ
    [Command("sample-2")]
    public void Sample_2()
    {
        ConsoleEx.WriteLine("# イテレーター構文: IEnumerator<T>を戻り値とするパターン", ConsoleColor.Magenta);
        foreach (var n in new CountUpEnumerable2(1))
        {
            ConsoleEx.WriteLine(n, ConsoleColor.Green);
            if (n == 10) { break; }
        }

        ConsoleEx.WriteLine("# イテレーター構文: IEnumerable<T>を戻り値とするパターン", ConsoleColor.Magenta);
        foreach (var n in MyEnumerable.CountUp(1))
        {
            ConsoleEx.WriteLine(n, ConsoleColor.Green);
            if (n == 10) { break; }
        }
    }


    //3. where, selectの実装と遅延実行
    [Command("sample-3")]
    public void Sample_3()
    {
        Console.WriteLine("set query");
        var query = new CountUpEnumerable(1)
            .MyWhere(n => n % 2 == 0)
            .MySelect(n => n * n)
            .MyTake(5);

        Console.WriteLine(">>start foreach");
        foreach (var n in query)
        {
            ConsoleEx.WriteLine(n, ConsoleColor.Green);
        }
        Console.WriteLine("<< end foreach");
    }

    //4. count,toListの実装と即時実行
    [Command("sample-4")]
    public void Sample_4()
    {
        ConsoleEx.WriteLine("# Count() と ToList()", ConsoleColor.Magenta);
        var ie = new CountUpIterator(1)
            .MyWhere(n => n % 2 == 0)
            .MySelect(n => n * n)
            .MyTake(5);

        Console.WriteLine("Count()");
        var count = ie.MyCount();
        ConsoleEx.WriteLine($"Count={count}", ConsoleColor.Green);
        ConsoleEx.WriteLine("-----------------------------", ConsoleColor.Magenta);
        Console.WriteLine("ToList()");
        var list = ie.MyToList();
        ConsoleEx.WriteLine($"[{string.Join(",", list)}]", ConsoleColor.Green);
    }

    //4. 即時実行されるメソッドを使うタイミング
    [Command("sample-5")]
    public void Sample_5()
    {
        ConsoleEx.WriteLine("# ToList()によるクエリ結果のキャッシュ", ConsoleColor.Magenta);
        var list = new CountUpIterator(1)
            .MyWhere(n => n % 2 == 0)
            .MySelect(n => n * n)
            .MyTake(5)
            .MyToList();
       　
        ConsoleEx.WriteLine($"Count= {list.Count} //List<T>のCountプロパティから取得出来る", ConsoleColor.Green);

        ConsoleEx.WriteLine("# ここでforeachしてもクエリの再評価は行われない", ConsoleColor.Magenta);
        Console.WriteLine(">>start foreach");
        foreach (var item in list)
        {
            ConsoleEx.WriteLine(item, ConsoleColor.Green);            
        }
        Console.WriteLine("<<end foreach");
    }

    //6. GroupByの実装
    [Command("sample-6")]
    public void Sample_6()
    {
        var groups = Student.EnumerateData().MyGroupBy(
            item => item.Class,
            item => item.Score,
            (key, scores) =>
            {
                Console.WriteLine($"ResultSelector: Key={key}");
                Console.WriteLine($"- Count");
                var count = scores.Count();
                Console.WriteLine($"- Max");
                var max = scores.Max();
                Console.WriteLine($"- Avarage");
                var avg = scores.Average();
                return new
                {
                    Class = key,
                    Count = count,
                    Max = max,
                    Average = avg
                };
            });

        foreach (var group in groups)
        {
            ConsoleEx.WriteLine(group, ConsoleColor.Green);
        }
    }
}


