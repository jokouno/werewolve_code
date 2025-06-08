using Werwolf.Data;
using Werwolf.Data.Actions;

namespace Werwolf.Tests
{
    public class GrabrauberTest
    {
        // 1 Grabrauber Selects AlteSchrulle Alive -> Nothing Happens
        [Fact]
        public void Grabrauber_Select_AlteSchrulle_Alive_NothingHappens()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf",
                nameof(Grabrauber),
                nameof(AlteSchrulle)
            };

            List<Role> roles = new List<Role>
            {
                new Raecher { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 1 },
                new Werwolf.Data.Werwolf { Count = 1 },
                new Grabrauber { Count = 1 },
                new AlteSchrulle { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var doctor = gm.AllPlayers.First(p => p.RoleName == nameof(Doctor));
            var amor = gm.AllPlayers.First(p => p.RoleName == nameof(Amor));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));
            var grabrauber = gm.AllPlayers.First(p => p.RoleName == nameof(Grabrauber));
            var alteSchrulle = gm.AllPlayers.First(p => p.RoleName == nameof(AlteSchrulle));

            werwolf.DoAction(new List<string> { villager.PlayerName }, ActionType.Kill);
            grabrauber.DoAction(new List<string> {alteSchrulle.PlayerName}, ActionType.StealRole);

            gm.ProcessNight();

            grabrauber = gm.AllPlayers.First(p => p.RoleName == nameof(Grabrauber));

            // Grabrauber
            Assert.True(gm.DeadPlayers.Count == 1);
            Assert.False(villager.IsAlive);
            Assert.True(alteSchrulle.IsAlive);
            Assert.True(grabrauber.RoleName == nameof(Grabrauber));
        }

        // 1 Grabrauber Selects AlteSchrulle Dead -> Gets Role AlteSchrulle
        [Fact]
        public void Grabrauber_Select_AlteSchrulle_Dead_GetsRole()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf",
                nameof(Grabrauber),
                nameof(AlteSchrulle)
            };

            List<Role> roles = new List<Role>
            {
                new Raecher { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 1 },
                new Werwolf.Data.Werwolf { Count = 1 },
                new Grabrauber { Count = 1 },
                new AlteSchrulle { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var doctor = gm.AllPlayers.First(p => p.RoleName == nameof(Doctor));
            var amor = gm.AllPlayers.First(p => p.RoleName == nameof(Amor));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));
            var grabrauber = gm.AllPlayers.First(p => p.RoleName == nameof(Grabrauber));
            var alteSchrulle = gm.AllPlayers.First(p => p.RoleName == nameof(AlteSchrulle));

            werwolf.DoAction(new List<string> { alteSchrulle.PlayerName }, ActionType.Kill);
            grabrauber.DoAction(new List<string> { alteSchrulle.PlayerName }, ActionType.StealRole);

            gm.ProcessNight();

            grabrauber = gm.AllPlayers.First(p => p.PlayerName == nameof(Grabrauber));

            // Grabrauber
            Assert.True(gm.DeadPlayers.Count == 1);
            Assert.False(alteSchrulle.IsAlive);
            Assert.True(grabrauber.RoleName == nameof(AlteSchrulle));
            Assert.True(grabrauber.ActionType == ActionType.NoVoteAllowed);
        }

        // 1 Grabrauber Selects Amor (OneTimeAction) Dead -> Gets Role Amor and OneTimeAction
        [Fact]
        public void Grabrauber_Select_Amor_Dead_GetsRole_GetsOneTimeAction()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf",
                nameof(Grabrauber),
                nameof(AlteSchrulle)
            };

            List<Role> roles = new List<Role>
            {
                new Raecher { Count = 1 },
                new Dorfbewohner { Count = 1 },
                new Doctor { Count = 1 },
                new Amor { Count = 1 },
                new Werwolf.Data.Werwolf { Count = 1 },
                new Grabrauber { Count = 1 },
                new AlteSchrulle { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var doctor = gm.AllPlayers.First(p => p.RoleName == nameof(Doctor));
            var amor = gm.AllPlayers.First(p => p.RoleName == nameof(Amor));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));
            var grabrauber = gm.AllPlayers.First(p => p.RoleName == nameof(Grabrauber));
            var alteSchrulle = gm.AllPlayers.First(p => p.RoleName == nameof(AlteSchrulle));

            werwolf.DoAction(new List<string> { amor.PlayerName }, ActionType.Kill);
            amor.DoAction(new List<string> {villager.PlayerName, grabrauber.PlayerName}, ActionType.Amorize);
            grabrauber.DoAction(new List<string> { amor.PlayerName }, ActionType.StealRole);

            gm.ProcessNight();

            grabrauber = gm.AllPlayers.First(p => p.PlayerName == nameof(Grabrauber));

            // Grabrauber
            Assert.True(gm.DeadPlayers.Count == 1);
            Assert.False(amor.IsAlive);
            Assert.True(grabrauber.RoleName == nameof(Amor));
            Assert.True(grabrauber.ActionType == ActionType.Amorize);
            Assert.False(grabrauber.HasUsedOneTimeAction);
            Assert.True(grabrauber.SelectedPlayersForAction.Count == 0);

            // Amor
            Assert.True(grabrauber.Connections.Any(x => x.ConnectionType == ConnectionType.Couple));
            Assert.True(villager.Connections.Any(x => x.ConnectionType == ConnectionType.Couple));
            Assert.True(grabrauber.Connections.First().DiesToo.Any(x => x == villager.PlayerName));
            Assert.True(villager.Connections.First().DiesToo.Any(x => x == grabrauber.PlayerName));
        }
    }
}
