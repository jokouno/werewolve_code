using System.Collections.Generic;
using System.Linq;
using Xunit;
using Werwolf.Data;
using Werwolf.Data.Actions;
using Werwolf.Workflow;

namespace Werwolf.Tests;

public class GameManagerTests
{
    public static IEnumerable<object[]> RoleInstances => new List<object[]>
    {
        new object[] { new AlteSchrulle() },
        new object[] { new Amor() },
        new object[] { new Doctor() },
        new object[] { new Dorfbewohner() },
        new object[] { new Grabrauber() },
        new object[] { new Hexe() },
        new object[] { new KittenWerwolf() },
        new object[] { new Raecher() },
        new object[] { new Seherin() },
        new object[] { new Werwolf.Data.Werwolf() }
    };

    public static GameManager InitializeTest(List<string> names, List<Role> roles)
    {
        var gm = new GameManager();

        gm.SetNames(names);
        gm.StartGame(roles);

        foreach (Role allPlayer in gm.AllPlayers)
        {
            Console.WriteLine($"{allPlayer.PlayerName}; {allPlayer.RoleName}");
        }

        return gm;
    }

    [Theory]
    [MemberData(nameof(RoleInstances))]
    public void Start_CreatesExpectedNumber(Role role)
    {
        role.Count = 2;
        var players = role.Start();
        Assert.Equal(2, players.Count);
        Assert.All(players, p => Assert.Equal(role.RoleName, p.RoleName));
    }

    [Fact]
    public void ProcessNight_KillHealedTarget_NoOneDies()
    {
        var gm = new GameManager();
        gm.SetNames(new List<string> { "Wolf", "Doc", "Villager" });

        var roles = new List<Role>
        {
            new Werwolf.Data.Werwolf { Count = 1 },
            new Doctor { Count = 1 },
            new Dorfbewohner { Count = 1 }
        };

        gm.StartGame(roles);

        var wolf = gm.AllPlayers.First(p => p.RoleName == nameof(Werwolf.Data.Werwolf));
        var doctor = gm.AllPlayers.First(p => p.RoleName == nameof(Doctor));
        var villager = gm.AllPlayers.First(p => p.RoleName == nameof(Dorfbewohner));

        wolf.DoAction(new List<string> { villager.PlayerName }, ActionType.Kill);
        doctor.DoAction(new List<string> { villager.PlayerName }, ActionType.Heal);

        gm.ProcessNight();

        Assert.Empty(gm.DeadPlayers);
        Assert.True(villager.IsAlive);
    }

    [Fact]
    public void EndPlayerVote_EliminatesHighestVotedPlayer()
    {
        var gm = new GameManager();
        gm.SetNames(new List<string> { "A", "B", "C" });

        var roles = new List<Role>
        {
            new Dorfbewohner { Count = 3 }
        };

        gm.StartGame(roles);

        var players = gm.AllPlayers.ToDictionary(p => p.PlayerName, p => p);

        // two votes for player B, one vote for player C
        gm.VotForVillagerElection(new List<RolePresentation>
        {
            RolePresentation.Clone(players["B"]),
            RolePresentation.Clone(players["B"]),
            RolePresentation.Clone(players["C"])
        });

        gm.EndPlayerVote();

        Assert.Single(gm.DeadPlayers);
        Assert.Equal("B", gm.DeadPlayers.First().PlayerName);
    }
}
