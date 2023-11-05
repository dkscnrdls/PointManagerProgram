using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;

namespace PointOfficeProgram
{
    internal class Program
    {
        // 형식은 '1 -- XXX -- 100' XXX는 성명, 100은 포인트
        static string DataStart = "-------------------------------Srt";
        static string DataEnd = "-------------------------------End";
        static string ExampleText = "XXX | 100";
        static string[] mainargs = null;
        public static void Main(string[] args)
        {
            Console.Clear();
            Console.Title = "포인트 관리 프로그램";
            ConsoleKey Keys = ConsoleKey.NoName;
            Menu();
            mainargs = args;
            
            while ((Keys = Key()) != ConsoleKey.Escape)
            {
                switch (Keys)
                {
                    case ConsoleKey.D1:
                        PointManagement();
                        Main(args);
                        break;
                    case ConsoleKey.D2:
                        Environment.Exit(Environment.ExitCode);
                        break;
                }
            }

        }

        private static ConsoleKey Key()
        {
            return Console.ReadKey().Key;
        }
        private static void Menu()
        {
            Console.WriteLine("----------포인트 관리 프로그램----------");
            Console.WriteLine("  메뉴를 선택하세요.");
            Console.WriteLine("1. 포인트 관리");
            Console.WriteLine("2. 종료");
        }
        public static void PointManagement()
        {
            ConsoleKey Keys = ConsoleKey.NoName;
            object Path = Directory.GetParent(Assembly.GetExecutingAssembly().Location);
            DirectoryInfo info = new DirectoryInfo(Path + @"\Data");

            if (info.Exists == false)
            {
                object Folder = Directory.CreateDirectory(Path.ToString());
                Directory.Move(Folder.ToString(), "Data");
            }
            object OnFolder = Directory.GetParent(Assembly.GetExecutingAssembly().Location) + @"\Data";
            object PointDataText = Directory.GetFiles(@OnFolder.ToString(), "*.txt");

            if (PointDataText == null)
            {
                Console.WriteLine("포인트 데이터 파일이 존재하지 않습니다. 새로 만드시겠습니까?");
                Console.ReadLine();
            }
            else
            {
                StreamReader streamReader = new StreamReader(@OnFolder.ToString() + @"\Data.txt", Encoding.ASCII);
                string Text = streamReader.ReadToEnd();
                streamReader.Close();
                Console.Clear();
                Console.WriteLine("데이터 파일");
                Console.WriteLine("     ");
                Console.WriteLine(Text);
                Console.WriteLine("     ");
                Console.WriteLine("메뉴를 고르세요");
                Console.WriteLine("1. 수정");
                Console.WriteLine("2. 메인 메뉴");
                Console.WriteLine("3. 데이터 파일 초기화");
                while ((Keys = Key()) != ConsoleKey.Escape)
                {
                    switch (Keys)
                    {
                        case ConsoleKey.D1:
                            Edit(OnFolder.ToString());
                            break;
                        case ConsoleKey.D2:
                            Main(mainargs);
                            break;
                        case ConsoleKey.D3:
                            DataFileReset(OnFolder.ToString());
                            break;
                    }
                }
            }
        }
        public static void Edit(string Path)
        {
            Console.Clear();
            StreamReader streamReader = new StreamReader(Path.ToString() + @"\Data.txt", Encoding.ASCII);
            string Data = "";
            Data = streamReader.ReadLine();
            if (Data == DataStart)
            {
                int Number = 2;
                Number = Number += 1;
                Data = streamReader.ReadLine();
                streamReader.Close();
                int InTextinInt = Data.IndexOf(Data);
                Console.WriteLine(InTextinInt.ToString());
                if (InTextinInt == 0)
                {
                    Console.WriteLine("바꿀 이름을 쓰세요.");
                    Console.WriteLine("이름 : ");
                    string Name = Console.ReadLine();
                    Console.WriteLine("바꿀 포인트를 쓰세요. (포인트 값을 냅둔다면 기존 포인트 값과 똑같이 적어주세요.)");
                    string Point = Console.ReadLine();
                    Write(Path, Number, Name, Point);
                }
            }
            else if (Data == DataEnd)
            {
                PointManagement();
            }
        }
        public static void Write(string Path, int Number, string Name, string Point)
        {
            StreamWriter streamWriter = new StreamWriter(Path.ToString() + @"\Data.txt", true, Encoding.ASCII);
            streamWriter.Write(DataStart + "\r\n{0} -- {1}\r\n" + DataEnd, Name, Point);
            streamWriter.Close();
            PointManagement();
        }
        public static void DataFileReset(string Path)
        {
            Console.WriteLine("데이터 파일을 정말로 초기화하시겠습니까? 확인하려면 Y, 취소하려면 N을 누르세요. (반드시 대문자로 적어주세요.");
            string Key = Console.ReadLine();
            if (Key == "Y")
            {
                File.WriteAllText(Path + @"\Data.txt", "-------------------------------Srt\r\nXXX -- 100\r\n-------------------------------End");
                PointManagement();
            }
            else if (Key == "N")
            {
                PointManagement();
            }
        }
    }
}