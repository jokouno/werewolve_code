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

            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));
            var grabrauber = gm.AllPlayers.First(p => p.RoleName == nameof(Grabrauber));
            var alteSchrulle = gm.AllPlayers.First(p => p.RoleName == nameof(AlteSchrulle));

            werwolf.DoAction(new List<string> { villager.PlayerName }, ActionType.Kill);
            grabrauber.DoAction(new List<string> {alteSchrulle.PlayerName}, ActionType.StealRole);

            _ = gm.EndNight();

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

            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));
            var grabrauber = gm.AllPlayers.First(p => p.RoleName == nameof(Grabrauber));
            var alteSchrulle = gm.AllPlayers.First(p => p.RoleName == nameof(AlteSchrulle));

            werwolf.DoAction(new List<string> { alteSchrulle.PlayerName }, ActionType.Kill);
            grabrauber.DoAction(new List<string> { alteSchrulle.PlayerName }, ActionType.StealRole);

            _ = gm.EndNight();

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

            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var amor = gm.AllPlayers.First(p => p.RoleName == nameof(Amor));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));
            var grabrauber = gm.AllPlayers.First(p => p.RoleName == nameof(Grabrauber));

            werwolf.DoAction(new List<string> { amor.PlayerName }, ActionType.Kill);
            amor.DoAction(new List<string> {villager.PlayerName, grabrauber.PlayerName}, ActionType.Amorize);
            grabrauber.DoAction(new List<string> { amor.PlayerName }, ActionType.StealRole);

            _ = gm.EndNight();

            grabrauber = gm.AllPlayers.First(p => p.PlayerName == nameof(Grabrauber));

            // Grabrauber
            Assert.True(gm.DeadPlayers.Count == 1);
            Assert.False(amor.IsAlive);
            Assert.True(grabrauber.RoleName == nameof(Amor));
            Assert.True(grabrauber.ActionType == ActionType.Amorize);
            Assert.False(grabrauber.HasUsedOneTimeAction);
            Assert.True(grabrauber.SelectedPlayersForAction.Count == 0);

            // Amor
            Assert.Contains(ConnectionType.Couple, grabrauber.Connections.Select(x => x.ConnectionType));
            Assert.Contains(ConnectionType.Couple, villager.Connections.Select(x => x.ConnectionType));
            Assert.Contains(villager.PlayerName, grabrauber.Connections.First().DiesToo!);
            Assert.Contains(grabrauber.PlayerName, villager.Connections.First().DiesToo!);
        }

        // 1 Grabrauber Selects Amor (OneTimeAction) Dies by voting -> Gets Role Amor and OneTimeAction
        [Fact]
        public void Grabrauber_Select_Amor_DiesByVoting_GetsRole_GetsOneTimeAction()
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

            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var amor = gm.AllPlayers.First(p => p.RoleName == nameof(Amor));
            var grabrauber = gm.AllPlayers.First(p => p.RoleName == nameof(Grabrauber));

            amor.DoAction(new List<string> { villager.PlayerName, grabrauber.PlayerName }, ActionType.Amorize);
            grabrauber.DoAction(new List<string> { amor.PlayerName }, ActionType.StealRole);

            _ = gm.EndNight();
            gm.VotForVillagerElection(new List<RolePresentation>
            {
                RolePresentation.Clone(amor),
                RolePresentation.Clone(amor),
                RolePresentation.Clone(villager)
            });

            _ = gm.EndPlayerVote();

            grabrauber = gm.AllPlayers.First(p => p.PlayerName == nameof(Grabrauber));

            // Grabrauber
            Assert.True(gm.DeadPlayers.Count == 1);
            Assert.False(amor.IsAlive);
            Assert.Equal(nameof(Amor), grabrauber.RoleName);
            Assert.Equal(ActionType.Amorize, grabrauber.ActionType);
            Assert.False(grabrauber.HasUsedOneTimeAction);
            Assert.Empty(grabrauber.SelectedPlayersForAction);

            // Amor
            Assert.Contains(ConnectionType.Couple, grabrauber.Connections.Select(x => x.ConnectionType));
            Assert.Contains(ConnectionType.Couple, villager.Connections.Select(x => x.ConnectionType));
            Assert.Contains(villager.PlayerName, grabrauber.Connections.First().DiesToo!);
            Assert.Contains(grabrauber.PlayerName, villager.Connections.First().DiesToo!);
        }

        // 1 Grabrauber Selects Grabrauber (OneTimeAction) Dead -> Gets Role Grabrauber and OneTimeAction
        [Fact]
        public void Grabrauber_Select_Grabrauber_Dead_GetsRole_GetsOneTimeAction()
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
                nameof(Grabrauber),
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
                new Grabrauber { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));
            var grabrauber = gm.AllPlayers.Where(p => p.RoleName == nameof(Grabrauber)).ToList();

            werwolf.DoAction(new List<string> { grabrauber[0].PlayerName }, ActionType.Kill);
            grabrauber[1].DoAction(new List<string> { grabrauber[0].PlayerName }, ActionType.StealRole);

            _ = gm.EndNight();

            grabrauber[1] = gm.AllPlayers.First(p => p.PlayerName == nameof(Grabrauber));

            // Grabrauber
            Assert.True(gm.DeadPlayers.Count == 1);
            Assert.False(grabrauber[0].IsAlive);
            Assert.True(grabrauber[1].RoleName == nameof(Grabrauber));
            Assert.True(grabrauber[1].ActionType == ActionType.StealRole);
            Assert.False(grabrauber[1].HasUsedOneTimeAction);
            Assert.True(grabrauber[1].SelectedPlayersForAction.Count == 0);
        }

        // 1 Grabrauber Selects Raecher (OneTimeAction) Dead -> Gets Role Raecher and OneTimeAction
        [Fact]
        public void Grabrauber_Select_Raecher_Dead_GetsRole_GetsOneTimeAction()
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
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));
            var grabrauber = gm.AllPlayers.First(p => p.RoleName == nameof(Grabrauber));

            werwolf.DoAction(new List<string> { raecher.PlayerName }, ActionType.Kill);
            raecher.DoAction(new List<string> { doctor.PlayerName }, ActionType.RevengeKill);
            grabrauber.DoAction(new List<string> { raecher.PlayerName }, ActionType.StealRole);

            _ = gm.EndNight();

            grabrauber = gm.AllPlayers.First(p => p.PlayerName == nameof(Grabrauber));

            // Grabrauber
            Assert.True(gm.DeadPlayers.Count == 2);
            Assert.False(doctor.IsAlive);
            Assert.False(raecher.IsAlive);
            Assert.True(grabrauber.RoleName == nameof(Raecher));
            Assert.True(grabrauber.ActionType == ActionType.RevengeKill);
            Assert.False(grabrauber.HasUsedOneTimeAction);
            Assert.True(grabrauber.SelectedPlayersForAction.Count == 0);

            grabrauber.DoAction(new List<string> { villager.PlayerName }, ActionType.RevengeKill);

            // Day Voting
            gm.VotForVillagerElection(new List<RolePresentation>
            {
                RolePresentation.Clone(grabrauber)
            });

            _ = gm.EndPlayerVote();

            // Grabrauber
            Assert.Equal(2, gm.DeadPlayers.Count);
            Assert.False(grabrauber.IsAlive);
            Assert.False(villager.IsAlive);
            Assert.True(grabrauber.RoleName == nameof(Raecher));
            Assert.True(grabrauber.ActionType == ActionType.RevengeKill);
            Assert.False(grabrauber.HasUsedOneTimeAction);
            Assert.Single(grabrauber.SelectedPlayersForAction);
        }

        // 1 Grabrauber Selects KittenWerwolf (OneTimeAction) Dead -> Gets Role KittenWerwolf and OneTimeAction
        [Fact]
        public void Grabrauber_Select_KittenWerwolf_Dead_GetsRole_GetsOneTimeAction()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf",
                nameof(Grabrauber),
                nameof(Hexe),
                nameof(KittenWerwolf),
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
                new KittenWerwolf { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var grabrauber = gm.AllPlayers.First(p => p.RoleName == nameof(Grabrauber));
            var hexe = gm.AllPlayers.First(p => p.RoleName == nameof(Hexe));
            var kittenWerwolf = gm.AllPlayers.First(p => p.RoleName == nameof(KittenWerwolf));

            kittenWerwolf.DoAction(new List<string> { raecher.PlayerName }, ActionType.Bite);
            hexe.DoAction(new List<string> { kittenWerwolf.PlayerName }, ActionType.InstantKill);
            grabrauber.DoAction(new List<string> { kittenWerwolf.PlayerName }, ActionType.StealRole);

            _ = gm.EndNight();

            grabrauber = gm.AllPlayers.First(p => p.PlayerName == nameof(Grabrauber));
            raecher = gm.AllPlayers.First(p => p.PlayerName == nameof(Raecher));

            // Grabrauber
            Assert.Single(gm.DeadPlayers);
            Assert.False(kittenWerwolf.IsAlive);
            Assert.Equal(nameof(Data.Werwolf), raecher.RoleName);
            Assert.Equal(nameof(KittenWerwolf), grabrauber.RoleName);
            Assert.False(grabrauber.HasUsedOneTimeAction);
            Assert.Empty(grabrauber.SelectedPlayersForAction);
            Assert.Equal(2, grabrauber.Actions.Count);
        }

        // 1 Grabrauber Dies Selects KittenWerwolf (OneTimeAction) Dead -> No Action
        [Fact]
        public void Grabrauber_Dies_Select_KittenWerwolf_Dead_NoAction()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf",
                nameof(Grabrauber),
                nameof(Hexe),
                nameof(KittenWerwolf),
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
                new KittenWerwolf { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));
            var grabrauber = gm.AllPlayers.First(p => p.RoleName == nameof(Grabrauber));
            var hexe = gm.AllPlayers.First(p => p.RoleName == nameof(Hexe));
            var kittenWerwolf = gm.AllPlayers.First(p => p.RoleName == nameof(KittenWerwolf));

            kittenWerwolf.DoAction(new List<string> { raecher.PlayerName }, ActionType.Bite);
            hexe.DoAction(new List<string> { kittenWerwolf.PlayerName }, ActionType.InstantKill);
            werwolf.DoAction(new List<string> { grabrauber.PlayerName }, ActionType.Kill);
            grabrauber.DoAction(new List<string> { kittenWerwolf.PlayerName }, ActionType.StealRole);

            _ = gm.EndNight();

            grabrauber = gm.AllPlayers.First(p => p.PlayerName == nameof(Grabrauber));
            raecher = gm.AllPlayers.First(p => p.PlayerName == nameof(Raecher));

            // Grabrauber
            Assert.Equal(2, gm.DeadPlayers.Count);
            Assert.False(kittenWerwolf.IsAlive);
            Assert.False(grabrauber.IsAlive);
            Assert.Equal(nameof(Data.Werwolf), raecher.RoleName);
            Assert.Equal(nameof(Grabrauber), grabrauber.RoleName);
            Assert.False(grabrauber.HasUsedOneTimeAction);
            Assert.Single(grabrauber.Actions);

            _ = gm.EndNight();

            grabrauber = gm.AllPlayers.First(p => p.PlayerName == nameof(Grabrauber));
            raecher = gm.AllPlayers.First(p => p.PlayerName == nameof(Raecher));

            // Grabrauber
            Assert.Empty(gm.DeadPlayers);
            Assert.False(kittenWerwolf.IsAlive);
            Assert.False(grabrauber.IsAlive);
            Assert.Equal(nameof(Data.Werwolf), raecher.RoleName);
            Assert.Equal(nameof(Grabrauber), grabrauber.RoleName);
            Assert.False(grabrauber.HasUsedOneTimeAction);
            Assert.Single(grabrauber.Actions);
        }

        // 1 Grabrauber Selects Dead KittenWerwolf (OneTimeAction) But KittenWerwolf heals -> No Action
        [Fact]
        public void Grabrauber_Dies_Select_KittenWerwolf_Dead_Heals_NoAction()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf",
                nameof(Grabrauber),
                nameof(Hexe),
                nameof(KittenWerwolf),
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
                new KittenWerwolf { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var doctor = gm.AllPlayers.First(p => p.RoleName == nameof(Doctor));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));
            var grabrauber = gm.AllPlayers.First(p => p.RoleName == nameof(Grabrauber));
            var hexe = gm.AllPlayers.First(p => p.RoleName == nameof(Hexe));
            var kittenWerwolf = gm.AllPlayers.First(p => p.RoleName == nameof(KittenWerwolf));

            kittenWerwolf.DoAction(new List<string> { raecher.PlayerName }, ActionType.Bite);
            hexe.DoAction(new List<string> { kittenWerwolf.PlayerName }, ActionType.InstantKill);
            werwolf.DoAction(new List<string> { grabrauber.PlayerName }, ActionType.Kill);
            grabrauber.DoAction(new List<string> { kittenWerwolf.PlayerName }, ActionType.StealRole);
            doctor.DoAction(new List<string> { kittenWerwolf.PlayerName }, ActionType.Heal);

            _ = gm.EndNight();

            grabrauber = gm.AllPlayers.First(p => p.PlayerName == nameof(Grabrauber));
            raecher = gm.AllPlayers.First(p => p.PlayerName == nameof(Raecher));

            // Grabrauber
            Assert.Single(gm.DeadPlayers);
            Assert.True(kittenWerwolf.IsAlive);
            Assert.False(grabrauber.IsAlive);
            Assert.Equal(nameof(Data.Werwolf), raecher.RoleName);
            Assert.Equal(nameof(Grabrauber), grabrauber.RoleName);
            Assert.False(grabrauber.HasUsedOneTimeAction);
            Assert.Single(grabrauber.Actions);

            _ = gm.EndNight();

            grabrauber = gm.AllPlayers.First(p => p.PlayerName == nameof(Grabrauber));
            raecher = gm.AllPlayers.First(p => p.PlayerName == nameof(Raecher));

            // Grabrauber
            Assert.Empty(gm.DeadPlayers);
            Assert.True(kittenWerwolf.IsAlive);
            Assert.False(grabrauber.IsAlive);
            Assert.Equal(nameof(Data.Werwolf), raecher.RoleName);
            Assert.Equal(nameof(Grabrauber), grabrauber.RoleName);
            Assert.False(grabrauber.HasUsedOneTimeAction);
            Assert.Single(grabrauber.Actions);
        }

        // 1 Grabrauber Selects Couple KittenWerwolf (OneTimeAction) -> Get Role, Gets OneTimeAction, No Couple
        [Fact]
        public void Grabrauber_Select_KittenWerwolf_Couple_Dead_GetsRole_NoCouple()
        {
            List<string> names = new List<string>
            {
                nameof(Raecher),
                "Dorfbewohner",
                "Doctor",
                "Amor",
                "Werwolf",
                nameof(Grabrauber),
                nameof(Hexe),
                nameof(KittenWerwolf),
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
                new KittenWerwolf { Count = 1 }
            };

            var gm = GameManagerTests.InitializeTest(names, roles);

            var raecher = gm.AllPlayers.First(p => p.RoleName == nameof(Raecher));
            var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));
            var amor = gm.AllPlayers.First(p => p.RoleName == nameof(Amor));
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));
            var grabrauber = gm.AllPlayers.First(p => p.RoleName == nameof(Grabrauber));
            var hexe = gm.AllPlayers.First(p => p.RoleName == nameof(Hexe));
            var kittenWerwolf = gm.AllPlayers.First(p => p.RoleName == nameof(KittenWerwolf));

            kittenWerwolf.DoAction(new List<string> { raecher.PlayerName }, ActionType.Bite);
            hexe.DoAction(new List<string> { kittenWerwolf.PlayerName }, ActionType.InstantKill);
            werwolf.DoAction(new List<string> { grabrauber.PlayerName }, ActionType.Kill);
            grabrauber.DoAction(new List<string> { kittenWerwolf.PlayerName }, ActionType.StealRole);
            amor.DoAction(new List<string> { kittenWerwolf.PlayerName, villager.PlayerName }, ActionType.Amorize);

            _ = gm.EndNight();

            grabrauber = gm.AllPlayers.First(p => p.PlayerName == nameof(Grabrauber));
            raecher = gm.AllPlayers.First(p => p.PlayerName == nameof(Raecher));

            Assert.Equal(3, gm.DeadPlayers.Count);

            // Amor
            Assert.False(kittenWerwolf.IsAlive);
            Assert.False(villager.IsAlive);
            Assert.Contains(ConnectionType.Couple, villager.Connections.Select(x => x.ConnectionType));
            Assert.Contains(ConnectionType.Couple, kittenWerwolf.Connections.Select(x => x.ConnectionType));

            // Grabrauber
            Assert.False(grabrauber.IsAlive);
            Assert.Equal(nameof(Data.Werwolf), raecher.RoleName);
            Assert.Equal(nameof(Grabrauber), grabrauber.RoleName);
            Assert.False(grabrauber.HasUsedOneTimeAction);
            Assert.Single(grabrauber.Actions);
            Assert.Single(grabrauber.Connections);
            Assert.Contains(ConnectionType.StealRole, grabrauber.Connections.Select(x => x.ConnectionType));

            _ = gm.EndNight();

            grabrauber = gm.AllPlayers.First(p => p.PlayerName == nameof(Grabrauber));
            raecher = gm.AllPlayers.First(p => p.PlayerName == nameof(Raecher));

            // Grabrauber
            Assert.Empty(gm.DeadPlayers);
            Assert.False(kittenWerwolf.IsAlive);
            Assert.False(villager.IsAlive);
            Assert.False(grabrauber.IsAlive);
            Assert.Equal(nameof(Data.Werwolf), raecher.RoleName);
            Assert.Equal(nameof(Grabrauber), grabrauber.RoleName);
            Assert.False(grabrauber.HasUsedOneTimeAction);
            Assert.Single(grabrauber.Actions);
        }
    }
}
