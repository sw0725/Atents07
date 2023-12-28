using System;
using System.Collections.Generic;
using System.Text;

namespace _01_Console
{
    class Character
    {
        float hp;
        float maxHp;
        public float HP 
        {
            get => hp;
            private set 
            {
                hp = value;
                hp = Math.Clamp(hp, 0, maxHp);
            } 
        }
        float mp;
        float maxMp;
        public float MP 
        {
            get => mp;
            private set
            {
                mp = value;
                mp = Math.Clamp(mp, 0, maxMp);
            }
        }

        float level;
        float exp;
        const float maxExp = 100;
        float attackPower;
        float defencePower;

        bool died = false;
        public bool Died => died;

        protected string name;
        public string Name => name;

        public Character()
        {
            hp = 100;
            maxHp = 100;
            mp = 50;
            maxMp = 50;
            level = 1;
            exp = 0.0f;
            attackPower = 10.0f;
            defencePower = 5.0f;
            name = "Noname";
        }
        public Character(string _name)
        {
            hp = 100;
            maxHp = 100;
            mp = 50;
            maxMp = 50;
            level = 1;
            exp = 0.0f;
            attackPower = 10.0f;
            defencePower = 5.0f;
            name = _name;
        }

        public void Attack(Character target) 
        {
            Console.WriteLine($"{name}의 공격");
            target.Defence(attackPower);
        }

        public virtual void Skill() 
        {
            if (MP > 0)
            {
                MP--;
                Console.WriteLine("캐릭터의 스킬사용");
            }
            else 
            {
                Console.WriteLine("마나가 부족합니다");
            }
        }    

        void Defence(float damage) 
        {
            HP -= damage - defencePower;
            Console.WriteLine($"{name}이 {damage-defencePower}만큼의 피해를 받았습니다.");
            Console.WriteLine($"남은 hp: {HP}");
            if (HP == 0) 
            {
                Die();
            }
        }

        void LevelUp() 
        {
        
        }

        void Die() 
        {
            Console.WriteLine($"{name}가 사망하였습니다.");
            died = true;
        }
    }
}
