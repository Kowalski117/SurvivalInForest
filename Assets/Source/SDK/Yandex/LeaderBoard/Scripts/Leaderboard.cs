using Agava.YandexGames;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    private const string AnonymousName = "Anonymous";
    private const string LeaderboardName = "LeaderboardName";

    [SerializeField] private LeaderboardView _leaderboardView;

    private readonly List<LeaderboardPlayer> _leaderboardPlayers = new();

    public void SetPlayer(int score)
    {
        if (PlayerAccount.IsAuthorized == false)
            return;

        Agava.YandexGames.Leaderboard.GetPlayerEntry(LeaderboardName, _ =>
        {
            Agava.YandexGames.Leaderboard.SetScore(LeaderboardName, score);
        });
    }

    public void Fill()
    {
        _leaderboardPlayers.Clear();

        if(PlayerAccount.IsAuthorized == false)
            return;

        Agava.YandexGames.Leaderboard.GetEntries(LeaderboardName, result =>
        {
            for(int i = 0; i < result.entries.Length; i++)
            {
                var rank = result.entries[i].rank;
                var score = result.entries[i].score;
                var name = result.entries[i].player.publicName;

                if (string.IsNullOrEmpty(name))
                    name = AnonymousName;

                _leaderboardPlayers.Add(new LeaderboardPlayer(rank, name, score));
            }

            _leaderboardView.ConstructLeaderboard(_leaderboardPlayers);
        });
    }
}
