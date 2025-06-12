using Werwolf.Data;
using Werwolf.Data.Actions;

namespace Werwolf.Tests
{
    public class AmorTest
    {
        // 1 Amor Select 2 Normal Roles -> Couple dies together
        [Fact]
        public void Amor_Select_NormalPlayers_Alive_Amorized()
        {
            List<string> names = new List<string>
            {
                "AlteSchrulle",
                "Dorfbewohner",
                "Amor",
                "Werwolf"
            };

            List<Role> roles = new List<Role>
            {
                new AlteSchrulle { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Amor { Count = 1 },
                new Werwolf.Data.Werwolf { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var alteSchrulle = gm.AllPlayers.First(p => p.RoleName == nameof(AlteSchrulle));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var amor = gm.AllPlayers.First(p => p.RoleName == nameof(Amor));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));

            amor.DoAction(new List<string> { alteSchrulle.PlayerName, villager.PlayerName }, ActionType.Amorize);
            werwolf.DoAction(new List<string>{ villager.PlayerName }, ActionType.Kill);

            _ = gm.EndNight();

            // Amor
            Assert.Equal(ConnectionType.Couple, alteSchrulle.Connections.First().ConnectionType);
            Assert.Contains(villager.PlayerName, alteSchrulle.Connections.First().DiesToo!);
            Assert.Equal(ConnectionType.Couple, villager.Connections.First().ConnectionType);
            Assert.Contains(alteSchrulle.PlayerName, villager.Connections.First().DiesToo!);

            // Werwolf
            Assert.True(gm.DeadPlayers.Count == 2);
            Assert.False(alteSchrulle.IsAlive);
            Assert.False(villager.IsAlive);
        }

        // 2 Amor Select 4 Normal Roles -> 2 different Couples
        [Fact]
        public void MultipleAmor_Select_NormalPlayers_Alive_Amorized()
        {
            List<string> names = new List<string>
            {
                "AlteSchrulle",
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Amor",
                "Werwolf"
            };

            List<Role> roles = new List<Role>
            {
                new AlteSchrulle { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 2 },
                new Werwolf.Data.Werwolf { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var alteSchrulle = gm.AllPlayers.First(p => p.RoleName == nameof(AlteSchrulle));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var doctor = gm.AllPlayers.First(p => p.RoleName == nameof(Doctor));
            var amor = gm.AllPlayers.Where(p => p.RoleName == nameof(Amor)).ToList();
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));

            amor.First().DoAction(new List<string> { alteSchrulle.PlayerName, villager.PlayerName }, ActionType.Amorize);
            amor.Last().DoAction(new List<string> { doctor.PlayerName, werwolf.PlayerName }, ActionType.Amorize);

            _ = gm.EndNight();

            // First Amor
            Assert.True(alteSchrulle.Connections.First().ConnectionType == ConnectionType.Couple);
            Assert.Contains(villager.PlayerName, alteSchrulle.Connections.First().DiesToo!);
            Assert.True(villager.Connections.First().ConnectionType == ConnectionType.Couple);
            Assert.Contains(alteSchrulle.PlayerName, villager.Connections.First().DiesToo!);

            // Second Amor
            Assert.True(doctor.Connections.First().ConnectionType == ConnectionType.Couple);
            Assert.Contains(werwolf.PlayerName, doctor.Connections.First().DiesToo!);
            Assert.True(werwolf.Connections.First().ConnectionType == ConnectionType.Couple);
            Assert.Contains(doctor.PlayerName, werwolf.Connections.First().DiesToo!);
        }

        // 2 Amor Select 4 Normal Roles -> 2 different Couples -> 1 Couple dies together
        [Fact]
        public void MultipleAmor_Select_NormalPlayers_Alive_Amorized_Dead()
        {
            List<string> names = new List<string>
            {
                "AlteSchrulle",
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Amor",
                "Werwolf"
            };

            List<Role> roles = new List<Role>
            {
                new AlteSchrulle { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 2 },
                new Werwolf.Data.Werwolf { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var alteSchrulle = gm.AllPlayers.First(p => p.RoleName == nameof(AlteSchrulle));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var doctor = gm.AllPlayers.First(p => p.RoleName == nameof(Doctor));
            var amor = gm.AllPlayers.Where(p => p.PlayerName.Contains("Amor")).ToList();
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));

            amor.First().DoAction(new List<string> { alteSchrulle.PlayerName, villager.PlayerName },
                ActionType.Amorize);
            amor.Last().DoAction(new List<string> { doctor.PlayerName, werwolf.PlayerName }, ActionType.Amorize);
            werwolf.DoAction(new List<string>{ doctor.PlayerName }, ActionType.Kill);

            _ = gm.EndNight();

            // First Amor
            Assert.True(alteSchrulle.Connections.First().ConnectionType == ConnectionType.Couple);
            Assert.Contains(villager.PlayerName, alteSchrulle.Connections.First().DiesToo!);
            Assert.True(villager.Connections.First().ConnectionType == ConnectionType.Couple);
            Assert.Contains(alteSchrulle.PlayerName, villager.Connections.First().DiesToo!);

            // Second Amor
            Assert.True(doctor.Connections.First().ConnectionType == ConnectionType.Couple);
            Assert.Contains(werwolf.PlayerName, doctor.Connections.First().DiesToo!);
            Assert.True(werwolf.Connections.First().ConnectionType == ConnectionType.Couple);
            Assert.Contains(doctor.PlayerName, werwolf.Connections.First().DiesToo!);

            // Werwolf
            Assert.True(gm.DeadPlayers.Count == 2);
            Assert.False(werwolf.IsAlive);
            Assert.False(doctor.IsAlive);
            Assert.True(alteSchrulle.IsAlive);
            Assert.True(villager.IsAlive);
        }

        // 2 Amor Select 3 Normal Roles -> 1 triple Couple dies together
        [Fact]
        public void MultipleAmor_CrossSelect_NormalPlayers_Alive_Amorized_Dead()
        {
            List<string> names = new List<string>
            {
                "AlteSchrulle",
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Amor",
                "Werwolf"
            };

            List<Role> roles = new List<Role>
            {
                new AlteSchrulle { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 2 },
                new Werwolf.Data.Werwolf { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var alteSchrulle = gm.AllPlayers.First(p => p.RoleName == nameof(AlteSchrulle));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var amor = gm.AllPlayers.Where(p => p.PlayerName.Contains("Amor")).ToList();
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));

            amor.First().DoAction(new List<string> { alteSchrulle.PlayerName, villager.PlayerName },
                ActionType.Amorize);
            amor.Last().DoAction(new List<string> { villager.PlayerName, werwolf.PlayerName }, ActionType.Amorize);
            werwolf.DoAction(new List<string> { alteSchrulle.PlayerName }, ActionType.Kill);

            _ = gm.EndNight();

            // First Amor
            Assert.True(alteSchrulle.Connections.First().ConnectionType == ConnectionType.Couple);
            Assert.Contains(villager.PlayerName, alteSchrulle.Connections.First().DiesToo!);
            Assert.True(villager.Connections.First().ConnectionType == ConnectionType.Couple);
            Assert.Contains(alteSchrulle.PlayerName, villager.Connections.First().DiesToo!);

            // Second Amor
            Assert.True(villager.Connections[1].ConnectionType == ConnectionType.Couple);
            Assert.True(villager.Connections[1].DiesToo?.First() == werwolf.PlayerName);
            Assert.True(werwolf.Connections.First().ConnectionType == ConnectionType.Couple);
            Assert.Contains(villager.PlayerName, werwolf.Connections.First().DiesToo!);

            // Werwolf
            Assert.True(gm.DeadPlayers.Count == 3);
            Assert.False(werwolf.IsAlive);
            Assert.False(villager.IsAlive);
            Assert.False(alteSchrulle.IsAlive);
        }

        // 1 Amor Select 1 different role and himself -> 1 Couple dies together
        [Fact]
        public void Amor_Select_Himself_Alive_Amorized()
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
                new Raecher() { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 1 },
                new Werwolf.Data.Werwolf { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var amor = gm.AllPlayers.Where(p => p.PlayerName.Contains("Amor")).ToList();
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));

            amor.First().DoAction(new List<string> { villager.PlayerName, amor.First().PlayerName },
                ActionType.Amorize);
            werwolf.DoAction(new List<string> { raecher.PlayerName }, ActionType.Kill);
            raecher.DoAction(new List<string>{villager.PlayerName}, ActionType.RevengeKill);

            _ = gm.EndNight();

            // First Amor
            Assert.True(villager.Connections.First().ConnectionType == ConnectionType.Couple);
            Assert.True(villager.Connections.First().DiesToo?.First() == amor.First().PlayerName);
            Assert.True(amor.First().Connections.First().ConnectionType == ConnectionType.Couple);
            Assert.True(amor.First().Connections.First().DiesToo?.First() == villager.PlayerName);

            // Werwolf
            Assert.True(gm.DeadPlayers.Count == 3);
            Assert.False(amor.First().IsAlive);
            Assert.False(villager.IsAlive);
            Assert.False(raecher.IsAlive);
        }

        // 1 Amor gets bitten after amorize -> 1 Couple, can select Player as Werwolf
        [Fact]
        public void Amor_GetsBitten_AfterSelect_CanKill()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf",
                nameof(KittenWerwolf)
            };

            List<Role> roles = new List<Role>
            {
                new Raecher() { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 1 },
                new Werwolf.Data.Werwolf { Count = 1 },
                new KittenWerwolf { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var amor = gm.AllPlayers.First(p => p.PlayerName == nameof(Amor));
            var kittenWerwolf = gm.AllPlayers.First(p => p.RoleName == nameof(KittenWerwolf));

            amor.DoAction(new List<string> { villager.PlayerName, raecher.PlayerName },
                ActionType.Amorize);
            kittenWerwolf.DoAction(new List<string> { amor.PlayerName }, ActionType.Bite);

            _ = gm.EndNight();

            // First Amor
            Assert.True(villager.Connections.First().ConnectionType == ConnectionType.Couple);
            Assert.True(villager.Connections.First().DiesToo?.First() == raecher.PlayerName);
            Assert.True(raecher.Connections.First().ConnectionType == ConnectionType.Couple);
            Assert.True(raecher.Connections.First().DiesToo?.First() == villager.PlayerName);

            _ = gm.EndNight();

            amor = gm.AllPlayers.First(p => p.PlayerName == nameof(Amor));

            // Werwolf
            Assert.True(gm.DeadPlayers.Count == 0);
            Assert.True(amor.IsAlive);
            Assert.True(villager.IsAlive);
            Assert.True(raecher.IsAlive);
            Assert.False(amor.HasActionSelection);
            Assert.True(amor.HasPlayerSelection);
            Assert.False(amor.HasUsedOneTimeAction);
            Assert.Single(amor.Actions);
            Assert.Equal(ActionType.Kill, amor.ActionType);
            Assert.Equal(nameof(Data.Werwolf), amor.RoleName);
        }
    }
}
