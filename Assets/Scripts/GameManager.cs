
public class GameManager : Singleton<GameManager>
{
    public delegate void Point(int point);
    public delegate void WinGame();
    public Point OnMakePoint;
    public WinGame OnWinGame;
    
    public int Score = 0;
    public int ScoreToWin = 3;

    public void MakePoint()
    {
        Score += 1;
        OnMakePoint?.Invoke(Score);

        if (Score >= ScoreToWin)
        {
            OnWinGame?.Invoke();
        }
    }
}
