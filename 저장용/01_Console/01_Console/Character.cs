using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
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
                if (hp != value) 
                {
                    hp = value;
                    Console.WriteLine($"남은 hp: {HP}");

                    if (hp <= 0) Die();
                    hp = Math.Clamp(hp, 0, maxHp);
                }
                
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
        protected float attackPower;
        float defencePower;

        public bool Died => hp < 1;

        protected string name;
        public string Name => name;

        protected float skillCost = 10.0f;
        private bool isSkillOn => MP > skillCost;

        Random random;

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

            random = new Random();
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

            random = new Random();
        }

        public void Attack(Character target) 
        {
            if(isSkillOn) 
            {
                if (random.NextSingle() < 0.3f)
                {
                    Skill(target);
                }
                else 
                {
                    Console.WriteLine($"{name}의 공격");
                    target.Defence(attackPower);
                }
            }
            else
            {
                Console.WriteLine($"{name}의 공격");
                target.Defence(attackPower);
            }
        }

        public void Skill(Character target) 
        {
            if (isSkillOn)
            {
                MP -= skillCost;
                target.Defence(OnSkill());
            }
            else 
            {
                Console.WriteLine("마나가 부족합니다");
            }
        }
        protected virtual float OnSkill()
        {
            Console.WriteLine("캐릭터의 스킬사용");
            return 10.0f;
        }

        void Defence(float damage) 
        {
            HP -= damage - defencePower;
            Console.WriteLine($"{name}이 {damage-defencePower}만큼의 피해를 받았습니다.");
        }

        void LevelUp() 
        {
        
        }

        void Die() 
        {
            Console.WriteLine($"{name}가 사망하였습니다.");
        }
    }
}
