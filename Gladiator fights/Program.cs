using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gladiator_fights
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }

    abstract class Gladiator
    {
        protected int _damageHero;
        protected int _healthHero;
        protected int _fullHealthHealth;
        protected float _armorHero;
        protected bool _heIsAliveHero = true;

        //public int Damage => _damage;

        public void TakeDamage(Gladiator gladiator)
        {
            gladiator._healthHero -= (int)(_damageHero * _armorHero);

            if(gladiator._healthHero <= 0)
            {
                _heIsAliveHero = false;               
            }
        }

        public void GiveDamage()
        {

        }

        protected int SetParameters(int minValue, int maxValue)
        {
            Random randomValue = new Random();

            return randomValue.Next(minValue, maxValue);
        }

        public void ShowHeroHealthInfo(Gladiator gladiator)
        {
            string heIsAliveHero = "";

            if(gladiator._heIsAliveHero == false)
            {
                heIsAliveHero = "умер";
            }
            else
            {
                heIsAliveHero = "живой";
            }

            Console.WriteLine("Здоровье героя|Защита героя|атака героя|Статус героя|");
            Console.WriteLine($"{_fullHealthHealth}{_healthHero}|{_armorHero * 100}|{_damageHero}|{heIsAliveHero}");
        }
    }

    class Cleric : Gladiator
    {

        private string _name = "Cleric";
        private int _magicPoints = 100;
        private int _maxDamage = 20;
        private int _minDamage = 10;
        private int _maxHealth = 100;
        private int _minHealth = 70;
        private float _armor = 0.6f;

        public Cleric(string name)
        {
            _name += $"--{name}";
            _damageHero = SetParameters(_minDamage, _maxDamage);
            _healthHero = SetParameters(_maxHealth, _minHealth);
            _armorHero = _armor;
            _fullHealthHealth = _healthHero;

        }
    }

    class Paladin : Gladiator
    {

        private string _name = "Paladin";
        private int _magicPoints = 60;
        private int _maxDamage = 20;
        private int _minDamage = 10;
        private int _maxHealth = 150;
        private int _minHealth = 100;
        private float _armor = 0.5f;

        public Paladin(string name)
        {
            _name += $"--{name}";
            _damageHero = SetParameters(_minDamage, _maxDamage);
            _healthHero = SetParameters(_maxHealth, _minHealth);
            _armorHero = _armor;
            _fullHealthHealth = _healthHero;
        }
    }

    class Warrior : Gladiator
    {
        private string _name = "Warrior";
        private int _maxDamage = 30;
        private int _minDamage = 20;
        private int _maxHealth = 250;
        private int _minHealth = 130;
        private float _armor = 0.4f;

        public Warrior(string name)
        {
            _name += $"--{name}";
            _damageHero = SetParameters(_minDamage, _maxDamage);
            _healthHero = SetParameters(_maxHealth, _minHealth);
            _armorHero = _armor;
            _fullHealthHealth = _healthHero;
        }

    }
}
