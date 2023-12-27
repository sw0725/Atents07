namespace _01_Console
{
    public enum Grade { A,B,C,D,F }
    internal class Program
    {
        static void Main(string[] args)
        {

        }
        static void InputName() 
        {
            Console.Write("이곳에 당신의 이름을 적어주십시오 : ");

            string name = Console.ReadLine();
            Console.WriteLine(name);
        }

        static void Day_231226()
        {
            Console.WriteLine("Hello, World!");     // 한줄을 출력하는 코드
            Console.WriteLine("고병조입니다.");

            Console.WriteLine("가가가");

            Console.Write("나나나");   // 글자를 출력하는 코드
            Console.Write("다다다");

            Console.WriteLine("라라라\n마마마");

            // 변수(Variable)
            // 변하는 숫자. 메모리에 기록해둔 값.
            // 변수를 사용하려면 미리 선언해야 한다.
            // 변수를 선언할 때는 데이터 타입과 이름을 명시해야 한다.

            // 키보드 입력을 한줄 받아서 input이라는 변수에 기록하기
            string input;                       // string 타입의 변수를 input이라는 이름으로 선언
            input = Console.ReadLine();
            // string input = Console.ReadLine();
            Console.WriteLine(input);           // input 변수의 내용을 출력하기


            // 실습
            // 시작하면 이름을 물어보고 이름을 3번 출력하는 코드 만들어보기
            Console.Write("당신의 이름은 무엇입니까? : ");
            input = Console.ReadLine();

            Console.WriteLine(input);
            Console.WriteLine(input);
            Console.WriteLine(input);

            /// 데이터 타입
            /// string : 문자열. 글자들을 저장하기 위한 데이터 타입
            /// int : 인티저. 정수형. 소수점 없는 숫자를 저장하기 위한 데이터 타입(32bit), +-21억까지는 안전.
            /// float : 플로트. 실수형. 소수점 있는 숫자를 저장하기 위한 데이터 타입(32bit), 태생적으로 오차가 있다.
            /// bool : 불. true 아니면 false만 저장하는 데이터 타입.

        }

        static void AgeCheck(int age) 
        {
            if (age >= 20)
            {
                Console.WriteLine("성인입니다.");
            }
            else
            {
                Console.WriteLine("미성년자입니다");
            }
        }

        static Grade GradeCheck(int score) 
        {
            Grade grade = Grade.F;
            if (score > 89)
            {
                Console.WriteLine("A");
                grade = Grade.A;
            }
            else if (score > 79)
            {
                Console.WriteLine("B");
                grade = Grade.B;
            }
            else if (score > 69)
            {
                Console.WriteLine("C");
                grade = Grade.C;
            }
            else if (score > 59)
            {
                Console.WriteLine("D");
                grade = Grade.D;
            }
            else if (60 > score)
            {
                Console.WriteLine("F");
            }
            return grade;
        }

        static void gugudan(int dan) 
        {
            for (int i = 1; i<10; i++) 
            {
                Console.WriteLine($"{dan}*{i}={dan*i}");
            }
        }

        static void Day_231227() 
        {
            InputName();

            string str1 = "Hello ";
            string str2 = "World";

            Console.WriteLine($"{str1}{str2}");

            Console.Write("나이를 입력해 주십시오 : ");

            int age;

            string Inputage = Console.ReadLine();
            int.TryParse(Inputage, out age);

            AgeCheck(age);

            Console.Write("성적을 입력해 주십시오 : ");

            int score;

            string Inputscore = Console.ReadLine();
            int.TryParse(Inputscore, out score);

            Grade grade = GradeCheck(score);
            switch (grade)
            {
                case Grade.A:
                case Grade.B:
                    Console.WriteLine("A,B등급은 ~한 혜택이 있습니다");
                    break;
                case Grade.C:
                    Console.WriteLine("C~한 혜택이 있습니다");
                    break;
                case Grade.D:
                    Console.WriteLine("D~한 혜택이 있습니다");
                    break;
                case Grade.F:
                    Console.WriteLine("혜택이 없습니다");
                    break;
                default:
                    Console.WriteLine("등급이 존재하지 않습니다.");
                    break;
            }

            Console.Write("단수를 입력해 주십시오 : ");
            int dan;

            string Inputdan = Console.ReadLine();
            int.TryParse(Inputdan, out dan);

            gugudan(dan);
        }

        static void DiceHigh() 
        {
            Random rand = new Random();
            bool stop = false;
            int count = 0;

            while (!stop)
            {
                int dice = rand.Next(5) + 1;

                Console.Write("주사위 눈 크기를 예측해 보십시오(0:High,1:Low) : ");

                int.TryParse(Console.ReadLine(), out int pradict);
                if (dice > 3)
                {
                    Console.WriteLine("High");
                    if (pradict == 0)
                    {
                        Console.WriteLine("성공");
                        count++;
                    }
                    else
                    {
                        Console.WriteLine("실패");
                        Console.WriteLine($"점수 : {count}");
                        count = 0;
                        stop = true;
                    }
                }
                else
                {
                    Console.WriteLine("Low");
                    if (pradict == 1)
                    {
                        Console.WriteLine("성공");
                        count++;
                    }
                    else
                    {
                        Console.WriteLine("실패");
                        Console.WriteLine($"점수 : {count}");
                        count = 0;
                        stop = true;
                    }
                }
            }
        }
        static void DiceHoll() 
        {
            Random rand = new Random();
            bool stop = false;
            int count = 0;

            while (!stop)
            {
                int dice = rand.Next(5) + 1;

                Console.Write("홀짝을 선택해 주십시오 (0:홀, 1:짝) : ");

                int.TryParse(Console.ReadLine(), out int pradict);
                if (dice % 2 == 0)//짝
                {
                    Console.WriteLine("짝");
                    if (pradict == 1)
                    {
                        Console.WriteLine("성공");
                        count++;
                    }
                    else
                    {
                        Console.WriteLine("실패");
                        Console.WriteLine($"점수 : {count}");
                        count = 0;
                        stop = true;
                    }
                }
                else
                {
                    Console.WriteLine("홀");
                    if (pradict == 0)
                    {
                        Console.WriteLine("성공");
                        count++;
                    }
                    else
                    {
                        Console.WriteLine("실패");
                        Console.WriteLine($"점수 : {count}");
                        count = 0;
                        stop = true;
                    }
                }
            }
        }
    }
}