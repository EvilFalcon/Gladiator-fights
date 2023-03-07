using System;
using System.Threading;

namespace Gladiator_fights
{
    internal class Program
    {

        static void Main()
        {
            Console.Title = "Might and magic 6 : Mandate of heaven";
            Arena arena = new Arena();
            arena.Open();
        }
    }

    class Arena
    {
        private Hero _redFighter;
        private Hero _greenFighter;

        public void Open()
        {
            _redFighter = CreateFighter("Red_Fighter");
            _greenFighter = CreateFighter("Green_Fighter");

            Fight();
            AnnounceWinner();
        }

        private void Fight()
        {
            int millisecondsTimeout = 500;
            Console.Clear();

            while(_redFighter.IsAlive == true && _greenFighter.IsAlive == true)
            {
                _redFighter.ShowHealthInfo();
                _greenFighter.ShowHealthInfo();

                _redFighter.Attack(_greenFighter);

                _greenFighter.Attack(_redFighter);

                Thread.Sleep(millisecondsTimeout);
                Console.Clear();
            }
        }

        private void AnnounceWinner()
        {
            if(_redFighter.IsAlive == false && _greenFighter.IsAlive == false)
            {
                Console.WriteLine("Ничья");
            }
            else if(_redFighter.IsAlive == true)
            {
                Console.Write("ПОБЕДИЛ :\n\n");
                _redFighter.ShowHealthInfo();
                Console.WriteLine($"\n\n\nПРОИГРАЛ :\n\n");
                _greenFighter.ShowHealthInfo();
            }
            else
            {
                Console.Write("ПОБЕДИЛ :\n\n");
                _greenFighter.ShowHealthInfo();
                Console.Write($"\n\n\nПРОИГРАЛ :\n\n");
                _redFighter.ShowHealthInfo();
            }

            Console.ReadKey();
        }

        private Hero CreateFighter(string name)
        {
            const string CommandSelectCleric = "1";
            const string CommandSelectKnight = "2";
            const string CommandSelectPaladin = "3";
            const string CommandSelectArcher = "4";
            const string CommandSelectSorcerer = "5";

            Console.WriteLine($"{CommandSelectCleric} : Выбрать героя Cleric");
            Console.WriteLine($"{CommandSelectKnight} : Выбрать героя Knight");
            Console.WriteLine($"{CommandSelectPaladin} : Выбрать героя Paladin");
            Console.WriteLine($"{CommandSelectArcher} : Выбрать героя Archer");
            Console.WriteLine($"{CommandSelectSorcerer} : Выбрать героя Sorcerer");

            switch(Console.ReadLine())
            {
                case CommandSelectCleric:
                return new Cleric(name, 9, 70, 45, 100);

                case CommandSelectKnight:
                return new Knight(name, 13, 120, 60);

                default:
                case CommandSelectPaladin:
                return new Paladin(name, 9, 100, 65);

                case CommandSelectArcher:
                return new Archer(name, 9, 100, 50);

                case CommandSelectSorcerer:
                return new Sorcerer(name, 9, 70, 45, 100);
            }
        }
    }

    abstract class Hero
    {
        protected static Random RandomProbability = new Random();

        protected int MaxPercent = 100;

        protected int SpellPoints = 0;
        protected int MaxSpellPoints = 0;
        protected string Name;
        protected int Damage;
        protected int BonusAttack = 2;
        protected int Health;
        protected int FullHealth;
        protected int ArmorPercent;
        protected int RegenerationSpellPoints = 10;

        public bool IsAlive => Health > 0;

        public virtual void TakeDamage(int damage)
        {
            Health -= (damage * (MaxPercent - ArmorPercent)) / MaxPercent;
        }

        public virtual void Attack(Hero enemy)
        {
            enemy.TakeDamage(Damage);
        }

        public virtual void RestoreSpellPoint()
        {
            if(SpellPoints < MaxSpellPoints)
            {
                SpellPoints += RegenerationSpellPoints;
            }

            if(SpellPoints > MaxSpellPoints)
            {
                SpellPoints = MaxSpellPoints;
            }
        }

        public virtual void RestoreHealthPoints(int regenerationHealth)
        {
            int newDamage = Damage * regenerationHealth;

            if(Health > FullHealth)
            {
                Health += newDamage;
            }

            if(Health > FullHealth)
            {
                Health = FullHealth;
            }
        }

        public void ShowHealthInfo()
        {
            string heIsAliveHero = "";

            if(IsAlive == false)
            {
                heIsAliveHero = "умер";
            }
            else
            {
                heIsAliveHero = "живой";
            }

            string text = $"|{Name,26}|{Health,-8}/{FullHealth,5}|{SpellPoints,-5}/{MaxSpellPoints,4}|{ArmorPercent,11}%|{Damage,11}|{heIsAliveHero,12}|";
            Console.WriteLine(new string('_', text.Length));
            Console.WriteLine("|Класс героя и его команда |Здоровье героя|Очки магии|Защита героя|атака героя|Статус героя|");
            Console.WriteLine(text);
            Console.WriteLine(new string('─', text.Length));
        }
    }

    class Cleric : Hero
    {
        private int _chanceOfTreatment = 30;
        private int _chanceOfRegeneration = 20;

        public Cleric(string name, int damage, int health, int armor, int spellPoints)
        {
            Name = "Cleric---[" + name + "]";
            Damage = damage;
            Health = health;
            ArmorPercent = armor;
            FullHealth = Health;
            SpellPoints = spellPoints;
            MaxSpellPoints = SpellPoints;
        }

        public override void Attack(Hero enemy)
        {
            if(RandomProbability.Next(MaxPercent + 1) < _chanceOfTreatment)
            {
                base.RestoreSpellPoint();
            }

            if(RandomProbability.Next(MaxPercent + 1) < _chanceOfRegeneration)
            {
                CastSpellHealth();
            }

            base.Attack(enemy);
        }

        private void CastSpellHealth()
        {
            int spellHealth = 30;
            int priceSpellPoints = 30;

            if(Health + spellHealth <= FullHealth && SpellPoints >= priceSpellPoints)
            {
                Console.WriteLine($"{Name} : Использовал заклинание лечения");

                Health += spellHealth;
                SpellPoints -= priceSpellPoints;
            }
        }
    }

    class Knight : Hero
    {
        private int doubleAttackChance = 30;

        public Knight(string name, int damage, int health, int armor)
        {
            Name = "Knight---[" + name + "]";
            Damage = damage;
            Health = health;
            ArmorPercent = armor;
            FullHealth = Health;
        }

        public override void Attack(Hero enemy)
        {
            if(RandomProbability.Next(MaxPercent + 1) < doubleAttackChance)
            {
                enemy.TakeDamage(Damage * BonusAttack);
            }
            else
            {
                base.Attack(enemy);
            }
        }
    }

    class Paladin : Hero
    {
        private int blockChance = 30;

        public Paladin(string name, int heroDamage, int healthHero, int armor)
        {
            Name = "Paladin---[" + name + "]";
            Damage = heroDamage;
            Health = healthHero;
            ArmorPercent = armor;
            FullHealth = Health;
        }

        public override void TakeDamage(int damage)
        {
            if(RandomProbability.Next(MaxPercent + 1) > blockChance)
            {
                base.TakeDamage(damage);
            }
        }
    }

    class Archer : Hero
    {
        private int _regenerationHealth = 2;
        private int _healthRegenerationChance = 25;
        private int _blessingChance = 70;
        private int _bonusDamageSpecialAbility = 2;
        private bool _isAbilityWorks = false;

        public Archer(string name, int heroDamage, int healthHero, int armor)
        {
            Name = "Archer---[" + name + "]";
            Damage = heroDamage;
            Health = healthHero;
            ArmorPercent = armor;
            FullHealth = Health;
        }

        public override void Attack(Hero enemy)
        {
            if(RandomProbability.Next(MaxPercent) < _blessingChance && _isAbilityWorks == false)
            {
                UseSpecialAbility();
                _isAbilityWorks = true;
            }

            if(RandomProbability.Next(MaxPercent) < _healthRegenerationChance)
            {
                base.RestoreHealthPoints(_regenerationHealth);
            }

            base.Attack(enemy);
        }

        public void UseSpecialAbility()
        {
            Damage *= _bonusDamageSpecialAbility;
        }
    }

    class Sorcerer : Hero
    {
        private int _spellPoints;
        private int _maxSpellPoints;
        private int _regenerationSpellPoints = 10;
        private int _chanceOfRegeneration = 20;
        private int _magicDamage = 10;
        private int _fireballCost = 15;

        public Sorcerer(string name, int heroDamage, int healthHero, int armor, int spellPoints)
        {
            Name = "Sorcerer---[" + name + "]";
            Damage = heroDamage;
            Health = healthHero;
            ArmorPercent = armor;
            FullHealth = Health;
            _spellPoints = spellPoints;
            _maxSpellPoints = _spellPoints;
        }

        public override void Attack(Hero enemy)
        {
            if(RandomProbability.Next(MaxPercent) < _chanceOfRegeneration)
            {
                RestoreSpellPoint();
            }

            FireballCast(enemy);
        }

        private void FireballCast(Hero enemy)
        {
            if(_fireballCost < _spellPoints)
            {
                enemy.TakeDamage(Damage + _magicDamage);
            }
        }
    }

}


