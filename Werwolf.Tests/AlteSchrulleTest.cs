using Werwolf.Data;
using Werwolf.Data.Actions;

namespace Werwolf.Tests
{
    public class AlteSchrulleTest
    {
        [Fact]
        public void AlteSchrulle_Select_NormalPlayer_Alive_CantVote()
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

            alteSchrulle.DoAction(new List<string> { villager.PlayerName}, ActionType.NoVoteAllowed);

            _ = gm.EndNight();

            Assert.False(villager.IsAllowedToVote);
        }

        [Fact]
        public void AlteSchrulle_Select_NormalPlayer_Dead_CantVote()
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
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));

            werwolf.DoAction(new List<string>{villager.PlayerName}, ActionType.Kill);
            alteSchrulle.DoAction(new List<string> { villager.PlayerName }, ActionType.NoVoteAllowed);

            _ = gm.EndNight();

            Assert.False(villager.IsAllowedToVote);
            Assert.False(villager.IsAlive);
            Assert.Single(gm.DeadPlayers);
            Assert.Equal("Dorfbewohner", gm.DeadPlayers.First().PlayerName);
        }

        [Fact]
        public void AlteSchrulle_Select_Self_CanVote()
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

            alteSchrulle.DoAction(new List<string> { alteSchrulle.PlayerName }, ActionType.NoVoteAllowed);

            _ = gm.EndNight();

            Assert.True(villager.IsAllowedToVote);
        }

        [Fact]
        public void AlteSchrulle_Select_NormalPlayer_MultipleRounds_Alive_CantVote()
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
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));

            alteSchrulle.DoAction(new List<string> { werwolf.PlayerName }, ActionType.NoVoteAllowed);

            _ = gm.EndNight();
            Assert.False(werwolf.IsAllowedToVote);

            alteSchrulle.DoAction(new List<string> { werwolf.PlayerName }, ActionType.NoVoteAllowed);

            Assert.False(werwolf.IsAllowedToVote);
        }

        [Fact]
        public void AlteSchrulle_Dies_Select_NormalPlayer_Alive_CantVote()
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
            var werwolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));

            alteSchrulle.DoAction(new List<string> { werwolf.PlayerName }, ActionType.NoVoteAllowed);
            werwolf.DoAction(new List<string>{alteSchrulle.PlayerName}, ActionType.Kill);

            _ = gm.EndNight();
            Assert.False(werwolf.IsAllowedToVote);

            _ = gm.EndNight();

            Assert.True(werwolf.IsAllowedToVote);
        }
    }
}
