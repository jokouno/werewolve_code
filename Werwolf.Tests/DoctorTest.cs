using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Werwolf.Data;
using Werwolf.Data.Actions;

namespace Werwolf.Tests
{
    public class DoctorTest
    {
        // 1 Doctor Heals 1 Person -> Stays Alive
        [Fact]
        public void Doctor_Select_Role_Alive_Healed()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf"
            };

            List<Role> roles = new List<Role>
            {
                new Raecher { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 1 },
                new Werwolf.Data.Werwolf { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var doctor = gm.AllPlayers.First(p => p.RoleName == nameof(Doctor));
            var amor = gm.AllPlayers.First(p => p.RoleName == nameof(Amor));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));

            doctor.DoAction(new List<string> { raecher.PlayerName }, ActionType.Heal);
            werwolf.DoAction(new List<string> { raecher.PlayerName }, ActionType.Kill);

            gm.ProcessNight();

            // Doctor
            Assert.True(gm.DeadPlayers.Count == 0);
            Assert.True(raecher.IsAlive);
        }

        // 1 Doctor Heals 1 Person not selected -> Nothing happens
        [Fact]
        public void Doctor_Select_Role_Alive_NothingHappens()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf"
            };

            List<Role> roles = new List<Role>
            {
                new Raecher { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 1 },
                new Werwolf.Data.Werwolf { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var doctor = gm.AllPlayers.First(p => p.RoleName == nameof(Doctor));
            var amor = gm.AllPlayers.First(p => p.RoleName == nameof(Amor));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));

            doctor.DoAction(new List<string> { raecher.PlayerName }, ActionType.Heal);
            werwolf.DoAction(new List<string> { villager.PlayerName }, ActionType.Kill);

            gm.ProcessNight();

            // Doctor
            Assert.True(gm.DeadPlayers.Count == 1);
            Assert.True(raecher.IsAlive);
            Assert.False(villager.IsAlive);
        }

        // 1 Doctor Heals 1 Person already dead -> Nothing happens
        [Fact]
        public void Doctor_Select_Role_Dead_NothingHappens()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf"
            };

            List<Role> roles = new List<Role>
            {
                new Raecher { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 1 },
                new Werwolf.Data.Werwolf { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var doctor = gm.AllPlayers.First(p => p.RoleName == nameof(Doctor));
            var amor = gm.AllPlayers.First(p => p.RoleName == nameof(Amor));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));

            werwolf.DoAction(new List<string> { villager.PlayerName }, ActionType.Kill);

            gm.ProcessNight();

            // Werwolf
            Assert.True(gm.DeadPlayers.Count == 1);
            Assert.False(villager.IsAlive);

            doctor.DoAction(new List<string> { villager.PlayerName }, ActionType.Heal);

            // Doctor
            Assert.True(gm.DeadPlayers.Count == 1);
            Assert.False(villager.IsAlive);
        }

        // 1 Doctor Heals Himself -> Stays alive
        [Fact]
        public void Doctor_Select_Himself_Alive_Healed()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf"
            };

            List<Role> roles = new List<Role>
            {
                new Raecher { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 1 },
                new Werwolf.Data.Werwolf { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var doctor = gm.AllPlayers.First(p => p.RoleName == nameof(Doctor));
            var amor = gm.AllPlayers.First(p => p.RoleName == nameof(Amor));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));

            werwolf.DoAction(new List<string> { doctor.PlayerName }, ActionType.Kill);
            doctor.DoAction(new List<string> { doctor.PlayerName }, ActionType.Heal);

            gm.ProcessNight();

            // Doctor
            Assert.True(gm.DeadPlayers.Count == 0);
            Assert.True(doctor.IsAlive);
        }

        // 1 Doctor Heals Himself Multiple Rounds -> Stays alive first round -> Dies Second Round
        //[Fact]
        //public void Doctor_Select_Himself_MultipleRounds_Alive_DiesSecondRound()
        //{
        //    List<string> names = new List<string>
        //    {
        //        nameof(Raecher),
        //        "Dorfbewohner",
        //        "Doctor",
        //        "Amor",
        //        "Werwolf"
        //    };

        //    List<Role> roles = new List<Role>
        //    {
        //        new AlteSchrulle { Count = 1 },
        //        new Dorfbewohner { Count = 1 },
        //        new Doctor { Count = 1 },
        //        new Amor { Count = 1 },
        //        new Werwolf.Data.Werwolf { Count = 1 }
        //    };

        //    var gm = GameManagerTests.InitializeTest(names, roles);

        //    var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
        //    var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
        //    var doctor = gm.AllPlayers.First(p => p.RoleName == nameof(Doctor));
        //    var amor = gm.AllPlayers.First(p => p.RoleName == nameof(Amor));
        //    var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));

        //    werwolf.DoAction(new List<string> { doctor.PlayerName }, ActionType.Kill);
        //    doctor.DoAction(new List<string> { doctor.PlayerName }, ActionType.Heal);

        //    gm.ProcessNight();

        //    // Doctor
        //    Assert.True(gm.DeadPlayers.Count == 0);
        //    Assert.True(doctor.IsAlive);

        //    werwolf.DoAction(new List<string> { doctor.PlayerName }, ActionType.Kill);
        //    doctor.DoAction(new List<string> { doctor.PlayerName }, ActionType.Heal);

        //    gm.ProcessNight();

        //    // Doctor
        //    Assert.True(gm.DeadPlayers.Count == 1);
        //    Assert.False(doctor.IsAlive);
        //}

        // 1 Doctor Heals right Role in Couple -> Both Stay alive
        [Fact]
        public void Doctor_Select_Couple_Alive_Healed()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf"
            };

            List<Role> roles = new List<Role>
            {
                new Raecher { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 1 },
                new Werwolf.Data.Werwolf { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var doctor = gm.AllPlayers.First(p => p.RoleName == nameof(Doctor));
            var amor = gm.AllPlayers.First(p => p.RoleName == nameof(Amor));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));

            werwolf.DoAction(new List<string> { villager.PlayerName }, ActionType.Kill);
            doctor.DoAction(new List<string> { villager.PlayerName }, ActionType.Heal);
            amor.DoAction(new List<string> { villager.PlayerName, doctor.PlayerName }, ActionType.Amorize);

            gm.ProcessNight();

            // Doctor
            Assert.True(gm.DeadPlayers.Count == 0);
            Assert.True(doctor.IsAlive);
            Assert.True(villager.IsAlive);
        }

        // 1 Doctor Heals wrong Role in Couple -> Both die
        [Fact]
        public void Doctor_Select_Couple_Alive_BothDie()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf"
            };

            List<Role> roles = new List<Role>
            {
                new Raecher { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 1 },
                new Werwolf.Data.Werwolf { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var doctor = gm.AllPlayers.First(p => p.RoleName == nameof(Doctor));
            var amor = gm.AllPlayers.First(p => p.RoleName == nameof(Amor));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));

            werwolf.DoAction(new List<string> { villager.PlayerName }, ActionType.Kill);
            doctor.DoAction(new List<string> { doctor.PlayerName }, ActionType.Heal);
            amor.DoAction(new List<string> { villager.PlayerName, doctor.PlayerName }, ActionType.Amorize);

            gm.ProcessNight();

            // Doctor
            Assert.True(gm.DeadPlayers.Count == 2);
            Assert.False(doctor.IsAlive);
            Assert.False(villager.IsAlive);
        }

        // 1 Doctor Heals Raecher -> All survive
        [Fact]
        public void Doctor_Select_Raecher_Alive_Healed()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf"
            };

            List<Role> roles = new List<Role>
            {
                new Raecher { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 1 },
                new Werwolf.Data.Werwolf { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var doctor = gm.AllPlayers.First(p => p.RoleName == nameof(Doctor));
            var amor = gm.AllPlayers.First(p => p.RoleName == nameof(Amor));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));

            werwolf.DoAction(new List<string> { raecher.PlayerName }, ActionType.Kill);
            doctor.DoAction(new List<string> { raecher.PlayerName }, ActionType.Heal);
            raecher.DoAction(new List<string> { villager.PlayerName }, ActionType.RevengeKill);

            gm.ProcessNight();

            // Doctor
            Assert.True(gm.DeadPlayers.Count == 0);
            Assert.True(villager.IsAlive);
            Assert.True(raecher.IsAlive);
        }
    }
}
