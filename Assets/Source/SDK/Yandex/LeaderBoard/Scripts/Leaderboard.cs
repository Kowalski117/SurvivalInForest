using Agava.YandexGames;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    private const string AnonymousName = "Anonymous";
    private const string LeaderboardName = "LeaderboardName";

    [SerializeField] private MainClock _timeHandler;
    [SerializeField] private LeaderboardView _leaderboardView;

    private readonly List<LeaderboardPlayer> _leaderboardPlayers = new();

    private void OnEnable()
    {
        if(_timeHandler)
            _timeHandler.OnDayUpdated += SetPlayer;
    }

    private void OnDisable()
    {
        if (_timeHandler)
            _timeHandler.OnDayUpdated -= SetPlayer;
    }

    public void SetPlayer(int score)
    {
#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
        if (PlayerAccount.IsAuthorized == false)
            return;

        Agava.YandexGames.Leaderboard.GetPlayerEntry(LeaderboardName, result =>
        {
            Debug.Log(result.score);
            Debug.Log(score);

            if(result.score > score)
                Agava.YandexGames.Leaderboard.SetScore(LeaderboardName, score);
        });
#endif
    }

    public void Fill()
    {
#if YANDEX_GAMES && UNITY_WEBGL && !UNITY_EDITOR
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
                var icon = result.entries[i].player.profilePicture;

                if (string.IsNullOrEmpty(name))
                    name = AnonymousName;

                _leaderboardPlayers.Add(new LeaderboardPlayer(rank, name, score));
            }

            _leaderboardView.ConstructLeaderboard(_leaderboardPlayers);
        });
#endif
    }
}
