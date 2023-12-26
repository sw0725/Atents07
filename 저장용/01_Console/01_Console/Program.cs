namespace _01_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("이곳에 당신의 이름을 적어주십시오 : ");

            string name = Console.ReadLine();
            for (int i = 0; i < 3; i++) 
            {
                Console.WriteLine(name);
            }
        }
    }
}
