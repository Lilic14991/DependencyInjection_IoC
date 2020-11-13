using System;

namespace PropertyDependencyInjection
{
    class Program
    {
        static void Main()
        {
            IMeleeWeapon MeleeWeapon = Generator.MeleeWeapon();
            IRangeWeapon RangeWeapon = Generator.RangeWeapon();

            Hunter hunter = Generator.MakeHunter();
            GearUp gearUp = Generator.ReadyGearUp();
            gearUp.HuntGearUp();

            // Property DI
            hunter.MeleeWeapon = MeleeWeapon;
            hunter.RangeWeapon = RangeWeapon;

            IBehemoth behemoth = Generator.MakeBehemoth(gearUp.behemothType);

            IHunt Hunt = Generator.Hunt(gearUp.meeleWeapon, gearUp.rangeWeapon);
            Hunt.Attack(hunter, behemoth);

            Hunt.HuntResult(hunter, behemoth);

            Console.ReadKey();

        }


    }

    public class GearUp
    {
        public string meeleWeapon;
        public string rangeWeapon;
        public int behemothType;


        public void HuntGearUp()
        {
            Console.WriteLine("Enter meele weapon you would like to equip: ");
            meeleWeapon = Console.ReadLine();

            Console.WriteLine("Enter range weapon you would like to equip: ");
            rangeWeapon = Console.ReadLine();

            Console.WriteLine("Enter Behemoth type you would like to encounter:\n 1 - Earth Behemoth\n 2 - Lightning Behemoth\n");
            behemothType = int.Parse(Console.ReadLine());
        }
    }

    public interface IHunt
    {
        void Attack(Hunter hunter, IBehemoth behemoth);
        void HuntStatus(Hunter hunter, IBehemoth behemoth);

        void HuntResult(Hunter hunter, IBehemoth behemoth);
        string meeleWeapon { get; set; }
        string rangeWeapon { get; set; }

    }

    public class Hunt : IHunt
    {
        public string meeleWeapon { get; set; }
        public string rangeWeapon { get; set; }

        public Hunt(string meeleWeapon, string rangeWeapon)
        {
            this.meeleWeapon = meeleWeapon;
            this.rangeWeapon = rangeWeapon;
        }

        public void Attack(Hunter hunter, IBehemoth behemoth)
        {
            while (hunter.health > 0 && behemoth.health > 0)
            {
                Console.WriteLine("Pick 1-4 options: \n 1 - Meele Weapon Attack\n 2 - Range Weapon\n 3 - Desperate Meele Attack \n 4 - Desperate Range Attack ");
                string pick = Console.ReadLine();


                if (pick == "")
                {
                    Console.WriteLine("Enter options from 1 - 4: ");
                }
                else
                {
                    switch (pick)
                    {
                        case "1":
                            hunter.MeleeWeaponAttack(meeleWeapon, behemoth.BehemothType);
                            behemoth.health -= 20;
                            HuntStatus(hunter, behemoth);
                            break;
                        case "2":
                            behemoth.BehemothBreath();
                            hunter.RangeWeaponMiss(rangeWeapon, behemoth.BehemothType);
                            hunter.health -= 10;
                            HuntStatus(hunter, behemoth);
                            break;
                        case "3":
                            behemoth.BehemothStomp();
                            hunter.MeleeWeaponMiss(meeleWeapon, behemoth.BehemothType);
                            hunter.health -= 15;
                            HuntStatus(hunter, behemoth);
                            break;
                        case "4":
                            hunter.RangeWeaponAttack(rangeWeapon, behemoth.BehemothType);
                            behemoth.health -= 30;
                            behemoth.BehemothStomp();
                            hunter.health -= 15;
                            HuntStatus(hunter, behemoth);
                            break;


                        default:
                            Console.WriteLine("Enter options from 1 - 4: ");
                            break;


                    }
                }
            }
        }

        public void HuntResult(Hunter hunter, IBehemoth behemoth)
        {
            if (hunter.health <= 0 && behemoth.health <= 0)
            {
                Console.WriteLine($"Hunter and Behemoth died of injuries!");
            }
            else if (hunter.health <= 0)
            {
                BehemothWins(hunter);
            }
            else
            {
                HunterWins(behemoth);
            }
        }

        public void HuntStatus(Hunter hunter, IBehemoth behemoth)
        {
            Console.WriteLine($"|Hunter HP: {hunter.health} using meele weapon: {meeleWeapon}\n& range weapon: {rangeWeapon} \n|Behemoth HP: {behemoth.health}");
        }

        public void BehemothWins(Hunter hunter)
        {
            if (hunter.health < 0)
            {
                Console.WriteLine("Devastatation!");
            }
            Console.WriteLine($"YOU HAVE LOST! Behemoth slained {hunter.GetType().Name}");
        }

        public void HunterWins(IBehemoth behemoth)
        {
            if (behemoth.health < 0)
            {
                Console.WriteLine("Brutal kill!");
            }
            Console.WriteLine($"YOU HAVE WON! Hunter slained {behemoth.BehemothType}");
        }


    }



    public class Hunter
    {
        public int health;
        

        
        public Hunter(int health)
        {
            this.health = health;
        }

        // Property DI
        public IMeleeWeapon MeleeWeapon { get; set; }
        public IRangeWeapon RangeWeapon { get; set; }

            
        public void MeleeWeaponAttack(string meeleWeapon, string target)
        {
            MeleeWeapon.MeleeWeaponHit(meeleWeapon, target);
        }

        public void MeleeWeaponMiss(string meleeWeapon, string target)
        {
            MeleeWeapon.MeleeWeaponMiss(meleeWeapon, target);
        }

        public void RangeWeaponAttack(string rangeWeapon, string target)
        {
            RangeWeapon.RangeWeaponHit(rangeWeapon, target);
        }

        public void RangeWeaponMiss(string rangeWeapon, string target)
        {
            RangeWeapon.RangeWeaponMiss(rangeWeapon, target);
        }
    }

    public interface IMeleeWeapon
    {
        void MeleeWeaponHit(string meeleWeapon, string target);
        void MeleeWeaponMiss(string meeleWeapon, string target);
    }

    public class MeleeWeapon : IMeleeWeapon
    {
        public void MeleeWeaponHit(string meeleWeapon, string target)
        {
            Console.WriteLine($"Meele attack with {meeleWeapon} hitted {target}");
        }

        public void MeleeWeaponMiss(string meeleWeapon, string target)
        {
            Console.WriteLine($"Meele attack with {meeleWeapon} missed {target}");
        }
    }

    public interface IRangeWeapon
    {
        void RangeWeaponHit(string rangeWeapon, string target);
        void RangeWeaponMiss(string ragneWeapon, string target);
    }

    public class RangeWeapon : IRangeWeapon
    {
        public void RangeWeaponHit(string rangeWeapon, string target)
        {
            Console.WriteLine($"Range attack with {rangeWeapon} damaged {target}");
        }

        public void RangeWeaponMiss(string rangeWeapon, string target)
        {
            Console.WriteLine($"Range attack with {rangeWeapon}  missed {target}");
        }
    }

    public interface IBehemoth
    {
        void BehemothBreath();
        void BehemothStomp();
        int health { get; set; }
        string BehemothType { get; set; }
    }

    public class EarthBehemoth : IBehemoth
    {
        public int health { get; set; }
        public string BehemothType { get; set; }
        public EarthBehemoth(int health, string behemothType)
        {
            this.health = health;
            BehemothType = behemothType;
        }

        public EarthBehemoth()
        {

        }

        public void BehemothBreath()
        {
            Console.WriteLine($"{BehemothType} breathes mud and rocks!");
        }

        public void BehemothStomp()
        {
            Console.WriteLine($"{BehemothType} stomps and shake the Earth!");
        }
    }

    public class LightningBehemoth : IBehemoth
    {
        public int health { get; set; }
        public string BehemothType { get; set; }

        public LightningBehemoth()
        {

        }

        public LightningBehemoth(int health, string behemothType)
        {
            this.health = health;
            BehemothType = behemothType;
        }

        public void BehemothBreath()
        {
            Console.WriteLine($"{BehemothType} launch the lightning from mouth!");
        }

        public void BehemothStomp()
        {
            Console.WriteLine($"{BehemothType} impales the ground with lightning spikes!");
        }
    }

    public static class Generator
    {
        public static IMeleeWeapon MeleeWeapon()
        {
            return new MeleeWeapon();
        }

        public static IRangeWeapon RangeWeapon()
        {
            return new RangeWeapon();
        }

        public static Hunter MakeHunter()
        {
            return new Hunter(100);
        }

        public static IBehemoth MakeBehemoth(int behemothType)
        {
            IBehemoth behemoth = new EarthBehemoth();

            if (behemothType == 1)
            {
                behemoth.BehemothType = "Earth Behemoth";
                return new EarthBehemoth(150, behemoth.BehemothType);
            }
            else
            {
                behemoth.BehemothType = "Lightning Behemoth";
                return new LightningBehemoth(150, behemoth.BehemothType);
            }
        }

        public static GearUp ReadyGearUp()
        {
            return new GearUp();
        }

        public static Hunt Hunt(string meeleWeapon, string rangeWeapon)
        {
            return new Hunt(meeleWeapon, rangeWeapon);
        }

    }
}
