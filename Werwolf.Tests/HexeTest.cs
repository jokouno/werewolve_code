using Werwolf.Data;
using Werwolf.Data.Actions;

namespace Werwolf.Tests
{
    public class HexeTest
    {
        // 1 Hexe Selects InstantKill AlteSchrulle Alive -> AlteSchrulle Dies
        [Fact]
        public void Hexe_Select_InstantKill_AlteSchrulle_Alive_Dies()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf",
                nameof(Grabrauber),
                nameof(AlteSchrulle),
                nameof(Hexe)
            };

            List<Role> roles = new List<Role>
            {
                new Raecher { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 1 },
                new Werwolf.Data.Werwolf { Count = 1 },
                new Grabrauber { Count = 1 },
                new AlteSchrulle { Count = 1 },
                new Hexe { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var alteSchrulle = gm.AllPlayers.First(p => p.RoleName == nameof(AlteSchrulle));
            var hexe = gm.AllPlayers.First(p => p.RoleName == nameof(Hexe));

            hexe.DoAction(new List<string> { alteSchrulle.PlayerName }, ActionType.InstantKill);

            _ = gm.EndNight();

            // Hexe
            Assert.Single(gm.DeadPlayers);
            Assert.False(alteSchrulle.IsAlive);
            Assert.Single(hexe.Actions);
            Assert.Equal(ActionType.HealAll , hexe.Actions.First().ActionType);
        }

        // 1 Hexe Selects HealAll -> NoOne Dies
        [Fact]
        public void Hexe_Select_HealAll_NoOneDies()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf",
                nameof(Grabrauber),
                nameof(AlteSchrulle),
                nameof(Hexe)
            };

            List<Role> roles = new List<Role>
            {
                new Raecher { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 1 },
                new Werwolf.Data.Werwolf { Count = 1 },
                new Grabrauber { Count = 1 },
                new AlteSchrulle { Count = 1 },
                new Hexe { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var doctor = gm.AllPlayers.First(p => p.RoleName == nameof(Doctor));
            var amor = gm.AllPlayers.First(p => p.RoleName == nameof(Amor));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));
            var grabrauber = gm.AllPlayers.First(p => p.RoleName == nameof(Grabrauber));
            var alteSchrulle = gm.AllPlayers.First(p => p.RoleName == nameof(AlteSchrulle));
            var hexe = gm.AllPlayers.First(p => p.RoleName == nameof(Hexe));

            hexe.DoAction(Enumerable.Empty<string>().ToList(), ActionType.HealAll);
            werwolf.DoAction(new List<string> { villager.PlayerName }, ActionType.Kill);

            _ = gm.EndNight();

            // Hexe
            Assert.Empty(gm.DeadPlayers);
            Assert.True(raecher.IsAlive);
            Assert.True(villager.IsAlive);
            Assert.True(doctor.IsAlive);
            Assert.True(amor.IsAlive);
            Assert.True(werwolf.IsAlive);
            Assert.True(grabrauber.IsAlive);
            Assert.True(alteSchrulle.IsAlive);
            Assert.True(hexe.IsAlive);
            Assert.Single(hexe.Actions);
            Assert.Equal(ActionType.InstantKill, hexe.Actions.First().ActionType);
        }

        // 1 Hexe Selects InstantKill Doctor Heals -> NoOne Dies
        [Fact]
        public void Hexe_Select_InstantKill_DoctorHeals_NoOneDies()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf",
                nameof(Grabrauber),
                nameof(AlteSchrulle),
                nameof(Hexe)
            };

            List<Role> roles = new List<Role>
            {
                new Raecher { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 1 },
                new Werwolf.Data.Werwolf { Count = 1 },
                new Grabrauber { Count = 1 },
                new AlteSchrulle { Count = 1 },
                new Hexe { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var doctor = gm.AllPlayers.First(p => p.RoleName == nameof(Doctor));
            var amor = gm.AllPlayers.First(p => p.RoleName == nameof(Amor));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));
            var grabrauber = gm.AllPlayers.First(p => p.RoleName == nameof(Grabrauber));
            var alteSchrulle = gm.AllPlayers.First(p => p.RoleName == nameof(AlteSchrulle));
            var hexe = gm.AllPlayers.First(p => p.RoleName == nameof(Hexe));

            hexe.DoAction(new List<string> { villager.PlayerName }, ActionType.InstantKill);
            doctor.DoAction(new List<string> { villager.PlayerName }, ActionType.Heal);

            _ = gm.EndNight();

            // Hexe
            Assert.Empty(gm.DeadPlayers);
            Assert.True(raecher.IsAlive);
            Assert.True(villager.IsAlive);
            Assert.True(doctor.IsAlive);
            Assert.True(amor.IsAlive);
            Assert.True(werwolf.IsAlive);
            Assert.True(grabrauber.IsAlive);
            Assert.True(alteSchrulle.IsAlive);
            Assert.True(hexe.IsAlive);
            Assert.Single(hexe.Actions);
            Assert.Equal(ActionType.HealAll, hexe.Actions.First().ActionType);
        }

        // 1 Hexe Selects HealAll Couple dies -> NoOne Dies
        [Fact]
        public void Hexe_Select_HealAll_Couple_DoctorHeals_NoOneDies()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf",
                nameof(Grabrauber),
                nameof(AlteSchrulle),
                nameof(Hexe)
            };

            List<Role> roles = new List<Role>
            {
                new Raecher { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 1 },
                new Werwolf.Data.Werwolf { Count = 1 },
                new Grabrauber { Count = 1 },
                new AlteSchrulle { Count = 1 },
                new Hexe { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var doctor = gm.AllPlayers.First(p => p.RoleName == nameof(Doctor));
            var amor = gm.AllPlayers.First(p => p.RoleName == nameof(Amor));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));
            var grabrauber = gm.AllPlayers.First(p => p.RoleName == nameof(Grabrauber));
            var alteSchrulle = gm.AllPlayers.First(p => p.RoleName == nameof(AlteSchrulle));
            var hexe = gm.AllPlayers.First(p => p.RoleName == nameof(Hexe));

            werwolf.DoAction(new List<string> { villager.PlayerName }, ActionType.Kill);
            amor.DoAction(new List<string> { villager.PlayerName, grabrauber.PlayerName }, ActionType.Amorize);
            hexe.DoAction(new List<string> { villager.PlayerName }, ActionType.HealAll);

            _ = gm.EndNight();

            // Hexe
            Assert.Empty(gm.DeadPlayers);
            Assert.True(raecher.IsAlive);
            Assert.True(villager.IsAlive);
            Assert.True(doctor.IsAlive);
            Assert.True(amor.IsAlive);
            Assert.True(werwolf.IsAlive);
            Assert.True(grabrauber.IsAlive);
            Assert.True(alteSchrulle.IsAlive);
            Assert.True(hexe.IsAlive);
            Assert.Single(hexe.Actions);
            Assert.Equal(ActionType.InstantKill, hexe.Actions.First().ActionType);
        }
    }
}
